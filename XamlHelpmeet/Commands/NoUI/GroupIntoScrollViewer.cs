// file:	Commands\NoUI\GroupIntoScrollViewer.cs
//
// summary:	Implements the group into scroll viewer class
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
	/// 	Group into scroll viewer.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class GroupIntoScrollViewer : CommandBase
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GroupIntoScrollViewer class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public GroupIntoScrollViewer(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "ScrollViewer with Root Grid";
			CommandName = "GroupIntoScrollViewer";
			ToolTip = "Group selection into a ScrollViewer with a root Grid being added.";
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
		/// 	Executes this GroupIntoScrollViewer.
		/// </summary>
		public override void Execute()
		{
			try
			{
				GroupInto("<ScrollViewer>\r\n<Grid>\r\n", "</Grid>\r\n</ScrollViewer>\r\n");
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage("Group Into " + Caption, ex.Message, String.Empty, ex.ToString());
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
			return vsCommandStatus.vsCommandStatusSupported | (IsTextSelected() ? vsCommandStatus.vsCommandStatusEnabled : vsCommandStatus.vsCommandStatusUnsupported);
		}

		#endregion

	}
}

