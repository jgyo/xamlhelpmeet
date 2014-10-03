// file:    Commands\NoUI\ChainsawDesignerExtraProperties.cs
//
// summary: Implements the chainsaw designer extra properties class
using System;
using System.Text;
using EnvDTE80;
using EnvDTE;
using System.Windows;
using XamlHelpmeet.UI.Utilities;
using System.ComponentModel.Design;

namespace XamlHelpmeet.Commands.NoUI
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
///     Chainsaw designer extra properties.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
public class ChainsawDesignerExtraProperties : CommandBase
{
    /// <summary>
    /// The logger.
    /// </summary>
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the ChainsawDesignerExtraProperties class.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="id">The id.</param>
    public ChainsawDesignerExtraProperties(DTE2 application, CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        Caption = "Chainsaw - Minimize Designer XAML";
        CommandName = "ChainsawDesignerExtraPropertiesCommand";
        ToolTip = "Use this function only on designer generated XAML.  Warning this thing is a chainsaw!";
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
            if (MessageBox.Show(
                        "This chainsaw will remove all Margins, MinHeights, MinWidths, x:Name and Name properties in your selection, and set all rows and columns to Auto size.",
                        "Are you sure?",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question,
                        MessageBoxResult.No) != MessageBoxResult.Yes)
            {
                return;
            }

            var selectedCodeBlock = this.Application.ActiveDocument.Selection as
                                    TextSelection;

            if (selectedCodeBlock == null)
            {
                return;
            }

            var sb = new StringBuilder(selectedCodeBlock.Text.Trim(
                                           WhiteSpaceCharacters));
            this.StripUnwantedProperty("Margin", sb);
            this.StripUnwantedProperty("MinHeight", sb);
            this.StripUnwantedProperty("MinWidth", sb);
            this.StripUnwantedProperty("x:Name", sb);
            this.StripUnwantedProperty("Name", sb);
            this.SetAllRowsAndColumnsToAuto(sb);
            var editPoint = selectedCodeBlock.TopPoint.CreateEditPoint();
            selectedCodeBlock.Delete();
            editPoint.Insert(sb.ToString());
        }
        catch (Exception ex)
        {
            UIUtilities.ShowExceptionMessage(Caption, ex.Message);
            logger.Debug("Exception raised running Chainsaw.", ex);
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
