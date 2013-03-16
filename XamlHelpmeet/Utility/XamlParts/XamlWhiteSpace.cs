// <copyright file="XamlWhiteSpace.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/7/2013</date>
// <summary>Implements the xaml white space class</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlHelpmeet.Utility.XamlParts
{
	/// <summary>
	/// 	Xaml white space.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Utility.XamlParts.XamlPart"/>
	public class XamlWhiteSpace : XamlPart
	{
		/// <summary>
		/// 	Initializes a new instance of the XamlWhiteSpace class.
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
		public XamlWhiteSpace(string text, int topPoint)
			: base(text, topPoint)
		{

		}
	}
}
