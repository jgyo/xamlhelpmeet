// <copyright file="XamlInnerText.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/7/2013</date>
// <summary>Implements the xaml inner text class</summary>
using System;
using System.Collections.Generic;
using System.Linq;

namespace XamlHelpmeet.Utility.XamlParts
{
	/// <summary>
	/// 	Xaml inner text.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Utility.XamlParts.XamlPart"/>
	public class XamlInnerText : XamlPart
	{
		/// <summary>
		/// 	Initializes a new instance of the XamlInnerText class.
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
		public XamlInnerText(string text, int topPoint, int bottomPoint)
			: base(text, topPoint)
		{

		}
	}
}
