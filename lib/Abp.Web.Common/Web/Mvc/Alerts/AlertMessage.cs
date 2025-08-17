namespace Abp.Web.Mvc.Alerts;

public class AlertMessage
{
    public string Text
    {
        get;
        set => field = Check.NotNullOrWhiteSpace(value, nameof(value));
    }

    public AlertType Type { get; set; }

    public string? Title { get; set; }

    public bool Dismissible { get; set; }

    public string DisplayType { get; set; }

    public AlertMessage(
        AlertType type,
        string text,
        string? title = null,
        bool dismissible = true,
        string? displayType = null
    )
    {
        Type = type;
        Text = Check.NotNullOrWhiteSpace(text, nameof(text));
        Title = title;
        Dismissible = dismissible;
        DisplayType = displayType ?? AlertDisplayType.PageAlert;
    }
}
