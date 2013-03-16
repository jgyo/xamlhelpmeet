// file:	ExtractProperty.cs
//
// summary:	Implements the extract property class
using System;
using System.ComponentModel;
using System.Xml;

namespace XamlHelpmeet.Model
{
    /// <summary>
    ///     Extract property.
    /// </summary>
    /// <seealso cref="T:System.ComponentModel.INotifyPropertyChanged"/>
    [Serializable]
    public class ExtractProperty : INotifyPropertyChanged
    {
        #region Fields

        private bool _isSelected;
        private string _propertyName;
        private string _propertyValue;
		[NonSerialized]
        private XmlAttribute _xmlAttribute;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the ExtractProperty class.
        /// </summary>
        /// <param name="XmlAttribute">
        ///     The XML attribute.
        /// </param>
        public ExtractProperty(XmlAttribute XmlAttribute)
        {
            this.XmlAttribute = XmlAttribute;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this ExtractProperty is selected.
        /// </summary>
        /// <value>
        ///     true if this ExtractProperty is selected, otherwise false.
        /// </value>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected == value)
                    return;
                _isSelected = value;
                OnPropertyChanged("IsSelected");
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
                return _propertyName;
            }
            private set
            {
                if (_propertyName == value)
                    return;
                _propertyName = value;
                OnPropertyChanged("PropertyName");
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
                return _propertyValue;
            }
            private set
            {
                if (_propertyValue == value)
                    return;
                _propertyValue = value;
                OnPropertyChanged("PropertyValue");
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
                return _xmlAttribute;
            }
            private set
            {
                if (_xmlAttribute == value)
                    return;
                _xmlAttribute = value;
                OnPropertyChanged("XmlAttribute");
                PropertyName = value.LocalName;
                PropertyValue = value.Value;
            }
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h == null)
                return;
            h(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}