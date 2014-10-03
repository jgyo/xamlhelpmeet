// file:    Commands\NoUI\GroupIntoBorderWithGridRoot.cs
//
// summary: Implements the group into border with grid root class
using System;

using EnvDTE80;
using EnvDTE;
using XamlHelpmeet.UI.Utilities;
using System.ComponentModel.Design;
using XamlHelpmeet.Utility;

namespace XamlHelpmeet.Commands.NoUI
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
///     Group into border with grid root.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
public class GroupIntoBorderWithGridRoot : CommandBase
{
    /// <summary>
    /// The logger.
    /// </summary>
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the GroupIntoBorderWithGridRoot class.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="id">The id.</param>
    public GroupIntoBorderWithGridRoot(DTE2 application, CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        Caption = "Border with Root Grid";
        CommandName = "GroupIntoBorderWithGridRoot";
        ToolTip = "Group selection into a border with root grid being added.";
    }

    #endregion

    #region Methods

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
            var selectedCodeBlock = Application.ActiveDocument.Selection as
                                    TextSelection;

            if (selectedCodeBlock.AreSiblingsSelected() == false)
            {
                UIUtilities.ShowExceptionMessage("Invalid Selection",
                                                 "Your selection must include complete controls with their opening and ending nodes.");
                return;
            }

            GroupInto("<Border>\r\n<Grid>\r\n", "</Grid>\r\n</Border>\r\n");
        }
        catch (Exception ex)
        {
            UIUtilities.ShowExceptionMessage(String.Format("Group Into {0}", Caption),
                                             ex.Message);
            logger.Debug("An exception was raised in GroupIntoBorderWithGridRoot::Execute.",
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

    #endregion

}
}
