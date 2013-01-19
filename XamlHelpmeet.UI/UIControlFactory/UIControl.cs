using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Extentions;

namespace XamlHelpmeet.UI.UIControlFactory
{
	[Serializable]
	public class UIControl : INotifyPropertyChanged, ISerializable
	{
		#region Constructors

		/// <summary>
		/// 	Initializes a new instance of the UIControl class.
		/// </summary>
		public UIControl()
		{
			UIControlRole = UIControlRole.Label;
			UIPlatform = UIPlatform.WPF;
			ControlProperties = new ObservableCollection<UIProperty>();
			ControlType = string.Empty;
		}

		/// <summary>
		/// 	Initializes a new instance of the UIControl class.
		/// </summary>
		/// <param name="UIPlatform">
		/// 	The platform.
		/// </param>
		/// <param name="UIControlRole">
		/// 	The control role.
		/// </param>
		/// <param name="ControlType">
		/// 	Type of the control.
		/// </param>
		public UIControl(UIPlatform UIPlatform, UIControlRole UIControlRole, string ControlType)
			: this()
		{
			this.UIPlatform = UIPlatform;
			this.UIControlRole = UIControlRole;
			this.ControlType = ControlType;
		}

		#endregion

		#region Methods

		private string GetControlHungarian()
		{
			switch (UIControlRole)
			{
				case UIControlRole.Border:
					return "bdr";
				case UIControlRole.CheckBox:
					return "chk";
				case UIControlRole.ComboBox:
					return "cbo";
				case UIControlRole.Grid:
					return "grid";
				case UIControlRole.Image:
					return "img";
				case UIControlRole.Label:
					return "lbl";
				case UIControlRole.TextBlock:
					return "tb";
				case UIControlRole.TextBox:
					return "txt";
				case UIControlRole.DataGrid:
					return "dg";
				case UIControlRole.DatePicker:
					return "dp";
				default:
					return "NOTASSIGNED";
			}
		}

		public string MakeControlFromDefaults(string MainTags, bool AddClosingTag, string Path)
		{
			var sb = new StringBuilder(1024);

			sb.AppendFormat("<{0}", ControlType);

			if (GenerateControlName && Path.IsNotNullOrEmpty())
			{
				sb.AppendFormat(" x:Name=\"{0}{1}\"", GetControlHungarian(), Path);
			}

			sb.Append(MainTags);
			sb.Append(AddClosingTag ? " />" : ">");

			return sb.ToString().Replace("  ", " ");
		}

		#endregion

		#region Properties

		public string BindingPropertyString
		{
			get;
			set;
		}

		public ObservableCollection<UIProperty> ControlProperties
		{
			get;
			private set;
		}
		public string ControlType
		{
			get
			{
				return _controlType;
			}
			set
			{
				_controlType = value;
				OnPropertyChanged("ControlType");
			}
		}

		private bool _includeValidatesOnExceptions;
		private bool _includeValidatesOnDataErrors;
		private bool _includeTargetNullValueForNullableBindings;
		private bool _includeNotifyOnValidationError;
		private string _controlType;
		private bool _generateControlName;
		public bool GenerateControlName
		{
			get
			{
				return _generateControlName;
			}
			set
			{
				_generateControlName = value;
				OnPropertyChanged("GenerateControlName");
			}
		}
		public bool IncludeNotifyOnValidationError
		{
			get
			{
				return _includeNotifyOnValidationError;
			}
			set
			{
				_includeNotifyOnValidationError = value;
				OnPropertyChanged("IncludeNotifyOnValidationError");
			}
		}

		public bool IncludeTargetNullValueForNullableBindings
		{
			get
			{
				return _includeTargetNullValueForNullableBindings;
			}
			set
			{
				_includeTargetNullValueForNullableBindings = value;
				OnPropertyChanged("IncludeTargetNullValueForNullableBindings");
			}
		}
		public bool IncludeValidatesOnDataErrors
		{
			get
			{
				return _includeValidatesOnDataErrors;
			}
			set
			{
				_includeValidatesOnDataErrors = value;
				OnPropertyChanged("IncludeValidatesOnDataErrors");
			}
		}
		public bool IncludeValidatesOnExceptions
		{
			get
			{
				return _includeValidatesOnExceptions;
			}
			set
			{
				_includeValidatesOnExceptions = value;
				OnPropertyChanged("IncludeValidatesOnExceptions");
			}
		}
		public UIControlRole UIControlRole
		{
			get;
			set;
		}
		public UIPlatform UIPlatform
		{
			get;
			set;
		}
		#endregion

		#region ISerializable Members

		/// <summary>
		/// 	Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with
		/// 	the data needed to serialize the target object.
		/// </summary>
		/// <seealso cref="M:System.Runtime.Serialization.ISerializable.GetObjectData(SerializationInfo,StreamingContext)"/>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("GenerateControlName", GenerateControlName);
			info.AddValue("IncludeNotifyOnValidationError", IncludeNotifyOnValidationError);
			info.AddValue("IncludeTargetNullValueForNullableBindings", IncludeTargetNullValueForNullableBindings);
			info.AddValue("IncludeValidatesOnDataErrors", IncludeValidatesOnDataErrors);
			info.AddValue("IncludeValidatesOnExceptions", IncludeValidatesOnExceptions);
			info.AddValue("UIControlRole", UIControlRole, typeof(UIControlRole));
			info.AddValue("UIPlatform", UIPlatform, typeof(UIPlatform));
			info.AddValue("ControlType", ControlType, typeof(string));

			var surrogate = new ObservableCollectionSerializationSurrogate<UIProperty>();
			surrogate.GetObjectData(ControlProperties, info, context);
		}

		/// <summary>
		/// 	A specialized constructor for the UIControl class for serialization.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// 	Thrown when one or more required arguments are null.
		/// </exception>
		protected UIControl(SerializationInfo info, StreamingContext context)
			: this()
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			GenerateControlName = (bool)info.GetValue("GenerateControlName", typeof(bool));

			IncludeNotifyOnValidationError = (bool)info.GetValue("IncludeNotifyOnValidationError", typeof(bool));
			IncludeTargetNullValueForNullableBindings = (bool)info.GetValue("IncludeTargetNullValueForNullableBindings", typeof(bool));
			IncludeValidatesOnDataErrors = (bool)info.GetValue("IncludeValidatesOnDataErrors", typeof(bool));
			IncludeValidatesOnExceptions = (bool)info.GetValue("IncludeValidatesOnExceptions", typeof(bool));
			UIControlRole = (UIControlRole)info.GetValue("UIControlRole", typeof(UIControlRole));
			UIPlatform = (UIPlatform)info.GetValue("UIPlatform", typeof(UIPlatform));
			ControlType = (string)info.GetValue("ControlType", typeof(string));

			var surrogate = new ObservableCollectionSerializationSurrogate<UIProperty>();
			ControlProperties = (ObservableCollection<UIProperty>)surrogate.SetObjectData(null, info, context, null);
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string PropertyName)
		{
			var h = PropertyChanged;
			if (h == null)
				return;

			h(this, new PropertyChangedEventArgs(PropertyName));
		}

		#endregion
	}
}
