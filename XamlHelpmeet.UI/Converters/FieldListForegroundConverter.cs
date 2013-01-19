using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Globalization;

namespace XamlHelpmeet.UI.Converters
{
	public class FieldListForegroundConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null && value == (object)true ? "Maroon" : "Black";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException("Sorry; the ConvertBack member of FieldListForegroundConverter is not implemented.");
		}

		#endregion
	}
}
