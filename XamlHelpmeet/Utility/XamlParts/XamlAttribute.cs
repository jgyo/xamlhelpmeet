// <copyright file="XamlAttribute.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/7/2013</date>
// <summary>Implements the xaml attribute class</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlHelpmeet.Utility.XamlParts
{
	/// <summary>
	/// 	Attribute for xaml.
	/// </summary>
	/// <seealso cref="T:XamlHelpmeet.Utility.XamlParts.XamlPart"/>
	public class XamlAttribute : XamlPart
	{
		/// <summary>
		/// 	Initializes a new instance of the XamlAttribute class.
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
		public XamlAttribute(string text, int topPoint, string name, string value)
			: base(text, topPoint)
		{
			Value = value;
			Name = name;
		}

		/// <summary>
		/// 	Gets the name of this attribute.
		/// </summary>
		/// <value>
		/// 	The name.
		/// </value>
		public string Name { get; private set; }

		/// <summary>
		/// 	Gets the value of this attribute.
		/// </summary>
		/// <value>
		/// 	The value.
		/// </value>
		public string Value { get; private set; }
	}
}