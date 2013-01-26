using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace XamlHelpmeet.Model
{
	/// <summary>
	/// 	Class entity.
	/// </summary>
	/// <seealso cref="T:System.ComponentModel.INotifyPropertyChanged"/>

	[Serializable]
	public class ClassEntity : INotifyPropertyChanged
	{
		private ObservableCollection<PropertyInformation> _propertyInformation;
		private string _className = string.Empty;
		private Exception _exception;
		private bool _isSilverlight;
		private string _silverlightVersion = string.Empty;
		private bool _success;

		/// <summary>
		/// 	Initializes a new instance of the ClassEntity class.
		/// </summary>

		public ClassEntity()
		{
			
		}

		/// <summary>
		/// 	Initializes a new instance of the ClassEntity class.
		/// </summary>
		/// <param name="ClassName">
		/// 	Name of the class.
		/// </param>
		/// <param name="IsSilverlight">
		/// 	true if this ClassEntity is silverlight.
		/// </param>

		public ClassEntity(string ClassName, bool IsSilverlight)
		{
			_className = ClassName;
			_isSilverlight = IsSilverlight;
		}

		/// <summary>
		/// 	Gets or sets the name of the class.
		/// </summary>
		/// <value>
		/// 	The name of the class.
		/// </value>

		public string ClassName
		{
			get
			{
				return _className;
			}
			set
			{
				_className = value;
				OnPropertyChanged("ClassName");
			}
		}

		/// <summary>
		/// 	Gets or sets the exception.
		/// </summary>
		/// <value>
		/// 	The exception.
		/// </value>

		public Exception Exception
		{
			get
			{
				return _exception;
			}
			set
			{
				_exception = value;
				OnPropertyChanged("Exception");
			}
		}

		/// <summary>
		/// 	Gets or sets a value indicating whether this ClassEntity is silverlight.
		/// </summary>
		/// <value>
		/// 	true if this ClassEntity is silverlight, false if not.
		/// </value>

		public bool IsSilverlight
		{
			get
			{
				return _isSilverlight;
			}
			set
			{
				_isSilverlight = value;
				OnPropertyChanged("IsSilverlight");
			}
		}

		/// <summary>
		/// Gets or sets the information describing the property.
		/// </summary>
		/// <value>
		/// Information describing the property.
		/// </value>
		public ObservableCollection<PropertyInformation> PropertyInformation
		{
			get
			{
				if (_propertyInformation == null)
					_propertyInformation = new ObservableCollection<PropertyInformation>();
				return _propertyInformation;
			}
		}
	
		/// <summary>
		/// 	Gets or sets the silverlight version.
		/// </summary>
		/// <value>
		/// 	The silverlight version.
		/// </value>

		public dynamic SilverlightVersion
		{
			get
			{
				return _silverlightVersion;
			}
			set
			{
				_silverlightVersion = value;
				OnPropertyChanged("SilverlightVersion");
			}
		}

		/// <summary>
		/// 	Gets or sets a value indicating success.
		/// </summary>
		/// <value>
		/// 	true if success is indicate, false if not.
		/// </value>

		public bool Success
		{
			get
			{
				return _success;
			}
			set
			{
				_success = value;
				OnPropertyChanged("Success");
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
		///     Event inherited from the INotifyPropertyChanged interface
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion "INotifyPropertyChanged Interface Implementation"
	}
}