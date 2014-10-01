// namespace: XamlHelpmeet.Model
//
// summary: PowerToy Models.

namespace XamlHelpmeet.Model
{
#region Imports

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using NLog;

using XamlHelpmeet.Extensions;

using YoderZone.Extensions.NLog;

#endregion

/// <summary>
/// Information about the property.
/// </summary>
/// <seealso cref="T:System.ComponentModel.INotifyPropertyChanged"/>
[Serializable]
public class PropertyInformation : INotifyPropertyChanged
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    #region Fields
    /// <summary>
    /// true if this XamlHelpmeet.Model.PropertyInformation can write.
    /// </summary>
    private bool canWrite;

    /// <summary>
    /// The description viewer position.
    /// </summary>
    private string descriptionViewerPosition;

    /// <summary>
    /// Information describing the field.
    /// </summary>
    private string fieldDescription;

    /// <summary>
    /// Type of the field list control.
    /// </summary>
    private ControlType fieldListControlType;

    /// <summary>
    /// true to field list include grid attached properties.
    /// </summary>
    private bool fieldListIncludeGridAttachedProperties;

    /// <summary>
    /// Name of the full.
    /// </summary>
    private string fullName;

    /// <summary>
    /// The generic arguments.
    /// </summary>
    private List<string> genericArguments;

    /// <summary>
    /// true if this XamlHelpmeet.Model.PropertyInformation has been used.
    /// </summary>
    private bool hasBeenUsed;

    /// <summary>
    /// true if this XamlHelpmeet.Model.PropertyInformation is selected.
    /// </summary>
    private bool isSelected;

    /// <summary>
    /// The label position.
    /// </summary>
    private string labelPosition;

    /// <summary>
    /// The name.
    /// </summary>
    private string name;

    /// <summary>
    /// Options for controlling the property.
    /// </summary>
    private List<PropertyParameter> propertyParameters;

    /// <summary>
    /// The string format.
    /// </summary>
    private string stringFormat;

    /// <summary>
    /// Name of the type.
    /// </summary>
    private string typeName;

    /// <summary>
    /// The type namespace.
    /// </summary>
    private string typeNamespace;

    #endregion

    #region Constructors
    /// <summary>
    /// Gets or sets a value indicating whether we can write.
    /// </summary>
    /// <param name="canWrite">
    /// true if this XamlHelpmeet.Model.PropertyInformation can write.
    /// </param>
    /// <param name="name">
    /// The name.
    /// </param>
    /// <param name="typeName">
    /// Name of the type.
    /// </param>
    /// <param name="typeNamespace">
    /// The type namespace.
    /// </param>
    ///
    /// ### <value>
    /// true if we can write, false if not.
    /// </value>
    public PropertyInformation(bool canWrite, string name, string typeName,
                               string typeNamespace)
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
    /// <summary>
    /// Gets or sets a value indicating whether we can write.
    /// </summary>
    /// <value>
    /// true if we can write, false if not.
    /// </value>
    public bool CanWrite
    {
        get { return this.canWrite; }
        set
        {
            this.canWrite = value;
            this.OnPropertyChanged("CanWrite");
        }
    }

    /// <summary>
    /// Gets the name of the private field.
    /// </summary>
    /// <value>
    /// The name of the private field.
    /// </value>
    public string CPrivateFieldName
    {
        get { return string.Concat("_", this.PascalFieldName); }
    }

    /// <summary>
    /// Gets the create struct parameter string.
    /// </summary>
    /// <value>
    /// The create struct parameter string.
    /// </value>
    public string CSParameterString
    {
        get
        {
            if (this.propertyParameters.Count == 0)
            {
                return string.Empty;
            }

            string result = this.propertyParameters.Aggregate(string.Empty, (current,
                            obj) => String.Format("{0}{1} {2}, ", current, obj.ParameterTypeName,
                                                  obj.ParameterName));

            return result.Substring(0, result.Length - 2);
        }
    }

    /// <summary>
    /// Gets or sets the description viewer position.
    /// </summary>
    /// <value>
    /// The description viewer position.
    /// </value>
    public string DescriptionViewerPosition
    {
        get { return this.descriptionViewerPosition; }
        set
        {
            this.descriptionViewerPosition = value;
            this.OnPropertyChanged("DescriptionViewerPosition");
        }
    }

