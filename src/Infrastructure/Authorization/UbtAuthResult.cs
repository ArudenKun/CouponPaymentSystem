namespace Infrastructure.Authorization;

internal sealed record UbtAuthResult(int Code, string Message = "");
