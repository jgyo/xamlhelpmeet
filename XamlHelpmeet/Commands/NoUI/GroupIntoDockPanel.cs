// file:    Commands\NoUI\GroupIntoDockPanel.cs
//
// summary: Implements the group into dock panel class
using System;

using EnvDTE;
using EnvDTE80;
using XamlHelpmeet.UI.Utilities;
using System.ComponentModel.Design;

namespace XamlHelpmeet.Commands.NoUI
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
///     Panel for editing the group into dock.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
public class GroupIntoDockPanel : CommandBase
{
    /// <summary>
    /// The logger.
    /// </summary>
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the GroupIntoDockPanel class.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="id">The id.</param>
    public GroupIntoDockPanel(DTE2 application, CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        Caption = "Group into DockPanel";
        CommandName = "GroupIntoDockPanel";
        ToolTip = "Group selection into a DockPanel.";
    }

    #endregion

    #region Method

    /// <summary>
    ///     Determine if we can execute.
    /// </summary>
    /// <param name="executeOption">
    ///     The execute option.
    /// </param>
    /// <returns>
    ///     true if we can execute, otherwise false.
    /// </returns>
    public override bool CanExecute(vsCommandExecOption executeOption)
    {
        logger.Debug("Entered member.");

        return base.CanExecute(executeOption) && IsTextSelected();
    }

    /// <summary>
    ///     Executes this GroupIntoDockPanel.
    /// </summary>
    public override void Execute()
    {
        logger.Debug("Entered member.");

        try
        {
            GroupInto("<DockPanel>\r\n", "</DockPanel>\r\n");
        }
        catch (Exception ex)
        {
            UIUtilities.ShowExceptionMessage("Group Into " + Caption, ex.Message);
            logger.Debug("An exception was raised in GroupIntoDockPanel::Execute.",
                         ex);
        }
    }

    /// <summary>
    ///     Gets the status.
    /// </summary>
    /// <returns>
    ///     The status.
    /// </returns>
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

    #endregion

}
}