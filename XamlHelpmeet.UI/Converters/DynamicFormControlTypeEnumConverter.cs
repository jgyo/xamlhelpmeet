// file:	Converters\DynamicFormControlTypeEnumConverter.cs
//
// summary:	Implements the dynamic form control type enum converter class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using XamlHelpmeet.UI.Enums;

namespace XamlHelpmeet.UI.Converters
{
    /// <summary>
    ///     Dynamic form control type enum converter.
    /// </summary>
    /// <seealso cref="T:System.Windows.Data.IValueConverter"/>
    [ValueConversion(typeof(DynamicFormControlType), typeof(string))]
	public class DynamicFormControlTypeEnumConverter
		: IValueConverter
	{
		#region IValueConverter Members

        /// <summary>
        ///     Converts a DynamicFormControlType value to a string.
        /// </summary>
        /// <seealso cref="M:System.Windows.Data.IValueConverter.Convert(object,Type,object,System.Globalization.CultureInfo)"/>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ((DynamicFormControlType)value).ToString();
		}

        /// <summary>
        ///     Converts a string value to a DynamicFormControlType value.
        /// </summary>
        /// <seealso cref="M:System.Windows.Data.IValueConverter.ConvertBack(object,Type,object,System.Globalization.CultureInfo)"/>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (DynamicFormControlType)Enum.Parse(typeof(DynamicFormControlType),
			                                          value.ToString());
		}

		#endregion
	}
}
