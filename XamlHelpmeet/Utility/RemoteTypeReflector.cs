// <copyright file="RemoteTypeReflector.cs" company="YoderZone.com">
// Copyright (c) 2014 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>9/20/2014</date>
// <summary>Implements the remote type reflector class</summary>
// <remarks>
// Licensed under the Microsoft Public License (Ms-PL); you may not
// use this file except in compliance with the License. You may obtain a copy
// of the License at
//
// https://remarker.codeplex.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
// License for the specific language governing permissions and limitations
// under the License.
// </remarks>
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using EnvDTE;
using XamlHelpmeet.Model;
using XamlHelpmeet.ReflectionLoader;
using XamlHelpmeet.UI.SelectClass;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Extensions;

namespace XamlHelpmeet.Utility
{
using System.Diagnostics.Contracts;
using System.Linq;

using NLog;

using VSLangProj;

using YoderZone.Extensions.NLog;

/// <summary>
/// Remote type reflector.
/// </summary>
public class RemoteTypeReflector
{
    /// <summary>
    /// The logger.
    /// </summary>
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    /// The secondary application domain.
    /// </summary>
    private AppDomain _secondaryAppDomain;

    /// <summary>
    /// Gets class entity from selected class.
    /// </summary>
    /// <exception cref="Exception">
    /// Thrown when an exception error condition occurs.
    /// </exception>
    /// <param name="TargetProject">
    /// Target project.
    /// </param>
    /// <param name="NameOfSourceCommand">
    /// Name of the source command.
    /// </param>
    /// <returns>
    /// The class entity from selected class.
    /// </returns>
    public ClassEntity GetClassEntityFromSelectedClass(Project TargetProject,
            string NameOfSourceCommand)
    {
        Contract.Requires(TargetProject != null);
        Contract.Requires(NameOfSourceCommand != null);

        logger.Debug("Entered member.");
        logger.Trace("TargetProject: {0}", TargetProject);
        logger.Trace("NameOfSourceCommand: {0}", NameOfSourceCommand);

        // Karl Shifflett in original vb code:
        //
        // 'TODO Karl you left off here.  must ensure that the SL versions is added
        // 'Dim strSilverlightVersion As String = String.Empty
        //
        // 'If bolIsSilverlight Then
        // '    strSilverlightVersion = Me.Application.ActiveDocument.ProjectItem.ContainingProject.Properties.Item(
        //                              "TargetFrameworkMoniker").Value.ToString.Replace("Silverlight,Version=v", String.Empty)
        // 'End If
        //
        // Yes, this portion of code was not implemented.

        string assemblyPath = GetAssemblyInformation(TargetProject);
        logger.Trace("assemblyPath: {0}", assemblyPath);

        if (assemblyPath.IsNullOrEmpty())
        {
            // This should never execute since, the menu option would be disabled.
            // If it does run, there is a programming error.
            throw new Exception("The project associated with the selected file is either not vb, cs or is blacklisted.");
        }

        RemoteWorker remoteWorker = null;
        RemoteResponse<ClassInformationList> remoteResponse = null;

        try
        {
            var appSetup = new AppDomainSetup
            {
                ApplicationBase = Path.GetDirectoryName(assemblyPath),
                DisallowApplicationBaseProbing = false,
                ShadowCopyFiles = "True"
            };
            logger.Trace("appSetup: {0}", appSetup);

            //++ Secondary Application Domain

            _secondaryAppDomain = AppDomain.CreateDomain("SecondaryAppDomain", null,
                                  appSetup);
            logger.Trace("_secondaryAppDomain: {0}", _secondaryAppDomain);

            AppDomain.CurrentDomain.AssemblyResolve +=
                SecondaryAppDomain_AssemblyResolve;

            //! This creates remoteWorker in a secondary domain, allowing it to run
            //! in a separate context from the current domain. This separates
            //! remoteWorker from the rest of the application (VS), so if the
            //! domain becomes unstable, it can be unloaded without harming the
            //! current domain.

            var location = Assembly.GetExecutingAssembly().Location;
            logger.Trace("location: {0}", location);

            var directoryName = Path.GetDirectoryName(location);
            logger.Trace("directoryName: {0}", directoryName);

            // ReSharper disable once AssignNullToNotNullAttribute
            var assemblyName = Path.Combine(directoryName,
                                            "XamlHelpmeet.ReflectionLoader.dll");
            logger.Trace("assemblyName: {0}", assemblyName);

            remoteWorker = this._secondaryAppDomain.CreateInstanceFromAndUnwrap(
                               assemblyName,
                               "XamlHelpmeet.ReflectionLoader.RemoteWorker") as RemoteWorker;
            logger.Trace("remoteWorker: {0}", remoteWorker);

            //+ remoteWorker inherits a MarshalByRefObject. This
            //+ enables access to objects returned by its
            //+ methods across domains.

            if (remoteWorker != null)
            {
                var isSilverlight = PtHelpers.IsProjectSilverlight(
                                        PtHelpers.GetProjectTypeGuids(TargetProject).Split(';'));
                logger.Trace("isSilverlight: {0}", isSilverlight);

                // remoteResponse is a helper class that is serialized so it can
                // wrap objects returned from a secondary (i.e., remote) application
                // domain.

                // A problem exists in this call when an object is returned from
                // the secondary domain that is of a different version than the
                // same object in the current domain. When that happens an
                // InvalidCastException is thrown, and remoteResponse will be
                // null. I think this may occur when XHM is used on its own
                // assemblies, if the assemblies have been recompiled since
                // the operating vsix file was produced. In fact it is likely
                // that the object that is causing the throw is remoteResponse
                // itself. This might be resolved by freezing the project
                // that RemoteResponse is in, and compile it only when necessary.
                // It would make sense also to move the class into the
                // ReflectionLoader namespace so it is less likely it would
                // need to be recompiled when other changes are needed in
                // the solution.
                remoteResponse = remoteWorker.GetClassEntityFromUserSelectedClass(
                                     assemblyPath, isSilverlight, GetProjectReferences(TargetProject));
                logger.Trace("remoteResponse: {0}", remoteResponse);
                logger.Trace("remoteResponse.ResponseStatus: {0}",
                             remoteResponse.ResponseStatus);

                if (remoteResponse.ResponseStatus != ResponseStatus.Success)
                {
                    UIUtilities.ShowExceptionMessage("Unable to Reflect Type",
                                                     String.Format("The following exception was returned. {0}",
                                                             remoteResponse.CustomMessageAndException));
                }
            }
            else
            {
                UIUtilities.ShowExceptionMessage("Unable To Create Worker",
                                                 "Can't create Secondary AppDomain RemoteWorker class. CreateInstance and Unwrap methods returned null.");
            }
        }
        catch (FileNotFoundException ex)
        {
            UIUtilities.ShowExceptionMessage("File Not Found",
                                             String.Format("File not found.{0}{0}Have you built your application?{0}{0}{1}",
                                                     Environment.NewLine, ex.Message));
        }
        catch (Exception ex)
        {
            UIUtilities.ShowExceptionMessage("Unable To Create Secondary AppDomain RemoteWorker",
                                             ex.Message);
        }
        finally
        {
            AppDomain.CurrentDomain.AssemblyResolve -=
                SecondaryAppDomain_AssemblyResolve;

            if (_secondaryAppDomain != null)
            {
                try
                {
                    AppDomain.Unload(_secondaryAppDomain);
                }
                catch (Exception ex)
                {
                    UIUtilities.ShowExceptionMessage("AppDomain.Unload Exception",
                                                     ex.Message);
                }
            }
            _secondaryAppDomain = null;
        }

        if (remoteResponse == null ||
                remoteResponse.ResponseStatus != ResponseStatus.Success ||
                remoteResponse.Result == null || remoteResponse.Result.Count == 0)
        {
            if (remoteResponse == null ||
                    remoteResponse.ResponseStatus == ResponseStatus.Success)
            { UIUtilities.ShowInformationMessage("No Model", "Unable to find a class suitable for this command."); }
            return null;
        }

        var form = new SelectClassFromAssembliesWindow(remoteResponse.Result,
                NameOfSourceCommand);

        if ((bool)!form.ShowDialog())
        { return null; }

        form.SelectedAssemblyNamespaceClass.ClassEntity.Success = true;
        if (form.SelectedAssemblyNamespaceClass.ClassEntity.IsSilverlight)
        {
            form.SelectedAssemblyNamespaceClass.ClassEntity.SilverlightVersion =
                TargetProject.Properties.Item("TargetFrameworkMoniker").Value.ToString().Replace("Silverlight,Version=v",
                        string.Empty);
        }

        return form.SelectedAssemblyNamespaceClass.ClassEntity;

    }

