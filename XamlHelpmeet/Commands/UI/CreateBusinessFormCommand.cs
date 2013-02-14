// file:	Commands\UI\CreateBusinessFormCommand.cs
//
// summary:	Implements the create business form command class
using System;
using System.Collections.Generic;
using System.Linq;
using XamlHelpmeet.UI;
using EnvDTE80;
using EnvDTE;
using XamlHelpmeet.Model;
using XamlHelpmeet.Utility;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.UI.CreateBusinessForm;
using System.ComponentModel.Design;

namespace XamlHelpmeet.Commands.UI
{
	/// <summary>
	/// 	Create business form command.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class CreateBusinessFormCommand : CommandBase
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the CreateBusinessFormCommand class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public CreateBusinessFormCommand(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "Create Business Form";
			CommandName = "CreateBusinessFormCommand";
			ToolTip = "Create business form";
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Executes this CreateBusinessFormCommand.
		/// </summary>
		public override void Execute()
		{
			try
			{
				ClassEntity classEntity = null;
				var Guids = PtHelpers.GetProjectTypeGuids(Application.ActiveDocument.ProjectItem.ContainingProject).Split(';');
				if (PtHelpers.IsProjectSilverlight(Guids))
				{
					classEntity = new ClassEntity(string.Empty, true);
					classEntity.SilverlightVersion = Application.ActiveDocument.ProjectItem.ContainingProject.Properties.Item("TargetFrameworkMoniker").Value.ToString().Replace("Silverlight,Version=v", string.Empty);
				}
				var createBusinessFormWindow = new CreateBusinessFormWindow(classEntity);
				var result = createBusinessFormWindow.ShowDialog();
				if (result ?? false)
				{
					var ts = Application.ActiveDocument.Selection as TextSelection;
					ts.Insert(createBusinessFormWindow.BusinessForm);
				}
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage(Caption, ex.Message, string.Empty, ex.ToString());
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
			// This command is supported and always enabled.
			return vsCommandStatus.vsCommandStatusSupported |
				vsCommandStatus.vsCommandStatusEnabled;
		}

		#endregion

	}
}
