// file:    Converters\FieldListForegroundConverter.cs
//
// summary: Implements the field list foreground converter class
using System;
using System.Windows.Data;
using System.Globalization;

namespace XamlHelpmeet.UI.Converters
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
///     Field list foreground converter.
/// </summary>
/// <seealso cref="T:System.Windows.Data.IValueConverter"/>
[ValueConversion(typeof(bool), typeof(string))]
public class FieldListForegroundConverter : IValueConverter
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    #region IValueConverter Members

    /// <summary>
    ///     Converts a bool value to a string of either "Maroon" (if true) or "Black" (if false).
    /// </summary>
    /// <seealso cref="M:System.Windows.Data.IValueConverter.Convert(object,Type,object,CultureInfo)"/>
    public object Convert(object value, Type targetType, object parameter,
                          CultureInfo culture)
    {
        return value != null && value == (object)true ? "Maroon" : "Black";
    }

    /// <summary>
    ///     This method is not implemented.
    /// </summary>
    /// <seealso cref="M:System.Windows.Data.IValueConverter.ConvertBack(object,Type,object,CultureInfo)"/>
    public object ConvertBack(object value, Type targetType, object parameter,
                              CultureInfo culture)
    {
        throw new NotImplementedException("Sorry; the ConvertBack member of FieldListForegroundConverter is not implemented.");
    }

    #endregion
}
}
