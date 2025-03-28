using Ardalis.SmartEnum;

namespace Domain.Common.Enums;

public class Document : SmartEnum<Document>
{
    public static readonly Document Match = new(nameof(Match), 0);
    public static readonly Document Mismatch = new(nameof(Mismatch), 1);

    protected Document(string name, int value)
        : base(name, value) { }
}
