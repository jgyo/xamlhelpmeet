// <copyright file="XamlCollection.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/8/2013</date>
// <summary>Implements the xaml collection class</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace XamlHelpmeet.Utility
{
	/// <summary>
	/// 	Collection of xaml nodes.
	/// </summary>
	/// <seealso cref="T:System.Collections.Generic.LinkedList{XamlHelpmeet.Utility.XamlNode}"/>
	public sealed class XamlNodeCollection : LinkedList<XamlNode>, System.Runtime.Serialization.ISerializable
	{
		private readonly XamlNode _owner;

		/// <summary>
		/// 	Initializes a new instance of the XamlNodeCollection class.
		/// </summary>
		/// <param name="owner">
		/// 	The owner.
		/// </param>
		public XamlNodeCollection(XamlNode owner)
		{
			_owner = owner;
		}

		/// <summary>
		/// 	Gets the owner.
		/// </summary>
		/// <value>
		/// 	The owner.
		/// </value>
		public XamlNode Owner
		{
			get
			{
				return _owner;
			}
		}

	}

	/// <summary>
	/// 	Xaml collection helper.
	/// </summary>
	public static class XamlCollectionHelper
	{
		/// <summary>
		/// 	An XamlCollection extension method that queries if 'target' is
		/// 	empty.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <returns>
		/// 	true if empty, otherwise false.
		/// </returns>
		public static bool IsEmpty(this XamlNodeCollection target)
		{
			return target == null || target.Count == 0;
		}
	}
}
