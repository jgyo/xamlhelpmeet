// file:	Commands\CommandBase.cs
//
// summary:	Implements the command base class

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Text;
using System.Xml;
using XamlHelpmeet.Extensions;


namespace XamlHelpmeet.Commands
{
	/// <summary>
	/// 	Command base.
	/// </summary>
	/// <seealso cref="T:System.IDisposable"/>
	public abstract class CommandBase : OleMenuCommand, IDisposable
	{
		#region Fields

		private static readonly char[] _whiteSpaceCharacters = new char[] { '\r', '\n', '\t', ' ' };
		private CommandBarControl _commandBaseCommandBarControl;
		private bool _isDisposed;

		#endregion Fields

		#region Constructors and Distructors

		/// <summary>
		/// Initializes a new instance of the CommandBase class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public CommandBase(DTE2 application, CommandID id)
			: base(Execute, id)
		{
			this.Application = application;
			this.BeforeQueryStatus += OnBeforeQueryStatus;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandBase" /> class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		/// <param name="Text">The text.</param>
		public CommandBase(DTE2 application, CommandID id, string Text)
			: base(Execute, id, Text)
		{
			this.Application = application;
		}

		/// <summary>
		/// 	Finalizes an instance of the CommandBase class.
		/// </summary>
		~CommandBase()
		{
			Dispose(false);
		}

		#endregion Constructors and Distructors

		/// <summary>
		/// Executes before the query status is read.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
		/// <exception cref="System.ArgumentNullException">sender</exception>
		protected virtual void OnBeforeQueryStatus(object sender, EventArgs e)
		{
			if (sender != this)
			{
				throw new ArgumentException("Only this should send calls to this.");
			}

			var status = GetStatus();
			this.Enabled = status.HasFlag(vsCommandStatus.vsCommandStatusEnabled);
			this.Supported = status.HasFlag(vsCommandStatus.vsCommandStatusSupported);
			this.Visible = !status.HasFlag(vsCommandStatus.vsCommandStatusInvisible);
		}

		#region Properties

		/// <summary>
		/// 	Gets or sets the caption.
		/// </summary>
		/// <value>
		/// 	The caption.
		/// </value>
		public string Caption
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the name of the command.
		/// </summary>
		/// <value>
		/// 	The name of the command.
		/// </value>
		public string CommandName
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the menu command.
		/// </summary>
		/// <value>
		/// 	The menu command.
		/// </value>
		public OleMenuCommand MenuCommand
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the tool tip.
		/// </summary>
		/// <value>
		/// 	The tool tip.
		/// </value>
		public string ToolTip
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets an array of white space characters consisting of CR, LF, TAB, and SPACE.
		/// </summary>
		/// <value>
		/// 	The white space characters.
		/// </value>
		protected static char[] WhiteSpaceCharacters
		{
			get
			{
				return _whiteSpaceCharacters;
			}
		}

		/// <summary>
		/// 	Gets or sets the application.
		/// </summary>
		/// <value>
		/// 	The application.
		/// </value>
		protected DTE2 Application
		{
			get;
			set;
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// 	Determine if we can execute.
		/// </summary>
		/// <param name="executeOption">
		/// 	The execute option.
		/// </param>
		/// <returns>
		/// 	true if we can execute, otherwise false.
		/// </returns>
		public virtual bool CanExecute(vsCommandExecOption executeOption)
		{
			return executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault;
		}

		/// <summary>
		/// 	Executes this CommandBase.
		/// </summary>
		public abstract void Execute();

		/// <summary>
		/// 	Gets the status.
		/// </summary>
		/// <returns>
		/// 	The status.
		/// </returns>
		public virtual vsCommandStatus GetStatus()
		{
			return vsCommandStatus.vsCommandStatusEnabled | vsCommandStatus.vsCommandStatusSupported;
		}

		/// <summary>
		/// 	Adds a name spaces.
		/// </summary>
		/// <param name="xmlIn">
		/// 	The XML in.
		/// </param>
		/// <param name="namespaceManager">
		/// 	Manager for namespace.
		/// </param>
		/// <param name="addedNamespaces">
		/// 	The added namespaces.
		/// </param>
		protected void AddNameSpaces(string xmlIn, XmlNamespaceManager namespaceManager, List<string> addedNamespaces)
		{
			var ht = new Hashtable();
			var lastIndexFound = -1;
			bool continueDo;
			do
			{
				continueDo = false;
				lastIndexFound = xmlIn.IndexOf(":", lastIndexFound + 1, StringComparison.Ordinal);
				if (lastIndexFound <= -1)
				{
					break;
				}
				for (int i = lastIndexFound; i >= 0; i--)
				{
					if (xmlIn.Substring(i, 1) != " " && xmlIn.Substring(i, 1) != "<")
						continue;
					var nameSpace = xmlIn.Substring(i + 1, lastIndexFound - i - 1);
					if (!ht.ContainsKey(nameSpace))
					{
						ht.Add(nameSpace, nameSpace);
						namespaceManager.AddNamespace(nameSpace, String.Format(CultureInfo.InvariantCulture, "urn:{0}", nameSpace));
						addedNamespaces.Add(String.Format("xmlns:{0}=\"urn:{0}\"", nameSpace));
					}

					// Shifflett's code had a "Continue Do" here, but C# does not
					// support that construct. C#'s "continue" restarts the
					// inner loop, which in this case is the for loop. By adding
					// a "continueDo" flag I have implemented the same logic. (jgy)
					continueDo = true;
					break;
				}
			}
			while (continueDo);
		}

		/// <summary>
		/// 	Executes this CommandBase.
		/// </summary>
		private static void Execute(object sender, EventArgs e)
		{
			if (!(sender is CommandBase))
			{
				throw new ArgumentException("The sender object is not of Type CommandBase.");
			}
			(sender as CommandBase).Execute();
		}

		#endregion Methods

		#region Helpers

		/// <summary>
		/// 	Group into.
		/// </summary>
		/// <param name="wrapperTop">
		/// 	The wrapper top.
		/// </param>
		/// <param name="wrapperBottom">
		/// 	The wrapper bottom.
		/// </param>
		protected void GroupInto(string wrapperTop, string wrapperBottom)
		{
			var selectedCodeBlock = Application.ActiveDocument.Selection as TextSelection;
			selectedCodeBlock.Trim();
			var vbCrLfArray = new string[] { Environment.NewLine };
			var selectedLines = selectedCodeBlock.Text.Trim().Split(vbCrLfArray, StringSplitOptions.None);
			selectedCodeBlock.Delete();
			var sb = new StringBuilder(4096);
			sb.AppendLine(wrapperTop);
			foreach (string selectedLine in selectedLines)
			{
				sb.AppendLine(selectedLine);
			}
			sb.AppendLine(wrapperBottom);
			selectedCodeBlock.ReplaceSelectedText(sb.ToString());
		}

		/// <summary>
		/// 	Queries if this CommandBase is text selected.
		/// </summary>
		/// <returns>
		/// 	true if text selected, otherwise false.
		/// </returns>
		protected bool IsTextSelected()
		{
			if (Application.ActiveDocument == null || Application.ActiveDocument.Selection == null)
				return false;
			var ts = Application.ActiveDocument.Selection as TextSelection;
			if (ts == null)
				return false;
			return ts.Text.Length > 0;
		}

		/// <summary>
		/// 	Sets all rows and columns to automatic.
		/// </summary>
		/// <param name="sb">
		/// 	The sb.
		/// </param>
		protected void SetAllRowsAndColumnsToAuto(StringBuilder sb)
		{
			SetDefinitionsToAuto(sb, "<RowDefinition Height=\"");
			SetDefinitionsToAuto(sb, "<ColumnDefinition Width=\"");

			sb.Replace("   ", " ").Replace("  ", " ");
			sb.Replace(" >", ">");
		}

		/// <summary>
		/// 	Strip unwanted property.
		/// </summary>
		/// <param name="propertyToStrip">
		/// 	The property to strip.
		/// </param>
		/// <param name="sb">
		/// 	The sb.
		/// </param>
		protected void StripUnwantedProperty(string propertyToStrip, StringBuilder sb)
		{
			var marginIndex = 0;
			var marginOpenIndex = 0;
			var marginCloseIndex = 0;

			//var marginsRemoved = false;	// This variable's value is never used
			propertyToStrip += "=";
			do
			{
				marginIndex = sb.ToString().IndexOf(propertyToStrip, StringComparison.InvariantCultureIgnoreCase);
				if (marginIndex < 0)
				{
					break;
				}
				marginOpenIndex = sb.ToString().IndexOf((char)34, marginIndex);
				if (marginOpenIndex <= marginIndex)
				{
					break;
				}
				marginCloseIndex = sb.ToString().IndexOf((char)34, marginOpenIndex + 1);
				if (marginCloseIndex <= marginIndex)
				{
					break;
				}
				sb.Remove(marginIndex, marginCloseIndex - marginIndex + 1);

				//marginsRemoved = true;	// value is never checked anywhere
			}
			while (true);
			sb.Replace("   ", " ").Replace("  ", " ");
			sb.Replace(" >", ">");
		}

		private static void SetDefinitionsToAuto(StringBuilder sb, string definitionTag)
		{
			int index;
			int openIndex = 0;
			int closeIndex;
			do
			{
				// Look for Definition tag.
				index = sb.ToString().IndexOf(definitionTag, openIndex);
				if (index < 0)
				{
					break;
				}

				// Calculate location of first character inside " mark.
				openIndex = index + definitionTag.Length;

				// Look for next " mark.
				closeIndex = sb.ToString().IndexOf((char)34, openIndex);

				// Rmove text between " marks.
				sb.Remove(openIndex, closeIndex - openIndex);

				// Replace that with "Auto."
				sb.Insert(openIndex, "Auto");
			}
			while (true);
		}

		#endregion Helpers

		#region Dispose Pattern Implementation

		/// <summary>
		/// 	Performs application-defined tasks associated with freeing, releasing, or
		/// 	resetting unmanaged resources.
		/// </summary>
		/// <seealso cref="M:System.IDisposable.Dispose()"/>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 	Performs application-defined tasks associated with freeing, releasing, or
		/// 	resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing">
		/// 	true to release both managed and unmanaged resources; false to release only
		/// 	unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
					if (_commandBaseCommandBarControl != null)
					{
						_commandBaseCommandBarControl.Delete();
						_commandBaseCommandBarControl = null;
					}
				}
			}
			_isDisposed = true;
		}

		#endregion Dispose Pattern Implementation
	}
}