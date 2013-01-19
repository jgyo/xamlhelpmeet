using System;

namespace XamlHelpmeet.Model
{
	[Serializable]
	public class PropertyParameter
	{
		public PropertyParameter(string ParameterName, string ParameterTypeName)
		{
			this.ParameterName = ParameterName;
			this.ParameterTypeName = ParameterTypeName;
		}

		public string ParameterName { get; set; }

		public string ParameterTypeName { get; set; }
	}
}