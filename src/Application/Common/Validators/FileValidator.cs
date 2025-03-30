using System.Web;
using FluentValidation;
using Humanizer;
using MimeMapping;

namespace Application.Common.Validators;

public class FileValidator : AbstractValidator<HttpPostedFileBase>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="files">The file extensions (ex: "zip"), the file name, or file path</param>
    public FileValidator(params string[] files)
        : this(4, files) { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="size">The maximum file size allowed</param>
    /// <param name="files">The file extensions (ex: "zip"), the file name, or file path</param>
    public FileValidator(int size, params string[] files)
    {
        var mb = size.Megabytes();
        RuleFor(p => p.ContentLength)
            .NotEmpty()
            .LessThanOrEqualTo((int)mb.Bytes)
            .WithMessage($"Only {mb.Humanize()} is allowed");

        var mimeTypes = files.Select(MimeUtility.GetMimeMapping);

        RuleFor(p => p.ContentType)
            .NotEmpty()
            .Must(files.Contains)
            .WithMessage(
                $"Only {string.Join(", ", mimeTypes.Select(s => $".{MimeUtility.GetExtensions(s)}"))} file types are allowed)"
            );
    }
}
