namespace XamlHelpmeet.Utility
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
///     Xaml collection helper.
/// </summary>
public static class XamlCollectionHelper
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    ///     An XamlCollection extension method that queries if 'target' is
    ///     empty.
    /// </summary>
    /// <param name="target">
    ///     The target to act on.
    /// </param>
    /// <returns>
    ///     true if empty, otherwise false.
    /// </returns>
    public static bool IsEmpty(this XamlNodeCollection target)
    {
        logger.Debug("Entered member.");

        return target == null || target.Count == 0;
    }
}
}