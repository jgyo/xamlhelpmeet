using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Design;
using EnvDTE80;
using EnvDTE;
using XamlHelpmeet.Extensions;
using XamlHelpmeet.Utility;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.Commands.NoUI
{
	public class NarrowSelectionCommand : CommandBase
	{
		/// <summary>
		/// Initializes a new instance of the NarrowSelection class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public NarrowSelectionCommand(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "Narrow Selection";
			CommandName = "NarrowSelection";
			ToolTip = "Remove containing tags from current selection.";
		}

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

		public override void Execute()
		{
			var selection = Application.ActiveDocument.Selection as TextSelection;
			var result = selection.ContractSelection();

			var errorMsg = string.Empty;

			switch (result)
			{
				case Utility.NarrowSelectionResult.SelectionIsEmpty:
					errorMsg = "Cannot narrow an empty selection.";
					break;
				case Utility.NarrowSelectionResult.InconsistentSelectionEnds:
					errorMsg = "Narrowing works only when the selection is of complete nodes, or children of a single parent.";
					break;
				case Utility.NarrowSelectionResult.PartialTagSelected:
					errorMsg = "A partial tag selection cannot be narrowed.";
					break;
				case Utility.NarrowSelectionResult.Success:
					
					break;
			}
			if(result== NarrowSelectionResult.Success)
				return;
			UIUtilities.ShowInformationMessage("Narrow Selection Error", errorMsg);
		}
	}
}
