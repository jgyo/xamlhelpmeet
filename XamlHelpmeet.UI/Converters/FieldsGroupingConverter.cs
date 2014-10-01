// file:    Converters\FieldsGroupingConverter.cs
//
// summary: Implements the fields grouping converter class
using System;
using System.Windows.Data;
using System.Globalization;

namespace XamlHelpmeet.UI.Converters
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
///     Fields grouping converter.
/// </summary>
/// <seealso cref="T:System.Windows.Data.IValueConverter"/>
[ValueConversion(typeof(string), typeof(string))]
public class FieldsGroupingConverter : IValueConverter
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    #region IValueConverter Members

    /// <summary>
    ///     Converts a string value of "false" to "Not Used". Otherwise returns "Used".
    /// </summary>
    /// <seealso cref="M:System.Windows.Data.IValueConverter.Convert(object,Type,object,CultureInfo)"/>
    public object Convert(object value, Type targetType, object parameter,
                          CultureInfo culture)
    {
        return value.ToString().ToLower() == "false" ? "Not Used" : "Used";
    }

    /// <summary>
    ///     Not implemented.
    /// </summary>
    /// <seealso cref="M:System.Windows.Data.IValueConverter.ConvertBack(object,Type,object,CultureInfo)"/>
    public object ConvertBack(object value, Type targetType, object parameter,
                              CultureInfo culture)
    {
        throw new NotImplementedException("Sorry; the ConvertBack member of FieldsGroupingConverter is not implemented.");
    }

    #endregion
}
}
