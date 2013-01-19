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
	[Guid(GuidList.guidXamlHelpmeetPkgString)]
	public sealed class XamlHelpmeetPackage : Package
	{
		#region Fields

		private DTE _dte;
		private Events _events;
		// TODO: Complete XamlHelpmeetPackage class
		private CommandEvents _fileSaveAll;
		private CommandEvents _fileSaveSelectedItems;
		private IVsUIShell _uiShell;
		private Dictionary<CommandID,CommandBase> _commandsDictionary;
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
			_commandsDictionary = new Dictionary<CommandID, CommandBase>();
		}

		private Dictionary<CommandID, CommandBase> CommandsDictionary
		{
			get
			{
				return _commandsDictionary;
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

		private void CallBack(object sender, EventArgs e)
		{
			var command = sender as MenuCommand;
			if (command == null)
				throw new ArgumentNullException("sender");

			var myCommand = CommandsDictionary[command.CommandID];
			myCommand.Execute();
		}

		#region Menu

		private void AddCommand(OleMenuCommand menuItem, CommandBase command)
		{
			_mcs.AddCommand(menuItem);
			CommandsDictionary.Add(menuItem.CommandID, command);
			menuItem.BeforeQueryStatus += command.BeforeQueryStatus;
		}

		private void BuildMenu()
		{
			CommandID menuCommandID;
			OleMenuCommand menuItem;

			_mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
			if (_mcs == null)
				throw new InvalidOperationException("No menu command service found.");

			try
			{
				// ==================  VISUAL STUDIO CODE EDITOR MENU  ==================
				// Create ViewModel Command
				
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCodeMenu, PkgCmdList.CreateViewModelCommandFromSelectedClassCommand);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);	
				AddCommand(menuItem, new CreateViewModelCommandFromSelectedClassCommand(Application));

				// Create AboutCommand Command

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetCodeMenu, PkgCmdList.AboutCommand);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new AboutCommand(Application));

				// ==================  VISUAL STUDIO XAML EDITOR MENU  ==================
				// Add root level context menu item

				// Edit grid columns and rows

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetXamlMenu, PkgCmdList.EditGridRowAndColumnsCommand);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new EditGridRowAndColumnsCommand(Application));

				// Extract selected properties to style

				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetXamlMenu, PkgCmdList.ExtractSelectedPropertiesToStyleCommand);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new ExtractSelectedPropertiesToStyleCommand(Application));

				// Create business form
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetXamlMenu, PkgCmdList.CreateBusinessFormCommand);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new CreateBusinessFormCommand(Application));

				// Create business form from selected class
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetXamlMenu, PkgCmdList.CreateFormListViewDataGridFromSelectedClassCommand);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new CreateFormListViewDataGridFromSelectedClassCommand(Application));

				// Fields list from selected class
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetXamlMenu, PkgCmdList.FieldsListFromSelectedClassCommand);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new FieldsListFromSelectedClassCommand(Application));

				// Remove all margins
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetXamlMenu, PkgCmdList.RemoveMarginsCommand);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new RemoveMarginsCommand(Application));

				// Set grid to auto layout
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetXamlMenu, PkgCmdList.ChangeGridToFlowLayout);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new ChangeGridToFlowLayout(Application));

				// Chainsaw
				menuCommandID = new CommandID(GuidList.guidXamlHelpmeetXamlMenu, PkgCmdList.ChainsawDesignerExtraProperties);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new ChainsawDesignerExtraProperties(Application));

				// GROUP INTO COMMANDS
				menuCommandID = new CommandID(GuidList.guidGroupIntoMenu, PkgCmdList.GroupIntoCanvas);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoCanvas(Application));

				menuCommandID = new CommandID(GuidList.guidGroupIntoMenu, PkgCmdList.GroupIntoDockPanel);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoDockPanel(Application));

				menuCommandID = new CommandID(GuidList.guidGroupIntoMenu, PkgCmdList.GroupIntoGrid);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoGrid(Application));

				menuCommandID = new CommandID(GuidList.guidGroupIntoMenu, PkgCmdList.GroupIntoScrollViewer);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoScrollViewer(Application));

				menuCommandID = new CommandID(GuidList.guidGroupIntoMenu, PkgCmdList.GroupIntoStackPanelVertical);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoStackPanelVertical(Application));

				menuCommandID = new CommandID(GuidList.guidGroupIntoMenu, PkgCmdList.GroupIntoStackPanelHorizontal);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoStackPanelHorizontal(Application));

				menuCommandID = new CommandID(GuidList.guidGroupIntoMenu, PkgCmdList.GroupIntoUniformGrid);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoUniformGrid(Application));

				menuCommandID = new CommandID(GuidList.guidGroupIntoMenu, PkgCmdList.GroupIntoViewBox);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoViewBox(Application));

				menuCommandID = new CommandID(GuidList.guidGroupIntoMenu, PkgCmdList.GroupIntoWrapPanel);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoWrapPanel(Application));

				menuCommandID = new CommandID(GuidList.guidGroupIntoMenu, PkgCmdList.GroupIntoGroupBox);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoGroupBox(Application));

				// GROUP INTO BORDER COMMANDS
				menuCommandID = new CommandID(GuidList.guidGroupIntoBorderMenu, PkgCmdList.GroupIntoBorderNoChildRoot);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoBorderNoChildRoot(Application));

				menuCommandID = new CommandID(GuidList.guidGroupIntoBorderMenu, PkgCmdList.GroupIntoBorderWithGridRoot);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoBorderWithGridRoot(Application));

				menuCommandID = new CommandID(GuidList.guidGroupIntoBorderMenu, PkgCmdList.GroupIntoBorderWithStackPanelVerticalRoot);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoBorderWithStackPanelVerticalRoot(Application));

				menuCommandID = new CommandID(GuidList.guidGroupIntoBorderMenu, PkgCmdList.GroupIntoBorderWithStackPanelHorizontalRoot);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new GroupIntoBorderWithStackPanelHorizontalRoot(Application));

				// Tools menu commands
				menuCommandID = new CommandID(GuidList.guidToolsMenu, PkgCmdList.ControlDefaultsCommand);
				menuItem = new OleMenuCommand(CallBack, menuCommandID);
				AddCommand(menuItem, new ControlDefaultsCommand(Application));

				// The AboutCommand is added above to a submenu in the Code
				// editor context menu. A duplicate should show in the tools
				// menu because of the definition in the vsct file.

				//AddCommand(menuItem, new AboutCommand(Application));
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

			CommandID[] commandIDs = null;
			CommandsDictionary.Keys.CopyTo(commandIDs, 0);
			for (int i = CommandsDictionary.Count-1; i >= 0; i--)
			{
				var command = CommandsDictionary[commandIDs[i]];
				_mcs.RemoveCommand(command.MenuCommand);
				command.Dispose();
			}

			CommandsDictionary.Clear();
			_commandsDictionary = null;

		}

		#endregion Package Members
	}
}