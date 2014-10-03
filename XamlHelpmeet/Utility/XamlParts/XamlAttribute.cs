// <copyright file="XamlAttribute.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/7/2013</date>
// <summary>Implements the xaml attribute class</summary>

namespace XamlHelpmeet.Utility.XamlParts
{
using System;
using System.Diagnostics.Contracts;

using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// Attribute for xaml.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Utility.XamlParts.XamlPart"/>
public class XamlAttribute : XamlPart
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    /// Initializes a new instance of the XamlAttribute class.
    /// </summary>
    /// <param name="text">
    /// The text.
    /// </param>
    /// <param name="topPoint">
    /// The top point.
    /// </param>
    /// <param name="name">
    /// The bottom point.
    /// </param>
    /// <param name="value">
    /// The value.
    /// </param>
    public XamlAttribute(string text, int topPoint, string name, string value)
    : base(text, topPoint)
    {
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(text));
        logger.Debug("Entered member.");

        Value = value;
        Name = name;
    }

    /// <summary>
    /// Gets the name of this attribute.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the value of this attribute.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public string Value { get; private set; }
}
}