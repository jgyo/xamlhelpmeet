// file:	Commands\UI\EditGridRowAndColumnsCommand.cs
//
// summary:	Implements the edit grid row and columns command class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;
using EnvDTE;
using System.Xml;
using XamlHelpmeet.UI.Utilities;
using System.IO;
using XamlHelpmeet.UI;
using System.Xml.Schema;
using XamlHelpmeet.UI.GridColumnAndRowEditor;

namespace XamlHelpmeet.Commands.UI
{
	/// <summary>
	/// 	Edit grid row and columns command.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class EditGridRowAndColumnsCommand : CommandBase
	{

		#region Fields

		private List<string> _addedNamespaces;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the EditGridRowAndColumnsCommand class.
		/// </summary>
		/// <param name="application">The application.</param>
		public EditGridRowAndColumnsCommand(DTE2 application)
			: base(application)
		{
			Caption = "Edit Grid Columns and Rows";
			CommandName = "EditGridRowAndColumnsCommand";
			ToolTip = "Edit grid columns and rows.";
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Adds a name spaces to 'NamespaceManager'.
		/// </summary>
		/// <param name="XMLIn">
		/// 	The XML in.
		/// </param>
		/// <param name="NamespaceManager">
		/// 	Manager for namespace.
		/// </param>
		public void AddNameSpaces(string XMLIn, XmlNamespaceManager NamespaceManager)
		{
			base.AddNameSpaces(XMLIn, NamespaceManager, _addedNamespaces);
		}

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
		/// 	Executes this EditGridRowAndColumnsCommand.
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
				if (!XAML.StartsWith("<Grid", StringComparison.InvariantCultureIgnoreCase) || !XAML.EndsWith("</Grid>", StringComparison.InvariantCultureIgnoreCase))
				{
					UIUtilities.ShowExceptionMessage("You must select a grid", "Your selection must begin and end with Grid tags.");
					return;
				}
				var nameTable = new NameTable();
				var nameSpaceManager = new XmlNamespaceManager(nameTable);
				AddNameSpaces(XAML, nameSpaceManager);
				var xmlParserContent = new XmlParserContext(null, nameSpaceManager, null, XmlSpace.None);
				var document = new XmlDocument() { PreserveWhitespace = true, XmlResolver = null };
				var XMLSettings = new XmlReaderSettings() { ValidationFlags = XmlSchemaValidationFlags.None, ValidationType = ValidationType.None };
				using (var reader = XmlReader.Create(new StringReader(XAML), XMLSettings, xmlParserContent))
				{
					document.Load(reader);
				}
				var gridRowColumnEditForm = new GridRowColumnEditWindow(document);
				if (gridRowColumnEditForm.ShowDialog() ?? false)
				{
					var sb = new StringBuilder(10240);
					var writerSettings = new XmlWriterSettings() { Indent = true, NewLineOnAttributes = false, OmitXmlDeclaration = true };
					using (var writer = XmlWriter.Create(sb, writerSettings))
					{
						gridRowColumnEditForm.Document.Save(writer);
					}
					foreach (string item in _addedNamespaces)
					{
						sb.Replace(item, string.Empty);
					}
					sb.Replace(" >", ">");
					sb.Replace("Tag=\"New\"", string.Empty);
					sb.Replace("    ", " ");
					sb.Replace("   ", " ");
					sb.Replace("  ", " ");
					var editPoint = selectedCodeBlock.TopPoint.CreateEditPoint();
					selectedCodeBlock.Delete();
					editPoint.Insert(sb.ToString());
				}
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
			if (IsTextSelected())
			{
				return vsCommandStatus.vsCommandStatusEnabled | vsCommandStatus.vsCommandStatusSupported;
			}
			return vsCommandStatus.vsCommandStatusSupported;
		}



		#endregion

	}
}
