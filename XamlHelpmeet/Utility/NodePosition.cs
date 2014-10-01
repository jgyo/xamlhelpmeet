// <copyright file="NodePosition.cs" company="YoderZone.com">
// Copyright (c) 2014 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>9/19/2014</date>
// <summary>Implements the node position class</summary>
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
/// Values that represent NodePosition.
/// </summary>
public enum NodePosition
{
    /// <summary>
    /// An enum constant representing the unknown option.
    /// </summary>
    Unknown = 0,
    /// <summary>
    /// An enum constant representing the top tag option.
    /// </summary>
    TopTag,
    /// <summary>
    /// An enum constant representing the bottom tag option.
    /// </summary>
    BottomTag,
    /// <summary>
    /// An enum constant representing the content option.
    /// </summary>
    Content,
    /// <summary>
    /// An enum constant representing the before option.
    /// </summary>
    Before,
    /// <summary>
    /// An enum constant representing the after option.
    /// </summary>
    After
}
}
