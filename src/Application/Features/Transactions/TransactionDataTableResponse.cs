using System.Collections;

namespace Application.Features.Transactions;

public class TransactionDataTableResponse
{
    public required long RecordsTotal { get; init; }
    public required long RecordsFiltered { get; init; }
    public required IEnumerable Data { get; init; }
}
