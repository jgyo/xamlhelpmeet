// file:	Commands\NoUI\GroupIntoBorderWithGridRoot.cs
//
// summary:	Implements the group into border with grid root class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE80;
using EnvDTE;
using XamlHelpmeet.UI.Utilities;
using System.ComponentModel.Design;
using XamlHelpmeet.Utility;

namespace XamlHelpmeet.Commands.NoUI
{
	/// <summary>
	/// 	Group into border with grid root.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class GroupIntoBorderWithGridRoot : CommandBase
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GroupIntoBorderWithGridRoot class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public GroupIntoBorderWithGridRoot(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "Border with Root Grid";
			CommandName = "GroupIntoBorderWithGridRoot";
			ToolTip = "Group selection into a border with root grid being added.";
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
				var selectedCodeBlock = Application.ActiveDocument.Selection as TextSelection;

				if (selectedCodeBlock.AreSiblingsSelected() == false)
				{
					UIUtilities.ShowExceptionMessage("Invalid Selection", "Your selection must include complete controls with their opening and ending nodes.");
					return;
				}
				GroupInto("<Border>\r\n<Grid>\r\n", "</Grid>\r\n</Border>\r\n");
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage(String.Format("Group Into {0}", Caption), ex.Message, String.Empty, ex.ToString());
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
