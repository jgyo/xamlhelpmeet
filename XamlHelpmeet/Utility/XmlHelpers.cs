using System;
using System.Text.RegularExpressions;

namespace XamlHelpmeet.Utility
{
	public static class XmlHelpers
	{
		/// <summary>
		///  Regular expression built for C# on: Sun, Mar 3, 2013, 03:59:31 PM
		///  Using Expresso Version: 3.0.3634, http://www.ultrapico.com.
		///  Gets a list of Tag names from a Xml string.
		/// </summary>
		public static Regex TagNameRegex = new Regex(
			  string.Format(STARTTAG_PATTERN, TAGNAMECHARS_PATTERN),
			RegexOptions.IgnoreCase
			| RegexOptions.Multiline
			| RegexOptions.Singleline
			| RegexOptions.CultureInvariant
			| RegexOptions.IgnorePatternWhitespace
			);

		private const string ENDTAG_PATTERN = @"</{0}>|<{0}(?:[\s/])[^>]*?/>";
		private const string STARTTAG_PATTERN = @"<(?<TagName>{0})[\s>]";
		private const string TAGNAMECHARS_PATTERN = @"[\w:\.]+?";

		/// <summary>
		/// 	A string extension method that determine if all root tags are
		/// 	closed.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <returns>
		/// 	true if all root tags are closed, otherwise false.
		/// </returns>
		public static bool AreAllRootTagsClosed(this string target)
		{
			int targetIndex = 0;
			while (targetIndex < target.Length)
			{
				var endOfNode = target.Substring(targetIndex).FindEndOfNode();
				if (endOfNode == -1)
					return false;
				if (endOfNode == 0)
					return true;
				targetIndex += endOfNode;
			}
			return true;
		}

		/// <summary>
		/// 	A string extension method that searches for the first end of node of
		/// 	the first node. If no new starting or ending nodes are found, this
		/// 	method returns 0. If a starting node is found, but the ending node is
		/// 	not found, or if an ending node is found before the first starting
		/// 	node, -1 is returned. Otherwise the index to the character following
		/// 	the end tag is returned.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <returns>
		/// 	The found end of node.
		/// </returns>
		public static int FindEndOfNode(this string target)
		{
			var firstCloseTagIndex = target.IndexOf("</");

			// If no start and end tags....
			if (TagNameRegex.IsMatch(target) == false && firstCloseTagIndex == -1)
				return 0;

			// If there are close tags without open tags....
			if (TagNameRegex.IsMatch(target) == false)
				return -1;

			// Get the first tagname found.
			var tagName = TagNameRegex.Match(target).Groups["TagName"].Captures[0].Value;

			var startRegex = GetStartTagRegex(tagName);
			var startMatches = startRegex.Matches(target);

			// if first close tag comes before first open tag....
			if (startMatches[0].Captures[0].Index > firstCloseTagIndex)
				return -1;

			var endRegex = GetEndTagRegex(tagName);
			var endMatches = endRegex.Matches(target);

			// Find the end tag for the first open tag.
			var startsIndex = -1;
			var stopsIndex = 0;
			var depth = 0;
			int startsCharIndex;

			while (stopsIndex < endMatches.Count)
			{
				startsIndex++;
				if (startsIndex > startMatches.Count)
					return -1;
				if (startsIndex < startMatches.Count)
					startsCharIndex = startMatches[startsIndex].Captures[0].Index;
				else
					// All the remaining end tags follow the last start tag.
					// This will cause the end tags to be exhausted below.
					startsCharIndex = target.Length;

				var stopsCharIndex = endMatches[stopsIndex].Captures[0].Index;
				if (startsCharIndex <= stopsCharIndex)
				{
					// increase depth for new starting node if before next
					// closing node. Otherwise delay this increment until after
					// the closing node logic.
					depth++;
					if (startsCharIndex < stopsCharIndex)
						// restart loop if current node is not self closing.
						continue;
				}
				do
				{
					// To get here stopsCharIndex <= startsCharIndex
					depth--;	// decrease depth for each end node
					if (depth == 0)
					{
						// Found the last node! Calculate the result and leave.
						var lastAngle = target.IndexOf('>', stopsCharIndex);
						return lastAngle + 1;
					}

					if (startsCharIndex != target.Length)
						// Catch up for last start node skipped above.
						depth++;

					stopsIndex++;
					stopsCharIndex = endMatches[stopsIndex].Captures[0].Index;

					// do this loop for each closing node before the next
					// opening node.
				} while (stopsCharIndex <= startsCharIndex);
			}

			// Didn't find the last node if we are here.
			return -1;
		}

		/// <summary>
		/// 	A string extension method that searches for the first end of node
		/// 	of the first node following the start index. If no new starting
		/// 	or ending nodes are found, this method returns 0. If a starting
		/// 	node is found, but the ending node is not found, or if an
		/// 	ending node is found before the first starting node, -1 is
		/// 	returned. Otherwise the index to the character following the
		/// 	end tag is returned.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <param name="start">
		/// 	The start.
		/// </param>
		/// <returns>
		/// 	The found end of node.
		/// </returns>
		public static int FindEndOfNode(this string target, int start)
		{
			var tempResult = target.Substring(start).FindEndOfNode();
			return tempResult < 1 ? tempResult : tempResult + start;
		}

