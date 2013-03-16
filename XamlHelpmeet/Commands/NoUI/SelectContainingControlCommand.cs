using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Design;
using EnvDTE80;
using EnvDTE;
using XamlHelpmeet.Utility;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Extensions;

namespace XamlHelpmeet.Commands.NoUI
{
	public class SelectContainingControlCommand : CommandBase
	{
		/// <summary>
		/// Initializes a new instance of the SelectContainingControlCommand class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public SelectContainingControlCommand(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "Select containing control";
			CommandName = "SelectContainingControlCommand";
			ToolTip = "Select the control that contains the cursor.";
		}
		public override void Execute()
		{
			var selection = Application.ActiveDocument.Selection as TextSelection;
            var ep = selection.GetEditorPoints();
			if (!selection.SelectNodes())
			{
				UIUtilities.ShowInformationMessage("Unable to Select Controls", "This should not have happened.");
			}
            if (selection.Text.Length <= ep.Text.Length)
                selection.ExpandSelection();
        }

		public override bool CanExecute(vsCommandExecOption executeOption)
		{
			return base.CanExecute(executeOption);
		}
	}
}