    /// <summary>
    /// Gets assembly information.
    /// </summary>
    /// <param name="TargetProject">
    /// Target project.
    /// </param>
    /// <returns>
    /// The assembly information.
    /// </returns>
    private string GetAssemblyInformation(Project TargetProject)
    {
        logger.Debug("Entered member.");
        logger.Trace("TargetProject: {0}", TargetProject);

        if ((TargetProject.Kind == PrjKind.prjKindVBProject ||
                TargetProject.Kind == PrjKind.prjKindCSharpProject) &&
                !(PtHelpers.IsProjectBlackListed(PtHelpers.GetProjectTypeGuids(
                            TargetProject).Split(';'))))
        {
            return PtHelpers.GetAssemblyPath(TargetProject);
        }

        return string.Empty;
    }

    /// <summary>
    /// Gets class entities for selected project.
    /// </summary>
    /// <exception cref="Exception">
    /// Thrown when an exception error condition occurs.
    /// </exception>
    /// <param name="TargetProject">
    /// Target project.
    /// </param>
    /// <param name="NameOfSourceCommand">
    /// Name of the source command.
    /// </param>
    /// <returns>
    /// The class entities for selected project.
    /// </returns>
    public ClassInformationList GetClassEntitiesForSelectedProject(
        Project TargetProject, string NameOfSourceCommand)
    {
        Contract.Requires<ArgumentNullException>(TargetProject != null);
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(
                    NameOfSourceCommand));
        logger.Debug("Entered member.");
        logger.Trace("TargetProject: {0}", TargetProject);
        logger.Trace("NameOfSourceCommand: {0}", NameOfSourceCommand);

