// <copyright file="TextSelectionExtensions.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/14/2013</date>
// <summary>Implements the text selection extensions class</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EnvDTE;

namespace XamlHelpmeet.Extensions
{
	/// <summary>
	/// 	Text selection extensions.
	/// </summary>
	public static class TextSelectionExtensions
	{
		/// <summary>
		/// 	A TextSelection extension method that sets a selection.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <param name="AnchorPoint">
		/// 	The anchor point.
		/// </param>
		/// <param name="ActivePoint">
		/// 	The active point.
		/// </param>
		public static void SetSelection(this TextSelection target, int AnchorPoint, int ActivePoint)
		{
			target.MoveToAbsoluteOffset(AnchorPoint);
			target.MoveToAbsoluteOffset(ActivePoint, true);
		}

		/// <summary>
		/// 	A TextSelection extension method that resets the selection.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <param name="originalPoints">
		/// 	The original points.
		/// </param>
		/// <returns>
		/// 	true if it succeeds, otherwise false.
		/// </returns>
		public static bool ResetSelection(this TextSelection target, EditorPoints originalPoints)
		{
			target.SetSelection(originalPoints.AnchorPoint, originalPoints.ActivePoint);
			// Usually called when a process fails. This makes it easier to
			// reply with false while reseting the seleciton.
			return false;
		}

		/// <summary>
		/// 	A TextSelection extension method that gets editor points.
		/// </summary>
		/// <param name="source">
		/// 	The source to act on.
		/// </param>
		/// <returns>
		/// 	The editor points.
		/// </returns>
		public static EditorPoints GetEditorPoints(this TextSelection source)
		{
			return EditorPoints.GetEditorPoints(source);
		}

		/// <summary>
		/// 	A TextSelection extension method that trims the given target.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		public static void Trim(this TextSelection target, bool? topActive = null)
		{
			if (topActive == null)
				topActive = target.IsActiveEndGreater == false;
			var text = target.Text.Replace(Environment.NewLine, "\n");
			var match = Regex.Match(text, @"\A\s*");
			if (match.Success== false)
				return;
			var len = match.Length;
			var topPoint = target.TopPoint.AbsoluteCharOffset+len;
			var bottomPoint = topPoint + text.Trim().Length;
			if ((bool)topActive)
				target.SetSelection(bottomPoint, topPoint);
			else
				target.SetSelection(topPoint, bottomPoint);
		}

		/// <summary>
		/// 	A TextSelection extension method that replaces the selected text.
		/// </summary>
		/// <param name="selection">
		/// 	The selection to act on.
		/// </param>
		/// <param name="newText">
		/// 	The new text.
		/// </param>
		public static void ReplaceSelectedText(this TextSelection selection, string newText)
		{
			selection.Trim();
			var editPoint = selection.TopPoint.CreateEditPoint();
			selection.Delete();
			var topPoint = selection.TopPoint.AbsoluteCharOffset;
			editPoint.Insert(newText);
			selection.MoveToAbsoluteOffset(selection.ActivePoint.AbsoluteCharOffset);
			selection.MoveToAbsoluteOffset(topPoint, true);
			selection.SmartFormat();
		}
	}
}
