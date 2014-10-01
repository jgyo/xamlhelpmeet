using System.Windows.Controls;
using XamlHelpmeet.Extensions;

namespace XamlHelpmeet.UI.ValidationRules
{
using NLog;

using YoderZone.Extensions.NLog;

public class RequiredEntryValidationRule : ValidationRule
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
    {
        if (value == null || value.ToString().IsNullOrEmpty())
        {
            return new ValidationResult(false, "This is a required entry field.");
        }

        return ValidationResult.ValidResult;
    }
}
}