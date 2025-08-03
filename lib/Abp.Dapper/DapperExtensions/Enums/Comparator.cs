using System.ComponentModel;

namespace Abp.Dapper.DapperExtensions.Enums;

public enum Comparator
{
    [Description("=")]
    Equal,

    [Description("!=")]
    NotEqual,

    [Description("<")]
    LessThan,

    [Description(">")]
    GreaterThan,

    [Description("<=")]
    LessThanOrEqual,

    [Description(">=")]
    GreaterThanOrEqual,
}
