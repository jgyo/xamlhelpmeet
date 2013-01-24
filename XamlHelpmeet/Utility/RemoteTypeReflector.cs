using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EnvDTE;
using VSLangProj;
using XamlHelpmeet.Model;
using XamlHelpmeet.ReflectionLoader;
using XamlHelpmeet.UI.SelectClass;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Extentions;

namespace XamlHelpmeet.Utility
{
	/// <summary>
	/// 	Remote type reflector.
	/// </summary>
	public class RemoteTypeReflector
	{
		private AppDomain _secondaryAppDomain;

		/// <summary>
		/// 	Gets class entity from selected class.
		/// </summary>
		/// <exception cref="Exception">
		/// 	Thrown when an exception error condition occurs.
		/// </exception>
		/// <param name="TargetProject">
		/// 	Target project.
		/// </param>
		/// <param name="NameOfSourceCommand">
		/// 	Name of the source command.
		/// </param>
		/// <returns>
		/// 	The class entity from selected class.
		/// </returns>
		public ClassEntity GetClassEntityFromSelectedClass(Project TargetProject, string NameOfSourceCommand)
		{
			// Note from Karl Shifflett in original vb code:
			//
			// 'TODO karl you left off here.  must ensure that the SL verions is added
			// 'Dim strSilverlightVersion As String = String.Empty
			// 'If bolIsSilverlight Then
			// '    strSilverlightVersion = Me.Application.ActiveDocument.ProjectItem.ContainingProject.Properties.Item("TargetFrameworkMoniker").Value.ToString.Replace("Silverlight,Version=v", String.Empty)
			// 'End If
			//
			// Yes, this portion of code was not implemented.

			string assemblyPath = GetAssemblyInformation(TargetProject);

			if (assemblyPath.IsNullOrEmpty())
			{
				// This should never execute since, the menu option would be disabled.
				// If it does run, there is a programming error.
				throw new Exception("The project associated with the selected file is either not vb, cs or is blacklisted.");
			}

			RemoteWorker remoteWorker = null;
			RemoteResponse<AssembliesNamespacesClasses> remoteResponse = null;

			try
			{
				var appSetup = new AppDomainSetup()
				{
					ApplicationBase = Path.GetDirectoryName(assemblyPath),
					DisallowApplicationBaseProbing = false,
					ShadowCopyFiles = "True"
				};

				_secondaryAppDomain = AppDomain.CreateDomain("SecondaryAppDomain", null, appSetup);
				AppDomain.CurrentDomain.AssemblyResolve += SecondaryAppDomain_AssemblyResolve;
				remoteWorker = _secondaryAppDomain.CreateInstanceFromAndUnwrap(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "XamlHelpmeet.ReflectionLoader.dll"), "XamlHelpmeet.ReflectionLoader.RemoteWorker") as RemoteWorker;

				if (remoteWorker != null)
				{
					var isSilverlight = PtHelpers.IsProjectSilverlight(PtHelpers.GetProjectTypeGuids(TargetProject).Split(';'));
					// BMK Reflection Code
					remoteResponse = remoteWorker.GetClassEntityFromUserSelectedClass(assemblyPath, isSilverlight, NameOfSourceCommand, GetProjectReferences(TargetProject));

					if (remoteResponse.ResponseStatus != ResponseStatus.Success)
					{
						UIUtilities.ShowExceptionMessage("Unable to Reflect Type", String.Format("The following exception was returned. {0}", remoteResponse.CustomMessageAndException), string.Empty, remoteResponse.Exception.ToString());
					}
				}
				else
				{
					UIUtilities.ShowExceptionMessage("Unable To Create Worker", "Can't create Secondary AppDomain RemoteWorker class. CreateInstance and Unwrap methods returned null.");
				}
			}
			catch (FileNotFoundException ex)
			{
				UIUtilities.ShowExceptionMessage("File Not Found", String.Format("File not found.{0}{0}Have you built your application?{0}{0}{1}", Environment.NewLine, ex.Message), String.Empty, ex.ToString());
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage("Unable To Create Secondary AppDomain RemoteWorker", ex.Message, String.Empty, ex.ToString());
			}
			finally
			{
				AppDomain.CurrentDomain.AssemblyResolve -= SecondaryAppDomain_AssemblyResolve;

				remoteWorker = null;
				if (_secondaryAppDomain != null)
				{
					try
					{
						AppDomain.Unload(_secondaryAppDomain);
					}
					catch (Exception ex)
					{
						UIUtilities.ShowExceptionMessage("AppDomain.Unload Exception", ex.Message, String.Empty, ex.ToString());
					}
				}
				_secondaryAppDomain = null;
			}

			if (remoteResponse == null || remoteResponse.ResponseStatus != ResponseStatus.Success || remoteResponse.Result == null || remoteResponse.Result.Count == 0)
			{
				if (remoteResponse.ResponseStatus == ResponseStatus.Success)
					UIUtilities.ShowInformationMessage("No Model", "Unable to find a class suitable for this command.");
				return null;
			}

			SelectClassFromAssembliesWindow frm = null;
			// ClassEntity classEntity = null;	// Not used in original

			frm = new SelectClassFromAssembliesWindow(remoteResponse.Result, NameOfSourceCommand);

			if (!frm.ShowDialog() ?? false)
				return null;

