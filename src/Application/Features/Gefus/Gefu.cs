using System.Diagnostics.CodeAnalysis;
using Abp.Extensions;
using FlatFiles;
using FlatFiles.TypeMapping;
using JetBrains.Annotations;

namespace CouponPaymentSystem.Application.Features.Gefus;

public class Gefu
{
    private const string DateFormat = "yyyyMMdd";

    // private const string CurrencyFormat = "0;00";
    private const char ZeroFillCharacter = '0';

    [field: CanBeNull, MaybeNull]
    private static IFixedLengthTypeMapper<GefuHeader> HeaderMapper
    {
        get
        {
            if (field is not null)
                return field;
            var mapper = FixedLengthTypeMapper.Define<GefuHeader>();
            mapper.Property(x => x.RecordType, 1);
            mapper
                .Property(x => x.CreationDate, 8)
                .InputFormat(DateFormat)
                .OutputFormat(DateFormat);
            return field = mapper;
        }
    }

    [field: CanBeNull, MaybeNull]
    private static IFixedLengthTypeMapper<GefuEntry> EntryMapper
    {
        get
        {
            if (field is not null)
                return field;
            var mapper = FixedLengthTypeMapper.Define<GefuEntry>();
            mapper.Property(x => x.RecordType, 1);
            mapper.Property(x => x.TransactionType, 2);
            mapper.Property(
                x => x.AccountNumber,
                new Window(16) { FillCharacter = ZeroFillCharacter }
            );
            mapper.Property(x => x.BranchCode, new Window(4) { FillCharacter = ZeroFillCharacter });
            mapper.Property(
                x => x.TransactionCode,
                new Window(5) { FillCharacter = ZeroFillCharacter }
            );
            mapper
                .Property(x => x.TransactionDate, 8)
                .InputFormat(DateFormat)
                .OutputFormat(DateFormat);
            mapper.Property(x => x.DebitCreditFlag, 1);
            mapper.Property(x => x.ValueDate, 8).InputFormat(DateFormat).OutputFormat(DateFormat);
            mapper.Property(
                x => x.TransactionCurrency,
                new Window(5) { FillCharacter = ZeroFillCharacter }
            );
            mapper.Property(
                x => x.AmountInLocalCurrency,
                new Window(14) { FillCharacter = ZeroFillCharacter }
            );
            mapper.Property(
                x => x.AmountInTransactionCurrency,
                new Window(14) { FillCharacter = ZeroFillCharacter }
            );
            mapper.Property(x => x.ConversionRate, 8);
            mapper.Property(x => x.ReferenceNumber, 40);
            mapper.Property(x => x.DocumentNumber, 12);
            mapper.Property(x => x.TransactionDescription, 40);
            mapper.Property(x => x.BeneficiaryIc, 16);
            mapper.Property(x => x.BeneficiaryName, 120);
            mapper.Property(x => x.BeneficiaryAddress1, 35);
            mapper.Property(x => x.BeneficiaryAddress2, 35);
            mapper.Property(x => x.BeneficiaryAddress3, 35);
            mapper.Property(x => x.City, 35);
            mapper.Property(x => x.State, 35);
            mapper.Property(x => x.Country, 35);
            mapper.Property(x => x.ZipCode, 35);
            mapper.Property(x => x.OptionsFlag, 2);
            mapper.Property(x => x.IssueCode, 5);
            mapper.Property(x => x.PayableBranch, 4);
            mapper.Property(x => x.FutureDateFlag, 1);
            mapper.Property(x => x.MisCode, 16);
            mapper.Property(x => x.ProcessingFlag, 1);
            mapper.Property(x => x.ReasonFlag, 2);
            mapper.Property(x => x.Udt1, 60);
            mapper.Property(x => x.Udt2, 60);
            mapper.Property(x => x.Udt3, 60);
            mapper.Property(x => x.Udt4, 60);
            mapper.Property(x => x.Udt5, 60);
            mapper.Property(x => x.Udt6, 60);
            mapper.Property(x => x.Udt7, 60);
            mapper.Property(x => x.Udt8, 60);
            mapper.Property(x => x.Udt9, 60);
            mapper.Property(x => x.Udt10, 60);
            mapper.Property(x => x.Udt11, 60);
            mapper.Property(x => x.Udt12, 60);
            mapper.Property(x => x.Udt13, 60);
            return field = mapper;
        }
    }

    [field: CanBeNull, MaybeNull]
    private static IFixedLengthTypeMapper<GefuFooter> FooterMapper
    {
        get
        {
            if (field is not null)
                return field;
            var mapper = FixedLengthTypeMapper.Define<GefuFooter>();
            mapper.Property(x => x.RecordType, 1);
            mapper.Property(
                x => x.NumberOfDebits,
                new Window(9) { FillCharacter = ZeroFillCharacter }
            );
            mapper.Property(
                x => x.TotalDebitAmountInLocalCurrency,
                new Window(15) { FillCharacter = ZeroFillCharacter }
            );
            mapper.Property(
                x => x.NumberOfCredits,
                new Window(9) { FillCharacter = ZeroFillCharacter }
            );
            mapper.Property(
                x => x.TotalCreditAmountInLocalCurrency,
                new Window(15) { FillCharacter = ZeroFillCharacter }
            );
            return field = mapper;
        }
    }

    private Gefu(GefuHeader header, IReadOnlyList<GefuEntry> entries, GefuFooter footer)
    {
        Header = header;
        Entries = entries;
        Footer = footer;
    }

    public GefuHeader Header { get; }
    public IReadOnlyList<GefuEntry> Entries { get; }
    public GefuFooter Footer { get; }

    public string Build()
    {
        var injector = new FixedLengthTypeMapperInjector();
        injector.WithDefault(EntryMapper);
        injector.When<GefuHeader>().Use(HeaderMapper);
        injector.When<GefuFooter>().Use(FooterMapper);

        using var stringWriter = new StringWriter();
        var writer = injector.GetWriter(stringWriter);
        writer.Write(Header);

        foreach (var entry in Entries)
        {
            writer.Write(entry);
        }

        writer.Write(Footer);
        return stringWriter.ToString();
    }

    public override string ToString() => Build();

    public static Gefu Parse(string content) => Parse(content.GetBytes());

    public static Gefu Parse(byte[] content)
    {
        using var stream = new MemoryStream(content);
        return Parse(stream);
    }

    public static Gefu Parse(Stream stream)
    {
        var selector = new FixedLengthTypeMapperSelector();
        selector.WithDefault(EntryMapper);
        selector.When(x => x.Length == 9).Use(HeaderMapper);
        selector.When(x => x.Length == 49).Use(FooterMapper);

        using var streamReader = new StreamReader(stream);
        var reader = selector.GetReader(
            streamReader,
            new FixedLengthOptions { HasRecordSeparator = false, IsFirstRecordHeader = false }
        );
        GefuHeader header = null!;
        List<GefuEntry> entries = [];
        GefuFooter footer = null!;
        while (reader.Read())
        {
            var current = reader.Current;
            switch (current)
            {
                case GefuHeader:
                    header = current.As<GefuHeader>();
                    continue;
                case GefuEntry:
                    entries.Add(current.As<GefuEntry>());
                    continue;
                case GefuFooter:
                    footer = current.As<GefuFooter>();
                    continue;
                default:
                    throw new InvalidCastException("Invalid Gefu");
            }
        }

        return new Gefu(header, entries.AsReadOnly(), footer);
    }
}
