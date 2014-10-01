// ***********************************************************************
// Assembly         : XamlHelpmeet
// Solution         : XamlHelpmeet
// File name        : XamlTagType.cs
// Author           : Gil Yoder
// Created          : 09 19,  2014
//
// Last Modified By : Gil Yoder
// Last Modified On : 09 19, 2014
// ***********************************************************************

namespace XamlHelpmeet.Utility.XamlParts
{
/// <summary>
///     Values that represent XamlTagType.
/// </summary>
public enum XamlTagType
{
    /// <summary>
    ///     Represents a tag type that has not yet been determined.
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     Represents a starting tag.
    /// </summary>
    Starting,

    /// <summary>
    ///     Represents an ending tag.
    /// </summary>
    Ending,

    /// <summary>
    ///     Represents a self-closing tag.
    /// </summary>
    SelfClosing
}
}