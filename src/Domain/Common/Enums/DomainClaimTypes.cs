using Ardalis.SmartEnum;

namespace Domain.Common.Enums;

public class DomainClaimTypes : SmartEnum<DomainClaimTypes, string>
{
    public static readonly DomainClaimTypes SessionId = new(nameof(SessionId), nameof(SessionId));

    protected DomainClaimTypes(string name, string value)
        : base(name, value) { }
}
