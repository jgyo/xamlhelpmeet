// <copyright file="ClassInformationList.cs" company="YoderZone.com">
// Copyright (c) 2014 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>9/19/2014</date>
// <summary>Implements the class information list class</summary>
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
using System.Collections.Generic;
using System.Linq;

namespace XamlHelpmeet.Model
{
using System.Diagnostics.Contracts;

using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// List of class information.
/// </summary>
/// <seealso cref="T:System.Collections.Generic.List{XamlHelpmeet.Model.ClassInformation}"/>
[Serializable]
public class ClassInformationList : List<ClassInformation>
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    /// Initializes a new instance of the
    /// XamlHelpmeet.Model.ClassInformationList class.
    /// </summary>
    public ClassInformationList()
    {
        logger.Debug("Entered member.");



    }

    /// <summary>
    /// Initializes a new instance of the
    /// XamlHelpmeet.Model.ClassInformationList class.
    /// </summary>
    /// <param name="capacity">
    /// The capacity.
    /// </param>
    public ClassInformationList(int capacity)
    : base(capacity)
    {
        Contract.Requires<ArgumentOutOfRangeException>(capacity >= 0);

        logger.Debug("Entered member.");



    }

    /// <summary>
    /// Initializes a new instance of the
    /// XamlHelpmeet.Model.ClassInformationList class.
    /// </summary>
    /// <param name="collection">
    /// The collection.
    /// </param>
    public ClassInformationList(IEnumerable<ClassInformation> collection)
    : base(collection)
    {
        logger.Debug("Entered member.");



    }

    /// <summary>
    /// Gets the selected item.
    /// </summary>
    /// <value>
    /// The selected item.
    /// </value>
    public ClassInformation SelectedItem
    {
        get
        {
            return (from anc in this
                    where anc.IsSelected
                    select anc).SingleOrDefault();
        }
    }
}
}
