// file:    Commands\UI\CreateViewModelCommandFromSelectedClassCommand.cs
//
// summary: Implements the create view model command from selected class command class

namespace XamlHelpmeet.Commands.UI
{
#region Imports

using System;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Forms;

using EnvDTE;

using EnvDTE80;

using NLog;

using XamlHelpmeet.Model;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.UI.ViewModelCreation;
using XamlHelpmeet.Utility;

using YoderZone.Extensions.NLog;

#endregion

/// <summary>
///     Create view model command from selected class command.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase" />
internal class CreateViewModelCommandFromSelectedClassCommand :
    CommandBase
{
    #region Static Fields

    /// <summary>
    ///     The logger.
    /// </summary>
    private static readonly Logger logger = SettingsHelper.CreateLogger();

    #endregion

    #region Constructors and Destructors

    /// <summary>
    ///     Initializes a new instance of the
    ///     CreateViewModelCommandFromSelectedClassCommand
    ///     class.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="id">The id.</param>
    public CreateViewModelCommandFromSelectedClassCommand(DTE2 application,
            CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        this.Caption = "Create ViewModel for Class";
        this.CommandName = "CreateViewModelCommandFromSelectedClassCommand";
        this.ToolTip = "Create ViewModel for class.";
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    ///     Executes this CreateViewModelCommandFromSelectedClassCommand.
    /// </summary>
    public override void Execute()
    {
        logger.Debug("Entered member.");

        try
        {
            var remoteTypeReflector = new RemoteTypeReflector();
            logger.Trace("remoteTypeReflector: {0}", remoteTypeReflector);

            ClassEntity classEntity =
                remoteTypeReflector.GetClassEntityFromSelectedClass(
                    this.Application.SelectedItems.Item(1)
                    .ProjectItem.ContainingProject,
                    this.Caption);
            logger.Trace("classEntity: {0}", classEntity);

            if (classEntity != null)
            {
                var obj = new CreateViewModelWindow(
                    classEntity,
                    this.Application.ActiveWindow.Caption.EndsWith(".vb"));
                logger.Trace("obj: {0}", obj);

                bool? result = obj.ShowDialog();
                logger.Trace("result: {0}", result);

                if (result ?? false)
                {
                    try
                    {
                        Clipboard.Clear();
                        Clipboard.SetText(obj.ViewModelText);
                    }
                    catch(Exception ex)
                    {
                        logger.Error(ex.Message, ex);
                        // throw;
                        // Had to do this to avoid useless exception message when running this code in a
                        // VPC, since Vista & VPC and the Clipboard don't play nice together sometimes.
                        // the operation works, you just get an exception for no reason.
                    }

                    UIUtilities.ShowInformationMessage(
                        "Ready to Paste",
                        "Position cursor inside a ViewModel file and execute a paste operation.");
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            logger.Error(ex.Message, ex);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            throw;
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

        if (this.Application.ActiveDocument == null
                || !(this.Application.ActiveDocument.Name.EndsWith("vb")
                     || this.Application.ActiveDocument.Name.EndsWith("cs")))
        {
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            return vsCommandStatus.vsCommandStatusUnsupported
                   | vsCommandStatus.vsCommandStatusInvisible;
        }

        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        return vsCommandStatus.vsCommandStatusEnabled |
               vsCommandStatus.vsCommandStatusSupported;
    }

    #endregion
}
}