// <copyright file="NodeEndMatches.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/7/2013</date>
// <summary>Implements the node end matches class</summary>
namespace XamlHelpmeet.Utility
{
#region Imports

using System.Text.RegularExpressions;

using NLog;

using YoderZone.Extensions.NLog;

#endregion

/// <summary>
///     Node end matches.
/// </summary>
public class NodeEndMatches
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    #region Fields

    private int _documentLevel = -1;

    #endregion

    #region Properties and Indexers

    /// <summary>
    ///     Gets or sets the document level.
    /// </summary>
    /// <value>
    ///     The document level.
    /// </value>
    public int DocumentLevel
    {
        get { return this._documentLevel; }
        set { this._documentLevel = value; }
    }

    /// <summary>
    ///     Gets the ending index.
    /// </summary>
    /// <value>
    ///     The ending index.
    /// </value>
    public int EndingIndex
    {
        get { return this.EndingTagMatch == null ? int.MaxValue : this.EndingTagMatch.Index; }
    }

    /// <summary>
    ///     Gets the length of the ending tag.
    /// </summary>
    /// <value>
    ///     The length of the ending tag.
    /// </value>
    public int EndingTagLength
    {
        get
        {
            if (this.EndingTagMatch == null)
            {
                return -1;
            }
            return this.EndingTagMatch.Length;
        }
    }

    /// <summary>
    ///     Gets or sets the ending tag match.
    /// </summary>
    /// <value>
    ///     The ending tag match.
    /// </value>
    public Match EndingTagMatch { get; set; }

    /// <summary>
    ///     Gets the ending tag text.
    /// </summary>
    /// <value>
    ///     The ending tag text.
    /// </value>
    public string EndingTagText
    {
        get { return this.EndingTagMatch == null ? string.Empty : this.EndingTagMatch.Value; }
    }

    /// <summary>
    ///     Gets the name of the node.
    /// </summary>
    /// <value>
    ///     The name of the node.
    /// </value>
    public string NodeName
    {
        get
        {
            return this.StartingTagMatch == null
                   ? string.Empty
                   : this.StartingTagMatch.Groups["StartTagName"].Captures[0].Value;
        }
    }

    /// <summary>
    ///     Gets the starting index.
    /// </summary>
    /// <value>
    ///     The starting index.
    /// </value>
    public int StartingIndex
    {
        get { return this.StartingTagMatch == null ? int.MinValue : this.StartingTagMatch.Index; }
    }

    /// <summary>
    ///     Gets the length of the starting tag.
    /// </summary>
    /// <value>
    ///     The length of the starting tag.
    /// </value>
    public int StartingTagLength
    {
        get
        {
            if (this.StartingTagMatch == null)
            {
                return -1;
            }
            return this.StartingTagMatch.Length;
        }
    }

    /// <summary>
    ///     Gets or sets the starting tag match.
    /// </summary>
    /// <value>
    ///     The starting tag match.
    /// </value>
    public Match StartingTagMatch { get; set; }

    /// <summary>
    ///     Gets the starting tag text.
    /// </summary>
    /// <value>
    ///     The starting tag text.
    /// </value>
    public string StartingTagText
    {
        get { return this.StartingTagMatch == null ? string.Empty : this.StartingTagMatch.Value; }
    }

    #endregion
}
}
