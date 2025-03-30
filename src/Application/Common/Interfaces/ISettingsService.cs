namespace Application.Common.Interfaces;

public interface ISettingsService
{
    int FileSize { get; }
    ICollection<string> FileExtensions { get; }
}
