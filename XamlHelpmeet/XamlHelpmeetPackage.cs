// file:	XamlHelpmeetPackage.cs
//
// summary:	Implements the Xaml Pt v 11 package class
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using XamlHelpmeet.Commands;
using XamlHelpmeet.Commands.NoUI;
using XamlHelpmeet.Commands.UI;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet
{
	/// <summary>
	/// 	This is the class that implements the package exposed by this assembly.
	///
	/// 	The minimum requirement for a class to be considered a valid package for Visual
	/// 	Studio is to implement the IVsPackage interface and register itself with the shell.
	/// 	This package uses the helper classes defined inside the Managed Package Framework
	/// 	(MPF)
	/// 	to do it: it derives from the Package class that provides the implementation of the
	/// 	IVsPackage interface and uses the registration attributes defined in the framework to
	/// 	register itself and its components with the shell.
	/// </summary>
	/// <seealso cref="T:Microsoft.VisualStudio.Shell.Package"/>
	[PackageRegistration(UseManagedResourcesOnly = true)]

	// This attribute is used to register the information needed to show this package
	// in the Help/About dialog of Visual Studio.
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[Guid(GuidList.guidXamlHelpmeetPkgString)]
	public sealed class XamlHelpmeetPackage : Package
	{
		#region Fields

		private DTE _dte;
		private Events _events;
		private CommandEvents _fileSaveAll;
		private CommandEvents _fileSaveSelectedItems;
		private IVsUIShell _uiShell;
		private List<CommandBase> _commandsList;
		private OleMenuCommandService _mcs;

		#endregion Fields

		/// <summary>
		/// 	Default constructor of the package. Inside this method you can place any
		/// 	initialization code that does not require any Visual Studio service because at
		/// 	this point the package object is created but not sited yet inside Visual Studio
		/// 	environment. The place to do all the other initialization is the Initialize
		/// 	method.
		/// </summary>
		public XamlHelpmeetPackage()
		{
			Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));
			_commandsList = new List<CommandBase>();
		}

		private List<CommandBase> CommandsList
		{
			get
			{
				return _commandsList;
			}
		}

		private DTE2 Application
		{
			get
			{
				return _dte as DTE2;
			}
		}

		/////////////////////////////////////////////////////////////////////////////
		// Overridden Package Implementation

		#region Package Members

		/// <summary>
		/// 	Initialization of the package; this method is called right after the package is
		/// 	sited, so this is the place where you can put all the initialization code that
		/// 	rely on services provided by VisualStudio.
		/// </summary>
		/// <seealso cref="M:Microsoft.VisualStudio.Shell.Package.Initialize()"/>
		protected override void Initialize()
		{
			Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this));
			base.Initialize();

			_dte = base.GetService(typeof(DTE)) as DTE;
			if (_dte == null)
			{
				throw new NullReferenceException("DTE is not available.");
			}
			if (Application == null)
			{
				throw new NullReferenceException("DTE not available as DTE2.");
			}

			var _addInInstance = base.GetService(typeof(AddIn)) as AddIn;

			_uiShell = base.GetService(typeof(IVsUIShell)) as IVsUIShell;
			_events = _dte.Events;

			BuildMenu();
		}

		#region Menu

		private void AddCommand(CommandBase command)
		{
			_mcs.AddCommand(command);
			CommandsList.Add(command);
		}

		private void BuildMenu()
		{
			CommandID menuCommandID;

			_mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
			if (_mcs == null)
				throw new InvalidOperationException("No menu command service found.");

			try
			{
				// ==================  VISUAL STUDIO CODE EDITOR MENU  ==================
				// Create ViewModel Command
				
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.CreateViewModelFromSelectedClassCommand);	
				AddCommand(new CreateViewModelCommandFromSelectedClassCommand(Application, menuCommandID));

				// Create AboutCommand Command

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.AboutCommand);				
				AddCommand(new AboutCommand(Application, menuCommandID));

				// ==================  VISUAL STUDIO XAML EDITOR MENU  ==================
				// Add root level context menu item

				// Edit grid columns and rows
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.EditGridRowAndColumnsCommand);				
				AddCommand(new EditGridRowAndColumnsCommand(Application, menuCommandID));

				// Extract selected properties to style
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.ExtractSelectedPropertiesToStyleCommand);				
				AddCommand(new ExtractSelectedPropertiesToStyleCommand(Application, menuCommandID));

				// Create business form
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.CreateBusinessFormCommand);				
				AddCommand(new CreateBusinessFormCommand(Application, menuCommandID));

				// Create business form from selected class
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.CreateFormListViewDataGridFromSelectedClassCommand);				
				AddCommand(new CreateFormListViewDataGridFromSelectedClassCommand(Application, menuCommandID));

				// Fields list from selected class
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.FieldsListFromSelectedClassCommand);				
				AddCommand(new FieldsListFromSelectedClassCommand(Application, menuCommandID));

				// Remove all margins
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.RemoveMarginsCommand);				
				AddCommand(new RemoveMarginsCommand(Application, menuCommandID));

				// Set grid to auto layout
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.ChangeGridToFlowLayoutCommand);				
				AddCommand(new ChangeGridToFlowLayout(Application, menuCommandID));

				// Chainsaw
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.ChainsawDesignerExtraPropertiesCommand);				
				AddCommand(new ChainsawDesignerExtraProperties(Application, menuCommandID));

				// GROUP INTO COMMANDS
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoCanvasCommand);				
				AddCommand(new GroupIntoCanvas(Application, menuCommandID));

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoDockPanelCommand);				
				AddCommand(new GroupIntoDockPanel(Application, menuCommandID));

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoGridCommand);				
				AddCommand(new GroupIntoGrid(Application, menuCommandID));

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoScrollViewerCommand);				
				AddCommand(new GroupIntoScrollViewer(Application, menuCommandID));

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoStackPanelVerticalCommand);				
				AddCommand(new GroupIntoStackPanelVertical(Application, menuCommandID));

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoStackPanelHorizontalCommand);				
				AddCommand(new GroupIntoStackPanelHorizontal(Application, menuCommandID));

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoUniformGridCommand);				
				AddCommand(new GroupIntoUniformGrid(Application, menuCommandID));

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoViewBoxCommand);				
				AddCommand(new GroupIntoViewBox(Application, menuCommandID));

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoWrapPanelCommand);				
				AddCommand(new GroupIntoWrapPanel(Application, menuCommandID));

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoGroupBoxCommand);				
				AddCommand(new GroupIntoGroupBox(Application, menuCommandID));

				// GROUP INTO BORDER COMMANDS
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoBorderNoChildRootCommand);				
				AddCommand(new GroupIntoBorderNoChildRoot(Application, menuCommandID));

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoBorderWithGridRootCommand);				
				AddCommand(new GroupIntoBorderWithGridRoot(Application, menuCommandID));

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoBorderWithStackPanelVerticalRootCommand);				
				AddCommand(new GroupIntoBorderWithStackPanelVerticalRoot(Application, menuCommandID));

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.GroupIntoBorderWithStackPanelHorizontalRootCommand);				
				AddCommand(new GroupIntoBorderWithStackPanelHorizontalRoot(Application, menuCommandID));

				// Tools menu commands
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCmdSet, PkgCmdList.ControlDefaultsCommand);				
				AddCommand(new ControlDefaultsCommand(Application, menuCommandID));
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage("BuildMenu Exception",
											   ex.Message,
											   string.Empty,
											   ex.ToString());
			}
		}

		#endregion Menu

		private void OnDisconnection()
		{
			// this removes or deletes menus in the reverse order they were added.

			foreach(var command in CommandsList)
			{
				_mcs.RemoveCommand(command.MenuCommand);
				command.Dispose();
			}

			CommandsList.Clear();
			_commandsList = null;

		}

		#endregion Package Members
	}
}