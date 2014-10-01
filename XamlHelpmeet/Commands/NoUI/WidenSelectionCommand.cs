// <copyright file="WidenSelectionCommand.cs" company="YoderZone.com">
// Copyright (c) 2014 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>9/19/2014</date>
// <summary>Implements the widen selection command class</summary>
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
namespace XamlHelpmeet.Commands.NoUI
{
using System;
using System.ComponentModel.Design;

using EnvDTE80;

using EnvDTE;

using NLog;

using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Utility;

using YoderZone.Extensions.NLog;

/// <summary>
/// A widen selection command.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
public class WidenSelectionCommand : CommandBase
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    /// Initializes a new instance of the WiddenSelection class.
    /// </summary>
    /// <param name="application">
    /// The application.
    /// </param>
    /// <param name="id">
    /// The id.
    /// </param>
    public WidenSelectionCommand(DTE2 application, CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        Caption = "Widen Selection";
        CommandName = "WiddenSelection";
        ToolTip = "Widen selection to containing tag.";
    }

    /// <summary>
    /// Executes this
    /// XamlHelpmeet.Commands.NoUI.WidenSelectionCommand.
    /// </summary>
    /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.Execute()"/>
    public override void Execute()
    {
        logger.Debug("Entered member.");

        var selectedCodeBlock = Application.ActiveDocument.Selection as
                                TextSelection;
        var result = selectedCodeBlock.ExpandSelection();

        var errorMsg = string.Empty;

        switch (result)
        {
            case WiddenSelectionResult.Unknown:
                throw new Exception();
            case WiddenSelectionResult.Success:
                break;
            case WiddenSelectionResult.NodeSelectError:
                errorMsg = "An error occurred while trying to select a node.";
                break;
            case WiddenSelectionResult.ParentSelectError:
                errorMsg =
                    "An error occurred while trying to select a selected node's parent.";
                break;
            case WiddenSelectionResult.LogicError:
                errorMsg = "Programming logic is insufficient to handle this request.";
                break;
        }

        if (result != WiddenSelectionResult.Success)
        {
            UIUtilities.ShowInformationMessage("Widen Selection Error", errorMsg);
        }
    }
}
}
