// <copyright file="NodeEndMatches.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/7/2013</date>
// <summary>Implements the node end matches class</summary>
using System;
using System.Text.RegularExpressions;

namespace XamlHelpmeet.Utility
{
	/// <summary>
	/// 	Node end matches.
	/// </summary>
	public class NodeEndMatches
	{
		private int _documentLevel = -1;

		/// <summary>
		/// 	Gets or sets the document level.
		/// </summary>
		/// <value>
		/// 	The document level.
		/// </value>
		public int DocumentLevel
		{
			get
			{
				return _documentLevel;
			}
			set
			{
				_documentLevel = value;
			}
		}

		/// <summary>
		/// 	Gets the ending index.
		/// </summary>
		/// <value>
		/// 	The ending index.
		/// </value>
		public int EndingIndex
		{
			get
			{
				if (EndingTagMatch == null)
					return int.MaxValue;
				return EndingTagMatch.Index;
			}
		}

		/// <summary>
		/// 	Gets the length of the ending tag.
		/// </summary>
		/// <value>
		/// 	The length of the ending tag.
		/// </value>
		public int EndingTagLength
		{
			get
			{
				if (EndingTagMatch == null)
					return -1;
				return EndingTagMatch.Length;
			}
		}

		/// <summary>
		/// 	Gets or sets the ending tag match.
		/// </summary>
		/// <value>
		/// 	The ending tag match.
		/// </value>
		public Match EndingTagMatch { get; set; }

		/// <summary>
		/// 	Gets the ending tag text.
		/// </summary>
		/// <value>
		/// 	The ending tag text.
		/// </value>
		public string EndingTagText
		{
			get
			{
				if (EndingTagMatch == null)
					return string.Empty;
				return EndingTagMatch.Value;
			}
		}

		/// <summary>
		/// 	Gets the name of the node.
		/// </summary>
		/// <value>
		/// 	The name of the node.
		/// </value>
		public string NodeName
		{
			get
			{
				if (StartingTagMatch == null)
					return string.Empty;
				return StartingTagMatch.Groups["StartTagName"].Captures[0].Value;
			}
		}

		/// <summary>
		/// 	Gets the starting index.
		/// </summary>
		/// <value>
		/// 	The starting index.
		/// </value>
		public int StartingIndex
		{
			get
			{
				if (StartingTagMatch == null)
					return int.MinValue;
				return StartingTagMatch.Index;
			}
		}

		/// <summary>
		/// 	Gets the length of the starting tag.
		/// </summary>
		/// <value>
		/// 	The length of the starting tag.
		/// </value>
		public int StartingTagLength
		{
			get
			{
				if (StartingTagMatch == null)
					return -1;
				return StartingTagMatch.Length;
			}
		}

		/// <summary>
		/// 	Gets or sets the starting tag match.
		/// </summary>
		/// <value>
		/// 	The starting tag match.
		/// </value>
		public Match StartingTagMatch { get; set; }

		/// <summary>
		/// 	Gets the starting tag text.
		/// </summary>
		/// <value>
		/// 	The starting tag text.
		/// </value>
		public string StartingTagText
		{
			get
			{
				if (StartingTagMatch == null)
					return string.Empty;
				return StartingTagMatch.Value;
			}
		}
	}
}