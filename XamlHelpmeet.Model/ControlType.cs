// <copyright file="ControlType.cs" company="YoderZone.com">
// Copyright (c) 2014 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>9/19/2014</date>
// <summary>Implements the control type class</summary>
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
namespace XamlHelpmeet.Model
{
/// <summary>
/// Values that represent ControlType.
/// </summary>
public enum ControlType
{
    /// <summary>
    /// An enum constant representing the CheckBox option.
    /// </summary>
    CheckBox = 1,
    /// <summary>
    /// An enum constant representing the ComboBox option.
    /// </summary>
    ComboBox = 2,
    /// <summary>
    /// An enum constant representing the Image option.
    /// </summary>
    Image = 3,
    /// <summary>
    /// An enum constant representing the Label option.
    /// </summary>
    Label = 4,
    /// <summary>
    /// An enum constant representing the no control option.
    /// </summary>
    None = 0,
    /// <summary>
    /// An enum constant representing the TextBlock option.
    /// </summary>
    TextBlock = 5,
    /// <summary>
    /// An enum constant representing the TextBox option.
    /// </summary>
    TextBox = 6,
    /// <summary>
    /// An enum constant representing the DatePicker option.
    /// </summary>
    DatePicker = 7
}
}