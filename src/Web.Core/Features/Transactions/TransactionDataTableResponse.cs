using System.Collections;

namespace Web.Core.Features.Transactions;

public sealed class TransactionDataTableResponse
{
    public required int TotalRecords { get; init; }
    public required int TotalRecordsFiltered { get; init; }
    public required IEnumerable Data { get; init; }
}
