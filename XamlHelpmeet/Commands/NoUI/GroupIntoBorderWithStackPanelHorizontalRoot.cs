using EnvDTE;
using EnvDTE80;
using System;
using System.ComponentModel.Design;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.Commands.NoUI
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
///     Group into border with stack panel horizontal root.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
public class GroupIntoBorderWithStackPanelHorizontalRoot : CommandBase
{
    /// <summary>
    /// The logger.
    /// </summary>
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the GroupIntoBorderWithStackPanelHorizontalRoot
    /// class.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="id">The id.</param>
    public GroupIntoBorderWithStackPanelHorizontalRoot(DTE2 application,
            CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        Caption = "Border With Root StackPanel - Horizontall";
        CommandName = "GroupIntoBorderWithStackPanelHorizontalRoot";
        ToolTip = "Group selection into a border with root stackpanel horizontal being added.";
    }

    #endregion

    /// <summary>
    ///     Determine if we can execute.
    /// </summary>
    /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.CanExecute(vsCommandExecOption)"/>
    public override bool CanExecute(vsCommandExecOption executeOption)
    {
        logger.Debug("Entered member.");

        return base.CanExecute(executeOption) && IsTextSelected();
    }

    /// <summary>
    ///     Executes this CommandBase.
    /// </summary>
    /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.Execute()"/>
    public override void Execute()
    {
        logger.Debug("Entered member.");

        try
        {
            GroupInto("<Border>\r\n<StackPanel Orientation=\"Horizontal\">\r\n",
                      "</StackPanel>\r\n</Border>\r\n");
        }
        catch (Exception ex)
        {
            UIUtilities.ShowExceptionMessage("Group Into " + Caption, ex.Message);
            logger.Debug("An exception was raised in GroupIntoBorderWithStackPanelHorizontalRoot::Execute.",
                         ex);
        }
    }

    /// <summary>
    ///     Gets the status.
    /// </summary>
    /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.GetStatus()"/>
    public override vsCommandStatus GetStatus()
    {
        logger.Debug("Entered member.");

        // This will add vsCommandStatusEnabled to vsCommandStatusSupported,
        // if IsTextSelected() returns true. Otherwise or'ing with
        // vsCommandStatusUnsupported leaves vsCommandStatusSupported
        // unchanged.
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags

        return IsTextSelected() ? base.GetStatus() :
               vsCommandStatus.vsCommandStatusSupported;
    }
}
}