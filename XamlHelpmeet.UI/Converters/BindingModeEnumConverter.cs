using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace XamlHelpmeet.UI.Converters
{
	[ValueConversion(typeof(BindingMode), typeof(string))]
	public class BindingModeEnumConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ((BindingMode)value).ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return Enum.Parse(typeof(BindingMode),
			                  value.ToString());
		}

		#endregion
	}
}
