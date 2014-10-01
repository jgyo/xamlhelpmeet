namespace XamlHelpmeet.Commands.UI
{
#region Imports

using System;
using System.ComponentModel.Design;

using EnvDTE;

using EnvDTE80;

using NLog;

using XamlHelpmeet.Model;
using XamlHelpmeet.UI;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Utility;

using YoderZone.Extensions.NLog;

#endregion

/// <summary>
///     Fields list from selected class command.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase" />
public class FieldsListFromSelectedClassCommand : CommandBase
{
    #region Static Fields

    /// <summary>
    ///     The logger.
    /// </summary>
    private static readonly Logger logger = SettingsHelper.CreateLogger();

    #endregion

    #region Constructors and Destructors

    /// <summary>
    ///     Initializes a new instance of the FieldsListFromSelectedClassCommand class.
    /// </summary>
    /// <param name="application">
    ///     The application.
    /// </param>
    /// <param name="id">
    ///     The id.
    /// </param>
    public FieldsListFromSelectedClassCommand(DTE2 application, CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        this.Caption = "Fields List For Selected Class";
        this.CommandName = "FieldsListFromSelectedClassCommand";
        this.ToolTip = "Show fields list for selected class";
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    ///     Executes this FieldsListFromSelectedClassCommand.
    /// </summary>
    /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.Execute()" />
    public override void Execute()
    {
        logger.Debug("Entered member.");

        try
        {
            var remoteTypeReflector = new RemoteTypeReflector();
            ClassEntity classEntity =
                remoteTypeReflector.GetClassEntityFromSelectedClass(
                    this.Application.SelectedItems.Item(1)
                    .ProjectItem.ContainingProject,
                    this.Caption);

            if (classEntity != null && classEntity.Success)
            {
                var obj = new FieldsListWindow(classEntity);
                obj.Topmost = true;
                obj.Show();
            }
        }
        catch (Exception ex)
        {
            UIUtilities.ShowExceptionMessage(this.Caption, ex.Message);
            logger.Error("An exception was raised in Execute().", ex);
        }
    }

    /// <summary>
    ///     Gets the status of the FieldsListFromSelectedClassCommand command.
    /// </summary>
    /// <remarks>
    ///     FieldsListFromSelectedClassCommand is always supported. If text is
    ///     selected, the
    ///     command is enabled.
    /// </remarks>
    /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.GetStatus()" />
    public override vsCommandStatus GetStatus()
    {
        logger.Debug("Entered member.");
        // This command is supported and always enabled.
        return vsCommandStatus.vsCommandStatusSupported |
               vsCommandStatus.vsCommandStatusEnabled;
    }

    #endregion
}
}