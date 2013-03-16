using System.Windows.Controls;
using XamlHelpmeet.Extensions;

namespace XamlHelpmeet.UI.ValidationRules
{
	public class RequiredEntryValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			if (value == null || value.ToString().IsNullOrEmpty())
			{
				return new ValidationResult(false, "This is a required entry field.");
			}

			return ValidationResult.ValidResult;
		}
	}
}