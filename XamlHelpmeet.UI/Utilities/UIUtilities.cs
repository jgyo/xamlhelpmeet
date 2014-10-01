// <copyright file="UIUtilities.cs" company="YoderZone.com">
// Copyright (c) 2014 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>9/19/2014</date>
// <summary>Implements the utilities class</summary>
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

using System;
using System.Windows;

namespace XamlHelpmeet.UI.Utilities
{
using System.Diagnostics.Contracts;

using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// An utilities.
/// </summary>
public class UIUtilities
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    /// Shows the exception message.
    /// </summary>
    /// <param name="heading">
    /// The heading.
    /// </param>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <returns>
    /// A MessageBoxResult.
    /// </returns>
    public static MessageBoxResult ShowExceptionMessage(string heading,
            string message)
    {
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(heading));
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(message));
        Contract.Ensures(Enum.IsDefined(typeof(MessageBoxResult),
                                        Contract.Result<MessageBoxResult>()));

        return MessageBox.Show(message, heading, MessageBoxButton.OK,
                               MessageBoxImage.Error, MessageBoxResult.OK);
    }

    /// <summary>
    /// Shows the information message.
    /// </summary>
    /// <param name="heading">
    /// The heading.
    /// </param>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <returns>
    /// A MessageBoxResult.
    /// </returns>
    public static MessageBoxResult ShowInformationMessage(string heading,
            string message)
    {
        return MessageBox.Show(message, heading, MessageBoxButton.OK,
                               MessageBoxImage.Information, MessageBoxResult.OK);
    }


}
}
