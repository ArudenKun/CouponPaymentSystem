using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Transactions;

[Mapper]
public static partial class TransactionMapper
{
    [MapperIgnoreTarget(nameof(TransactionDto.Age))]
    public static partial TransactionDto ToDto(this Transaction transaction);
}
