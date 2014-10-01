// file:    Commands\UI\CreateFormListViewDataGridFromSelectedClassCommand.cs
//
// summary: Implements the create form list view data grid from selected class command class

namespace XamlHelpmeet.Commands.UI
{
#region Imports

using System;
using System.ComponentModel.Design;
using System.Windows;

using EnvDTE;

using EnvDTE80;

using NLog;

using XamlHelpmeet.Model;
using XamlHelpmeet.UI.DynamicForm;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Utility;

using YoderZone.Extensions.NLog;

#endregion

/// <summary>
///     Create form list view data grid from selected class command.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase" />
public class CreateFormListViewDataGridFromSelectedClassCommand :
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
    ///     CreateFormListViewDataGridFromSelectedClassCommand class.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="id">The id.</param>
    public CreateFormListViewDataGridFromSelectedClassCommand(
        DTE2 application, CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        this.Caption = "Create Form, ListView or DataGrid From Selected Class";
        this.CommandName = "CreateFormListViewDataGridFromSelectedClassCommand";
        this.ToolTip = "Create Form, ListView or DataGrid From Selected Class";
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    ///     Executes this CreateFormListViewDataGridFromSelectedClassCommand.
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

            if (classEntity == null)
            {
                return;
            }

            var obj = new CreateBusinessFormFromClassWindow(classEntity);
            logger.Trace("obj: {0}", obj);

            bool? result = obj.ShowDialog();
            logger.Trace("result: {0}", result);

            if (result ?? false)
            {
                try
                {
                    Clipboard.Clear();
                    Clipboard.SetText(obj.BusinessForm);
                }
                catch //(Exception ex)
                {
                    // Had to do this to avoid useless exception message
                    // when running this code in a VPC, since Vista &
                    // VPC and the Clipboard don't play nice together
                    // sometimes.
                    // the operation works, you just get an exception
                    // for no reason.
                }
            }
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
            return vsCommandStatus.vsCommandStatusUnsupported
                   & vsCommandStatus.vsCommandStatusInvisible;
        }

        return vsCommandStatus.vsCommandStatusEnabled |
               vsCommandStatus.vsCommandStatusSupported;
    }

    #endregion
}
}