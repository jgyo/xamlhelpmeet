using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Design;
using EnvDTE80;
using EnvDTE;
using System.Text.RegularExpressions;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Extensions;
using XamlHelpmeet.Utility;

namespace XamlHelpmeet.Commands.NoUI
{
	public class WidenSelectionCommand : CommandBase
	{
		/// <summary>
		/// Initializes a new instance of the WiddenSelection class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public WidenSelectionCommand(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "Widden Selection";
			CommandName = "WiddenSelection";
			ToolTip = "Widden selection to containing tag.";
		}

		public override void Execute()
		{
			var selectedCodeBlock = Application.ActiveDocument.Selection as TextSelection;
			var result = selectedCodeBlock.ExpandSelection();

			var errorMsg = string.Empty;

			switch (result)
			{
				case WiddenSelectionResult.Unknown:
					throw new Exception();
				case WiddenSelectionResult.Success:
					break;
				case WiddenSelectionResult.NodeSelectError:
					errorMsg = "An error occured while trying to select a node.";
					break;
				case WiddenSelectionResult.ParentSelectError:
					errorMsg = "An error occured while trying to select a selected node's parent.";
					break;
				case WiddenSelectionResult.LogicError:
					errorMsg = "Programming logic is insufficient to handle this request.";
					break;
			}

			if (result != WiddenSelectionResult.Success)
			{
				UIUtilities.ShowInformationMessage("Widden Selection Error", errorMsg);
			}
		}

		private bool IsSelfClosing(Regex regex, string XAML)
		{
			var matches = regex.Matches(XAML);
			if (XAML.EndsWith("/>") == false || matches.Count > 1)
				return false;
			return matches.Count == 1 && XAML.StartsWith(String.Format("<{0}", matches[0].Groups[1].Value));
		}
		private bool CheckSelection(Regex regex, string XAML)
		{
			var tag = regex.Match(XAML).Groups[1].Value;
			return XAML.StartsWith(string.Format("<{0}", tag), StringComparison.InvariantCultureIgnoreCase) && XAML.EndsWith(string.Format("</{0}>", tag), StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
