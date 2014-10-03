// file:    Commands\UI\CreateBusinessFormCommand.cs
//
// summary: Implements the create business form command class

namespace XamlHelpmeet.Commands.UI
{
#region Imports

using System;
using System.ComponentModel.Design;

using EnvDTE;

using EnvDTE80;

using NLog;

using XamlHelpmeet.Model;
using XamlHelpmeet.UI.CreateBusinessForm;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Utility;

using YoderZone.Extensions.NLog;

#endregion

/// <summary>
///     Create business form command.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase" />
public class CreateBusinessFormCommand : CommandBase
{
    #region Static Fields

    /// <summary>
    ///     The logger.
    /// </summary>
    private static readonly Logger logger = SettingsHelper.CreateLogger();

    #endregion

    #region Constructors and Destructors

    /// <summary>
    ///     Initializes a new instance of the CreateBusinessFormCommand class.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="id">The id.</param>
    public CreateBusinessFormCommand(DTE2 application, CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        this.Caption = "Create Business Form";
        this.CommandName = "CreateBusinessFormCommand";
        this.ToolTip = "Create business form";
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    ///     Executes this CreateBusinessFormCommand.
    /// </summary>
    public override void Execute()
    {
        logger.Debug("Entered member.");

        try
        {
            ClassEntity classEntity = null;
            string[] Guids = PtHelpers.GetProjectTypeGuids(
                                 this.Application.ActiveDocument.ProjectItem.ContainingProject)
                             .Split(';');
            if (PtHelpers.IsProjectSilverlight(Guids))
            {
                classEntity = new ClassEntity(string.Empty, true)
                {
                    SilverlightVersion =
                        this.Application
                        .ActiveDocument
                        .ProjectItem
                        .ContainingProject
                        .Properties.Item(
                            "TargetFrameworkMoniker")
                        .Value.ToString()
                        .Replace(
                            "Silverlight,Version=v",
                            string.Empty)
                };
            }

            var createBusinessFormWindow = new CreateBusinessFormWindow(classEntity);
            bool? result = createBusinessFormWindow.ShowDialog();
            if (!(result ?? false))
            {
                return;
            }

            var ts = this.Application.ActiveDocument.Selection as TextSelection;
            if (ts == null)
            {
                throw new Exception("ts is null.");
            }

            ts.Insert(createBusinessFormWindow.BusinessForm);
        }
        catch (Exception ex)
        {
            UIUtilities.ShowExceptionMessage(this.Caption, ex.Message);
            logger.Debug("An exception was raised in Execute().", ex);
        }
    }

    /*    /// <summary>
        ///     Gets the status.
        /// </summary>
        /// <returns>
        ///     The status.
        /// </returns>
        public override vsCommandStatus GetStatus()
        {
            logger.Debug("Entered member.");
            // This command is supported and always enabled.
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            return vsCommandStatus.vsCommandStatusSupported |
                   vsCommandStatus.vsCommandStatusEnabled;
        }*/

    #endregion
}
}