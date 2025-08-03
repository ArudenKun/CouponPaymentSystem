using Abp.Dapper.DapperExtensions.Sql;

namespace Abp.Dapper.DapperExtensions.Predicate;

public interface IPredicate
{
    string GetSql(
        ISqlGenerator sqlGenerator,
        IDictionary<string, object> parameters,
        bool isDml = false
    );
}
