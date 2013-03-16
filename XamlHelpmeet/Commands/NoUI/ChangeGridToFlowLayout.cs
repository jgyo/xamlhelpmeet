// file:	Commands\NoUI\ChangeGridToFlowLayout.cs
//
// summary:	Implements the change grid to flow layout class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE80;
using EnvDTE;
using System.Windows;
using XamlHelpmeet.UI.Utilities;
using System.ComponentModel.Design;
using XamlHelpmeet.Utility;
using XamlHelpmeet.Extensions;

namespace XamlHelpmeet.Commands.NoUI
{
	/// <summary>
	/// 	Change grid to flow layout.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class ChangeGridToFlowLayout : CommandBase
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ChangeGridToFlowLayout class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public ChangeGridToFlowLayout(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "Change Grid To Flow Layout";
			CommandName = "ChangeGridToFlowLayoutCommand";
			ToolTip = "Use this function to change a Grid to flow layout.";
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Determine if we can execute.
		/// </summary>
		/// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.CanExecute(vsCommandExecOption)"/>
		public override bool CanExecute(vsCommandExecOption executeOption)
		{
			return base.CanExecute(executeOption) && IsTextSelected();
		}

		/// <summary>
		/// 	Executes this CommandBase.
		/// </summary>
		/// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.Execute()"/>
		public override void Execute()
		{
			try
			{
				if (MessageBox.Show("This will remove all Margins, MinHeights, MinWidths and set all rows and columns to Auto size.", "Change Grid To Flow Layout?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					var selectedCodeBlock = Application.ActiveDocument.Selection as TextSelection;
					var sb = new StringBuilder(selectedCodeBlock.Text.Trim(WhiteSpaceCharacters));

					if (selectedCodeBlock.IsNodeSelected() == false)
					{
						UIUtilities.ShowExceptionMessage("You must select a control", "Your selection must begin and end with both control tags.");
						return;
					}
					selectedCodeBlock.Trim();
					StripUnwantedProperty("Margin", sb);
					StripUnwantedProperty("MinHeight", sb);
					StripUnwantedProperty("MinWidth", sb);
					SetAllRowsAndColumnsToAuto(sb);
					selectedCodeBlock.ReplaceSelectedText(sb.ToString());
				}
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage(Caption, ex.Message, String.Empty, ex.ToString());
			}
		}

		/// <summary>
		/// 	Gets the status.
		/// </summary>
		/// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.GetStatus()"/>
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
