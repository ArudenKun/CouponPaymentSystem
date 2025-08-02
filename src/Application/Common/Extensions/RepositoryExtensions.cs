using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.NHibernate.Repositories;
using NHibernate;
using NHibernate.Persister.Entity;

namespace CouponPaymentSystem.Application.Common.Extensions;

public static class RepositoryExtensions
{
    public static async Task BulkInsertAsync<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository,
        IEnumerable<TEntity> entities
    )
        where TEntity : class, IEntity<TPrimaryKey>
    {
        var session = GetSession(repository);
        var classMapping = (SingleTableEntityPersister)
            session.SessionFactory.GetClassMetadata(typeof(TEntity));

        // Get Microsoft.Data.SqlClient types via reflection
        var sqlConnectionType = Type.GetType(
            "Microsoft.Data.SqlClient.SqlConnection, Microsoft.Data.SqlClient"
        );
        var sqlCommandType = Type.GetType(
            "Microsoft.Data.SqlClient.SqlCommand, Microsoft.Data.SqlClient"
        );
        var sqlBulkCopyType = Type.GetType(
            "Microsoft.Data.SqlClient.SqlBulkCopy, Microsoft.Data.SqlClient"
        );
        var sqlBulkCopyOptionsType = Type.GetType(
            "Microsoft.Data.SqlClient.SqlBulkCopyOptions, Microsoft.Data.SqlClient"
        );

        if (
            sqlConnectionType == null
            || sqlCommandType == null
            || sqlBulkCopyType == null
            || sqlBulkCopyOptionsType == null
        )
        {
            throw new InvalidOperationException("Microsoft.Data.SqlClient assembly not found");
        }

        // Create command
        var insertEntitiesCmd = Activator.CreateInstance(sqlCommandType);
        var transaction = session.GetCurrentTransaction();
        if (transaction != null && transaction.IsActive)
        {
            // Get Enlist method via reflection
            var enlistMethod = transaction.GetType().GetMethod("Enlist");
            enlistMethod?.Invoke(transaction, [insertEntitiesCmd]);
        }

        var entityTable = GenerateDataTable<TEntity, TPrimaryKey>(session, entities, classMapping);

        // Create SqlBulkCopy with options
        var checkConstraints = Enum.Parse(sqlBulkCopyOptionsType, "CheckConstraints");
        var fireTriggers = Enum.Parse(sqlBulkCopyOptionsType, "FireTriggers");
        var options = (int)checkConstraints | (int)fireTriggers;

        var bulkCopy = Activator.CreateInstance(
            sqlBulkCopyType,
            session.Connection, // Connection
            options, // SqlBulkCopyOptions
            GetPropertyValue(sqlCommandType, insertEntitiesCmd, "Transaction") // Transaction
        );

        // Set destination table
        var destinationTableNameProp = sqlBulkCopyType.GetProperty("DestinationTableName");
        destinationTableNameProp?.SetValue(bulkCopy, classMapping.TableName);

        // Get ColumnMappings collection
        var columnMappingsProp = sqlBulkCopyType.GetProperty("ColumnMappings");
        var columnMappings = columnMappingsProp?.GetValue(bulkCopy);

        // Get Add method
        var addMethod = columnMappings
            ?.GetType()
            .GetMethod("Add", [typeof(string), typeof(string)]);

        // Add column mappings
        foreach (DataColumn column in entityTable.Columns)
        {
            addMethod?.Invoke(columnMappings, [column.ColumnName, column.ColumnName]);
        }

        // Get WriteToServerAsync method
        var writeToServerAsyncMethod = sqlBulkCopyType.GetMethod(
            "WriteToServerAsync",
            [typeof(DataTable), typeof(CancellationToken)]
        );

        // Execute bulk insert asynchronously
        var task = (Task?)
            writeToServerAsyncMethod?.Invoke(bulkCopy, [entityTable, CancellationToken.None]);

        if (task != null)
            await task.ConfigureAwait(false);

        // If the bulk copy implements IDisposable, dispose it
        var disposable = bulkCopy as IDisposable;
        disposable?.Dispose();
    }

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    private static DataTable GenerateDataTable<TEntity, TPrimaryKey>(
        ISession session,
        IEnumerable<TEntity> entities,
        SingleTableEntityPersister classMapping
    )
        where TEntity : class, IEntity<TPrimaryKey>
    {
        var generator = classMapping.IdentifierGenerator;

        var entityTable = new DataTable();
        var propertyNames = classMapping.PropertyNames;

        var identifierColumnNames = classMapping.IdentifierColumnNames.FirstOrDefault();
        if (identifierColumnNames != null)
        {
            entityTable.Columns.Add(identifierColumnNames, typeof(long));
        }

        var persistedProperties = propertyNames
            .Select(propertyName =>
            {
                var propertyType = classMapping.GetPropertyType(propertyName);
                if (propertyType.IsCollectionType)
                {
                    return null;
                }

                var type = propertyType.ReturnedClass;
                if (propertyType.IsEntityType)
                {
                    type = typeof(long);
                }

                var columnName = classMapping.GetPropertyColumnNames(propertyName).FirstOrDefault();
                if (columnName == null)
                {
                    return null;
                }

                entityTable.Columns.Add(columnName, type);

                return new
                {
                    ColumnName = columnName,
                    PropertyName = propertyName,
                    propertyType.ReturnedClass.IsEnum,
                    Type = propertyType.ReturnedClass,
                };
            })
            .Where(x => x != null);

        foreach (var entity in entities)
        {
            var row = entityTable.NewRow();

            if (identifierColumnNames != null)
            {
                if (typeof(TPrimaryKey) == typeof(long) || typeof(TPrimaryKey) == typeof(int))
                {
                    var value = (TPrimaryKey)
                        generator.Generate(session.GetSessionImplementation(), null);
                    row[identifierColumnNames] = value;
                    entity.Id = value;
                }
            }

            foreach (var persistedProperty in persistedProperties)
            {
                var columnName = persistedProperty?.ColumnName;
                if (columnName != null)
                {
                    object value = classMapping.GetPropertyValue(
                        entity,
                        persistedProperty?.PropertyName
                    );

                    if (value == null)
                    {
                        row[columnName] = DBNull.Value;
                    }
                    else
                    {
                        if (persistedProperty is { IsEnum: true })
                        {
                            row[columnName] = Enum.GetName(persistedProperty.Type, value);
                        }
                        else if (value is IEntity)
                        {
                            row[columnName] = (value as IEntity)?.Id;
                        }
                        else
                        {
                            row[columnName] = value;
                        }
                    }
                }
            }

            entityTable.Rows.Add(row);
        }

        return entityTable;
    }

    public static ISession GetSession<TEntity, TPrimaryKey>(
        IRepository<TEntity, TPrimaryKey> repository
    )
        where TEntity : class, IEntity<TPrimaryKey>
    {
        if (repository is NhRepositoryBase<TEntity, TPrimaryKey> nhRepository)
        {
            return nhRepository.Session;
        }

        throw new InvalidOperationException(
            $"Could not find ISession property on repository of type {repository.GetType().FullName}"
        );
    }

    private static object? GetPropertyValue(Type type, object instance, string propertyName) =>
        type.GetProperty(
                propertyName,
                BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Static
            )
            ?.GetValue(instance);

    // private static object? GetFieldValue(Type type, object instance, string fieldName) =>
    //     type.GetField(
    //             fieldName,
    //             BindingFlags.Instance
    //                 | BindingFlags.Public
    //                 | BindingFlags.NonPublic
    //                 | BindingFlags.Static
    //         )
    //         ?.GetValue(instance);
}
