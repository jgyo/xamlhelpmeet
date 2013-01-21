// file:	Commands\NoUI\GroupIntoStackPanelHorizontal.cs
//
// summary:	Implements the group into stack panel horizontal class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using XamlHelpmeet.UI.Utilities;
using System.ComponentModel.Design;

namespace XamlHelpmeet.Commands.NoUI
{
	/// <summary>
	/// 	Group into stack panel horizontal.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class GroupIntoStackPanelHorizontal : CommandBase
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GroupIntoStackPanelHorizontal class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public GroupIntoStackPanelHorizontal(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "StackPanel - Horizontal";
			CommandName = "GroupIntoStackPanelHorizontal";
			ToolTip = "Group selection into a stackpanel - horizontal.";
		}

		#endregion

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
				GroupInto("<StackPanel Orientation=\"Horizontal\">\r\n", "</StackPanel>\r\n");
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage("Group Into " + Caption, ex.Message, String.Empty, ex.ToString());
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
			return vsCommandStatus.vsCommandStatusSupported | (IsTextSelected() ? vsCommandStatus.vsCommandStatusEnabled : vsCommandStatus.vsCommandStatusUnsupported);
		}
	}
}

