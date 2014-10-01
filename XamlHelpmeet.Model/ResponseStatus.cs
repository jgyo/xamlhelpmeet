// ***********************************************************************
// Assembly         : XamlHelpmeet.Model
// Solution         : XamlHelpmeet
// File name        : ResponseStatus.cs
// Author           : Gil Yoder
// Created          : 09 19,  2014
//
// Last Modified By : Gil Yoder
// Last Modified On : 09 19, 2014
// ***********************************************************************

namespace XamlHelpmeet.Model
{
/// <summary>
///     Values that represent ResponseStatus.
/// </summary>
public enum ResponseStatus
{
    /// <summary>
    ///     An enum constant representing the canceled option.
    /// </summary>
    Canceled,

    /// <summary>
    ///     An enum constant representing the exception option.
    /// </summary>
    Exception,

    /// <summary>
    ///     An enum constant representing the failed option.
    /// </summary>
    Failed,

    /// <summary>
    ///     An enum constant representing the not found option.
    /// </summary>
    NotFound,

    /// <summary>
    ///     An enum constant representing the success option.
    /// </summary>
    Success
}
}