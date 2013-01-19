// file:	Utility\PtHelpers.cs
//
// summary:	Implements the point helpers class
using System;
using System.Collections;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;

namespace XamlHelpmeet.Utility
{
	/// <summary>
	/// 	Point helpers.
	/// </summary>
	public static class PtHelpers
	{

		private static Hashtable _blackListedProjectTypes;

		/// <summary>
		/// 	Gets a project's assembly path.
		/// </summary>
		/// <param name="Project">
		/// 	The project.
		/// </param>
		/// <returns>
		/// 	The assembly path.
		/// </returns>
		public static string GetAssemblyPath(Project Project)
		{
			var fullPath = Path.GetDirectoryName(Project.FullName);
			var outputPath = Project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
			var outputDirectory = Path.Combine(fullPath, outputPath);
			var outputFileName = Project.Properties.Item("OutputFileName").Value.ToString();
			var assemblyPath = Path.Combine(outputDirectory, outputFileName);
			return assemblyPath;
		}

		/// <summary>
		/// 	Gets a project's type guids.
		/// </summary>
		/// <remarks>
		/// 	Shifflett's notes:
		/// 	
		/// 	Carlos is the Visual Studio Add-In Grand Master and Microsoft MVP I learned how
		/// 	to figure out if a project is a Silverlight project from these two posts, then
		/// 	wrote a little of my own code.
		/// 	
		/// 	www.mztools.com/Articles/2007/MZ2007016.aspx
		/// 	www.mztools.com/Articles/2007/MZ2007012.aspx.
		/// </remarks>
		/// <param name="Project">
		/// 	The project.
		/// </param>
		/// <returns>
		/// 	The project's type guids.
		/// </returns>
		public static string GetProjectTypeGuids(Project Project)
		{
			var projectTypeGuids = string.Empty;
			object service = GetService(Project.DTE, typeof(IVsSolution));
			var ivsSolution = service as IVsSolution;
			IVsHierarchy ivsHierarchy = null;
			IVsAggregatableProject ivsAggregatableProject;
			int result = ivsSolution.GetProjectOfUniqueName(Project.UniqueName, out ivsHierarchy);
			if (result != 0)
				return projectTypeGuids;
			ivsAggregatableProject = ivsHierarchy as IVsAggregatableProject;
			result = ivsAggregatableProject.GetAggregateProjectTypeGuids(out projectTypeGuids);
			return projectTypeGuids;
		}

		/// <summary>
		/// 	Queries aryGuids to determine if a project type is blacklisted.
		/// </summary>
		/// <param name="aryGuids">
		/// 	An array of guids belonging to a project.
		/// </param>
		/// <returns>
		/// 	true if the project type is blacklisted, false if not.
		/// </returns>
		public static bool IsProjectBlackListed(string[] aryGuids)
		{
			// Shifflett notes here: some are here because I have not
			// tested them, other because I don't want the add-in
			// trying to load web sites or test projects.

			if (_blackListedProjectTypes == null)
			{
				_blackListedProjectTypes = new Hashtable();
				_blackListedProjectTypes.Add("{349C5851-65DF-11DA-9384-00065B846F21}", String.Empty); // Web Application
				_blackListedProjectTypes.Add("{E24C65DC-7377-472B-9ABA-BC803B73C61A}", String.Empty); // Web Site
				_blackListedProjectTypes.Add("{C252FEB5-A946-4202-B1D4-9916A0590387}", String.Empty); // Visual Database Tools
				_blackListedProjectTypes.Add("{A9ACE9BB-CECE-4E62-9AA4-C7E7C5BD2124}", String.Empty); // Database
				_blackListedProjectTypes.Add("{4F174C21-8C12-11D0-8340-0000F80270F8}", String.Empty); // Database other
				_blackListedProjectTypes.Add("{3AC096D0-A1C2-E12C-1390-A8335801FDAB}", String.Empty); // Test
				_blackListedProjectTypes.Add("{D59BE175-2ED0-4C54-BE3D-CDAA9F3214C8}", String.Empty); // Workflow VB
				_blackListedProjectTypes.Add("{14822709-B5A1-4724-98CA-57A101D1B079}", String.Empty); // Workflow C#
				_blackListedProjectTypes.Add("{978C614F-708E-4E1A-B201-565925725DBA}", String.Empty); // SET UP
			}

			foreach (var item in aryGuids)
			{
				if (_blackListedProjectTypes.ContainsKey(item.ToUpper()))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 	Determines if a project with the given guids is a Silverlight project.
		/// </summary>
		/// <param name="aryGuids">
		/// 	An array of guids belonging to a project.
		/// </param>
		/// <returns>
		/// 	true if the project is a Silverlight project, false if not.
		/// </returns>
		public static bool IsProjectSilverlight(string[] aryGuids)
		{
			foreach (var item in aryGuids)
			{
				if (string.Compare("{A1591282-1198-4647-A2B1-27E5FF5F6F3B}", item, true) == 0)
					return true;
			}
			return false;
		}


		private static object GetService(DTE ServiceProvider, Guid GUID)
		{
			object service = null;
			var serviceProvider = ServiceProvider as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
			IntPtr intPtr;
			int hr;
			var sidGuid = GUID;
			var iidGuid = sidGuid;
			hr = serviceProvider.QueryService(ref sidGuid, ref iidGuid, out intPtr);
			if (hr != 0)
			{
				Marshal.ThrowExceptionForHR(hr);
			}
			else
				if (!intPtr.Equals(IntPtr.Zero))
				{
					service = Marshal.GetObjectForIUnknown(intPtr);
					Marshal.Release(intPtr);
				}
			return service;
		}


		private static object GetService(DTE serviceProvider, Type type)
		{
			return GetService(serviceProvider, type.GUID);
		}
	}
}