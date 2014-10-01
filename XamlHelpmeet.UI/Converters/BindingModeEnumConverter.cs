// file:    Converters\BindingModeEnumConverter.cs
//
// summary: Implements the binding mode enum converter class
using System;
using System.Windows.Data;

namespace XamlHelpmeet.UI.Converters
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
///     Binding mode enum converter.
/// </summary>
/// <seealso cref="T:System.Windows.Data.IValueConverter"/>
[ValueConversion(typeof(BindingMode), typeof(string))]
public class BindingModeEnumConverter
    : IValueConverter
{
    /// <summary>
    /// The logger.
    /// </summary>
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    #region IValueConverter Members

    /// <summary>
    ///     Converts a BindingMode value to a string.
    /// </summary>
    /// <seealso cref="M:System.Windows.Data.IValueConverter.Convert(object,Type,object,System.Globalization.CultureInfo)"/>
    public object Convert(object value, Type targetType, object parameter,
                          System.Globalization.CultureInfo culture)
    {
        return ((BindingMode)value).ToString();
    }

    /// <summary>
    ///     Converts a string value to a BindingMode enumeration.
    /// </summary>
    /// <seealso cref="M:System.Windows.Data.IValueConverter.ConvertBack(object,Type,object,System.Globalization.CultureInfo)"/>
    public object ConvertBack(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
    {
        return Enum.Parse(typeof(BindingMode),
                          value.ToString());
    }

    #endregion
}
}
