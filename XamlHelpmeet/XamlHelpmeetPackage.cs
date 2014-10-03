// ***********************************************************************
// Assembly         : XamlHelpmeet
// Solution         : XamlHelpmeet
// File name        : XamlHelpmeetPackage.cs
// Author           : Gil Yoder
// Created          : 09 19,  2014
//
// Last Modified By : Gil Yoder
// Last Modified On : 09 20, 2014
// ***********************************************************************

namespace XamlHelpmeet
{
#region Imports

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

using EnvDTE;

using EnvDTE80;

using Microsoft.VisualStudio.Shell;

using NLog;

using XamlHelpmeet.Commands;
using XamlHelpmeet.Commands.NoUI;
using XamlHelpmeet.Commands.UI;
using XamlHelpmeet.UI.Utilities;

using YoderZone.Extensions.NLog;

#endregion

/// <summary>
///     This is the class that implements the package exposed by this assembly.
///     The minimum requirement for a class to be considered a valid package for
///     Visual
///     Studio is to implement the IVsPackage interface and register itself with
///     the shell.
///     This package uses the helper classes defined inside the Managed Package
///     Framework
///     (MPF) to do it: it derives from the Package class that provides the
///     implementation
///     of the IVsPackage interface and uses the registration attributes defined in
///     the
///     framework to register itself and its components with the shell.
/// </summary>
/// <seealso cref="T:Microsoft.VisualStudio.Shell.Package" />
[PackageRegistration(UseManagedResourcesOnly = true)]

// This attribute is used to register the information needed to show this package
// in the Help/About dialog of Visual Studio.
[InstalledProductRegistration("#110", "#112", "1.0",
                              IconResourceID = 400)]
[ProvideMenuResource("Menus.ctmenu", 1)]

// This attribute causes the package to be loaded before the menu items appear, thus
// allowing execution tests to disable or enable items as appropriate.
[ProvideAutoLoad("{f1536ef8-92ec-443c-9ed7-fdadf150da82}")]
[Guid(GuidList.guidXamlHelpmeetPkgString)]
public sealed class XamlHelpmeetPackage : Package
{
    #region Static Fields

    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    #endregion

    #region Fields

    private readonly List<CommandBase> _commandsList;

    private DTE _dte;

    //private Events _events;
    //private CommandEvents _fileSaveAll;
    //private CommandEvents _fileSaveSelectedItems;
    private OleMenuCommandService _mcs;

    #endregion

    //private IVsUIShell _uiShell;

    #region Constructors and Destructors

    /// <summary>
    ///     Default constructor of the package. Inside this method you can place any
    ///     initialization code that does not require any Visual Studio service because
    ///     at
    ///     this point the package object is created but not sited yet inside Visual
    ///     Studio
    ///     environment. The place to do all the other initialization is the Initialize
    ///     method.
    /// </summary>
    public XamlHelpmeetPackage()
    {
        logger.Debug("Entered constructor.");

        this._commandsList = new List<CommandBase>();
    }


    static XamlHelpmeetPackage()
    {
        var settingsConfig = SettingsHelper.NewConfiguration("XamlHelpmeet",
                             "YoderZone");
        var fileTarget = FileTargetFactory.CreateFileTarget("xhmFileTarget",
                         "XHM${shortdate}.log", settingsConfig);
        var loggingRule = RuleFactory.CreateRule("XamlHelpmeet.*", fileTarget);
        settingsConfig.AddTarget(fileTarget, true);
        settingsConfig.AddRule("xhmRule", loggingRule);
    }

    #endregion

    #region Properties

    private DTE2 Application
    {
        get
        {
            return this._dte as DTE2;
        }
    }

    private List<CommandBase> CommandsList
    {
        get
        {
            return this._commandsList;
        }
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////
    // Overridden Package Implementation

    #region Methods

    /// <summary>
    ///     Initialization of the package; this method is called right after the
    ///     package is
    ///     sited, so this is the place where you can put all the initialization code
    ///     that
    ///     rely on services provided by VisualStudio.
    /// </summary>
    /// <seealso cref="M:Microsoft.VisualStudio.Shell.Package.Initialize()" />
    protected override void Initialize()
    {
        logger.Debug("Entered member.");

        base.Initialize();

        this._dte = this.GetService(typeof(DTE)) as DTE;

        if (this._dte == null)
        {
            throw new NullReferenceException("DTE is not available.");
        }
        if (this.Application == null)
        {
            throw new NullReferenceException("DTE not available as DTE2.");
        }

        //var _addInInstance = base.GetService(typeof(AddIn)) as AddIn;

        //_uiShell = base.GetService(typeof(IVsUIShell)) as IVsUIShell;
        //_events = _dte.Events;

        this.BuildMenu();
    }

    private void AddCommand(CommandBase command)
    {
        logger.Debug("Entered member.");

        this._mcs.AddCommand(command);
        this.CommandsList.Add(command);
    }

    private void BuildMenu()
    {
        logger.Debug("Entered member.");

        this._mcs = this.GetService(typeof(IMenuCommandService)) as
                    OleMenuCommandService;

        if (this._mcs == null)
        {
            return;
        }

        // The implementations I've seen in samples all simply skip the
        // menu implementation if the menu command service returns as
        // null rather than throwing an error. There may be circumstances
        // where BuildMenu might run where an error like this would prevent
        // otherwise acceptable use.
        //
        //  throw new InvalidOperationException("No menu command service found.");

        try
        {
            // ==================  VISUAL STUDIO CODE EDITOR MENU  ==================
            // Create ViewModel Command

            var menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.CreateViewModelFromSelectedClassCommand);
            this.AddCommand(
                new CreateViewModelCommandFromSelectedClassCommand(
                    this.Application,
                    menuCommandID));

            // Create AboutCommand Command

            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.AboutCommand);
            this.AddCommand(new AboutCommand(this.Application, menuCommandID));

            // ==================  VISUAL STUDIO XAML EDITOR MENU  ==================
            // Add root level context menu item

            // Select Container Control
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.SelectContainingControlCommand);
            this.AddCommand(new SelectContainingControlCommand(this.Application,
                            menuCommandID));

