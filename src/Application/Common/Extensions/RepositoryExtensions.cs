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
        if (transaction is { IsActive: true })
        {
            // Get Enlist method via reflection
            var enlistMethod = transaction.GetType().GetMethod("Enlist");
            enlistMethod?.Invoke(transaction, [insertEntitiesCmd]);
        }

        var entityTable = GenerateDataTable<TEntity, TPrimaryKey>(entities, classMapping);

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
        IEnumerable<TEntity> entities,
        SingleTableEntityPersister classMapping
    )
        where TEntity : class, IEntity<TPrimaryKey>
    {
        var entityTable = new DataTable();
        var propertyNames = classMapping.PropertyNames;
        var addedColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // Track added columns

        // Handle identifier column
        var identifierColumnNames = classMapping.IdentifierColumnNames.FirstOrDefault();
        if (!string.IsNullOrEmpty(identifierColumnNames))
        {
            if (!addedColumns.Contains(identifierColumnNames))
            {
                entityTable.Columns.Add(identifierColumnNames, typeof(TPrimaryKey));
                addedColumns.Add(identifierColumnNames);
            }
        }

        // Process properties
        var persistedProperties = new List<PropertyMappingInfo>();
        foreach (var propertyName in propertyNames)
        {
            var propertyType = classMapping.GetPropertyType(propertyName);

            // Skip collections
            if (propertyType.IsCollectionType)
            {
                continue;
            }

            // Get column name
            var columnNames = classMapping.GetPropertyColumnNames(propertyName);
            var columnName = columnNames?.FirstOrDefault();
            if (string.IsNullOrEmpty(columnName))
            {
                continue;
            }

            // Skip duplicate columns
            if (addedColumns.Contains(columnName ?? string.Empty))
            {
                continue;
            }

            // Determine type
            Type type = propertyType.ReturnedClass;
            bool isEntity = false;

            if (propertyType.IsEntityType)
            {
                type = typeof(TPrimaryKey);
                isEntity = true;
            }

            // Add column
            entityTable.Columns.Add(columnName, type);
            addedColumns.Add(columnName);

            persistedProperties.Add(
                new PropertyMappingInfo
                {
                    ColumnName = columnName,
                    PropertyName = propertyName,
                    IsEnum = propertyType.ReturnedClass.IsEnum,
                    Type = propertyType.ReturnedClass,
                    IsEntity = isEntity,
                }
            );
        }

        // Track unique rows if needed (using primary key)
        var uniqueRows = new HashSet<TPrimaryKey>();

        // Process entities
        foreach (var entity in entities)
        {
            if (entity == null)
                continue;

            // Check for duplicate entities (optional)
            if (uniqueRows.Contains(entity.Id))
            {
                continue; // Skip duplicate entities
            }

            uniqueRows.Add(entity.Id);

            var row = entityTable.NewRow();

            // Set ID
            if (!string.IsNullOrEmpty(identifierColumnNames))
            {
                row[identifierColumnNames] = entity.Id ?? (object)DBNull.Value;
            }

            // Set property values
            foreach (var property in persistedProperties)
            {
                object value = classMapping.GetPropertyValue(entity, property.PropertyName);

                if (value == null)
                {
                    row[property.ColumnName] = DBNull.Value;
                }
                else if (property.IsEnum)
                {
                    row[property.ColumnName] = Enum.GetName(property.Type, value);
                }
                else if (property.IsEntity)
                {
                    if (value is IEntity<TPrimaryKey> entityValue)
                    {
                        row[property.ColumnName] = entityValue.Id;
                    }
                    else
                    {
                        row[property.ColumnName] = DBNull.Value;
                    }
                }
                else
                {
                    row[property.ColumnName] = Convert.ChangeType(value, property.Type);
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

    private class PropertyMappingInfo
    {
        public required string ColumnName { get; init; }
        public required string PropertyName { get; init; }
        public required bool IsEnum { get; init; }
        public required Type Type { get; init; }
        public required bool IsEntity { get; init; }
    }
}
