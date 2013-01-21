// file:	Commands\UI\AboutCommand.cs
//
// summary:	Implements the about command class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlHelpmeet.Commands;
using XamlHelpmeet.UI;
using EnvDTE80;
using EnvDTE;
using System.ComponentModel.Design;

namespace XamlHelpmeet.Commands.UI
{
	/// <summary>
	/// 	About command.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class AboutCommand : CommandBase
	{
		/// <summary>
		/// Initializes a new instance of the AboutCommand class.
		/// </summary>
		/// <param name="application">The application.</param>
		/// <param name="id">The id.</param>
		public AboutCommand(DTE2 application, CommandID id)
			: base(application, id)
		{
			Caption = "About Xaml Helpmeet";
			CommandName = String.Format("AboutCommand");
			ToolTip = "About Xaml Helpmeet";
		}

		/// <summary>
		/// 	Executes this CommandBase.
		/// </summary>
		/// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.Execute()"/>
		public override void Execute()
		{
			var obj = new AboutWindow();
			obj.ShowDialog();
		}
	}
}
