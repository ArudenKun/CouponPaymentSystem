using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using Abp.Dependency;
using Abp.Runtime;
using LinqToDB.Common;
using LinqToDB.Interceptors;

namespace Abp.LinqToDb.Interceptors;

public class WithNoLockInterceptor : CommandInterceptor, ITransientDependency
{
    private const string InterceptionContextKey = "Abp.LinqToDb.Interceptors.WithNolockInterceptor";

    private static readonly Regex TableAliasRegex = new Regex(
        @"(?<tableAlias>AS \[Extent\d+\](?! WITH \(NOLOCK\)))",
        RegexOptions.Multiline | RegexOptions.IgnoreCase
    );

    private readonly IAmbientScopeProvider<InterceptionContext> _interceptionScopeProvider;

    public WithNoLockInterceptor(
        IAmbientScopeProvider<InterceptionContext> interceptionScopeProvider
    )
    {
        _interceptionScopeProvider = interceptionScopeProvider;
    }

    public InterceptionContext? NolockingContext =>
        _interceptionScopeProvider.GetValue(InterceptionContextKey);

    public override Option<object?> ExecuteScalar(
        CommandEventData eventData,
        DbCommand command,
        Option<object?> result
    )
    {
        if (NolockingContext?.UseNoLocking ?? false)
        {
            command.CommandText = TableAliasRegex.Replace(
                command.CommandText,
                "${tableAlias} WITH (NOLOCK)"
            );
            NolockingContext.CommandText = command.CommandText;
        }

        return base.ExecuteScalar(eventData, command, result);
    }

    public override Option<DbDataReader> ExecuteReader(
        CommandEventData eventData,
        DbCommand command,
        CommandBehavior commandBehavior,
        Option<DbDataReader> result
    )
    {
        if (NolockingContext?.UseNoLocking ?? false)
        {
            command.CommandText = TableAliasRegex.Replace(
                command.CommandText,
                "${tableAlias} WITH (NOLOCK)"
            );
            NolockingContext.CommandText = command.CommandText;
        }

        return base.ExecuteReader(eventData, command, commandBehavior, result);
    }

    public IDisposable UseNoLocking() =>
        _interceptionScopeProvider.BeginScope(
            InterceptionContextKey,
            new InterceptionContext(string.Empty, true)
        );

    public sealed class InterceptionContext
    {
        public InterceptionContext(string commandText, bool useNoLocking)
        {
            CommandText = commandText;
            UseNoLocking = useNoLocking;
        }

        public string CommandText { get; set; }

        public bool UseNoLocking { get; set; }
    }
}
