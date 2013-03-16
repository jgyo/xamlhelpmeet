// <copyright file="XamlPart.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/7/2013</date>
// <summary>Implements the xaml part class</summary>
using System;
using System.Collections.Generic;
using System.Linq;

namespace XamlHelpmeet.Utility.XamlParts
{
	/// <summary>
	/// 	Xaml part.
	/// </summary>
	public abstract class XamlPart
	{
		private readonly string _text;
		private readonly int _topPoint;
		private readonly int _bottomPoint;

		/// <summary>
		/// 	Initializes a new instance of the XamlPart class.
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
		public XamlPart(string text, int topPoint)
		{
			_text = text;
			_topPoint = topPoint;
			_bottomPoint = topPoint + text.Length;
		}

		/// <summary>
		/// 	Gets the bottom point.
		/// </summary>
		/// <value>
		/// 	The bottom point.
		/// </value>
		public int BottomPoint
		{
			get
			{
				return _bottomPoint;
			}
		}

		/// <summary>
		/// 	Gets the text that defines this part.
		/// </summary>
		/// <value>
		/// 	The text.
		/// </value>
		public string Text
		{
			get
			{
				return _text;
			}
		}

		/// <summary>
		/// 	Gets the top point.
		/// </summary>
		/// <value>
		/// 	The top point.
		/// </value>
		public int TopPoint
		{
			get
			{
				return _topPoint;
			}
		}
	}
}
