// <copyright file="XamlCollection.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/8/2013</date>
// <summary>Implements the xaml collection class</summary>

using System.Collections.Generic;

namespace XamlHelpmeet.Utility
{
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
///     Collection of xaml nodes.
/// </summary>
/// <seealso cref="T:System.Collections.Generic.LinkedList{XamlHelpmeet.Utility.XamlNode}"/>
[ComVisible(false)]
[Serializable]
public sealed class XamlNodeCollection : LinkedList<XamlNode>
{
    /// <summary>
    /// The logger.
    /// </summary>
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    private readonly XamlNode _owner;

    internal XamlNodeCollection(SerializationInfo info,
                                StreamingContext context) :base(info, context)
    {
        _owner = (XamlNode)info.GetValue("_owner", typeof(XamlNode));
    }

    /// <summary>
    /// Implements the <see cref="T:System.Runtime.Serialization.ISerializable"/> interface and returns the data needed to serialize the <see cref="T:System.Collections.Generic.LinkedList`1"/> instance.
    /// </summary>
    /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"/> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.LinkedList`1"/> instance.</param><param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"/> object that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.LinkedList`1"/> instance.</param><exception cref="T:System.ArgumentNullException"><paramref name="info"/> is null.</exception>
    public override void GetObjectData(SerializationInfo info,
                                       StreamingContext context)
    {
        info.AddValue("_owner", _owner);
        base.GetObjectData(info, context);
    }

    /// <summary>
    ///     Initializes a new instance of the XamlNodeCollection class.
    /// </summary>
    /// <param name="owner">
    ///     The owner.
    /// </param>
    internal XamlNodeCollection(XamlNode owner)
    {
        logger.Debug("Entered member.");

        _owner = owner;
    }

    /// <summary>
    ///     Gets the owner.
    /// </summary>
    /// <value>
    ///     The owner.
    /// </value>
    public XamlNode Owner
    {
        get
        {
            return _owner;
        }
    }

}
}
