// file:	Commands\NoUI\GroupIntoUniformGrid.cs
//
// summary:	Implements the group into uniform grid class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE80;
using EnvDTE;
using XamlHelpmeet.UI.Utilities;
using System.ComponentModel.Design;

namespace XamlHelpmeet.Commands.NoUI
{
	/// <summary>
	/// 	Group into uniform grid.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class GroupIntoUniformGrid : CommandBase
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GroupIntoUniformGrid class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public GroupIntoUniformGrid(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "UniformGrid";
			CommandName = "GroupIntoUniformGrid";
			ToolTip = "Group selection into a UniformGrid.";
		}

		#endregion

		#region Methods

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
		/// 	Executes this GroupIntoUniformGrid.
		/// </summary>
		public override void Execute()
		{
			try
			{
				GroupInto("<UniformGrid>\r\n", "</UniformGrid>\r\n");
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

