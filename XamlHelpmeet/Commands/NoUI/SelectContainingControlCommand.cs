// <copyright file="SelectContainingControlCommand.cs" company="YoderZone.com">
// Copyright (c) 2014 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>9/19/2014</date>
// <summary>Implements the select containing control command class</summary>
// <remarks>
// Licensed under the Microsoft Public License (Ms-PL); you may not
// use this file except in compliance with the License. You may obtain a copy
// of the License at
//
// https://remarker.codeplex.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
// License for the specific language governing permissions and limitations
// under the License.
// </remarks>

using System.ComponentModel.Design;
using EnvDTE80;
using EnvDTE;
using XamlHelpmeet.Utility;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Extensions;

namespace XamlHelpmeet.Commands.NoUI
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// The select containing control command.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
public class SelectContainingControlCommand : CommandBase
{
    /// <summary>
    /// The logger.
    /// </summary>
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    /// Initializes a new instance of the SelectContainingControlCommand
    /// class.
    /// </summary>
    /// <param name="application">
    /// The application.
    /// </param>
    /// <param name="id">
    /// The command id.
    /// </param>
    public SelectContainingControlCommand(DTE2 application, CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        Caption = "Select containing control";
        CommandName = "SelectContainingControlCommand";
        ToolTip = "Select the control that contains the cursor.";
    }

    /// <summary>
    /// Executes this
    /// XamlHelpmeet.Commands.NoUI.SelectContainingControlCommand.
    /// </summary>
    /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.Execute()"/>
    public override void Execute()
    {
        logger.Debug("Entered member.");

        var selection = Application.ActiveDocument.Selection as TextSelection;
        var ep = selection.GetEditorPoints();
        if (!selection.SelectNodes())
        {
            UIUtilities.ShowInformationMessage("Unable to Select Controls",
                                               "This should not have happened.");
        }

        if (selection != null && selection.Text.Length <= ep.Text.Length)
        { selection.ExpandSelection(); }
    }
}
}
