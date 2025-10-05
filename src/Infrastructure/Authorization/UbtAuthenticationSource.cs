using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using Abp;
using Abp.Authorization.Users;
using Dapper;
using Domain.Configuration;
using Domain.Tenants;
using Domain.Users;

namespace Infrastructure.Authorization;

internal class UbtAuthenticationSource : DefaultExternalAuthenticationSource<Tenant, User>
{
    private static readonly ConcurrentDictionary<string, long> Sessions = new();

    private readonly CpsOptions _options;

    public UbtAuthenticationSource(CpsOptions options)
    {
        _options = options;
    }

    public override string Name => "UBT";

    /// <inheritdoc/>
    public override async Task<bool> TryAuthenticateAsync(string sessionId, string _, Tenant tenant)
    {
        if (string.IsNullOrEmpty(sessionId))
            return false;

        if (!long.TryParse(sessionId, out var sid))
            return false;

        using var conn = await CreateUbtConnectionAsync();
        var result = await conn.QueryFirstOrDefaultDictionaryAsync(
            "sp_CheckSessionLoginIsValid",
            new { Session_ID = sid, sys_ID = _options.SysId },
            commandType: CommandType.StoredProcedure
        );

        if (result is null)
            return false;

        var authResult = new UbtAuthResult(
            (int)result.ElementAtOrDefault(0).Value,
            result.ElementAtOrDefault(1).Value as string ?? ""
        );

        var ubtContext = await CreateUbtContextAsync(sid);
        if (ubtContext is null)
            return false;

        Sessions[ubtContext.DomainId] = sid;
        return authResult.Code switch
        {
            3 => true,
            _ => false,
        };
    }

    /// <inheritdoc/>
    public override async Task<User> CreateUserAsync(string sessionId, Tenant tenant)
    {
        if (!int.TryParse(sessionId, out var sid))
        {
            throw new AbpException("Invalid Session");
        }

        var ubtContext = await CreateUbtContextAsync(sid);
        if (ubtContext is null)
        {
            throw new AbpException($"Invalid Session {sid}");
        }

        var user = await base.CreateUserAsync(ubtContext.DomainId, tenant);
        if (user is null)
        {
            throw new AbpException("Failed to create user:" + ubtContext.DomainId);
        }

        user.UserName = ubtContext.DomainId;
        user.Name = ubtContext.FirstName;
        user.Surname = ubtContext.LastName;
        user.TenantId = ubtContext.DivisionId;
        user.IsEmailConfirmed = true;
        user.IsActive = true;
        return user;
    }

    public override async Task UpdateUserAsync(User user, Tenant tenant)
    {
        var sid = Sessions[user.UserName];
        var ubtContext = await CreateUbtContextAsync(sid);
        if (ubtContext is null)
        {
            throw new AbpException("Failed to update: " + user.UserName);
        }

        user.UserName = ubtContext.DomainId;
        user.Name = ubtContext.FirstName;
        user.Surname = ubtContext.LastName;
        user.TenantId = ubtContext.DivisionId;
    }

    protected virtual async Task<UbtContext?> CreateUbtContextAsync(long sessionId)
    {
        using var connection = await CreateUbtConnectionAsync();
        var user = await connection.QueryFirstOrDefaultAsync(
            "sp_GetUserCredentials",
            new { sessionID = sessionId },
            commandType: CommandType.StoredProcedure
        );
        var division = await connection.QueryFirstOrDefaultAsync(
            "sp_GetUserDivision",
            new { sessionID = sessionId },
            commandType: CommandType.StoredProcedure
        );

        if (user is null || division is null)
            return null;

        return new UbtContext(
            user.usr_UserID,
            user.usr_Fname,
            user.userLname,
            user.App_Role_ID,
            user.App_Role_Name,
            division.usr_DivID,
            division.Division_Name
        );
    }

    private async Task<SqlConnection> CreateUbtConnectionAsync()
    {
        var conn = new SqlConnection(_options.Aso.ConnectionString);
        await conn.OpenAsync();
        return conn;
    }
}

file static class Extensions
{
    public static async Task<IDictionary<string, object>?> QueryFirstOrDefaultDictionaryAsync(
        this IDbConnection conn,
        string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        var result = await conn.QueryAsync(sql, param, transaction, commandTimeout, commandType);
        return result.OfType<IDictionary<string, object>>().FirstOrDefault();
    }
}
