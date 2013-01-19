﻿// file:	Commands\NoUI\GroupIntoBorderWithStackPanelVerticalRoot.cs
//
// summary:	Implements the group into border with stack panel vertical root class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.Commands.NoUI
{
	/// <summary>
	/// 	Group into border with stack panel vertical root.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class GroupIntoBorderWithStackPanelVerticalRoot : CommandBase
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GroupIntoBorderWithStackPanelVerticalRoot
		/// class.
		/// </summary>
		/// <param name="application">The application.</param>
		public GroupIntoBorderWithStackPanelVerticalRoot(DTE2 application)
			: base(application)
		{
			Caption = "Border With Root StackPanel - Vertical";
			CommandName = "GroupIntoBorderWithStackPanelVerticalRoot";
			ToolTip = "Group selection into a border with root stackpanel vertical being added.";
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
		/// 	Executes this GroupIntoBorderWithStackPanelVerticalRoot.
		/// </summary>
		public override void Execute()
		{
			try
			{
				GroupInto("<Border>\r\n<StackPanel>\r\n", "</StackPanel>\r\n</Border>\r\n");
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage(String.Format("Group Into {0}", Caption), ex.Message, String.Empty, ex.ToString());
			}
		}

		#endregion

	}
}
