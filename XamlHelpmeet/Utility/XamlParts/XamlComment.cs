// <copyright file="XamlComment.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/7/2013</date>
// <summary>Implements the xaml comment class</summary>

namespace XamlHelpmeet.Utility.XamlParts
{
using System;
using System.Diagnostics.Contracts;

using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// Xaml comment.
/// </summary>
/// <seealso cref="T:XamlHelpmeet.Utility.XamlParts.XamlPart"/>
public class XamlComment : XamlPart
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    /// <summary>
    /// Initializes a new instance of the XamlComment class.
    /// </summary>
    /// <param name="text">
    /// The text.
    /// </param>
    /// <param name="topPoint">
    /// The top point.
    /// </param>
    public XamlComment(string text, int topPoint)
    : base(text, topPoint)
    {
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(text));
        logger.Debug("Entered member.");

        // TODO: Complete XamlComment method in XamlComment
        // Extract comment from text with regular expression.
    }

    /// <summary>
    /// Gets the comment text.
    /// </summary>
    /// <value>
    /// The comment text.
    /// </value>
    public string CommentText { get; private set; }
}
}
