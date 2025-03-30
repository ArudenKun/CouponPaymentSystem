using Application.Common;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Features.Users;

[Mapper]
public static partial class UserMapper
{
    [MapperIgnoreSource(nameof(AppPrincipal.SessionId))]
    [MapperIgnoreSource(nameof(AppPrincipal.Claims))]
    [MapperIgnoreSource(nameof(AppPrincipal.Identities))]
    [MapperIgnoreSource(nameof(AppPrincipal.Identity))]
    public static partial User MapToUser(this AppPrincipal appPrincipal);
}
