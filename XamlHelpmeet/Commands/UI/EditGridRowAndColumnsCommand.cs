// file:    Commands\UI\EditGridRowAndColumnsCommand.cs
//
// summary: Implements the edit grid row and columns command class

namespace XamlHelpmeet.Commands.UI
{
#region Imports

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

using EnvDTE;

using EnvDTE80;

using NLog;

using XamlHelpmeet.Extensions;
using XamlHelpmeet.UI.GridColumnAndRowEditor;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Utility;

using YoderZone.Extensions.NLog;

#endregion

/// <summary>
///     Edit grid row and columns command.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase" />
public class EditGridRowAndColumnsCommand : CommandBase
{
    #region Static Fields

    /// <summary>
    ///     The logger.
    /// </summary>
    private static readonly Logger logger = SettingsHelper.CreateLogger();

    #endregion

    #region Fields

    private List<string> _addedNamespaces;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    ///     Initializes a new instance of the EditGridRowAndColumnsCommand class.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="id">The id.</param>
    public EditGridRowAndColumnsCommand(DTE2 application, CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        this.Caption = "Edit Grid Columns and Rows";
        this.CommandName = "EditGridRowAndColumnsCommand";
        this.ToolTip = "Edit grid columns and rows.";
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    ///     Adds a name spaces to 'NamespaceManager'.
    /// </summary>
    /// <param name="xmlIn">
    ///     The XML in.
    /// </param>
    /// <param name="NamespaceManager">
    ///     Manager for namespace.
    /// </param>
    public void AddNameSpaces(string xmlIn,
                              XmlNamespaceManager NamespaceManager)
    {
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(xmlIn));
        logger.Debug("Entered member.");

        this.AddNameSpaces(xmlIn, NamespaceManager, this._addedNamespaces);
    }

    /// <summary>
    ///     Determine if we can execute.
    /// </summary>
    /// <param name="ExecuteOption">
    ///     The execute option.
    /// </param>
    /// <returns>
    ///     true if we can execute, otherwise false.
    /// </returns>
    public override bool CanExecute(vsCommandExecOption ExecuteOption)
    {
        logger.Debug("Entered member.");

        return base.CanExecute(ExecuteOption) && this.IsTextSelected();
    }

    /// <summary>
    ///     Executes this EditGridRowAndColumnsCommand.
    /// </summary>
    public override void Execute()
    {
        logger.Debug("Entered member.");

        try
        {
            if (this._addedNamespaces == null)
            {
                this._addedNamespaces = new List<string>();
            }
            else
            {
                this._addedNamespaces.Clear();
            }
            var selectedCodeBlock = this.Application.ActiveDocument.Selection as
                                    TextSelection;
            if (selectedCodeBlock == null)
            {
                throw new InvalidOperationException("selectionCodeBlock == null");
            }

            string XAML = selectedCodeBlock.Text.Trim(WhiteSpaceCharacters);

            // Modified to beaf up the selection test. Old test just insured
            // that the selection began with a starting grid tag, and ended
            // with an ending grid tag, whether or not they belonged to the
            // same element.

            if (selectedCodeBlock.IsNodeSelected("Grid") == false)
            {
                UIUtilities.ShowExceptionMessage(
                    "You must select a grid",
                    "You must select a Grid control for this command.");
                return;
            }
            var nameTable = new NameTable();
            var nameSpaceManager = new XmlNamespaceManager(nameTable);
            this.AddNameSpaces(XAML, nameSpaceManager);
            var xmlParserContent = new XmlParserContext(
                null,
                nameSpaceManager,
                null,
                XmlSpace.None);
            var document = new XmlDocument { PreserveWhitespace = true, XmlResolver = null };
            var XMLSettings = new XmlReaderSettings
            {
                ValidationFlags =
                XmlSchemaValidationFlags.None,
                ValidationType = ValidationType.None
            };
            using (
                XmlReader reader = XmlReader.Create(
                                       new StringReader(XAML),
                                       XMLSettings,
                                       xmlParserContent))
            {
                document.Load(reader);
            }
            var gridRowColumnEditForm = new GridRowColumnEditWindow(document);
            if (gridRowColumnEditForm.ShowDialog() ?? false)
            {
                var sb = new StringBuilder(10240);
                var writerSettings = new XmlWriterSettings
                {
                    Indent = true,
                    NewLineOnAttributes = false,
                    OmitXmlDeclaration = true
                };
                using (XmlWriter writer = XmlWriter.Create(sb, writerSettings))
                {
                    gridRowColumnEditForm.UsersXamlDocument.Save(writer);
                }
                foreach (var item in this._addedNamespaces)
                {
                    sb.Replace(item, string.Empty);
                }
                sb.Replace(" >", ">");
                sb.Replace("Tag=\"New\"", string.Empty);
                sb.Replace("    ", " ");
                sb.Replace("   ", " ");
                sb.Replace("  ", " ");
                // Added a reformatting step to the process.
                selectedCodeBlock.ReplaceSelectedText(sb.ToString());
            }
        }
        catch (Exception ex)
        {
            UIUtilities.ShowExceptionMessage(this.Caption, ex.Message);
            logger.Error("An exception was raised in Execute().", ex);
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

        return this.IsTextSelected() ? base.GetStatus() :
               vsCommandStatus.vsCommandStatusSupported;
    }

    #endregion
}
}