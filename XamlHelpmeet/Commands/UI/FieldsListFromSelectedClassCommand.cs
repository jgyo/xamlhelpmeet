// file:	Commands\UI\FieldsListFromSelectedClassCommand.cs
//
// summary:	Implements the fields list from selected class command class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE80;
using EnvDTE;
using XamlHelpmeet.Utility;
using XamlHelpmeet.UI;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.Commands.UI
{
	/// <summary>
	/// 	Fields list from selected class command.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class FieldsListFromSelectedClassCommand : CommandBase
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the FieldsListFromSelectedClassCommand class.
		/// </summary>
		/// <param name="application">The application.</param>
		public FieldsListFromSelectedClassCommand(DTE2 application)
			: base(application)
		{
			Caption = "Fields List For Selected Class";
			CommandName = "FieldsListFromSelectedClassCommand";
			ToolTip = "Show fields list for selected class";
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Determine if the FieldsListFromSelectedClassCommand can be executed.
		/// </summary>
		/// <param name="ExecuteOption">
		/// 	The execute option.
		/// </param>
		/// <returns>
		/// 	true if we can execute, false if not.
		/// </returns>
		public override bool CanExecute(vsCommandExecOption ExecuteOption)
		{
			return base.CanExecute(ExecuteOption) && !IsTextSelected();
		}

		/// <summary>
		/// 	Executes this FieldsListFromSelectedClassCommand.
		/// </summary>
		public override void Execute()
		{
			try
			{
				var remoteTypeReflector = new RemoteTypeReflector();
				var classEntity = remoteTypeReflector.GetClassEntityFromSelectedClass(Application.SelectedItems.Item(1).ProjectItem.ContainingProject, Caption);

				if (classEntity != null && classEntity.Success)
				{
					var obj = new FieldsListWindow(classEntity);
					obj.Topmost = true;
					obj.Show();
				}
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage(Caption, ex.Message, String.Empty, ex.ToString());
			}
		}

		/// <summary>
		/// 	Gets the status of the FieldsListFromSelectedClassCommand command.
		/// </summary>
		/// <remarks>
		/// 	FieldsListFromSelectedClassCommand is always supported. If text is selected, the
		/// 	command is enabled.
		/// </remarks>
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

		#endregion

	}
}
