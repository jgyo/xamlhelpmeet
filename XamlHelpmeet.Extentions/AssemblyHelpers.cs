using Mono.Cecil;
using VSLangProj;

namespace XamlHelpmeet.Extensions
{
using System;
using System.Diagnostics.Contracts;

using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
///     Assembly helpers.
/// </summary>
/// <remarks>
///     This class encapsulates Karl Shifflett's IsMicrosoftAssembly logic
///     in several extension methods so they may be applied as members
///     of various class types.
/// </remarks>
public static class AssemblyHelpers
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    ///     A string extension method that queries if 'AssemblyName' is
    ///     a Microsoft assembly.
    /// </summary>
    /// <param name="AssemblyName">
    ///     The AssemblyName to act on.
    /// </param>
    /// <returns>
    ///     true if Microsoft assembly, false if not.
    /// </returns>
    /// <remarks>
    ///     The logic for this extension method taken from Karl
    ///     Shifflett's IsMicrosoftAssembly method in RemoteWorker.vb
    ///     in his XAMLPowerToys.ReflectionLoader project.
    /// </remarks>

    public static bool IsMicrosoftAssembly(this string AssemblyName)
    {
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(
                    AssemblyName));

        logger.Debug("Entered member.");

        AssemblyName = AssemblyName.ToLower();
        return AssemblyName.StartsWith("system") ||
               AssemblyName.StartsWith("mscorlib") ||
               AssemblyName.StartsWith("presentationframework") ||
               AssemblyName.StartsWith("presentationcore") ||
               AssemblyName.StartsWith("microsoft") ||
               AssemblyName.StartsWith("windowsbase") ||
               AssemblyName.StartsWith("wpftoolkit") ||
               AssemblyName.StartsWith("uiautomationprovider");
    }

    /// <summary>
    ///     A string extension method that queries if 'reference' is a Microsoft assembly.
    /// </summary>
    /// <param name="reference">
    ///     The reference.
    /// </param>
    /// <returns>
    ///     true if microsoft assembly, false if not.
    /// </returns>

    public static bool IsMicrosoftAssembly(this Reference reference)
    {
        Contract.Requires<ArgumentNullException>(reference != null);
        Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(
                reference.Name));

        logger.Debug("Entered member.");
        logger.Trace("reference: {0}", reference);
        logger.Trace("reference.Name: {0}", reference.Name);

        return reference.Name.IsMicrosoftAssembly();
    }

    /// <summary>
    ///     A string extension method that queries if 'reference' is a Microsoft assembly.
    /// </summary>
    /// <param name="reference">
    ///     The reference.
    /// </param>
    /// <returns>
    ///     true if microsoft assembly, false if not.
    /// </returns>

    public static bool IsMicrosoftAssembly(this AssemblyNameReference
                                           reference)
    {
        Contract.Requires<ArgumentNullException>(reference != null);
        Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(
                reference.Name));

        logger.Debug("Entered member.");
        logger.Trace("reference: {0}", reference);

        return reference.Name.IsMicrosoftAssembly();
    }

    /// <summary>
    ///     A string extension method that queries if 'AssemblyName' is
    ///     not a Microsoft assembly.
    /// </summary>
    /// <param name="AssemblyName">
    ///     The AssemblyName to act on.
    /// </param>
    /// <returns>
    ///     true if not microsoft assembly, false if it is a Microsoft assembly.
    /// </returns>

    public static bool IsNotMicrosoftAssembly(this string AssemblyName)
    {
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(
                    AssemblyName));
        logger.Debug("Entered member.");
        logger.Trace("AssemblyName: {0}", AssemblyName);

        return !AssemblyName.IsMicrosoftAssembly();
    }

    /// <summary>
    ///     A string extension method that queries if 'reference' is not a Microsoft
    ///     assembly.
    /// </summary>
    /// <param name="reference">
    ///     The reference.
    /// </param>
    /// <returns>
    ///     true if not microsoft assembly, false if it is a Microsoft assembly.
    /// </returns>

    public static bool IsNotMicrosoftAssembly(this Reference reference)
    {
        Contract.Requires<ArgumentNullException>(reference != null);
        Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(
                reference.Name));
        logger.Debug("Entered member.");
        logger.Trace("reference: {0}", reference);
        logger.Trace("reference.Name: {0}", reference.Name);

        return !reference.IsMicrosoftAssembly();
    }

    /// <summary>
    ///     A string extension method that queries if 'reference' is not a Microsoft
    ///     assembly.
    /// </summary>
    /// <param name="reference">
    ///     The reference.
    /// </param>
    /// <returns>
    ///     true if not microsoft assembly, false if it is a Microsoft assembly.
    /// </returns>

    public static bool IsNotMicrosoftAssembly(this AssemblyNameReference
            reference)
    {
        Contract.Requires<ArgumentNullException>(reference != null);
        Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(
                reference.Name));
        logger.Debug("Entered member.");
        logger.Trace("reference: {0}", reference);
        logger.Trace("reference.Name: {0}", reference.Name);

        return !reference.IsMicrosoftAssembly();
    }
}
}