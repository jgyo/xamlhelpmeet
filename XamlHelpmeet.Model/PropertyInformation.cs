using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using XamlHelpmeet.Extentions;

// namespace: XamlHelpmeet.Model
//
// summary:	PowerToy Models.
namespace XamlHelpmeet.Model
{
	[Serializable]
	public class PropertyInformation : INotifyPropertyChanged
	{
		private string _stringFormat;
		private bool _canWrite;
		private string _descriptionViewerPosition;
		private string _fieldDescription;
		private ControlType _fieldListControlType;
		private bool _fieldListIncludGridAttachedProperties;
		private string _fullName;
		private List<string> _genericArguments;
		private bool _hasBeenUsed;
		private bool _isSelected;
		private string _labelPosition;
		private string _name;
		private List<PropertyParameter> _propertyParameters;
		private string _typeName;
		private string _typeNamespace;

		/// <summary>
		/// 	Gets or sets a value indicating whether we can write.
		/// </summary>
		/// <value>
		/// 	true if we can write, false if not.
		/// </value>

		// BMK Complete Constructor
		public PropertyInformation(bool CanWrite, string Name, string TypeName, string TypeNamespace)
		{
			this.CanWrite = CanWrite;
			this.Name = Name;
			this.TypeName = TypeName;
			this.TypeNamespace = TypeNamespace;

			if (TypeNamespace.Contains("Decimal"))
			{
				StringFormat = "{0:c}";
			}
			else if (TypeName.Contains("Date"))
			{
				StringFormat = "{0:d}";
			}
		}

		public bool CanWrite
		{
			get
			{
				return _canWrite;
			}
			set
			{
				_canWrite = value;
				OnPropertyChanged("CanWrite");
			}
		}

		/// <summary>
		/// 	Gets the name of the private field.
		/// </summary>
		/// <value>
		/// 	The name of the private field.
		/// </value>

		public string CPrivateFieldName
		{
			get
			{
				return string.Concat("_", PascalFieldName);
			}
		}

		public string CSParameterString
		{
			get
			{
				if (_propertyParameters.Count == 0)
					return string.Empty;

				var result = string.Empty;

				foreach (var obj in _propertyParameters)
				{
					result = String.Format("{0}{1} {2}, ", result, obj.ParameterTypeName, obj.ParameterName);
				}
				return result.Substring(0, result.Length-2);
			}
		}

		/// <summary>
		/// 	Gets or sets the description viewer position.
		/// </summary>
		/// <value>
		/// 	The description viewer position.
		/// </value>

		public string DescriptionViewerPosition
		{
			get
			{
				return _descriptionViewerPosition;
			}
			set
			{
				_descriptionViewerPosition = value;
				OnPropertyChanged("DescriptionViewerPosition");
			}
		}

		/// <summary>
		/// 	Gets or sets the information describing the field.
		/// </summary>
		/// <value>
		/// 	Information describing the field.
		/// </value>

		public string FieldDescription
		{
			get
			{
				return _fieldDescription;
			}
			set
			{
				_fieldDescription = value;
				OnPropertyChanged("FieldDescription");
			}
		}

		/// <summary>
		/// 	Gets or sets the type of the field list control.
		/// </summary>
		/// <value>
		/// 	The type of the field list control.
		/// </value>

		public ControlType FieldListControlType
		{
			get
			{
				return _fieldListControlType;
			}
			set
			{
				_fieldListControlType = value;
				OnPropertyChanged("FieldListControlType");
			}
		}

		/// <summary>
		/// 	Gets or sets a value indicating whether the field list includ grid attached
		/// 	properties.
		/// </summary>
		/// <value>
		/// 	true if field list includ grid attached properties, false if not.
		/// </value>

		public bool FieldListIncludGridAttachedProperties
		{
			get
			{
				return _fieldListIncludGridAttachedProperties;
			}
			set
			{
				_fieldListIncludGridAttachedProperties = value;
				OnPropertyChanged("FieldListIncludGridAttachedProperties");
			}
		}

		/// <summary>
		/// 	Gets or sets the full name of the property.
		/// </summary>
		/// <value>
		/// 	The full name of the property.
		/// </value>

		public string FullName
		{
			get
			{
				return _fullName;
			}
			set
			{
				_fullName = value;
				OnPropertyChanged("FullName");
			}
		}

		/// <summary>
		/// 	Gets or sets the generic arguments.
		/// </summary>
		/// <value>
		/// 	The generic arguments.
		/// </value>

		public List<string> GenericArguments
		{
			get
			{
				if (_genericArguments == null)
					_genericArguments = new List<string>();
				return _genericArguments;
			}
			set
			{
				_genericArguments = value;
				OnPropertyChanged("GenericArguments");
			}
		}

		/// <summary>
		/// 	Gets or sets a value indicating whether this PropertyInformation has been
		/// 	used.
		/// </summary>
		/// <value>
		/// 	true if this PropertyInformation has been used, false if not.
		/// </value>

		public bool HasBeenUsed
		{
			get
			{
				return _hasBeenUsed;
			}
			set
			{
				_hasBeenUsed = value;
				OnPropertyChanged("HasBeenUsed");
			}
		}

