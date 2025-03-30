using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Transactions;

[Mapper]
public static partial class TransactionMapper
{
    public static partial TransactionDto MapToDto(this Transaction transaction);
}