        string assemblyPath = GetAssemblyInformation(TargetProject);
        logger.Trace("assemblyPath: {0}", assemblyPath);

        if (assemblyPath.IsNullOrEmpty())
        {
            throw new Exception("The project associated with the selected file is either not vb, cs or is blacklisted.");
        }

        RemoteWorker remoteWorker = null;
        RemoteResponse<ClassInformationList> remoteResponse = null;

        try
        {
            var appSetup = new AppDomainSetup
            {
                ApplicationBase = Path.GetDirectoryName(assemblyPath),
                DisallowApplicationBaseProbing = false,
                ShadowCopyFiles = "True"
            };

            logger.Trace("appSetup: {0}", appSetup);

            _secondaryAppDomain = AppDomain.CreateDomain("SecondaryAppDomain", null,
                                  appSetup);
            logger.Trace("_secondaryAppDomain: {0}", _secondaryAppDomain);

            AppDomain.CurrentDomain.AssemblyResolve +=
                SecondaryAppDomain_AssemblyResolve;
            var location = Assembly.GetExecutingAssembly().Location;
            logger.Trace("location: {0}", location);

            var directoryName = Path.GetDirectoryName(
                                    location);
            logger.Trace("directoryName: {0}", directoryName);

            remoteWorker = this._secondaryAppDomain.CreateInstanceFromAndUnwrap(
                               // ReSharper disable once AssignNullToNotNullAttribute
                               Path.Combine(directoryName,
                                            "XamlHelpmeet.ReflectionLoader.dll"),
                               "XamlHelpmeet.ReflectionLoader.RemoteWorker") as RemoteWorker;
            logger.Trace("remoteWorker: {0}", remoteWorker);

            if (remoteWorker != null)
            {
                var isSilverlight = PtHelpers.IsProjectSilverlight(
                                        PtHelpers.GetProjectTypeGuids(TargetProject).Split(';'));
                logger.Trace("isSilverlight: {0}", isSilverlight);

                remoteResponse = remoteWorker.GetClassEntityFromUserSelectedClass(
                                     assemblyPath, isSilverlight, GetProjectReferences(TargetProject));
                logger.Trace("remoteResponse: {0}", remoteResponse);

                if (remoteResponse.ResponseStatus != ResponseStatus.Success)
                {
                    UIUtilities.ShowExceptionMessage("Unable to Reflect Type",
                                                     "The following exception was returned. " +
                                                     remoteResponse.CustomMessageAndException);
                }
                else if (remoteResponse.CustomMessage.IsNotNullOrEmpty())
                {
                    UIUtilities.ShowInformationMessage("Reflection Error",
                                                       string.Format("Unable to reflect the following:\r\n\r\n{0}\r\nAt least one other assembly however successfully reflected.",
                                                               remoteResponse.CustomMessage));
                }
            }
            else
            {
                UIUtilities.ShowExceptionMessage("Unable To Create Worker",
                                                 "Can't create Secondary AppDomain RemoteWorker class. CreateInstance and Unwrap methods returned null.");
            }
        }
        catch (FileNotFoundException ex)
        {
            UIUtilities.ShowExceptionMessage("File Not Found",
                                             String.Format("File not found.{0}{0}Have you built your application?{0}{0}{1}",
                                                     Environment.NewLine, ex.Message));
            logger.Error("File not found in GetClassEntitiesForSelectedProject().");
        }
        catch (Exception ex)
        {
            UIUtilities.ShowExceptionMessage("Unable To Create Secondary AppDomain RemoteWorker",
                                             ex.Message);
            logger.Error("Unable To Create Secondary AppDomain RemoteWorker in GetClassEntitiesForSelectedProject().");
        }
        finally
        {
            AppDomain.CurrentDomain.AssemblyResolve -=
                SecondaryAppDomain_AssemblyResolve;

            if (_secondaryAppDomain != null)
            {
                try
                {
                    AppDomain.Unload(_secondaryAppDomain);
                }
                catch (Exception ex)
                {
                    UIUtilities.ShowExceptionMessage("AppDomain.Unload Exception",
                                                     ex.Message);
                    logger.Debug("AppDomain.Unload Exception was raised in GetClassEntitiesForSelectedProject.",
                                 ex);
                }
            }
            _secondaryAppDomain = null;
        }

