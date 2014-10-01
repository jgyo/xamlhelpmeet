// <copyright file="PropertyParameter.cs" company="YoderZone.com">
// Copyright (c) 2014 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>9/19/2014</date>
// <summary>Implements the property parameter class</summary>
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

namespace XamlHelpmeet.Model
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// A property parameter.
/// </summary>
[Serializable]
public class PropertyParameter
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    /// Initializes a new instance of the
    /// XamlHelpmeet.Model.PropertyParameter class.
    /// </summary>
    /// <param name="parameterName">
    /// Name of the parameter.
    /// </param>
    /// <param name="parameterTypeName">
    /// Name of the parameter type.
    /// </param>
    public PropertyParameter(string parameterName, string parameterTypeName)
    {
        logger.Debug("Entered member.");

        this.ParameterName = parameterName;
        this.ParameterTypeName = parameterTypeName;
    }

    /// <summary>
    /// Gets or sets the name of the parameter.
    /// </summary>
    /// <value>
    /// The name of the parameter.
    /// </value>
    public string ParameterName { get; set; }

    /// <summary>
    /// Gets or sets the name of the parameter type.
    /// </summary>
    /// <value>
    /// The name of the parameter type.
    /// </value>
    public string ParameterTypeName { get; set; }
}
}