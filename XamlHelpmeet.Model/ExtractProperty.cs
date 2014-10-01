// ***********************************************************************
// Assembly         : XamlHelpmeet.Model
// Solution         : XamlHelpmeet
// File name        : ExtractProperty.cs
// Author           : Gil Yoder
// Created          : 09 19,  2014
//
// Last Modified By : Gil Yoder
// Last Modified On : 09 19, 2014
// ***********************************************************************

namespace XamlHelpmeet.Model
{
#region Imports

using System;
using System.ComponentModel;
using System.Xml;

using NLog;

using YoderZone.Extensions.NLog;

#endregion

/// <summary>
///     Extract property.
/// </summary>
/// <seealso cref="T:System.ComponentModel.INotifyPropertyChanged" />
[Serializable]
public class ExtractProperty : INotifyPropertyChanged
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    #region Fields

    /// <summary>
    ///     true if this XamlHelpmeet.Model.ExtractProperty is selected.
    /// </summary>
    private bool isSelected;

    /// <summary>
    ///     Name of the property.
    /// </summary>
    private string propertyName;

    /// <summary>
    ///     The property value.
    /// </summary>
    private string propertyValue;

    /// <summary>
    ///     The XML attribute.
    /// </summary>
    [NonSerialized]
    private XmlAttribute xmlAttribute;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    ///     Initializes a new instance of the ExtractProperty class.
    /// </summary>
    /// <param name="XmlAttribute">
    ///     The XML attribute.
    /// </param>
    public ExtractProperty(XmlAttribute XmlAttribute)
    {
        logger.Debug("Entered member.");

        this.XmlAttribute = XmlAttribute;
    }

    #endregion

    #region Public Events

    /// <summary>
    ///     Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region Public Properties

    /// <summary>
    ///     Gets or sets a value indicating whether this ExtractProperty is
    ///     selected.
    /// </summary>
    /// <value>
    ///     true if this ExtractProperty is selected, otherwise false.
    /// </value>
    public bool IsSelected
    {
        get
        {
            return this.isSelected;
        }
        set
        {
            if (this.isSelected == value)
            {
                return;
            }
            this.isSelected = value;
            this.OnPropertyChanged("IsSelected");
        }
    }

    /// <summary>
    ///     Gets the name of the property.
    /// </summary>
    /// <value>
    ///     The name of the property.
    /// </value>
    public string PropertyName
    {
        get
        {
            return this.propertyName;
        }
        private set
        {
            if (this.propertyName == value)
            {
                return;
            }
            this.propertyName = value;
            this.OnPropertyChanged("PropertyName");
        }
    }

    /// <summary>
    ///     Gets the property value.
    /// </summary>
    /// <value>
    ///     The property value.
    /// </value>
    public string PropertyValue
    {
        get
        {
            return this.propertyValue;
        }
        private set
        {
            if (this.propertyValue == value)
            {
                return;
            }
            this.propertyValue = value;
            this.OnPropertyChanged("PropertyValue");
        }
    }

    /// <summary>
    ///     Gets the XML attribute.
    /// </summary>
    /// <value>
    ///     The XML attribute.
    /// </value>
    public XmlAttribute XmlAttribute
    {
        get
        {
            return this.xmlAttribute;
        }
        private set
        {
            if (this.xmlAttribute == value)
            {
                return;
            }
            this.xmlAttribute = value;
            this.OnPropertyChanged("XmlAttribute");
            this.PropertyName = value.LocalName;
            this.PropertyValue = value.Value;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Executes the property changed action.
    /// </summary>
    /// <param name="name">
    ///     The name.
    /// </param>
    private void OnPropertyChanged(string name)
    {
        logger.Debug("Entered member.");

        PropertyChangedEventHandler h = this.PropertyChanged;
        if (h == null)
        {
            return;
        }
        h(this, new PropertyChangedEventArgs(name));
    }

    #endregion
}
}