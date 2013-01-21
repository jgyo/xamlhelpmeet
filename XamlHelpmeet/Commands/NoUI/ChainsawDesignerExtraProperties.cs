// file:	Commands\NoUI\ChainsawDesignerExtraProperties.cs
//
// summary:	Implements the chainsaw designer extra properties class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;
using EnvDTE;
using System.Windows;
using XamlHelpmeet.UI.Utilities;
using System.ComponentModel.Design;

namespace XamlHelpmeet.Commands.NoUI
{
	/// <summary>
	/// 	Chainsaw designer extra properties.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class ChainsawDesignerExtraProperties : CommandBase
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ChainsawDesignerExtraProperties class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public ChainsawDesignerExtraProperties(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "Chainsaw - Minimize Designer XAML";
			CommandName = "ChainsawDesignerExtraPropertiesCommand";
			ToolTip = "Use this function only on designer generated XAML.  Warning this thing is a chainsaw!";
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
				if (MessageBox.Show("This chainsaw will remove all Margins, MinHeights, MinWidths, x:Name and Name properties, and set all rows and columns to Auto size.", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					var selectedCodeBlock = Application.ActiveDocument.Selection as TextSelection;
					var sb = new StringBuilder(selectedCodeBlock.Text.Trim(WhiteSpaceCharacters));
					StripUnwantedProperty("Margin", sb);
					StripUnwantedProperty("MinHeight", sb);
					StripUnwantedProperty("MinWidth", sb);
					StripUnwantedProperty("x:Name", sb);
					StripUnwantedProperty("Name", sb);
					SetAllRowsAndColumnsToAuto(sb);
					var editPoint = selectedCodeBlock.TopPoint.CreateEditPoint();
					selectedCodeBlock.Delete();
					editPoint.Insert(sb.ToString());
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
