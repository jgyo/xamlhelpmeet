namespace XamlHelpmeet.Extensions
{
using System;
using System.Diagnostics.Contracts;

using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
///     String extensions.
/// </summary>
public static class StringExtensions
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    ///     A char extension method that queries if a character is letter or digit.
    /// </summary>
    /// <param name="target">
    ///     The target to act on.
    /// </param>
    /// <returns>
    ///     true if the character is letter or digit, otherwise false.
    /// </returns>
    public static bool IsLetterOrDigit(this char target)
    {
        logger.Debug("Entered member.");

        return char.IsLetterOrDigit(target);
    }

    /// <summary>
    ///     A string extension method that queries if a specified character in a string is
    ///     letter or digit.
    /// </summary>
    /// <param name="target">
    ///     The target to act on.
    /// </param>
    /// <param name="index">
    ///     Zero-based index of the character to test.
    /// </param>
    /// <returns>
    ///     true if the character is a letter or digit, otherwise false.
    /// </returns>
    public static bool IsLetterOrDigit(this string target, int index)
    {
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(target));
        Contract.Requires<ArgumentOutOfRangeException>(0 <= index);
        Contract.Requires<ArgumentOutOfRangeException>(index < target.Length);
        logger.Debug("Entered member.");
        logger.Trace("target: {0}", target);
        logger.Trace("index: {0}", index);

        return target[index].IsLetterOrDigit();
    }

    /// <summary>
    ///     A string extension method that query if a string is not null.
    /// </summary>
    /// <param name="target">
    ///     The Target to act on.
    /// </param>
    /// <returns>
    ///     true if not null, false if not.
    /// </returns>
    public static bool IsNotNull(this string target)
    {
        logger.Debug("Entered member.");

        return !target.IsNull();
    }

    /// <summary>
    ///     A string extension method that queries if a string is not null or empty.
    /// </summary>
    /// <param name="target">
    ///     The Target to act on.
    /// </param>
    /// <returns>
    ///     true if a not null or empty, false if it is null or empty.
    /// </returns>
    public static bool IsNotNullOrEmpty(this string target)
    {
        logger.Debug("Entered member.");

        return !target.IsNullOrEmpty();
    }

    /// <summary>
    ///     A string extension method that queries if the string is null, empty or whitespace.
    /// </summary>
    /// <param name="target">
    ///     The Target to act on.
    /// </param>
    /// <returns>
    ///     true if a null, empty, or whitespace, false if not.
    /// </returns>
    public static bool IsNotNullOrWhiteSpace(this string target)
    {
        logger.Debug("Entered member.");
        logger.Trace("Target: {0}", target);

        return !target.IsNullOrWhiteSpace();
    }

    /// <summary>
    ///     A char extension method that query if a character is not uppercase.
    /// </summary>
    /// <param name="c">
    ///     The character to test.
    /// </param>
    /// <returns>
    ///     true if not uppercase, otherwise false.
    /// </returns>
    public static bool IsNotUpper(this char c)
    {
        logger.Debug("Entered member.");

        return !c.IsUpper();
    }

    /// <summary>
    ///     A string extension method that queries if a specified character is not uppercase.
    /// </summary>
    /// <param name="target">
    ///     The target to act on.
    /// </param>
    /// <param name="index">
    ///     Zero-based index of the character to test.
    /// </param>
    /// <returns>
    ///     true if not uppercase, otherwise false.
    /// </returns>
    public static bool IsNotUpper(this string target,
                                  int index)
    {
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(target));
        Contract.Requires<ArgumentOutOfRangeException>(0 <= index);
        Contract.Requires<ArgumentOutOfRangeException>(index < target.Length);
        logger.Debug("Entered member.");
        logger.Trace("target: {0}", target);
        logger.Trace("index: {0}", index);

        return !target.IsUpper(index);
    }

    /// <summary>
    ///     A string extension method that queries if 'Target' is null.
    /// </summary>
    /// <param name="Target">
    ///     The Target to act on.
    /// </param>
    /// <returns>
    ///     true if null, otherwise false.
    /// </returns>
    public static bool IsNull(this string Target)
    {
        logger.Debug("Entered member.");

        return Target == null;
    }

    /// <summary>
    ///     A string extension method that queries if a null or is empty.
    /// </summary>
    /// <param name="target">
    ///     The target to act on.
    /// </param>
    /// <returns>
    ///     true if a null or is empty, otherwise false.
    /// </returns>
    public static bool IsNullOrEmpty(this string target)
    {
        logger.Debug("Entered member.");

        return string.IsNullOrEmpty(target);
    }

    /// <summary>
    ///     A string extension method that query if a string is is null.
    /// </summary>
    /// <summary>
    ///     A string extension method that query if a string is null, empty or white space.
    /// </summary>
    /// <param name="Target">
    ///     The Target to act on.
    /// </param>
    /// <returns>
    ///     true if null, false if not.
    /// </returns>
    /// <returns>
    ///     true if null or white space, false if not.
    /// </returns>
    public static bool IsNullOrWhiteSpace(this string Target)
    {
        logger.Debug("Entered member.");

        return string.IsNullOrWhiteSpace(Target);
    }

    /// <summary>
    ///     A string extension method that query if a specified character in a string is
    ///     uppercase.
    /// </summary>
    /// <param name="target">
    ///     The target to act on.
    /// </param>
    /// <param name="index">
    ///     Zero-based index of the character to test.
    /// </param>
    /// <returns>
    ///     true if uppercase, otherwise false.
    /// </returns>
    public static bool IsUpper(this string target, int index)
    {
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(target));
        Contract.Requires<ArgumentOutOfRangeException>(0 <= index);
        Contract.Requires<ArgumentOutOfRangeException>(index < target.Length);
        logger.Debug("Entered member.");
        logger.Trace("target: {0}", target);
        logger.Trace("index: {0}", index);

        return target[index].IsUpper();
    }

    /// <summary>
    ///     A character extension method that queries if a character is upcase.
    /// </summary>
    /// <param name="c">
    ///     The character to test.
    /// </param>
    /// <returns>
    ///     true if uppercase, otherwise false.
    /// </returns>
    public static bool IsUpper(this char c)
    {
        logger.Debug("Entered member.");

        return char.IsUpper(c);
    }

    /// <summary>
    ///     A char extension method that converts a Target character to a lowercase character.
    /// </summary>
    /// <param name="target">
    ///     The Target character to act on.
    /// </param>
    /// <returns>
    ///     Target as a lowercase chararacter.
    /// </returns>
    public static char ToLower(this char target)
    {
        logger.Debug("Entered member.");

        if (target >= 'A' && target <= 'Z')
        {
            return (char)(target - 'A' + 'a');
        }
        return target;
    }
}
}