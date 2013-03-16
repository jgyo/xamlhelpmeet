// <copyright file="EditorPoints.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/10/2013</date>
// <summary>Implements the editor points class</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;

namespace XamlHelpmeet.Extensions
{
	/// <summary>
	/// 	Editor points.
	/// </summary>
	/// <remarks>
	///     Like TextSelector EditorPoint offsets are 1 based.
	/// </remarks>
	public struct EditorPoints
	{
		private readonly static EditorPoints _invalid = new EditorPoints();

		private EditorPoints(TextSelection sel)
			: this()
		{
			ActivePoint = sel.ActivePoint.AbsoluteCharOffset;
			AnchorPoint = sel.AnchorPoint.AbsoluteCharOffset;
			BottomPoint = sel.BottomPoint.AbsoluteCharOffset;
			TopPoint = sel.TopPoint.AbsoluteCharOffset;
			IsActiveEndGreater = sel.IsActiveEndGreater;
			Text = sel.Text;
			sel.SelectAll();

			DocumentText = sel.Text.Replace(Environment.NewLine, "\n");
		}

		private EditorPoints(int topPoint, int bottomPoint, string documentText)
			: this()
		{
			TopPoint = topPoint;
			BottomPoint = bottomPoint;
			ActivePoint = topPoint;
			AnchorPoint = bottomPoint;
			DocumentText = documentText;
			Text = DocumentText.Substring(TopPoint, BottomPoint - TopPoint);
		}

		private static EditorPoints Invalid
		{
			get
			{
				return _invalid;
			}
		}

		/// <summary>
		/// 	Gets the active point.
		/// </summary>
		/// <value>
		/// 	The active point.
		/// </value>
		public int ActivePoint { get; private set; }

		/// <summary>
		/// 	Gets the anchor point.
		/// </summary>
		/// <value>
		/// 	The anchor point.
		/// </value>
		public int AnchorPoint { get; private set; }

		/// <summary>
		/// 	Gets the bottom point.
		/// </summary>
		/// <value>
		/// 	The bottom point.
		/// </value>
		public int BottomPoint { get; private set; }

		/// <summary>
		/// 	Gets the document text.
		/// </summary>
		/// <value>
		/// 	The document text.
		/// </value>
		public string DocumentText { get; private set; }

		/// <summary>
		/// 	Gets a value indicating whether this object is empty.
		/// </summary>
		/// <value>
		/// 	true if this object is empty, otherwise false.
		/// </value>
		public bool IsEmpty
		{
			get
			{
				return TopPoint == BottomPoint;
			}
		}
		/// <summary>
		/// 	Gets a value indicating whether this object is invalid.
		/// </summary>
		/// <value>
		/// 	true if this object is invalid, otherwise false.
		/// </value>
		public bool IsInvalid
		{
			get
			{
				return Equals(EditorPoints.Invalid);
			}
		}

		/// <summary>
		/// 	Gets the text that was selected in the TextSelection when
		/// 	this instance was created.
		/// </summary>
		/// <value>
		/// 	The text.
		/// </value>
		public string Text { get; private set; }

		/// <summary>
		/// 	Gets the top point.
		/// </summary>
		/// <value>
		/// 	The top point.
		/// </value>
		public int TopPoint { get; private set; }

		/// <summary>
		/// 	Creates an EditorPoints object from the given TextSelection.
		/// 	Normally the process modifies the TextSelection, but if
		/// 	keepSelection is set to true, the TextSelection is restored
		/// 	before returning to the caller.
		/// </summary>
		/// <param name="sel">
		/// 	The TextSelection from which to create the EditorPoints object.
		/// </param>
		/// <param name="keepSelection">
		/// 	(optional) Whether to prevent changes to the TextSelection.
		/// </param>
		/// <returns>
		/// 	The editor points.
		/// </returns>
		public static EditorPoints GetEditorPoints(TextSelection sel)
		{
			var ep = new EditorPoints(sel);
			ep.RestoreSelectedText(sel);

			return ep;
		}

		public static EditorPoints GetEditorPoints(int topPoint, int bottomPoint, string documentText)
		{
			return new EditorPoints(topPoint, bottomPoint, documentText);
		}

		public bool? IsActiveEndGreater { get; set; }

		/// <summary>
		/// 	Restore selected text.
		/// </summary>
		/// <param name="sel">
		/// 	The TextSelection to restore.
		/// </param>
		/// <returns>
		/// 	The original state of the TextSelection before calling creating
		/// 	this instance of EditorPoints.
		/// </returns>
		public TextSelection RestoreSelectedText(TextSelection sel)
		{
			if (IsActiveEndGreater == null)
				IsActiveEndGreater = sel.IsActiveEndGreater;

			if((bool)IsActiveEndGreater)
			{
				sel.MoveToAbsoluteOffset(TopPoint);
				sel.MoveToAbsoluteOffset(BottomPoint, true);
			}
			else
			{
				sel.MoveToAbsoluteOffset(BottomPoint);
				sel.MoveToAbsoluteOffset(TopPoint, true);
			}
			return sel;
		}

		public static EditorPoints GetInvalidEditorPoints()
		{
			return _invalid;
		}
	}
}
