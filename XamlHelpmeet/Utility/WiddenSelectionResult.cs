// <copyright file="WiddenSelectionResult.cs" company="YoderZone.com">
// Copyright (c) 2014 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>9/19/2014</date>
// <summary>Implements the widden selection result class</summary>
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
namespace XamlHelpmeet.Utility
{
/// <summary>
/// Values that represent WiddenSelectionResult.
/// </summary>
public enum WiddenSelectionResult
{
    /// <summary>
    /// Represents an undetermined result.
    /// </summary>
    Unknown=0,
    /// <summary>
    /// An enum constant representing the success option.
    /// </summary>
    Success,
    /// <summary>
    /// An enum constant representing the node select error option.
    /// </summary>
    NodeSelectError,
    /// <summary>
    /// An enum constant representing the parent select error option.
    /// </summary>
    ParentSelectError,
    /// <summary>
    /// An enum constant representing the logic error option.
    /// </summary>
    LogicError

}
}
