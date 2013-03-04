using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using EnvDTE80;
using EnvDTE;
using System.Text.RegularExpressions;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.Commands.NoUI
{
	public class WiddenSelection : CommandBase
	{
		/// <summary>
		/// Initializes a new instance of the WiddenSelection class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public WiddenSelection(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "Widden Selection";
			CommandName = "WiddenSelection";
			ToolTip = "Widden selection to containing tag.";
		}

		public override void Execute()
		{
			var selectedCodeBlock = Application.ActiveDocument.Selection as TextSelection;
			var XAML = selectedCodeBlock.Text.Trim(WhiteSpaceCharacters);
			var regex = new Regex(@"<(\w+)");

			if ((selectedCodeBlock.IsEmpty || IsSelfClosing(regex, XAML)) && (!regex.IsMatch(XAML) || !CheckSelection(regex, XAML)))
			{
				// The selection does not contain a control
				UIUtilities.ShowInformationMessage("Selection Is Invalid", "You can use this command without a selection, but selections must begin and end with a control's start and end tags.");
				return;
			}

			// Find next previous opening tag
			

			// Find that tags closing tag

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
