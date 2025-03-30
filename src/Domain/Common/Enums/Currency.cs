using Ardalis.SmartEnum;

namespace Domain.Common.Enums;

public abstract class Currency : SmartEnum<Currency, string>
{
    public static readonly Currency Peso = new PesoType();

    public static readonly Currency Dollar = new DollarType();

    private Currency(string name)
        : base(name, name) { }

    private class PesoType : Currency
    {
        internal PesoType()
            : base(nameof(Peso)) { }
    }

    private class DollarType : Currency
    {
        internal DollarType()
            : base(nameof(Dollar)) { }
    }
}