    /// <summary>
    /// Gets or sets the information describing the field.
    /// </summary>
    /// <value>
    /// Information describing the field.
    /// </value>
    public string FieldDescription
    {
        get { return this.fieldDescription; }
        set
        {
            this.fieldDescription = value;
            this.OnPropertyChanged("FieldDescription");
        }
    }

    /// <summary>
    /// Gets or sets the type of the field list control.
    /// </summary>
    /// <value>
    /// The type of the field list control.
    /// </value>
    public ControlType FieldListControlType
    {
        get { return this.fieldListControlType; }
        set
        {
            this.fieldListControlType = value;
            this.OnPropertyChanged("FieldListControlType");
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the field list includ
    /// grid attached properties.
    /// </summary>
    /// <value>
    /// true if field list includes grid attached properties, false if not.
    /// </value>
    public bool FieldListIncludeGridAttachedProperties
    {
        get { return this.fieldListIncludeGridAttachedProperties; }
        set
        {
            this.fieldListIncludeGridAttachedProperties = value;
            this.OnPropertyChanged("FieldListIncludeGridAttachedProperties");
        }
    }

    /// <summary>
    /// Gets or sets the full name of the property.
    /// </summary>
    /// <value>
    /// The full name of the property.
    /// </value>
    public string FullName
    {
        get { return this.fullName; }
        set
        {
            this.fullName = value;
            this.OnPropertyChanged("FullName");
        }
    }

    /// <summary>
    /// Gets or sets the generic arguments.
    /// </summary>
    /// <value>
    /// The generic arguments.
    /// </value>
    public List<string> GenericArguments
    {
        get
        {
            return this.genericArguments ?? (this.genericArguments = new
            List<string>());
        }
        set
        {
            this.genericArguments = value;
            this.OnPropertyChanged("GenericArguments");
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this PropertyInformation
    /// has been used.
    /// </summary>
    /// <value>
    /// true if this PropertyInformation has been used, false if not.
    /// </value>
    public bool HasBeenUsed
    {
        get { return this.hasBeenUsed; }
        set
        {
            this.hasBeenUsed = value;
            this.OnPropertyChanged("HasBeenUsed");
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this PropertyInformation
    /// is selected.
    /// </summary>
    /// <value>
    /// true if this PropertyInformation is selected, false if not.
    /// </value>
    public bool IsSelected
    {
        get { return this.isSelected; }
        set
        {
            this.isSelected = value;
            this.OnPropertyChanged("IsSelected");
        }
    }

    /// <summary>
    /// Gets or sets the label position.
    /// </summary>
    /// <value>
    /// The label position.
    /// </value>
    public string LabelPosition
    {
        get { return this.labelPosition; }
        set
        {
            this.labelPosition = value;
            this.OnPropertyChanged("LabelPosition");
        }
    }

    /// <summary>
    /// Gets the label text.
    /// </summary>
    /// <value>
    /// The label text.
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
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name
    {
        get { return this.name; }
        set
        {
            if (this.name == value)
            {
                return;
            }

            this.name = value;
            this.OnPropertyChanged("Name");
            UpdateFullName();
        }
    }

    /// <summary>
    /// Updates the full name.
    /// </summary>
    private void UpdateFullName()
    {
        logger.Debug("Entered member.");

        FullName = string.Format("{0} - {1}", this.NameAndWritable,
                                 this.TypeName);
    }

    /// <summary>
    /// Gets the name and writable of the type and whether it is writable.
    /// </summary>
    /// <value>
    /// The name and writable.
    /// </value>
    public string NameAndWritable
    {
        get { return this.CanWrite ? this.Name : String.Format("{0}  (r)", this.Name); }
    }

    /// <summary>
    /// Gets the name of the name space type.
    /// </summary>
    /// <value>
    /// The name of the name space type.
    /// </value>
    public string NameSpaceTypeName
    {
        get { return string.Concat(this.TypeNamespace, ":", this.TypeName); }
    }

    /// <summary>
    /// Gets the name in Pascal style.
    /// </summary>
    /// <value>
    /// The name in Pascal case.
    /// </value>
    public string PascalFieldName
    {
        get { return string.Concat(this.Name[0].ToLower(), this.Name.Substring(1)); }
    }

    /// <summary>
    /// Gets or sets options for controlling the property.
    /// </summary>
    /// <value>
    /// Options that control the property.
    /// </value>
    public List<PropertyParameter> PropertyParameters
    {
        get { return this.propertyParameters; }
        set
        {
            this.propertyParameters = value;
            this.OnPropertyChanged("PropertyParameters");
        }
    }

    /// <summary>
    /// Gets or sets the string format.
    /// </summary>
    /// <value>
    /// The string format.
    /// </value>
    public string StringFormat
    {
        get { return this.stringFormat; }
        set
        {
            this.stringFormat = value;
            this.OnPropertyChanged("StringFormat");
        }
    }

    /// <summary>
    /// Gets or sets the name of the type.
    /// </summary>
    /// <value>
    /// The name of the type.
    /// </value>
    public string TypeName
    {
        get { return this.typeName; }
        set
        {
            if (this.typeName == value)
            {
                return;
            }

            this.typeName = value;
            this.OnPropertyChanged("TypeName");
            UpdateFullName();
        }
    }

    /// <summary>
    /// Gets or sets the type namespace.
    /// </summary>
    /// <value>
    /// The type namespace.
    /// </value>
    public string TypeNamespace
    {
        get { return this.typeNamespace; }
        set
        {
            this.typeNamespace = value;
            this.OnPropertyChanged("TypeNamespace");
        }
    }

    #endregion

    #region INotifyPropertyChanged Members
    /// <summary>
    /// Event inherited from the INotifyPropertyChanged interface.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region Methods (public)
    /// <summary>
    /// Property parameter string.
    /// </summary>
    /// <param name="Language">
    /// The language.
    /// </param>
    /// <returns>
    /// A string.
    /// </returns>
    public string PropertyParameterString(LanguageTypes Language)
    {
        logger.Debug("Entered member.");

        if (this.PropertyParameters.Count == 0 ||
                Language == LanguageTypes.Unknown)
        {
            return string.Empty;
        }
        var sb = new StringBuilder(512);
        string LanguageTypeFormat = Language == LanguageTypes.VisualBasic ?
                                    "{3}{0} As {1}, " : "{3}{1} {0}, ";
        foreach (var obj in this.PropertyParameters)
        {
            sb.AppendFormat(LanguageTypeFormat, obj.ParameterName,
                            obj.ParameterTypeName);
        }
        sb.Length = sb.Length - 2;
        return sb.ToString();
    }

    /// <summary>
    /// VB private field name.
    /// </summary>
    /// <param name="UseHungarian">
    /// true to use Hungarian notation.
    /// </param>
    /// <returns>
    /// A string.
    /// </returns>
    public string VBPrivateFieldName(bool UseHungarian)
    {
        logger.Debug("Entered member.");

        return string.Concat(UseHungarian ? this.GetHungarian() : "_",
                             this.PascalFieldName);
    }

    /// <summary>
    /// VB type name.
    /// </summary>
    /// <returns>
    /// A string.
    /// </returns>
    public string VBTypeName()
    {
        logger.Debug("Entered member.");

        return this.TypeName.EndsWith("]") ? this.TypeName.Replace("[",
                "(").Replace("]", ")") : this.TypeName;
    }

    #endregion

    #region Methods (private)
    /// <summary>
    /// Gets the Hungarian name.
    /// </summary>
    /// <returns>
    /// The Hungarian name.
    /// </returns>
    private string GetHungarian()
    {
        logger.Debug("Entered member.");

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
    /// Executes the property changed action.
    /// </summary>
    /// <param name="PropertyName">
    /// Name of the property.
    /// </param>
    private void OnPropertyChanged(string PropertyName)
    {
        logger.Debug("Entered member.");

        var h = this.PropertyChanged;
        if (h == null)
        {
            return;
        }
        h(this, new PropertyChangedEventArgs(PropertyName));
    }

    #endregion
}
}
