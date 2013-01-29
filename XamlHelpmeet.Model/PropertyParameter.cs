using System;

namespace XamlHelpmeet.Model
{
	[Serializable]
	public class PropertyParameter
	{
		public PropertyParameter(string parameterName, string parameterTypeName)
		{
			this.ParameterName = parameterName;
			this.ParameterTypeName = parameterTypeName;
		}

		public string ParameterName { get; set; }

		public string ParameterTypeName { get; set; }
	}
}