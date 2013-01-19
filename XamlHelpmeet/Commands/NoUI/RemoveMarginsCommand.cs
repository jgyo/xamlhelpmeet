using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;
using XamlHelpmeet.UI.Utilities;

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
		public RemoveMarginsCommand(DTE2 application)
			: base(application)
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
				StripUnwantedProperty("Margin", sb);

				var editPoint = selectedCodeBlock.TopPoint.CreateEditPoint();
				selectedCodeBlock.Delete();
				editPoint.Insert(sb.ToString());
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
