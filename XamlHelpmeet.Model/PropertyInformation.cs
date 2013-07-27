 // namespace: XamlHelpmeet.Model
//
// summary:	PowerToy Models.

namespace XamlHelpmeet.Model
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using XamlHelpmeet.Extensions;

    #endregion

    [Serializable]
    public class PropertyInformation : INotifyPropertyChanged
    {
        #region Fields

        private bool _canWrite;

        private string _descriptionViewerPosition;

        private string _fieldDescription;

        private ControlType _fieldListControlType;

        private bool _fieldListIncludeGridAttachedProperties;

        private string _fullName;

        private List<string> _genericArguments;

        private bool _hasBeenUsed;

        private bool _isSelected;

        private string _labelPosition;

        private string _name;

        private List<PropertyParameter> _propertyParameters;

        private string _stringFormat;

        private string _typeName;

        private string _typeNamespace;

        #endregion

        #region Constructors

        /// <summary>
        ///     Gets or sets a value indicating whether we can write.
        /// </summary>
        /// <value>
        ///     true if we can write, false if not.
        /// </value>
        // BMK Complete Constructor
        public PropertyInformation(bool canWrite, string name, string typeName, string typeNamespace)
        {
            this.CanWrite = canWrite;
            this.Name = name;
            this.TypeName = typeName;
            this.TypeNamespace = typeNamespace;

            if (typeNamespace.Contains("Decimal"))
            {
                this.StringFormat = "{0:c}";
            }
            else if (typeName.Contains("Date"))
            {
                this.StringFormat = "{0:d}";
            }
        }

        #endregion

        #region Properties and Indexers

        public bool CanWrite
        {
            get { return this._canWrite; }
            set
            {
                this._canWrite = value;
                this.OnPropertyChanged("CanWrite");
            }
        }

        /// <summary>
        ///     Gets the name of the private field.
        /// </summary>
        /// <value>
        ///     The name of the private field.
        /// </value>
        public string CPrivateFieldName
        {
            get { return string.Concat("_", this.PascalFieldName); }
        }

        public string CSParameterString
        {
            get
            {
                if (this._propertyParameters.Count == 0)
                {
                    return string.Empty;
                }

                string result = string.Empty;

                foreach (var obj in this._propertyParameters)
                {
                    result = String.Format("{0}{1} {2}, ", result, obj.ParameterTypeName, obj.ParameterName);
                }
                return result.Substring(0, result.Length - 2);
            }
        }

        /// <summary>
        ///     Gets or sets the description viewer position.
        /// </summary>
        /// <value>
        ///     The description viewer position.
        /// </value>
        public string DescriptionViewerPosition
        {
            get { return this._descriptionViewerPosition; }
            set
            {
                this._descriptionViewerPosition = value;
                this.OnPropertyChanged("DescriptionViewerPosition");
            }
        }

        /// <summary>
        ///     Gets or sets the information describing the field.
        /// </summary>
        /// <value>
        ///     Information describing the field.
        /// </value>
        public string FieldDescription
        {
            get { return this._fieldDescription; }
            set
            {
                this._fieldDescription = value;
                this.OnPropertyChanged("FieldDescription");
            }
        }

        /// <summary>
        ///     Gets or sets the type of the field list control.
        /// </summary>
        /// <value>
        ///     The type of the field list control.
        /// </value>
        public ControlType FieldListControlType
        {
            get { return this._fieldListControlType; }
            set
            {
                this._fieldListControlType = value;
                this.OnPropertyChanged("FieldListControlType");
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the field list includ grid attached
        ///     properties.
        /// </summary>
        /// <value>
        ///     true if field list includ grid attached properties, false if not.
        /// </value>
        public bool FieldListIncludeGridAttachedProperties
        {
            get { return this._fieldListIncludeGridAttachedProperties; }
            set
            {
                this._fieldListIncludeGridAttachedProperties = value;
                this.OnPropertyChanged("FieldListIncludeGridAttachedProperties");
            }
        }

        /// <summary>
        ///     Gets or sets the full name of the property.
        /// </summary>
        /// <value>
        ///     The full name of the property.
        /// </value>
        public string FullName
        {
            get { return this._fullName; }
            set
            {
                this._fullName = value;
                this.OnPropertyChanged("FullName");
            }
        }

        /// <summary>
        ///     Gets or sets the generic arguments.
        /// </summary>
        /// <value>
        ///     The generic arguments.
        /// </value>
        public List<string> GenericArguments
        {
            get
            {
                if (this._genericArguments == null)
                {
                    this._genericArguments = new List<string>();
                }
                return this._genericArguments;
            }
            set
            {
                this._genericArguments = value;
                this.OnPropertyChanged("GenericArguments");
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this PropertyInformation has been
        ///     used.
        /// </summary>
        /// <value>
        ///     true if this PropertyInformation has been used, false if not.
        /// </value>
        public bool HasBeenUsed
        {
            get { return this._hasBeenUsed; }
            set
            {
                this._hasBeenUsed = value;
                this.OnPropertyChanged("HasBeenUsed");
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this PropertyInformation is
        ///     selected.
        /// </summary>
        /// <value>
        ///     true if this PropertyInformation is selected, false if not.
        /// </value>
        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                this._isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        /// <summary>
        ///     Gets or sets the label position.
        /// </summary>
        /// <value>
        ///     The label position.
        /// </value>
        public string LabelPosition
        {
            get { return this._labelPosition; }
            set
            {
                this._labelPosition = value;
                this.OnPropertyChanged("LabelPosition");
            }
        }

        /// <summary>
        ///     Gets the label text.
        /// </summary>
        /// <value>
        ///     The label text.
        /// </value>
        public string LabelText
        {
            get
            {
                string value = this.Name;
                var sb = new StringBuilder(256);
                bool foundUppercase = false;

                for (int i = 0; i < value.Length; i++)
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
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name
        {
            get { return this._name; }
            set
            {
                if (this._name == value)
                {
                    return;
                }

                this._name = value;
                this.OnPropertyChanged("Name");
                UpdateFullName();
            }
        }

        private void UpdateFullName()
        {
            FullName = string.Format("{0} - {1}", this.NameAndWritable, this.TypeName);
        }

        /// <summary>
        ///     Gets the name and writeable.
        /// </summary>
        /// <value>
        ///     The name and writeable.
        /// </value>
        public string NameAndWritable
        {
            get { return this.CanWrite ? this.Name : String.Format("{0}  (r)", this.Name); }
        }

        /// <summary>
        ///     Gets the name of the name space type.
        /// </summary>
        /// <value>
        ///     The name of the name space type.
        /// </value>
        public string NameSpaceTypeName
        {
            get { return string.Concat(this.TypeNamespace, ":", this.TypeName); }
        }

        /// <summary>
        ///     Gets the name in Pascal style.
        /// </summary>
        /// <value>
        ///     The name in Pascal case.
        /// </value>
        public string PascalFieldName
        {
            get { return string.Concat(this.Name[0].ToLower(), this.Name.Substring(1)); }
        }

        /// <summary>
        ///     Gets or sets options for controlling the property.
        /// </summary>
        /// <value>
        ///     Options that control the property.
        /// </value>
        public List<PropertyParameter> PropertyParameters
        {
            get { return this._propertyParameters; }
            set
            {
                this._propertyParameters = value;
                this.OnPropertyChanged("PropertyParameters");
            }
        }

        public string StringFormat
        {
            get { return this._stringFormat; }
            set
            {
                this._stringFormat = value;
                this.OnPropertyChanged("StringFormat");
            }
        }

        /// <summary>
        ///     Gets or sets the name of the type.
        /// </summary>
        /// <value>
        ///     The name of the type.
        /// </value>
        public string TypeName
        {
            get { return this._typeName; }
            set
            {
                if (this._typeName == value)
                {
                    return;
                }

                this._typeName = value;
                this.OnPropertyChanged("TypeName");
                UpdateFullName();
            }
        }

        /// <summary>
        ///     Gets or sets the type namespace.
        /// </summary>
        /// <value>
        ///     The type namespace.
        /// </value>
        public string TypeNamespace
        {
            get { return this._typeNamespace; }
            set
            {
                this._typeNamespace = value;
                this.OnPropertyChanged("TypeNamespace");
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        ///     Event inherited from the INotifyPropertyChanged interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods (public)

        public string PropertyParameterString(LanguageTypes Language)
        {
            if (this.PropertyParameters.Count == 0 || Language == LanguageTypes.Unknown)
            {
                return string.Empty;
            }
            var sb = new StringBuilder(512);
            string LanguageTypeFormat = Language == LanguageTypes.VisualBasic ? "{3}{0} As {1}, " : "{3}{1} {0}, ";
            foreach (var obj in this.PropertyParameters)
            {
                sb.AppendFormat(LanguageTypeFormat, obj.ParameterName, obj.ParameterTypeName);
            }
            sb.Length = sb.Length - 2;
            return sb.ToString();
        }

        public string VBPrivateFieldName(bool UseHungarian)
        {
            return string.Concat(UseHungarian ? this.GetHungarian() : "_", this.PascalFieldName);
        }

        public string VBTypeName()
        {
            return this.TypeName.EndsWith("]") ? this.TypeName.Replace("[", "(").Replace("]", ")") : this.TypeName;
        }

        #endregion

        #region Methods (private)

        private string GetHungarian()
        {
            switch (this.TypeName)
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
        ///     Executes the property changed action.
        /// </summary>
        /// <param name="PropertyName">
        ///     Name of the property.
        /// </param>
        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler h = this.PropertyChanged;
            if (h == null)
            {
                return;
            }
            h(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
    }
}
