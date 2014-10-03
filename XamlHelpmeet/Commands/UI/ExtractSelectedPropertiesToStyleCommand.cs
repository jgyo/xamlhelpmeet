// file:    Commands\UI\ExtractSelectedPropertiesToStyleCommand.cs
//
// summary: Implements the extract selected properties to style command class

namespace XamlHelpmeet.Commands.UI
{
#region Imports

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;

using EnvDTE;

using EnvDTE80;

using NLog;

using XamlHelpmeet.Extensions;
using XamlHelpmeet.UI.ExtractPropertiesToStyle;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Utility;

using YoderZone.Extensions.NLog;

#endregion

/// <summary>
///     Extract selected properties to style command.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase" />
public class ExtractSelectedPropertiesToStyleCommand : CommandBase
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
    ///     Initializes a new instance of the ExtractSelectedPropertiesToStyleCommand
    ///     class.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="id">The id.</param>
    public ExtractSelectedPropertiesToStyleCommand(DTE2 application,
            CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        this.Caption = "Extract Properties to Style";
        this.CommandName = "ExtractSelectedPropertiesToStyleCommand";
        this.ToolTip = "Extract selected properties to style.";
    }

    #endregion

    #region Public Methods and Operators

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
    ///     Executes this ExtractSelectedPropertiesToStyleCommand.
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
            string XAML = selectedCodeBlock.Text.Trim(WhiteSpaceCharacters);
            var nameTable = new NameTable();
            var nameSpaceManager = new XmlNamespaceManager(nameTable);
            this.AddNameSpaces(XAML, nameSpaceManager);

            var xmlParseContext = new XmlParserContext(
                null,
                nameSpaceManager,
                null,
                XmlSpace.None);
            var document = new XmlDocument();
            document.PreserveWhitespace = true;
            document.XmlResolver = null;

            var xmlSettings = new XmlReaderSettings();
            xmlSettings.ValidationFlags = XmlSchemaValidationFlags.None;
            xmlSettings.ValidationType = ValidationType.None;

            using (
                XmlReader reader = XmlReader.Create(
                                       new StringReader(XAML),
                                       xmlSettings,
                                       xmlParseContext))
            {
                document.Load(reader);
            }

            bool isSilverlight =
                PtHelpers.IsProjectSilverlight(
                    PtHelpers.GetProjectTypeGuids(
                        this.Application.SelectedItems.Item(1)
                        .ProjectItem.ContainingProject)
                    .Split(';'));
            string silverlightVersion = string.Empty;
            if (isSilverlight)
            {
                silverlightVersion = this.Application.ActiveDocument.ProjectItem
                                     .ContainingProject.Properties.Item(
                                         "TargetFrameworkMoniker")
                                     .Value.ToString()
                                     .Replace("Silverlight,Version=v", String.Empty);
            }

            var extractSelectedPropertiesToStyle =
                new ExtractSelectedPropertiesToStyleWindow(
                document,
                isSilverlight,
                silverlightVersion);
            bool? result = extractSelectedPropertiesToStyle.ShowDialog();

            if (result ?? false)
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
                    extractSelectedPropertiesToStyle.Document.Save(writer);
                }

                foreach (var item in this._addedNamespaces)
                {
                    sb.Replace(item, string.Empty);
                }

                sb.Replace(" >", ">");
                sb.Replace("    ", " ");
                sb.Replace("   ", " ");
                sb.Replace("  ", " ");

                EditPoint editPoint = selectedCodeBlock.TopPoint.CreateEditPoint();
                selectedCodeBlock.Delete();
                editPoint.Insert(sb.ToString());
                sb.Length = 0;

                sb.AppendFormat(
                    isSilverlight
                    ? "<Style TargetType=\"{0}\""
                    : "<Style TargetType=\"{{x:Type {0}}}\"",
                    extractSelectedPropertiesToStyle.TypeName);

                sb.AppendFormat(
                    extractSelectedPropertiesToStyle.StyleName.IsNotNullOrEmpty()
                    ? " x:Key=\"{0}\">"
                    : ">",
                    extractSelectedPropertiesToStyle.StyleName);

                sb.Append(Environment.NewLine);

                foreach (var item in extractSelectedPropertiesToStyle.ExtractedProperties)
                {
                    if (item.IsSelected)
                    {
                        sb.AppendFormat(
                            "<Setter Property=\"{0}\" Value=\"{1}\" />{2}",
                            item.PropertyName,
                            item.PropertyValue,
                            Environment.NewLine);
                    }
                }

                sb.AppendLine("</Style>");
                Clipboard.Clear();
                Clipboard.SetText(sb.ToString());
                UIUtilities.ShowInformationMessage(
                    "Paste Style",
                    "Place insertion point and paste created style into the resource section of a XAML document.");
            }
        }
        catch (XmlException ex)
        {
            UIUtilities.ShowExceptionMessage(
                "Paste Style",
                "Place insertion point and paste created style into the resource section of a XAML document.");
            logger.Error("An XmlException was raised in Execute().", ex);
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
        // vsCommandStatus.vsCommandStatusUnsupported has a value
        // of zero, so or'ing it with any other value returns the other
        // value.
        return this.IsTextSelected() ? base.GetStatus() :
               vsCommandStatus.vsCommandStatusSupported;
    }

    #endregion

    #region Methods

    private void AddNameSpaces(string XMLIn,
                               XmlNamespaceManager NameSpaceManager)
    {
        logger.Debug("Entered member.");

        this.AddNameSpaces(XMLIn, NameSpaceManager, this._addedNamespaces);
    }

    #endregion
}
}