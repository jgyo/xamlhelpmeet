using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Utility;
using XamlHelpmeet.Extensions;

namespace XamlHelpmeet.Commands.NoUI
{
	/// <summary>
	/// 	Remove margins command.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class RemoveMarginsCommand : CommandBase
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the RemoveMarginsCommand class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public RemoveMarginsCommand(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "Remove Margins";
			CommandName = "RemoveMarginsCommand";
			ToolTip = "Remove all margins from selected text.";
		}

		#endregion

		#region Method

		/// <summary>
		/// 	Determine if we can execute.
		/// </summary>
		/// <param name="executeOption">
		/// 	The execute option.
		/// </param>
		/// <returns>
		/// 	true if we can execute, otherwise false.
		/// </returns>
		public override bool CanExecute(vsCommandExecOption executeOption)
		{
			return base.CanExecute(executeOption) && IsTextSelected();
		}

		/// <summary>
		/// 	Executes this RemoveMarginsCommand.
		/// </summary>
		public override void Execute()
		{
			try
			{
				var selectedCodeBlock = Application.ActiveDocument.Selection as TextSelection;
				var sb = new StringBuilder(selectedCodeBlock.Text.Trim(WhiteSpaceCharacters));

				if (selectedCodeBlock.IsNodeSelected() == false)
				{
					UIUtilities.ShowExceptionMessage("You must select a control", "Your selection must begin and end with both control tags.");
					return;
				}
				StripUnwantedProperty("Margin", sb);
				selectedCodeBlock.ReplaceSelectedText(sb.ToString());
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
			// This will add vsCommandStatusEnabled to vsCommandStatusSupported,
			// if IsTextSelected() returns true. Otherwise or'ing with
			// vsCommandStatusUnsupported leaves vsCommandStatusSupported
			// unchanged.
			return vsCommandStatus.vsCommandStatusSupported |
				(IsTextSelected() ?
				vsCommandStatus.vsCommandStatusEnabled :
				vsCommandStatus.vsCommandStatusUnsupported);
		}
		#endregion
	}
}
