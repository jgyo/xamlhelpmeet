// file:	Commands\UI\ControlDefaultsCommand.cs
//
// summary:	Implements the control defaults command class
using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE80;
using EnvDTE;
using XamlHelpmeet.UI;
using System.ComponentModel.Design;

namespace XamlHelpmeet.Commands.UI
{
	/// <summary>
	/// 	Control defaults command.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class ControlDefaultsCommand : CommandBase
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ControlDefaultsCommand class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public ControlDefaultsCommand(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "Set Control Defaults";
			CommandName = "SetControlDefaultsCommand";
			ToolTip = "Set control defaults for controls created by this software.";
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Executes this ControlDefaultsCommand.
		/// </summary>
		public override void Execute()
		{
			var obj = new UIControlDefaultsWindow();
			obj.ShowDialog();
			obj = null;
		}
		
		#endregion
	}
}
