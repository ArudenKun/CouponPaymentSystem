using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using Abp;
using Abp.Authorization.Users;
using Abp.Dependency;
using Dapper;
using Domain.Configuration;
using Domain.Tenants;
using Domain.Users;

namespace Infrastructure.Authorization;

internal class UbtAuthenticationSource : DefaultExternalAuthenticationSource<Tenant, User>
{
    private static readonly ConcurrentDictionary<string, string> SessionCache = new();

    private readonly CpsOptions _options;
    private readonly IIocResolver _iocResolver;

    public UbtAuthenticationSource(CpsOptions options, IIocResolver iocResolver)
    {
        _options = options;
        _iocResolver = iocResolver;
    }

    public override string Name => "UBT";


    /// <inheritdoc/>
    public override async Task<bool> TryAuthenticateAsync(
        string sessionId,
        string _,
        Tenant tenant
    )
    {
        if (string.IsNullOrEmpty(sessionId))
            return false;

        using var conn = await CreateUbtConnectionAsync();
        var result = await conn.QueryFirstOrDefaultDictionaryAsync("sp_CheckSessionLoginIsValid",
            new { Session_ID = sessionId, sys_ID = _options.SysId }, commandType: CommandType.StoredProcedure);

        if (result is null)
            return false;
        
        
    }

    /// <inheritdoc/>
    public override async Task<User> CreateUserAsync(string userNameOrEmailAddress, Tenant tenant)
    {
        var user = await base.CreateUserAsync(userNameOrEmailAddress, tenant);
        var ubtContext = await CreateUbtContextAsync(userNameOrEmailAddress);
        if (ubtContext == null)
        {
            throw new AbpException(
                "Failed to fetch user details for session:" + userNameOrEmailAddress
            );
        }

        UpdateUserFromPrincipal(user);

        user.IsEmailConfirmed = true;
        user.IsActive = true;

        return user;
    }

    public override async Task UpdateUserAsync(User user, Tenant tenant)
    {
        var principalContext = await CreateUbtContextAsync(tenant, user);
        using ()
        {
            var userPrincipal = FindUserPrincipalByIdentity(principalContext, user.UserName);

            if (userPrincipal == null)
            {
                throw new AbpException("Unknown LDAP user: " + user.UserName);
            }

            UpdateUserFromPrincipal(user, userPrincipal);
        }
    }

    protected virtual void UpdateUserFromPrincipal(User user, UbtContext context)
    {
        user.UserName = context.DomainId;
        user.Name = context.FirstName;
        user.Surname = context.LastName;
    }

    protected virtual async Task<UbtContext?> CreateUbtContextAsync(string sessionId)
    {
        return new UbtContext(
            contextType,
            ConvertToNullIfEmpty(await _settings.GetDomain(tenant?.Id)),
            ConvertToNullIfEmpty(await _settings.GetContainer(tenant?.Id)),
            options,
            ConvertToNullIfEmpty(await _settings.GetUserName(tenant?.Id)),
            ConvertToNullIfEmpty(await _settings.GetPassword(tenant?.Id))
        );
    }

    private async Task<SqlConnection> CreateUbtConnectionAsync()
    {
        var conn = new SqlConnection(_options.ConnectionString);
        await conn.OpenAsync();
        return conn;
    }
}

file static class Extensions
{
    public static async Task<IDictionary<string, object>?> QueryFirstOrDefaultDictionaryAsync(this IDbConnection conn,
        string sql,
        object? param = null, IDbTransaction? transaction = null, bool buffered = true, int? commandTimeout = null,
        CommandType? commandType = null)
    {
        var result = await conn.QueryAsync(sql, param, transaction, commandTimeout, commandType);
        return result.OfType<IDictionary<string, object>>().FirstOrDefault();
    }
}