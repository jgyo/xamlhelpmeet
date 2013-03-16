// <copyright file="XamlComment.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/7/2013</date>
// <summary>Implements the xaml comment class</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlHelpmeet.Utility.XamlParts
{
	/// <summary>
	/// 	Xaml comment.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Utility.XamlParts.XamlPart"/>
	public class XamlComment : XamlPart
	{
		/// <summary>
		/// 	Initializes a new instance of the XamlComment class.
		/// </summary>
		/// <param name="text">
		/// 	The text.
		/// </param>
		/// <param name="topPoint">
		/// 	The top point.
		/// </param>
		/// <param name="bottomPoint">
		/// 	The bottom point.
		/// </param>
		public XamlComment(string text, int topPoint)
			: base(text, topPoint)
		{
			// TODO: Complete XamlComment method in XamlComment
			// Extract comment from text with Regex expression.
		}

		/// <summary>
		/// 	Gets the comment text.
		/// </summary>
		/// <value>
		/// 	The comment text.
		/// </value>
		public string CommentText { get; private set; }
	}
}
