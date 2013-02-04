// file:	Converters\ControlTypeEnumConverter.cs
//
// summary:	Implements the ControlType converter class
using System;
using System.Windows.Data;
using XamlHelpmeet.Model;

namespace XamlHelpmeet.UI.Converters
{
    /// <summary>
    ///     ControlType enum converter.
    /// </summary>
    /// <seealso cref="T:System.Windows.Data.IValueConverter"/>
    [ValueConversion(typeof(ControlType), typeof(string))]
    public class ControlTypeEnumConverterWithSelect : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        ///     Converts a ControlType value into a string.
        /// </summary>
        /// <remarks>Converts the ControlType enumeration into strings matching
        ///          the enumeration names, except for None which returns Select.</remarks>
        /// <seealso cref="M:System.Windows.Data.IValueConverter.Convert(object,Type,object,System.Globalization.CultureInfo)"/>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var controlType = (ControlType)value;
            if (controlType == ControlType.None)
                return "Select";
            return controlType.ToString();
        }

        /// <summary>
        ///     Converts a string value into an enum value of type ControlType.
        /// </summary>
        /// <seealso cref="M:System.Windows.Data.IValueConverter.ConvertBack(object,Type,object,System.Globalization.CultureInfo)"/>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var typeString = value.ToString();
            if (typeString == "Select")
                return ControlType.None;
            return Enum.Parse(typeof(ControlType), typeString);
        }

        #endregion
    }
}