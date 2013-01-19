// file:	Commands\UI\CreateViewModelCommandFromSelectedClassCommand.cs
//
// summary:	Implements the create view model command from selected class command class
using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE80;
using EnvDTE;
using XamlHelpmeet.Utility;
using XamlHelpmeet.UI;
using System.Windows.Forms;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.UI.ViewModelCreation;

namespace XamlHelpmeet.Commands.UI
{
	/// <summary>
	/// 	Create view model command from selected class command.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	class CreateViewModelCommandFromSelectedClassCommand : CommandBase
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the CreateViewModelCommandFromSelectedClassCommand
		/// class.
		/// </summary>
		/// <param name="application">The application.</param>
		public CreateViewModelCommandFromSelectedClassCommand(DTE2 application)
			: base(application)
		{
			Caption = "Create ViewModel for Class";
			CommandName = "CreateViewModelCommandFromSelectedClassCommand";
			ToolTip = "Create ViewModel for class.";
		}
		#endregion

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
			return base.CanExecute(ExecuteOption) && !IsTextSelected();
		}

		/// <summary>
		/// 	Executes this CreateViewModelCommandFromSelectedClassCommand.
		/// </summary>
		public override void Execute()
		{
			try
			{
				var remoteTypeReflector = new RemoteTypeReflector();
				var classEntity = remoteTypeReflector.GetClassEntityFromSelectedClass(Application.SelectedItems.
				                                                                      Item(1).ProjectItem.
				                                                                      ContainingProject,
				                                                                      Caption);
				if (classEntity != null)
				{
					var obj = new CreateViewModelWindow(classEntity,
					                                    Application.ActiveWindow.Caption.EndsWith(".vb"));
					var result = obj.ShowDialog();
					if (result ?? false)
					{
						try
						{
							Clipboard.Clear();
							Clipboard.SetText(obj.ViewModelText);
						}
						catch //(Exception ex)
						{
							// Had to do this to avoid useless exception message when running this code in a 
							// VPC, since Vista & VPC and the Clipboard don't play nice together sometimes.
							// the operation works, you just get an exception for no reason.
						}
						UIUtilities.ShowInformationMessage("Ready to Paste",
						                                 "Position curosr inside a ViewModel file and execute a paste operation."
						);
					}
				}
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage(Caption,
				                               ex.Message,
				                               string.Empty,
				                               ex.ToString());
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
			if (Application.ActiveDocument == null || !(Application.ActiveDocument.Name.
			EndsWith("vb") || Application.ActiveDocument.Name.EndsWith("cs")))
			{
				return vsCommandStatus.vsCommandStatusUnsupported | vsCommandStatus.vsCommandStatusInvisible;
			}
			return vsCommandStatus.vsCommandStatusEnabled | vsCommandStatus.vsCommandStatusSupported;
		}
		#endregion

	}
}