            // Widen Selection
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.WidenSelectionCommand);
            this.AddCommand(new WidenSelectionCommand(this.Application,
                            menuCommandID));

            // Narrow Selection
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.NarrowSelectionCommand);
            this.AddCommand(new NarrowSelectionCommand(this.Application,
                            menuCommandID));

            // Edit grid columns and rows
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.EditGridRowAndColumnsCommand);
            this.AddCommand(new EditGridRowAndColumnsCommand(this.Application,
                            menuCommandID));

            // Extract selected properties to style
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.ExtractSelectedPropertiesToStyleCommand);
            this.AddCommand(
                new ExtractSelectedPropertiesToStyleCommand(this.Application,
                        menuCommandID));

            // Create business form
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.CreateBusinessFormCommand);
            this.AddCommand(new CreateBusinessFormCommand(this.Application,
                            menuCommandID));

            // Create business form from selected class
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.CreateFormListViewDataGridFromSelectedClassCommand);
            this.AddCommand(
                new CreateFormListViewDataGridFromSelectedClassCommand(
                    this.Application,
                    menuCommandID));

            // Fields list from selected class
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.FieldsListFromSelectedClassCommand);
            this.AddCommand(
                new FieldsListFromSelectedClassCommand(this.Application, menuCommandID));

            // Remove all margins
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.RemoveMarginsCommand);
            this.AddCommand(new RemoveMarginsCommand(this.Application,
                            menuCommandID));

            // Set grid to auto layout
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.ChangeGridToFlowLayoutCommand);
            this.AddCommand(new ChangeGridToFlowLayout(this.Application,
                            menuCommandID));

            // Chainsaw
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.ChainsawDesignerExtraPropertiesCommand);
            this.AddCommand(
                new ChainsawDesignerExtraProperties(this.Application, menuCommandID));

            // GROUP INTO COMMANDS
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoCanvasCommand);
            this.AddCommand(new GroupIntoCanvas(this.Application, menuCommandID));

            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoDockPanelCommand);
            this.AddCommand(new GroupIntoDockPanel(this.Application, menuCommandID));

            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoGridCommand);
            this.AddCommand(new GroupIntoGrid(this.Application, menuCommandID));

            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoScrollViewerCommand);
            this.AddCommand(new GroupIntoScrollViewer(this.Application,
                            menuCommandID));

            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoStackPanelVerticalCommand);
            this.AddCommand(new GroupIntoStackPanelVertical(this.Application,
                            menuCommandID));

            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoStackPanelHorizontalCommand);
            this.AddCommand(new GroupIntoStackPanelHorizontal(this.Application,
                            menuCommandID));

            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoUniformGridCommand);
            this.AddCommand(new GroupIntoUniformGrid(this.Application,
                            menuCommandID));

            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoViewBoxCommand);
            this.AddCommand(new GroupIntoViewBox(this.Application, menuCommandID));

            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoWrapPanelCommand);
            this.AddCommand(new GroupIntoWrapPanel(this.Application, menuCommandID));

            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoGroupBoxCommand);
            this.AddCommand(new GroupIntoGroupBox(this.Application, menuCommandID));

            // GROUP INTO BORDER COMMANDS
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoBorderNoChildRootCommand);
            this.AddCommand(new GroupIntoBorderNoChildRoot(this.Application,
                            menuCommandID));

            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoBorderWithGridRootCommand);
            this.AddCommand(new GroupIntoBorderWithGridRoot(this.Application,
                            menuCommandID));

            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoBorderWithStackPanelVerticalRootCommand);
            this.AddCommand(
                new GroupIntoBorderWithStackPanelVerticalRoot(this.Application,
                        menuCommandID));

            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.GroupIntoBorderWithStackPanelHorizontalRootCommand);
            this.AddCommand(
                new GroupIntoBorderWithStackPanelHorizontalRoot(this.Application,
                        menuCommandID));

            // Tools menu commands
            menuCommandID = new CommandID(
                GuidList.guidXamlHelpmeetCmdSet,
                PkgCmdList.ControlDefaultsCommand);
            this.AddCommand(new ControlDefaultsCommand(this.Application,
                            menuCommandID));
        }
        catch (Exception ex)
        {
            UIUtilities.ShowExceptionMessage("BuildMenu Exception", ex.Message);
            logger.Debug("An exception occurred in BuildMenu().");
        }
    }

    #endregion
}
}