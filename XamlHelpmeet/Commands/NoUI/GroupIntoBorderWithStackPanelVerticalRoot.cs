// <copyright file="GroupIntoBorderWithStackPanelVerticalRoot.cs" company="YoderZone.com">
// Copyright (c) 2014 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>9/19/2014</date>
// <summary>Implements the group into border with stack panel vertical root class</summary>
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
using EnvDTE;
using EnvDTE80;
using System;
using System.ComponentModel.Design;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.Commands.NoUI
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// Group into border with stack panel vertical root.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Commands.CommandBase"/>
public class GroupIntoBorderWithStackPanelVerticalRoot : CommandBase
{
    /// <summary>
    /// The logger.
    /// </summary>
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the
    /// GroupIntoBorderWithStackPanelVerticalRoot class.
    /// </summary>
    /// <param name="application">
    /// The application.
    /// </param>
    /// <param name="id">
    /// The id.
    /// </param>
    public GroupIntoBorderWithStackPanelVerticalRoot(DTE2 application,
            CommandID id)
    : base(application, id)
    {
        logger.Debug("Entered member.");

        Caption = "Border With Root StackPanel - Vertical";
        CommandName = "GroupIntoBorderWithStackPanelVerticalRoot";
        ToolTip = "Group selection into a border with root stackpanel vertical being added.";
    }

    #endregion

    #region Methods
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
    /// Executes this GroupIntoBorderWithStackPanelVerticalRoot.
    /// </summary>
    /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.Execute()"/>
    public override void Execute()
    {
        logger.Debug("Entered member.");

        try
        {
            GroupInto("<Border>\r\n<StackPanel>\r\n",
                      "</StackPanel>\r\n</Border>\r\n");
        }
        catch (Exception ex)
        {
            UIUtilities.ShowExceptionMessage(String.Format("Group Into {0}", Caption),
                                             ex.Message);
            logger.Debug("An exception was raised in GroupIntoBorderWithStackPanelVerticalRoot::Execute.",
                         ex);
        }
    }

    /// <summary>
    /// Gets the status.
    /// </summary>
    /// <seealso cref="M:XamlHelpmeet.Commands.CommandBase.GetStatus()"/>
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

    #endregion
}
}