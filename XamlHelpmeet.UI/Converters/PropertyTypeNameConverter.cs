using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using XamlHelpmeet.Extentions;
using System.Globalization;

namespace XamlHelpmeet.UI.Converters
{
	[ValueConversion(typeof(string), typeof(string))]
	public class PropertyTypeNameConverter
		: IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value,
							  Type targetType,
							  object parameter,
							  CultureInfo culture)
		{
			if (value == null || value.ToString().IsNullOrEmpty())
			{
				return string.Empty;
			}
			return String.Format("Data Type - {0}       ",
								 value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException("Sorry; the ConvertBack member of PropertyTypeNameConverter is not implemented.");
		}

		#endregion
	}
}