		/// <summary>
		/// 	A string extension method that searches for the first end of node
		/// 	named by tagName. Returns the index to the character following the
		/// 	tag, if the node is found. Otherwise -1 is returned.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <param name="tagName">
		/// 	Name of the tag.
		/// </param>
		/// <returns>
		/// 	The found end of node.
		/// </returns>
		public static int FindEndOfNode(this string target, string tagName)
		{
			var startRegex = GetStartTagRegex(tagName);
			if (!startRegex.IsMatch(target))
				return -1;
			var match = startRegex.Match(target);
			var startIndex = match.Captures[0].Index;
			var tempResult = target.Substring(startIndex).FindEndOfNode();
			return tempResult == -1 ? -1 : tempResult + startIndex;
		}

		/// <summary>
		/// 	A string extension method that searches for the first end of
		/// 	node named by tagName. Returns the index to the character
		/// 	following the tag, if the node is found. Otherwise -1 is
		/// 	returned.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <param name="tagName">
		/// 	Name of the tag.
		/// </param>
		/// <param name="start">
		/// 	The start.
		/// </param>
		/// <returns>
		/// 	The found end of node.
		/// </returns>
		public static int FindEndOfNode(this string target, string tagName, int start)
		{
			var tempResult = target.Substring(start).FindEndOfNode(tagName);
			return tempResult == -1 ? -1 : tempResult + start;
		}

		/// <summary>
		/// 	A string extension method that queries if 'target' is complete
		/// 	control.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <returns>
		/// 	true if complete control, otherwise false.
		/// </returns>
		public static bool IsCompleteControl(this string target)
		{
			if (TagNameRegex.IsMatch(target) == false)
				return false;

			// Get the first tagname found.
			var tagName = TagNameRegex.Match(target).Groups["TagName"].Captures[0].Value;

			return target.IsCompleteControl(tagName);
		}

		/// <summary>
		/// 	A string extension method that queries if 'target' is complete
		/// 	control.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <param name="tagName">
		/// 	Name of the tag.
		/// </param>
		/// <returns>
		/// 	true if complete control, otherwise false.
		/// </returns>
		public static bool IsCompleteControl(this string target, string tagName)
		{
			var endTagRegex = RegexFactory(ENDTAG_PATTERN, tagName);
			var startTagRegex = RegexFactory(STARTTAG_PATTERN, tagName);

			//if(target.EndsWith(tagNamePatternStop) ==false)
			//	return false;

			var starts = startTagRegex.Matches(target);
			var stops = endTagRegex.Matches(target);
			if (starts.Count != stops.Count)
				return false;

			if (starts[0].Captures[0].Index != 0 || stops[stops.Count - 1].Captures[0].Length + stops[stops.Count - 1].Captures[0].Index != target.Length)
				return false;

			var startsIndex = -1;
			var stopsIndex = 0;
			var depth = 0;
			int startsCharIndex;

			while (stopsIndex < stops.Count)
			{
				startsIndex++;
				if (startsIndex > starts.Count)
					return false;
				if (startsIndex < starts.Count)
					startsCharIndex = starts[startsIndex].Captures[0].Index;
				else
					startsCharIndex = target.Length;
				var stopsCharIndex = stops[stopsIndex].Captures[0].Index;
				if (startsCharIndex <= stopsCharIndex)
				{
					depth++;
					continue;
				}
				if (startsCharIndex >= stopsCharIndex)
				{
					while (startsCharIndex >= stopsCharIndex)
					{
						depth--;
						stopsIndex++;
						if (depth == 0)
							break;
						if (stopsIndex < stops.Count)
						{
							stopsCharIndex = stops[stopsIndex].Captures[0].Index;
							continue;
						}
						if (depth != 0)
							return false;	// Last stop tag, but still in deep.
						break;
					}
					if (depth == 0 && (stopsIndex < stops.Count || startsIndex < starts.Count))
						return false;	// Found last tag before end of open tags.
					if (startsCharIndex < target.Length)
						depth++;
				}
			}
			return true;
		}

		private static Regex GetEndTagRegex(string tagName)
		{
			return RegexFactory(ENDTAG_PATTERN, tagName);
		}

		private static Regex GetStartTagRegex(string tagName)
		{
			return RegexFactory(STARTTAG_PATTERN, tagName);
		}

		private static Regex RegexFactory(string regexPattern, string tagName)
		{
			return new Regex(string.Format(regexPattern, tagName),
							RegexOptions.IgnoreCase
						| RegexOptions.Multiline
						| RegexOptions.Singleline
						| RegexOptions.CultureInvariant
						| RegexOptions.IgnorePatternWhitespace
						);
		}
	}
}