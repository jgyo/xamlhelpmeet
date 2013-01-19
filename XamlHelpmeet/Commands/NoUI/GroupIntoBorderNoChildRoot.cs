﻿// file:	Commands\NoUI\GroupIntoBorderNoChildRoot.cs
//
// summary:	Implements the group into border no child root class
using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE80;
using EnvDTE;
using XamlHelpmeet.UI.Utilities;
using System.Globalization;

namespace XamlHelpmeet.Commands.NoUI
{
	/// <summary>
	/// 	Group into border no child root.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
	public class GroupIntoBorderNoChildRoot : CommandBase
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GroupIntoBorderNoChildRoot class.
		/// </summary>
		/// <param name="application">The application.</param>
		public GroupIntoBorderNoChildRoot(DTE2 application)
			: base(application)
		{
			Caption = "Border without Child Root";
			CommandName = "GroupIntoBorderNoChildRoot";
			ToolTip = "Group selection into a border without an additional root child being added.";
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
		/// 	Executes this GroupIntoBorderNoChildRoot.
		/// </summary>
		public override void Execute()
		{
			try
			{
				GroupInto("<Border>", "</Border>");
			}
			catch (Exception ex)
			{
				UIUtilities.ShowExceptionMessage(String.Format(CultureInfo.CurrentCulture, "Group Into {0}", Caption), ex.Message, String.Empty, ex.ToString());
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
			return vsCommandStatus.vsCommandStatusSupported |
				(IsTextSelected() ?
				vsCommandStatus.vsCommandStatusEnabled :
				vsCommandStatus.vsCommandStatusUnsupported);
		}

		#endregion

	}
}
