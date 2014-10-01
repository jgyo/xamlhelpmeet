// <copyright file="NarrowSelectionCommand.cs" company="YoderZone.com">
// Copyright (c) 2014 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>9/19/2014</date>
// <summary>Implements the narrow selection command class</summary>
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

namespace XamlHelpmeet.Commands.NoUI
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// A narrow selection command.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
public class NarrowSelectionCommand : CommandBase
{
    /// <summary>
    /// The logger.
    /// </summary>
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    /// Initializes a new instance of the NarrowSelection class.
    /// </summary>
    /// <param name="application">
    /// The application.
    /// </param>
    /// <param name="id">
    /// The id.
    /// </param>
    public NarrowSelectionCommand(DTE2 application, CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        Caption = "Narrow Selection";
        CommandName = "NarrowSelection";
        ToolTip = "Remove containing tags from current selection.";
    }

    /// <summary>
    /// Determine if we can execute.
    /// </summary>
    /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.CanExecute(vsCommandExecOption)"/>
    ///
    /// ### <param name="executeOption">
    /// The execute option.
    /// </param>
    /// ### <returns>
    /// true if we can execute, otherwise false.
    /// </returns>
    public override bool CanExecute(vsCommandExecOption executeOption)
    {
        logger.Debug("Entered member.");

        return base.CanExecute(executeOption) && IsTextSelected();
    }

    /// <summary>
    /// Gets the status.
    /// </summary>
    /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.GetStatus()"/>
    ///
    /// ### <returns>
    /// The status.
    /// </returns>
    public override vsCommandStatus GetStatus()
    {
        logger.Debug("Entered member.");

        // This will add vsCommandStatusEnabled to vsCommandStatusSupported,
        // if IsTextSelected() returns true. Otherwise or'ing with
        // vsCommandStatusUnsupported leaves vsCommandStatusSupported
        // unchanged.


        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        return vsCommandStatus.vsCommandStatusSupported |
               (IsTextSelected() ?
                vsCommandStatus.vsCommandStatusEnabled :
                vsCommandStatus.vsCommandStatusUnsupported);
    }

    /// <summary>
    /// Executes this
    /// XamlHelpmeet.Commands.NoUI.NarrowSelectionCommand.
    /// </summary>
    /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.Execute()"/>
    public override void Execute()
    {
        logger.Debug("Entered member.");

        var selection = Application.ActiveDocument.Selection as TextSelection;
        var result = selection.ContractSelection();

        var errorMsg = string.Empty;

        switch (result)
        {
            case Utility.NarrowSelectionResult.SelectionIsEmpty:
                errorMsg = "Cannot narrow an empty selection.";
                break;
            case Utility.NarrowSelectionResult.InconsistentSelectionEnds:
                errorMsg =
                    "Narrowing works only when the selection is of complete nodes, or children of a single parent.";
                break;
            case Utility.NarrowSelectionResult.PartialTagSelected:
                errorMsg = "A partial tag selection cannot be narrowed.";
                break;
            case Utility.NarrowSelectionResult.Success:

                break;
        }
        if(result== NarrowSelectionResult.Success)
        { return; }
        UIUtilities.ShowInformationMessage("Narrow Selection Error", errorMsg);
    }
}
}
