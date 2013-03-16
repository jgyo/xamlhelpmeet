// <copyright file="SpanType.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/11/2013</date>
// <summary>Implements the span type class</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlHelpmeet.Utility
{
	/// <summary>
	/// 	Values that represent SpanType.
	/// </summary>
	public enum SpanType
	{
		/// <summary>
		/// 	Unknown or erroneous.
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// 	Outside a child node but within a parent node.
		/// </summary>
		BetweenNodes,
		/// <summary>
		/// 	Between a node's tags that has no children.
		/// </summary>
		BetweenNodeTags,
		/// <summary>
		/// 	Within a start tag.
		/// </summary>
		StartingTag,
		/// <summary>
		/// 	Within an end tag.
		/// </summary>
		EndingTag,
		/// <summary>
		/// 	Within a self-closing tag.
		/// </summary>
		SelfClosingTag,
		/// <summary>
		/// 	Outside the root node.
		/// </summary>
		OutSide
	}
}
