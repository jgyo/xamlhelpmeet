using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace XamlHelpmeet.Utility.XamlParts
{
	public class XamlTag : XamlPart
	{
		public XamlTag(Match match) : base(match.Value, match.Index)
		{
			Match matchResults = null;
			try
			{
				matchResults = Regex.Match(match.Value,
					@"^((?'Start'<(?'TagName'[\w:.]+?)(?=[\s>])[^><]*?(?<!/)>)|
					(?'End'</(?'TagName'[\w:.]+?)(?=[\s>]).*?>)|
					(?'SelfClosed'<(?'TagName'[\w:.]+?)(?=[\s/])[^><]*?/>))$",
					RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
				if (matchResults.Success)
				{
					Name = matchResults.Groups["TagName"].Captures[0].Value;
					TagType = matchResults.Groups["Start"].Success ? XamlTagType.Starting :
						matchResults.Groups["End"].Success ? XamlTagType.Ending :
						matchResults.Groups["SelfClosed"].Success ? XamlTagType.SelfClosing :
						XamlTagType.Unknown;
				}
				else
				{
					throw new ArgumentException("The argument does not contain an XML tag match.", "match");
				}
			}
			catch (ArgumentException ex)
			{
				throw new ArgumentException("The regular expression contains a syntax error.", ex);
			}
			TagText = match.Value;
			StartingIndex = match.Index;
			Length = TagText.Length;
			EndingIndex = StartingIndex + Length;
		}
		public int StartingIndex { get; private set; }

		public int EndingIndex { get; private set; }

		public int Length { get; private set; }		

		public string TagText { get; private set; }
		public XamlTagType TagType { get; private set; }

		public string Name
		{
			get;
			private set;
		}

		private List<XamlAttribute> _attributes;
		public IReadOnlyList<XamlAttribute> GetAttributes()
		{
			if (_attributes == null)
			{
				MatchCollection matches = null;
				try
				{
					var regexObj = new Regex(@"(?:\s+(?<Attribute>(?<AttribName>\w+?(?::\w+?)?)=""(?<AttribValue>.*?)""))", RegexOptions.Singleline | RegexOptions.IgnoreCase);
					matches = regexObj.Matches(TagText);
					if (matches.Count > 0)
					{
						_attributes = new List<XamlAttribute>();
						foreach (Match match in matches)
						{
							var attrib = new XamlAttribute(match.Value, TopPoint + match.Index, match.Groups["AttribName"].Captures[0].Value, match.Groups["AttribValue"].Captures[0].Value);
							_attributes.Add(attrib);
						}
					}
				}
				catch (ArgumentException ex)
				{
					// Syntax error in the regular expression
					throw new ArgumentException("Syntax error in regular expression.", ex);
				}
			}
			return _attributes;
		}
	}
}
