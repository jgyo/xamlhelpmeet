// file:	Converters\PropertyTypeNameConverter.cs
//
// summary:	Implements the property type name converter class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
 using XamlHelpmeet.Extensions;
using System.Globalization;

namespace XamlHelpmeet.UI.Converters
{
    /// <summary>
    ///     Property type name converter.
    /// </summary>
    /// <seealso cref="T:System.Windows.Data.IValueConverter"/>
	[ValueConversion(typeof(string), typeof(string))]
	public class PropertyTypeNameConverter
		: IValueConverter
	{
		#region IValueConverter Members

        /// <summary>
        ///     Converts a value.
        /// </summary>
        /// <seealso cref="M:System.Windows.Data.IValueConverter.Convert(object,Type,object,CultureInfo)"/>
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

        /// <summary>
        ///     Converts a value.
        /// </summary>
        /// <seealso cref="M:System.Windows.Data.IValueConverter.ConvertBack(object,Type,object,CultureInfo)"/>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException("Sorry; the ConvertBack member of PropertyTypeNameConverter is not implemented.");
		}

		#endregion
	}
}
