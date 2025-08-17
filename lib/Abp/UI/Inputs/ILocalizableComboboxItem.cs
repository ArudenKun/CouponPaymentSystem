using System.Text.Json.Serialization;
using Abp.Localization;

namespace Abp.UI.Inputs;

public interface ILocalizableComboboxItem
{
    string Value { get; set; }

    [JsonConverter(typeof(LocalizableStringToStringJsonConverter))]
    ILocalizableString DisplayText { get; set; }
}
