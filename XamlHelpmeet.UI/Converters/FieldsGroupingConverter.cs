using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Globalization;

namespace XamlHelpmeet.UI.Converters
{
	public class FieldsGroupingConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.ToString().ToLower() == "false" ? "Not Used." : "Used";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException("Sorry; the ConvertBack member of FieldsGroupingConverter is not implemented.");
		}

		#endregion
	}
}
