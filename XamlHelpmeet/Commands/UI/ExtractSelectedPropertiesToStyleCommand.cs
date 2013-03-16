// file:	Commands\UI\ExtractSelectedPropertiesToStyleCommand.cs
//
// summary:	Implements the extract selected properties to style command class
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using EnvDTE;
using EnvDTE80;
using XamlHelpmeet.UI;
using XamlHelpmeet.UI.ExtractPropertiesToStyle;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Utility;
 using XamlHelpmeet.Extensions;
using System.ComponentModel.Design;

namespace XamlHelpmeet.Commands.UI
{
	/// <summary>
	/// 	Extract selected properties to style command.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class ExtractSelectedPropertiesToStyleCommand : CommandBase
	{
		#region Fields

		private List<string> _addedNamespaces;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ExtractSelectedPropertiesToStyleCommand
		/// class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public ExtractSelectedPropertiesToStyleCommand(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "Extract Properties to Style";
			CommandName = "ExtractSelectedPropertiesToStyleCommand";
			ToolTip = "Extract selected properties to style.";
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// 	Determine if we can execute.
		/// </summary>
		/// <param name="ExecuteOption">
		/// 	The execute option.
		/// </param>
		/// <returns>
		/// 	true if we can execute, otherwise false.
		/// </returns>
		public override bool CanExecute(vsCommandExecOption ExecuteOption)
		{
			return base.CanExecute(ExecuteOption) && IsTextSelected();
		}

		/// <summary>
		/// 	Executes this ExtractSelectedPropertiesToStyleCommand.
		/// </summary>
		public override void Execute()
		{
			try
			{
				if (_addedNamespaces == null)
				{
					_addedNamespaces = new List<string>();
				}
				else
				{
					_addedNamespaces.Clear();
				}
				var selectedCodeBlock = Application.ActiveDocument.Selection as TextSelection;
				var XAML = selectedCodeBlock.Text.Trim(WhiteSpaceCharacters);
				var nameTable = new NameTable();
				var nameSpaceManager = new XmlNamespaceManager(nameTable);
				AddNameSpaces(XAML, nameSpaceManager);

				var xmlParseContext = new XmlParserContext(null, nameSpaceManager, null, XmlSpace.None);
				var document = new XmlDocument();
				document.PreserveWhitespace = true;
				document.XmlResolver = null;

				var xmlSettings = new XmlReaderSettings();
				xmlSettings.ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.None;
				xmlSettings.ValidationType = ValidationType.None;

				using (var reader = XmlReader.Create(new StringReader(XAML), xmlSettings, xmlParseContext))
				{
					document.Load(reader);
				}

				var isSilverlight = PtHelpers.IsProjectSilverlight(PtHelpers.GetProjectTypeGuids(Application.SelectedItems.Item(1).ProjectItem.ContainingProject).Split(';'));
				var silverlightVersion = string.Empty;
				if (isSilverlight)
				{
					silverlightVersion = Application.ActiveDocument.ProjectItem.ContainingProject.Properties.Item("TargetFrameworkMoniker").Value.ToString().Replace("Silverlight,Version=v", String.Empty);
				}

				var extractSelectedPropertiesToStyle = new ExtractSelectedPropertiesToStyleWindow(document, isSilverlight, silverlightVersion);
				var result = extractSelectedPropertiesToStyle.ShowDialog();

				if (result ?? false)
				{
					var sb = new StringBuilder(10240);
					var writerSettings = new XmlWriterSettings()
					{
						Indent = true,
						NewLineOnAttributes = false,
						OmitXmlDeclaration = true
					};

					using (var writer = XmlWriter.Create(sb, writerSettings))
					{
						extractSelectedPropertiesToStyle.Document.Save(writer);
					}

					foreach (string item in _addedNamespaces)
					{
						sb.Replace(item, string.Empty);
					}

					sb.Replace(" >", ">");
					sb.Replace("    ", " ");
					sb.Replace("   ", " ");
					sb.Replace("  ", " ");

					var editPoint = selectedCodeBlock.TopPoint.CreateEditPoint();
					selectedCodeBlock.Delete();
					editPoint.Insert(sb.ToString());
					sb.Length = 0;

					sb.AppendFormat(
						isSilverlight ? "<Style TargetType=\"{0}\"" : "<Style TargetType=\"{{x:Type {0}}}\"",
						extractSelectedPropertiesToStyle.TypeName);

					sb.AppendFormat(extractSelectedPropertiesToStyle.StyleName.IsNotNullOrEmpty() ?
						" x:Key=\"{0}\">" : ">",
						extractSelectedPropertiesToStyle.StyleName
						);

					sb.Append(Environment.NewLine);

					foreach (var item in extractSelectedPropertiesToStyle.ExtractedProperties)
					{
						if (item.IsSelected)
						{
							sb.AppendFormat("<Setter Property=\"{0}\" Value=\"{1}\" />{2}", item.PropertyName, item.PropertyValue, Environment.NewLine);
						}
					}

					sb.AppendLine("</Style>");
					Clipboard.Clear();
					Clipboard.SetText(sb.ToString());
					UIUtilities.ShowInformationMessage("Paste Style", "Place insertion point and paste created style into the resource section of a XAML document.");
				}
			}
			catch (XmlException ex)
			{
				UIUtilities.ShowExceptionMessage("Paste Style",
					"Place insertion point and paste created style into the resource section of a XAML document.",
					string.Empty,
					ex.ToString());
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage(Caption, ex.Message, string.Empty, ex.ToString());
			}
		}

		/// <summary>
		/// 	Gets the status.
		/// </summary>
		/// <returns>
		/// 	The status.
		/// </returns>
		public override vsCommandStatus GetStatus()
		{
			// vsCommandStatus.vsCommandStatusUnsupported has a value
			// of zero, so or'ing it with any other value returns the other
			// value.
			return vsCommandStatus.vsCommandStatusSupported |
				(IsTextSelected() ?
				vsCommandStatus.vsCommandStatusEnabled :
				vsCommandStatus.vsCommandStatusUnsupported);
		}

		private void AddNameSpaces(string XMLIn, XmlNamespaceManager NameSpaceManager)
		{
			AddNameSpaces(XMLIn, NameSpaceManager, _addedNamespaces);
		}

		#endregion Methods
	}
}