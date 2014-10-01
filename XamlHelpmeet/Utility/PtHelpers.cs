// file:    Utility\PtHelpers.cs
//
// summary: Implements the point helpers class
// The primary purpose of this class is to identify Silverlight projects.
using System;
using System.Collections;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;

namespace XamlHelpmeet.Utility
{
using System.Collections.Generic;
using System.Linq;

using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
///     Point helpers.
/// </summary>
public static class PtHelpers
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();


    private static Hashtable _blackListedProjectTypes;

    /// <summary>
    ///     Gets a project's assembly path.
    /// </summary>
    /// <param name="Project">
    ///     The project.
    /// </param>
    /// <returns>
    ///     The assembly path.
    /// </returns>
    public static string GetAssemblyPath(Project Project)
    {
        logger.Debug("Entered member.");

        var fullPath = Path.GetDirectoryName(Project.FullName);
        var outputPath =
            Project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
        if (fullPath == null)
        {
            throw new Exception("fullPath is null.");
        }

        var outputDirectory = Path.Combine(fullPath, outputPath);
        var outputFileName =
            Project.Properties.Item("OutputFileName").Value.ToString();
        var assemblyPath = Path.Combine(outputDirectory, outputFileName);
        return assemblyPath;
    }

    /// <summary>
    ///     Gets a project's type guids.
    /// </summary>
    /// <remarks>
    ///     Shifflett's notes:
    ///
    ///     Carlos is the Visual Studio Add-In Grand Master and Microsoft MVP I learned how
    ///     to figure out if a project is a Silverlight project from these two posts, then
    ///     wrote a little of my own code.
    ///
    ///     www.mztools.com/Articles/2007/MZ2007016.aspx
    ///     www.mztools.com/Articles/2007/MZ2007012.aspx.
    /// </remarks>
    /// <param name="Project">
    ///     The project.
    /// </param>
    /// <returns>
    ///     The project's type guids.
    /// </returns>
    public static string GetProjectTypeGuids(Project Project)
    {
        logger.Debug("Entered member.");

        var projectTypeGuids = string.Empty;
        object service = GetService(Project.DTE, typeof(IVsSolution));
        var ivsSolution = service as IVsSolution;
        if (ivsSolution != null)
        {
            IVsHierarchy ivsHierarchy;
            int result = ivsSolution.GetProjectOfUniqueName(Project.UniqueName,
                         out ivsHierarchy);
            if (result != 0)
            {
                return projectTypeGuids;
            }

            var ivsAggregatableProject = ivsHierarchy as IVsAggregatableProject;
            if (ivsAggregatableProject != null)
            {
                result = ivsAggregatableProject.GetAggregateProjectTypeGuids(
                             out projectTypeGuids);
            }
        }
        return projectTypeGuids;
    }

    /// <summary>
    ///     Queries aryGuids to determine if a project type is blacklisted.
    /// </summary>
    /// <param name="aryGuids">
    ///     An array of guids belonging to a project.
    /// </param>
    /// <returns>
    ///     true if the project type is blacklisted, false if not.
    /// </returns>
    public static bool IsProjectBlackListed(IEnumerable<string> aryGuids)
    {
        logger.Debug("Entered member.");
        // Shifflett notes here: some are here because I have not
        // tested them, other because I don't want the add-in
        // trying to load web sites or test projects.

        if (_blackListedProjectTypes == null)
        {
            _blackListedProjectTypes = new Hashtable
            {
                {
                    "{349C5851-65DF-11DA-9384-00065B846F21}",
                    String.Empty
                },
                {
                    "{E24C65DC-7377-472B-9ABA-BC803B73C61A}",
                    String.Empty
                },
                {
                    "{C252FEB5-A946-4202-B1D4-9916A0590387}",
                    String.Empty
                },
                {
                    "{A9ACE9BB-CECE-4E62-9AA4-C7E7C5BD2124}",
                    String.Empty
                },
                {
                    "{4F174C21-8C12-11D0-8340-0000F80270F8}",
                    String.Empty
                },
                {
                    "{3AC096D0-A1C2-E12C-1390-A8335801FDAB}",
                    String.Empty
                },
                {
                    "{D59BE175-2ED0-4C54-BE3D-CDAA9F3214C8}",
                    String.Empty
                },
                {
                    "{14822709-B5A1-4724-98CA-57A101D1B079}",
                    String.Empty
                },
                {
                    "{978C614F-708E-4E1A-B201-565925725DBA}",
                    String.Empty
                }
            };
        }

        return aryGuids.Any(item => _blackListedProjectTypes.ContainsKey(
                                item.ToUpper()));
    }

    /// <summary>
    ///     Determines if a project with the given guids is a Silverlight project.
    /// </summary>
    /// <param name="aryGuids">
    ///     An array of guids belonging to a project.
    /// </param>
    /// <returns>
    ///     true if the project is a Silverlight project, false if not.
    /// </returns>
    public static bool IsProjectSilverlight(IEnumerable<string> aryGuids)
    {
        logger.Debug("Entered member.");

        return aryGuids.Any(item =>
                            System.String.Compare("{A1591282-1198-4647-A2B1-27E5FF5F6F3B}", item,
                                    System.StringComparison.OrdinalIgnoreCase) == 0);
    }


    private static object GetService(DTE ServiceProvider, Guid GUID)
    {
        logger.Debug("Entered member.");

        object service = null;
        var serviceProvider = ServiceProvider as
                              Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
        IntPtr intPtr;
        var sidGuid = GUID;
        var iidGuid = sidGuid;
        if (serviceProvider == null)
        {
            throw new Exception("serviceProvider is null.");
        }

        int hr = serviceProvider.QueryService(ref sidGuid, ref iidGuid,
                                              out intPtr);
        if (hr != 0)
        {
            Marshal.ThrowExceptionForHR(hr);
        }
        else if (!intPtr.Equals(IntPtr.Zero))
        {
            service = Marshal.GetObjectForIUnknown(intPtr);
            Marshal.Release(intPtr);
        }
        return service;
    }


    private static object GetService(DTE serviceProvider, Type type)
    {
        logger.Debug("Entered member.");

        return GetService(serviceProvider, type.GUID);
    }
}
}