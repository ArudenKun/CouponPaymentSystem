using Application.Common;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Users;

[Mapper]
public static partial class UserMapper
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial User MapToUser(this AppPrincipal appPrincipal);
}
