using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace XamlHelpmeet.UI.UIControlFactory
{
	[Serializable]
	public class UIProperty : INotifyPropertyChanged, ISerializable
	{

		public UIProperty()
		{
		}

		public UIProperty(string PropertyName, string PropertyValue)
		{
			this.PropertyName = PropertyName;
			this.PropertyValue = PropertyValue;
		}

		private string _propertyValue;
		private string _propertyName;
		public string PropertyName
		{
			get
			{
				return _propertyName;
			}
			set
			{
				_propertyName = value;
				OnPropertyChanged("PropertyName");
			}
		}

		public string PropertyValue
		{
			get
			{
				return _propertyValue;
			}
			set
			{
				_propertyValue = value;
				OnPropertyChanged("PropertyValue");
			}
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string PropertyName)
		{
			var h = PropertyChanged;
			if (h == null)
			{
				return;
			}
			h(this, new PropertyChangedEventArgs(PropertyName));
		}

		#endregion INotifyPropertyChanged Members

		#region "ISerializable Interface Implementation"

		/// <summary>
		///     Method inherited from the ISerializable interface
		/// </summary>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("PropertyValue", PropertyValue);
			info.AddValue("PropertyName", PropertyName);
		}

		protected UIProperty(SerializationInfo info, StreamingContext context)
		{
			PropertyName = (string)info.GetValue("PropertyName", typeof(string));
			PropertyValue = (string)info.GetValue("PropertyValue", typeof(string));
		}

		#endregion "ISerializable Interface Implementation"
	}
}