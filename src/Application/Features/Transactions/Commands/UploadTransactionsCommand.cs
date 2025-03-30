using System.Web;
using Application.Common.Interfaces;
using Application.Common.Validators;
using Domain.Common.Enums;
using FluentValidation;

namespace Application.Features.Transactions.Commands;

public sealed class UploadTransactionsCommand : IAppRequest<TransactionDataTableResponse>
{
    public UploadTransactionsCommand(HttpPostedFileBase file, string currency)
    {
        File = file;
        Currency = currency;
    }

    public HttpPostedFileBase File { get; }
    public string Currency { get; }
}

public sealed class UploadTransactionsCommandValidator
    : AbstractValidator<UploadTransactionsCommand>
{
    public UploadTransactionsCommandValidator(ISettingsService settingsService)
    {
        RuleFor(p => p.File)
            .NotNull()
            .WithMessage("File cannot be null")
            .SetValidator(
                new FileValidator(
                    settingsService.FileSize,
                    settingsService.FileExtensions.ToArray()
                )
            );

        RuleFor(p => p.Currency)
            .NotNull()
            .WithMessage("Currency cannot be null")
            .Must(p => Currency.TryFromName(p, out _))
            .WithMessage(p => $"{p.Currency} is invalid currency");
    }
}
