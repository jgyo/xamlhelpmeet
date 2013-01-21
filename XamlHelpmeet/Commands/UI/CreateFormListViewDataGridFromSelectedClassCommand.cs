﻿// file:	Commands\UI\CreateFormListViewDataGridFromSelectedClassCommand.cs
//
// summary:	Implements the create form list view data grid from selected class command class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE80;
using EnvDTE;
using XamlHelpmeet.Utility;
using XamlHelpmeet.UI;
using System.Windows;
using System.Windows.Forms;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.UI.DynamicForm;

namespace XamlHelpmeet.Commands.UI
{
	/// <summary>
	/// 	Create form list view data grid from selected class command.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class CreateFormListViewDataGridFromSelectedClassCommand : CommandBase
	{
		/// <summary>
		/// Initializes a new instance of the
		/// CreateFormListViewDataGridFromSelectedClassCommand class.
		/// </summary>
		/// <param name="application">The application.</param>
		public CreateFormListViewDataGridFromSelectedClassCommand(DTE2 application)
			: base(application)
		{
			Caption = "Create Form, ListView or DataGrid From Selected Class";
			CommandName = "CreateFormListViewDataGridFromSelectedClassCommand";
			ToolTip = "Create Form, ListView or DataGrid From Selected Class";
		}

		#region Methods

		/// <summary>
		/// 	Determine if we can execute.
		/// </summary>
		/// <param name="ExecuteOption">
		/// 	The execute option.
		/// </param>
		/// <returns>
		/// 	true if we can execute, otherwise false.
		/// </returns>
		public override bool CanExecute(vsCommandExecOption ExecuteOption)
		{
			return base.CanExecute(ExecuteOption) && ! IsTextSelected();
		}

		/// <summary>
		/// 	Executes this CreateFormListViewDataGridFromSelectedClassCommand.
		/// </summary>
		public override void Execute()
		{
			try
			{
				var remoteTypeReflector = new RemoteTypeReflector();
				var classEntity = remoteTypeReflector.
					GetClassEntityFromSelectedClass(Application.
					SelectedItems.
					Item(1).ProjectItem.
					ContainingProject, 
					Caption);

				if (classEntity == null)
					return;

				var obj = new CreateBusinessFormFromClassWindow(classEntity);
				var result = obj.ShowDialog();

				if (result ?? false)
				{
					try
					{
						System.Windows.Clipboard.Clear();
						System.Windows.Clipboard.SetText(obj.BusinessForm);
					}
					catch //(Exception ex)
					{
						// Had to do this to avoid useless exception message
						// when running this code in a VPC, since Vista &
						// VPC and the Clipboard don't play nice together
						// sometimes.
						// the operation works, you just get an exception
						// for no reason.
					}
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
		/// <returns>
		/// 	The status.
		/// </returns>
		public override vsCommandStatus GetStatus()
		{
			if (Application.ActiveDocument == null ||
				!(Application.ActiveDocument.Name.EndsWith("vb") ||
				Application.ActiveDocument.Name.EndsWith("cs"))
				)
				return vsCommandStatus.vsCommandStatusUnsupported &
					vsCommandStatus.vsCommandStatusInvisible;

			return vsCommandStatus.vsCommandStatusEnabled |
				vsCommandStatus.vsCommandStatusSupported;
		}
		
		#endregion
	}
}