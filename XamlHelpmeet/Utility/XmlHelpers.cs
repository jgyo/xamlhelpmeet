using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XamlHelpmeet.Utility
{
	public static class XmlHelpers
	{
		private const string TAGNAME_PATTERN = @"<(?<TagName>{0})[\s>]";
		private const string TAGNAMECHARS_PATTERN = @"[\w:\.]+?";
		private const string ENDTAG_PATTERN = @"</{0}>|<{0}(?:[\s/])[^>]*?/>";

		/// <summary>
		///  Regular expression built for C# on: Sun, Mar 3, 2013, 03:59:31 PM
		///  Using Expresso Version: 3.0.3634, http://www.ultrapico.com.
		///  Gets a list of Tag names from a Xml string.
		/// </summary>
		public static Regex TagNameRegex = new Regex(
			  string.Format(TAGNAME_PATTERN, TAGNAMECHARS_PATTERN),
			RegexOptions.IgnoreCase
			| RegexOptions.Multiline
			| RegexOptions.Singleline
			| RegexOptions.CultureInvariant
			| RegexOptions.IgnorePatternWhitespace
			);

		public static bool IsCompleteControl(this string target)
		{
			if(TagNameRegex.IsMatch(target)==false)
				return false;

			// Get the first tagname found.
			var tagName = TagNameRegex.Match(target).Groups["TagName"].Captures[0].Value;

			return target.IsCompleteControl(tagName);
		}

		public static bool IsCompleteControl(this string target, string tagName)
		{

			var tagNamePatternStart = string.Format(TAGNAME_PATTERN, tagName);
			var tagNamePatternStop = string.Format(ENDTAG_PATTERN, tagName);

			var endTagRegex = new Regex(tagNamePatternStop,
				RegexOptions.IgnoreCase
			| RegexOptions.Multiline
			| RegexOptions.Singleline
			| RegexOptions.CultureInvariant
			| RegexOptions.IgnorePatternWhitespace
			);
			var startTagRegex = new Regex(tagNamePatternStart,
				RegexOptions.IgnoreCase
			| RegexOptions.Multiline
			| RegexOptions.Singleline
			| RegexOptions.CultureInvariant
			| RegexOptions.IgnorePatternWhitespace
			);

			//if(target.EndsWith(tagNamePatternStop) ==false)
			//	return false;

			var starts = startTagRegex.Matches(target);
			var stops = endTagRegex.Matches(target);
			if(starts.Count != stops.Count)
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
	}
}
