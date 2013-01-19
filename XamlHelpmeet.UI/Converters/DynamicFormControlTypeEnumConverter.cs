using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using XamlHelpmeet.UI.Enums;

namespace XamlHelpmeet.UI.Converters
{
	public class DynamicFormControlTypeEnumConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ((DynamicFormControlType)value).ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (DynamicFormControlType)Enum.Parse(typeof(DynamicFormControlType),
			                                          value.ToString());
		}

		#endregion
	}
}
