using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using XamlHelpmeet.UI.Utilities;
using XamlHelpmeet.Extensions;

namespace XamlHelpmeet.UI.UIControlFactory
{
	[Serializable]
	public class UIControl : INotifyPropertyChanged, ISerializable
    {
        #region Fields

        private SerializableObservableCollection<UIProperty> _controlProperties;
        private string _controlType;

        private bool _generateControlName;

        private bool _includeNotifyOnValidationError;

        private bool _includeTargetNullValueForNullableBindings;

        private bool _includeValidatesOnDataErrors;

        private bool _includeValidatesOnExceptions;
        #endregion
        #region Constructors

        /// <summary>
		/// 	Initializes a new instance of the UIControl class.
		/// </summary>
		public UIControl()
		{
			ControlRole = UIControlRole.Label;
            Platform = UIPlatform.WPF;
            _controlProperties = new SerializableObservableCollection<UIProperty>();
            ControlType = string.Empty;
		}

		/// <summary>
		/// 	Initializes a new instance of the UIControl class.
		/// </summary>
		/// <param name="uiPlatform">
		/// 	The platform.
		/// </param>
		/// <param name="UIControlRole">
		/// 	The control role.
		/// </param>
		/// <param name="ControlType">
		/// 	Type of the control.
		/// </param>
		public UIControl(UIPlatform uiPlatform, UIControlRole UIControlRole, string ControlType)
			: this()
		{
			this.Platform = uiPlatform;
			this.ControlRole = UIControlRole;
			this.ControlType = ControlType;
		}

		#endregion

		#region Methods

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

        private string GetControlHungarian()
		{
			switch (ControlRole)
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
		#endregion

		#region Properties

        public string BindingPropertyString
		{
			get;
			set;
		}

        public SerializableObservableCollection<UIProperty> ControlProperties
        {
            get
            {
                return _controlProperties;
            }
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

        public string ControlRoleName
        {
            get
            {
                return ControlRole.ToString();
            }
        }
		public UIControlRole ControlRole
		{
			get;
			set;
		}
		public UIPlatform Platform
		{
			get;
			set;
		}
		#endregion

		#region ISerializable Members

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
            ControlRole = (UIControlRole)info.GetValue("UIControlRole", typeof(UIControlRole));
            Platform = (UIPlatform)info.GetValue("Platform", typeof(UIPlatform));
            ControlType = (string)info.GetValue("ControlType", typeof(string));
            _controlProperties = (SerializableObservableCollection<UIProperty>)info.GetValue("ControlProperties", typeof(SerializableObservableCollection<UIProperty>));
        }

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
			info.AddValue("UIControlRole", ControlRole, typeof(UIControlRole));
			info.AddValue("Platform", Platform, typeof(UIPlatform));
			info.AddValue("ControlType", ControlType, typeof(string));
            info.AddValue("ControlProperties", ControlProperties, typeof(SerializableObservableCollection<UIProperty>));
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
