using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace XamlHelpmeet.Model
{
	public class ExtractProperty
	{
		public XmlAttribute XmlAttribute { get; private set; }

		public bool IsSelected { get; set; }

		public string PropertyName { get; set; }

		public string PropertyValue { get; set; }

		public ExtractProperty(XmlAttribute XmlAttribute)
		{
			this.XmlAttribute = XmlAttribute;
		}
	}
}
