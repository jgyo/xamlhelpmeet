// file:    Converters\ControlTypeEnumConverter.cs
//
// summary: Implements the ControlType converter class
using System;
using System.Windows.Data;
using XamlHelpmeet.Model;

namespace XamlHelpmeet.UI.Converters
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
///     ControlType enum converter.
/// </summary>
/// <remarks>
/// Converts the ControlType enumeration into string values matching the
/// enumeration names.
/// </remarks>
/// <seealso cref="T:System.Windows.Data.IValueConverter"/>
[ValueConversion(typeof(ControlType), typeof(string))]
public class ControlTypeEnumConverter : IValueConverter
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    #region IValueConverter Members

    /// <summary>
    ///     Converts a ControlType value into a string.
    /// </summary>
    /// <seealso cref="M:System.Windows.Data.IValueConverter.Convert(object,Type,object,System.Globalization.CultureInfo)"/>
    public object Convert(object value, Type targetType, object parameter,
                          System.Globalization.CultureInfo culture)
    {
        var controlType = (ControlType)value;
        return controlType.ToString();
    }

    /// <summary>
    ///     Converts a string value into an enum value of type ControlType.
    /// </summary>
    /// <seealso cref="M:System.Windows.Data.IValueConverter.ConvertBack(object,Type,object,System.Globalization.CultureInfo)"/>
    public object ConvertBack(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
    {
        var typeString = value.ToString();
        return Enum.Parse(typeof(ControlType), typeString);
    }

    #endregion
}
}