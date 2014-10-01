// file:    Commands\UI\ControlDefaultsCommand.cs
//
// summary: Implements the control defaults command class

namespace XamlHelpmeet.Commands.UI
{
#region Imports

using System.ComponentModel.Design;

using EnvDTE80;

using NLog;

using XamlHelpmeet.UI;

using YoderZone.Extensions.NLog;

#endregion

/// <summary>
///     Control defaults command.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase" />
public class ControlDefaultsCommand : CommandBase
{
    #region Static Fields

    /// <summary>
    ///     The logger.
    /// </summary>
    private static readonly Logger logger = SettingsHelper.CreateLogger();

    #endregion

    #region Constructors and Destructors

    /// <summary>
    ///     Initializes a new instance of the ControlDefaultsCommand class.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="id">The id.</param>
    public ControlDefaultsCommand(DTE2 application, CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        this.Caption = "Set Control Defaults";
        this.CommandName = "SetControlDefaultsCommand";
        this.ToolTip =
            "Set control defaults for controls created by this software.";
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    ///     Executes this ControlDefaultsCommand.
    /// </summary>
    public override void Execute()
    {
        logger.Debug("Entered member.");

        var obj = new UIControlDefaultsWindow();
        obj.ShowDialog();
        obj = null;
    }

    #endregion
}
}