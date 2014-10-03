// <copyright file="UIHelpers.cs" company="YoderZone.com">
// Copyright (c) 2014 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>9/19/2014</date>
// <summary>Implements the helpers class</summary>
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
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.Editors;
using XamlHelpmeet.UI.Enums;
using XamlHelpmeet.Extensions;
using XamlHelpmeet.UI.DynamicForm;

namespace XamlHelpmeet.UI.Utilities
{
using System.Diagnostics.Contracts;

using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// A helper class for UI logic.
/// </summary>
public class UIHelpers
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    /// Query if 'Name' is microsoft assembly.
    /// </summary>
    /// <param name="name">
    /// The name.
    /// </param>
    /// <returns>
    /// true if microsoft assembly, false if not.
    /// </returns>
    public static bool IsMicrosoftAssembly(string name)
    {
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name));
        logger.Debug("Entered member.");

        return name.IsMicrosoftAssembly();
    }

    /// <summary>
    /// Dynamic form editor factory.
    /// </summary>
    /// <param name="pi">
    /// The pi.
    /// </param>
    /// <returns>
    /// A DynamicFormEditor.
    /// </returns>
    public static DynamicFormEditor DynamicFormEditorFactory(
        PropertyInformation pi)
    {
        Contract.Requires<ArgumentNullException>(pi != null);
        Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(pi.Name));

        var listBoxContent = new DynamicFormListBoxContent
        {
            AssociatedLabel =
            ParsePropertyNameForLabel(
                pi.Name),
            CanWrite = pi.CanWrite,
            BindingMode =
            pi.CanWrite
            ? BindingMode.TwoWay
            : BindingMode.OneWay,
            BindingPath = pi.Name
        };

        if (pi.TypeName.IndexOf("Boolean", System.StringComparison.Ordinal) == -1)
        {
            listBoxContent.ControlType = pi.CanWrite ?
                                         DynamicFormControlType.TextBox : DynamicFormControlType.TextBlock;
        }
        else
        {
            listBoxContent.ControlType = DynamicFormControlType.CheckBox;
        }

        listBoxContent.DataType = pi.TypeName;
        listBoxContent.TypeNamespace = pi.TypeNamespace;

        if (listBoxContent.DataType.Contains("Int32"))
        {
            listBoxContent.DataType = "Integer";
        }
        else if (listBoxContent.DataType.Contains("Int16"))
        {
            listBoxContent.DataType = "Short";
        }
        else if (listBoxContent.DataType.Contains("Int64"))
        {
            listBoxContent.DataType = "Long";
        }

        if (pi.TypeName.Contains("Decimal"))
        {
            listBoxContent.StringFormat = "{0:c}";
        }
        else if (pi.TypeName.Contains("Date"))
        {
            listBoxContent.StringFormat = "{0:d}";
        }
        else
        {
            listBoxContent.StringFormat = string.Empty;
        }
        return new DynamicFormEditor
        {
            DataContext = listBoxContent
        };
    }

    /// <summary>
    /// Searches for the first anscestor window.
    /// </summary>
    /// <param name="DependencyObject">
    /// The dependency object.
    /// </param>
    /// <returns>
    /// The found anscestor window.
    /// </returns>
    public static Window FindAnscestorWindow(DependencyObject
            DependencyObject)
    {
        while (DependencyObject != null)
        {
            DependencyObject = VisualTreeHelper.GetParent(DependencyObject);
            if (DependencyObject is Window)
            {
                break;
            }
        }
        return DependencyObject as Window;
    }

    /// <summary>
    /// Gets sample formats.
    /// </summary>
    /// <returns>
    /// The sample formats.
    /// </returns>
    public static ListCollectionView GetSampleFormats()
    {
        logger.Debug("Entered member.");

        var obj = new List<SampleFormat>
        {
            new SampleFormat("Date", "12/25/1965", "{0:d}"),
            new SampleFormat(
                "Date",
                "Saturday, December 25, 1965",
                "{0:D}"),
            new SampleFormat(
                "Date",
                "Saturday, December 25, 1965 7:25 AM",
                "{0:f}"),
            new SampleFormat(
                "Date",
                "Saturday, December 25, 1965 7:25:42 AM",
                "{0:F}"),
            new SampleFormat("Date", "12/25/1965 7:25 AM", "{0:g}"),
            new SampleFormat("Date", "12/25/1965 7:25:42 AM", "{0:G}"),
            new SampleFormat("Date", "December 25", "{0:M}"),
            new SampleFormat("Date",
            "Sat, 25 Dec 1965 7:25:42 GMT",
            "{0:R}"),
            new SampleFormat("Double, Decimal", "$75,234.89", "{0:c}"),
            new SampleFormat("Double, Decimal", "75234.89", "{0:F}"),
            new SampleFormat("Double, Decimal", "75234", "{0:F0}"),
            new SampleFormat("Double, Decimal", "75234.8933", "{0:F4}"),
            new SampleFormat("Double, Decimal", "75,234.89", "{0:N}"),
            new SampleFormat("Double, Decimal", "75,234.8933", "{0:N4}"),
            new SampleFormat("Double, Decimal", "7,523,489.33 %", "{0:P}"),
            new SampleFormat("Integer, Short", "42", "{0:D}"),
            new SampleFormat("Integer, Short", "00042", "{0:D5}"),
            new SampleFormat("Integer, Short", "42", "{0:F0}"),
            new SampleFormat("Integer, Short", "2a", "{0:x}"),
            new SampleFormat("Integer, Short", "2A", "{0:X}"),
            new SampleFormat("Integer, Short", "00002A", "{0:X6}")
        };

        var sampleFormatList = new ListCollectionView(obj);
        sampleFormatList.GroupDescriptions.Add(new
                                               PropertyGroupDescription("DataType"));
        sampleFormatList.SortDescriptions.Add(new SortDescription("DataType",
                                              ListSortDirection.Ascending));
        sampleFormatList.SortDescriptions.Add(new SortDescription("Example",
                                              ListSortDirection.Descending));

        return sampleFormatList;
    }

    /// <summary>
    /// Gets sorted enum names.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown when one or more arguments have unsupported or illegal values.
    /// </exception>
    /// <param name="T">
    /// The Type to process.
    /// </param>
    /// <returns>
    /// An array of string.
    /// </returns>
    public static string[] GetSortedEnumNames(Type T)
    {
        Contract.Requires<ArgumentNullException>(T != null);
        logger.Debug("Entered member.");

        if (!T.IsEnum)
        {
            throw new ArgumentException("Must be an enum.", "T");
        }

        var strout = Enum.GetNames(T);
        Array.Sort(strout);
        return strout;
    }

    /// <summary>
    /// Parse property name for label.
    /// </summary>
    /// <param name="toParse">
    /// to parse.
    /// </param>
    /// <returns>
    /// A string.
    /// </returns>
    public static string ParsePropertyNameForLabel(string toParse)
    {
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(toParse));
        logger.Debug("Entered member.");

        var sb = new StringBuilder(256);
        var foundUpperCase = false;
        var onlyUpperCase = true;

        for (var i = 0; i < toParse.Length; i++)
        {
            if (!toParse.IsNotUpper(i))
            { continue; }
            onlyUpperCase = false;
            break;
        }

        if (onlyUpperCase)
        {
            return toParse;
        }

        for (var i = 0; i < toParse.Length; i++)
        {
            if (!foundUpperCase && toParse.IsUpper(i))
            {
                foundUpperCase = true;
                if (i == 0)
                {
                    sb.Append(toParse[i]);
                }
                else
                {
                    sb.Append(" ");
                    sb.Append(toParse[i]);
                }
                continue;
            }
            if (!foundUpperCase)
            {
                continue;
            }
            if (toParse.IsUpper(i))
            {
                sb.Append(" ");
                sb.Append(toParse[i]);
            }
            else if (toParse.IsLetterOrDigit(i))
            {
                sb.Append(toParse[i]);
            }
        }

        return sb.ToString();
    }
}
}
