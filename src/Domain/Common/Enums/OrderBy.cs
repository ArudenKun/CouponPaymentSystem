using Ardalis.SmartEnum;

namespace Domain.Common.Enums;

public class OrderBy : SmartEnum<OrderBy, string>
{
    public static readonly OrderBy Ascending = new OrderBy(nameof(Ascending), "ASC");
    public static readonly OrderBy Descending = new OrderBy(nameof(Descending), "DESC");

    protected OrderBy(string name, string value)
        : base(name, value) { }
}