        if (remoteResponse == null ||
                remoteResponse.ResponseStatus != ResponseStatus.Success)
        {
            return null;
        }
        return remoteResponse.Result;
    }

    /// <summary>
    /// Gets project references.
    /// </summary>
    /// <param name="TargetProject">
    /// Target project.
    /// </param>
    /// <returns>
    /// The project references.
    /// </returns>
    private IEnumerable<string> GetProjectReferences(Project TargetProject)
    {
        Contract.Requires<ArgumentNullException>(TargetProject != null);
        Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

        logger.Debug("Entered member.");

        var list = new List<string>();
        var vsProject = TargetProject.Object as VSProject;

        if (vsProject == null)
        {
            return list;
        }

        foreach (Reference reference in vsProject.References)
        {
            if (reference.IsMicrosoftAssembly())
            {
                continue;
            }

            if (reference.Path.IsNullOrEmpty())
            {
                UIUtilities.ShowExceptionMessage("Broken Reference Found",
                                                 String.Format("The {0} reference is broken or unresolved. It will be ignored for now, but you should correct it or remove the unused reference.",
                                                         reference.Name));
                continue;
            }

            list.Add(reference.Path);
        }

        return list;
    }

    /// <summary>
    /// Event handler. Called by SecondaryAppDomain for assembly resolve
    /// events.
    /// </summary>
    /// <param name="sender">
    /// Source of the event.
    /// </param>
    /// <param name="args">
    /// Resolve event information.
    /// </param>
    /// <returns>
    /// An Assembly.
    /// </returns>
    private Assembly SecondaryAppDomain_AssemblyResolve(object sender,
            ResolveEventArgs args)
    {
        logger.Debug("Entered member.");

        var name = args.Name;

        return (from item in AppDomain.CurrentDomain.GetAssemblies()
                let foundName = item.FullName
                                where foundName == name
                                select item).FirstOrDefault();
    }
}
}