			frm.SelectedAssemblyNamespaceClass.ClassEntity.Success = true;
			if (frm.SelectedAssemblyNamespaceClass.ClassEntity.IsSilverlight)
			{
				frm.SelectedAssemblyNamespaceClass.ClassEntity.SilverlightVersion =
					TargetProject.Properties.Item("TargetFrameworkMoniker").Value.ToString().Replace("Silverlight,Version=v", string.Empty);
			}

			return frm.SelectedAssemblyNamespaceClass.ClassEntity;

		}

		private string GetAssemblyInformation(Project TargetProject)
		{
			if ((TargetProject.Kind == VSLangProjPrjKind.prjKindVBProject || TargetProject.Kind == VSLangProjPrjKind.prjKindCSharpProject) && !(PtHelpers.IsProjectBlackListed(PtHelpers.GetProjectTypeGuids(TargetProject).Split(';'))))
			{
				return PtHelpers.GetAssemblyPath(TargetProject);
			}
			return string.Empty;
		}

		/// <summary>
		/// 	Gets class entities for selected project.
		/// </summary>
		/// <exception cref="Exception">
		/// 	Thrown when an exception error condition occurs.
		/// </exception>
		/// <param name="TargetProject">
		/// 	Target project.
		/// </param>
		/// <param name="NameOfSourceCommand">
		/// 	Name of the source command.
		/// </param>
		/// <returns>
		/// 	The class entities for selected project.
		/// </returns>
		public AssembliesNamespacesClasses GetClassEntitiesForSelectedProject(Project TargetProject, string NameOfSourceCommand)
		{
			string assemblyPath = GetAssemblyInformation(TargetProject);

			if (assemblyPath.IsNullOrEmpty())
			{
				throw new Exception("The project associated with the selected file is either not vb, cs or is blacklisted.");
			}

			RemoteWorker remoteWorker = null;
			RemoteResponse<AssembliesNamespacesClasses> remoteResponse = null;

			try
			{
				var appSetup = new AppDomainSetup()
				{
					ApplicationBase = Path.GetDirectoryName(assemblyPath),
					DisallowApplicationBaseProbing = false,
					ShadowCopyFiles = "True"
				};

				_secondaryAppDomain = AppDomain.CreateDomain("SecondaryAppDomain", null, appSetup);
				AppDomain.CurrentDomain.AssemblyResolve += SecondaryAppDomain_AssemblyResolve;
				remoteWorker = _secondaryAppDomain.CreateInstanceFromAndUnwrap(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "XamlHelpmeet.ReflectionLoader.dll"), "XamlHelpmeet.ReflectionLoader.RemoteWorker") as RemoteWorker;

				if (remoteWorker != null)
				{
					var isSilverlight = PtHelpers.IsProjectSilverlight(PtHelpers.GetProjectTypeGuids(TargetProject).Split(';'));
					remoteResponse = remoteWorker.GetClassEntityFromUserSelectedClass(assemblyPath, isSilverlight, NameOfSourceCommand, GetProjectReferences(TargetProject));

					if (remoteResponse.ResponseStatus != ResponseStatus.Success)
					{
						UIUtilities.ShowExceptionMessage("Unable to Reflect Type", "The following exception was returned. " + remoteResponse.CustomMessageAndException, string.Empty, remoteResponse.Exception.ToString());
					}
				}
				else
				{
					UIUtilities.ShowExceptionMessage("Unable To Create Worker", "Can't create Secondary AppDomain RemoteWorker class. CreateInstance and Unwrap methods returned null.");
				}
			}
			catch (FileNotFoundException ex)
			{
				UIUtilities.ShowExceptionMessage("File Not Found", String.Format("File not found.{0}{0}Have you built your application?{0}{0}{1}", Environment.NewLine, ex.Message), String.Empty, ex.ToString());
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage("Unable To Create Secondary AppDomain RemoteWorker", ex.Message, String.Empty, ex.ToString());
			}
			finally
			{
				AppDomain.CurrentDomain.AssemblyResolve -= SecondaryAppDomain_AssemblyResolve;

				remoteWorker = null;
				if (_secondaryAppDomain != null)
				{
					try
					{
						AppDomain.Unload(_secondaryAppDomain);
					}
					catch (Exception ex)
					{
						UIUtilities.ShowExceptionMessage("AppDomain.Unload Exception", ex.Message, String.Empty, ex.ToString());
					}
				}
				_secondaryAppDomain = null;
			}

			if (remoteResponse != null || remoteResponse.ResponseStatus != ResponseStatus.Success)
			{
				return null;
			}
			return remoteResponse.Result;
		}

		private List<string> GetProjectReferences(Project TargetProject)
		{
			var list = new List<string>();
			var vsProject = TargetProject.Object as VSProject;

			foreach (Reference reference in vsProject.References)
			{
				if (reference.IsMicrosoftAssembly())
				{
					continue;
				}

				if (reference.Path.IsNullOrEmpty())
				{
					UIUtilities.ShowExceptionMessage("Broken Reference Found", String.Format("The {0} reference is broken or unresoloved. It will be ignored for now, but you should correct it or remove the unused reference.", reference.Name));
					continue;
				}

				list.Add(reference.Path);
			}

			return list;
		}

		private Assembly SecondaryAppDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			var name = args.Name;

			foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
			{
				var foundName = item.FullName;

				if (foundName == name)
				{
					return item;
				}
			}

			return null;
		}
	}
}