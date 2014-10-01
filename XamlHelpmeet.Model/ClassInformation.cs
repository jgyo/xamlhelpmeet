// <copyright file="ClassInformation.cs" company="YoderZone.com">
// Copyright (c) 2014 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>9/19/2014</date>
// <summary>Implements the assemblies namespaces class</summary>
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
/// Assemblies namespaces class.
/// </summary>
[Serializable]
public class ClassInformation
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    /// Initializes a new instance of the ClassInformation class.
    /// </summary>
    /// <param name="assemblyName">
    /// Name of the assembly.
    /// </param>
    /// <param name="nameSpace">
    /// The namespace.
    /// </param>
    /// <param name="typeName">
    /// Name of the type.
    /// </param>
    /// <param name="classEntity">
    /// The class entity.
    /// </param>
    public ClassInformation(string assemblyName,
                            string nameSpace,
                            string typeName,
                            ClassEntity classEntity)
    {
        this.ClassEntity = classEntity;
        this.Namespace = nameSpace;
        this.TypeName = typeName;
        this.AssemblyName = assemblyName;
    }

    /// <summary>
    /// Gets the name of the assembly.
    /// </summary>
    /// <value>
    /// The name of the assembly.
    /// </value>
    public string AssemblyName { get; private set; }

    /// <summary>
    /// Gets the name of the type.
    /// </summary>
    /// <value>
    /// The name of the type.
    /// </value>
    public string TypeName { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether this
    /// ClassInformation is selected.
    /// </summary>
    /// <value>
    /// true if this ClassInformation is selected, otherwise
    /// false.
    /// </value>
    public bool IsSelected { get; set; }

    /// <summary>
    /// Gets the namespace.
    /// </summary>
    /// <value>
    /// The namespace.
    /// </value>
    public string Namespace { get; private set; }

    /// <summary>
    /// Gets or sets the class entity.
    /// </summary>
    /// <value>
    /// The class entity.
    /// </value>
    public ClassEntity ClassEntity { get; set; }

}
}
