using System;
using System.Collections.Generic;
using System.Linq;

namespace XamlHelpmeet.Model
{
	[Serializable]
	public class SampleFormat
	{
		public string DataType { get; set; }

		public string Example { get; set; }

		public SampleFormat(string DataType, string Example, string StringFormat)
		{
			this.DataType = DataType;
			this.Example = Example;
			this.StringFormat = StringFormat;
		}
		public string StringFormat { get; set; }

		public string StringFormatAndExample
		{
			get
			{
				return ToString();
			}
		}

		public string StringFormatParsedValue
		{
			get
			{
				// I don't understand the purpose of replacing the backslash
				// character here with nothing. Perhaps it is to prevent escaped
				// characters from being used, but the non-escaped characters
				// would remain. 
				return StringFormat.Replace(@"\", string.Empty);
			}
		}



		public override String ToString()
		{
			return string.Format("{0} - {1}", StringFormatParsedValue, Example);
		}

	}
}
