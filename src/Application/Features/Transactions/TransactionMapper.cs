using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Transactions;

[Mapper]
public static partial class TransactionMapper
{
    // [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    // public static partial TransactionDataTableResponse MapToResponseDto(
    //     this Transaction transaction
    // );
}