		/// <summary>
		/// 	Gets or sets a value indicating whether this PropertyInformation is selected.
		/// </summary>
		/// <value>
		/// 	true if this PropertyInformation is selected, false if not.
		/// </value>

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				_isSelected = value;
				OnPropertyChanged("IsSelected");
			}
		}

		/// <summary>
		/// 	Gets or sets the label position.
		/// </summary>
		/// <value>
		/// 	The label position.
		/// </value>

		public string LabelPosition
		{
			get
			{
				return _labelPosition;
			}
			set
			{
				_labelPosition = value;
				OnPropertyChanged("LabelPosition");
			}
		}

		/// <summary>
		/// 	Gets the label text.
		/// </summary>
		/// <value>
		/// 	The label text.
		/// </value>

		public string LabelText
		{
			get
			{
				var value = Name;
				var sb = new StringBuilder(256);
				var foundUppercase = false;

				for (var i = 0; i < value.Length; i++)
				{
					if (!foundUppercase && value.IsUpper(i))
					{
						foundUppercase = true;
						if (i == 0)
						{
							sb.Append(value[i]);
						}
						else
						{
							sb.Append(' ');
							sb.Append(value[i]);
						}
						continue;
					}
					if (!foundUppercase)
					{
						continue;
					}
					if (value.IsUpper(i))
					{
						sb.Append(' ');
						sb.Append(value[i]);
					}
					else
					{
						if (value.IsLetterOrDigit(i))
						{
							sb.Append(value[i]);
						}
					}
				}
				return sb.ToString();
			}
		}

		/// <summary>
		/// 	Gets or sets the name.
		/// </summary>
		/// <value>
		/// 	The name.
		/// </value>

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				OnPropertyChanged("Name");
			}
		}

		/// <summary>
		/// 	Gets the name and writeable.
		/// </summary>
		/// <value>
		/// 	The name and writeable.
		/// </value>

		public string NameAndWriteable
		{
			get
			{
				if (CanWrite)
				{
					return Name;
				}
				return String.Format("{0}  (r)", Name);
			}
		}

		/// <summary>
		/// 	Gets the name of the name space type.
		/// </summary>
		/// <value>
		/// 	The name of the name space type.
		/// </value>

		public string NameSpaceTypeName
		{
			get
			{
				return string.Concat(TypeNamespace, ":", TypeName);
			}
		}

		/// <summary>
		/// 	Gets the name in Pascal style.
		/// </summary>
		/// <value>
		/// 	The name in Pascal case.
		/// </value>

		public string PascalFieldName
		{
			get
			{
				return string.Concat(Name[0].ToLower(), Name.Substring(1));
			}
		}

		/// <summary>
		/// 	Gets or sets options for controlling the property.
		/// </summary>
		/// <value>
		/// 	Options that control the property.
		/// </value>

		public List<PropertyParameter> PropertyParameters
		{
			get
			{
				return _propertyParameters;
			}
			set
			{
				_propertyParameters = value;
				OnPropertyChanged("PropertyParameters");
			}
		}

		public string StringFormat
		{
			get
			{
				return _stringFormat;
			}
			set
			{
				_stringFormat = value;
				OnPropertyChanged("StringFormat");
			}
		}

		/// <summary>
		/// 	Gets or sets the name of the type.
		/// </summary>
		/// <value>
		/// 	The name of the type.
		/// </value>

		public string TypeName
		{
			get
			{
				return _typeName;
			}
			set
			{
				_typeName = value;
				OnPropertyChanged("TypeName");
			}
		}

		/// <summary>
		/// 	Gets or sets the type namespace.
		/// </summary>
		/// <value>
		/// 	The type namespace.
		/// </value>

		public string TypeNamespace
		{
			get
			{
				return _typeNamespace;
			}
			set
			{
				_typeNamespace = value;
				OnPropertyChanged("TypeNamespace");
			}
		}

		public string PropertyParameterString(LanguageTypes Language)
		{
			if (PropertyParameters.Count == 0 || Language == LanguageTypes.Unknown)
			{
				return string.Empty;
			}
			var sb = new StringBuilder(512);
			var LanguageTypeFormat = Language == LanguageTypes.VisualBasic ? "{3}{0} As {1}, " : "{3}{1} {0}, ";
			foreach (var obj in PropertyParameters)
			{
				sb.AppendFormat(LanguageTypeFormat, obj.ParameterName, obj.ParameterTypeName);
			}
			sb.Length = sb.Length - 2;
			return sb.ToString();
		}

		public string VBPrivateFieldName(bool UseHungarian)
		{
			if (UseHungarian)
			{
				return string.Concat(GetHungarian(), PascalFieldName);
			}

			return string.Concat("_", PascalFieldName);
		}

		public string VBTypeName()
		{
			if (TypeName.EndsWith("]"))
			{
				return TypeName.Replace("[", "(").Replace("]", ")");
			}

			return TypeName;
		}

		private string GetHungarian()
		{
			switch (TypeName)
			{
				case "Boolean":
					return "_bol";
				case "Byte":
					return "_byt";
				case "Char":
					return "_chr";
				case "DateTime":
					return "_dat";
				case "Decimal":
					return "_dec";
				case "Double":
					return "_dbl";
				case "Int16":
					return "_i16";
				case "Integer":
				case "Int32":
					return "_int";
				case "Int64":
					return "_i64";
				case "Single":
					return "_sng";
				case "String":
					return "_str";
				default:
					return "_obj";
			}
		}

		/// <summary>
		/// 	Executes the property changed action.
		/// </summary>
		/// <param name="PropertyName">
		/// 	Name of the property.
		/// </param>

		private void OnPropertyChanged(string PropertyName)
		{
			var h = PropertyChanged;
			if (h == null)
			{
				return;
			}
			h(this, new PropertyChangedEventArgs(PropertyName));
		}

		#region "INotifyPropertyChanged Interface Implementation"

		/// <summary>
		/// Event inherited from the INotifyPropertyChanged interface
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion "INotifyPropertyChanged Interface Implementation"
	}
}