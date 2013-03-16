using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using XamlHelpmeet.Extensions;
using System.Globalization;

namespace XamlHelpmeet.UI.ValidationRules
{
	public class PositiveIntegerValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (((string)value).IsNullOrEmpty())
			{
				return ValidationResult.ValidResult;
			}

			var test = -1;

			if (!int.TryParse(value.ToString(), out test) || test < 0)
			{
				return new ValidationResult(false, "Value must be non-negative interger.");
			}

			return ValidationResult.ValidResult;
		}
	}
}
