using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using XamlHelpmeet.UI.Enums;

namespace XamlHelpmeet.UI.DynamicForm
{
	public class DynamicFormListBoxContent : INotifyPropertyChanged
	{
		/************************************ FIELDS **********************************/
		private int _width;

		private string _typeNamespace;

		private string _stringFormat;

		private bool _renderAsGridTemplateColumn;

		private int _maximumLength;

		private string _dataType;

		private DynamicFormControlType _controlType;

		private string _controlLabel;

		private bool _canWrite;

		private string _bindingPath;

		private BindingMode _bindingMode;

		private string _associatedLabel;

		private string _labelPosition;

		private string _descriptionViewerPosition;

		private string _fieldDescription;

		#region Properties

		/******************************************************************************/
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

		public string AssociatedLabel
		{
			get
			{
				return _associatedLabel;
			}
			set
			{
				_associatedLabel = value;
				OnPropertyChanged("AssociatedLabel");
			}
		}

		public BindingMode BindingMode
		{
			get
			{
				return _bindingMode;
			}
			set
			{
				_bindingMode = value;
				OnPropertyChanged("BindingMode");
			}
		}

		public string BindingPath
		{
			get
			{
				return _bindingPath;
			}
			set
			{
				_bindingPath = value;
				OnPropertyChanged("BindingPath");
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

		public string ControlLabel
		{
			get
			{
				return _controlLabel;
			}
			set
			{
				_controlLabel = value;
				OnPropertyChanged("ControlLabel");
			}
		}

		public DynamicFormControlType ControlType
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

		public string DataType
		{
			get
			{
				return _dataType;
			}
			set
			{
				_dataType = value;
				OnPropertyChanged("DataType");
			}
		}

		public string FullName
		{
			get
			{
				// BMK Inser logic here
				return String.Format("{0}{1} - {2}",
				                     BindingPath,
				                     CanWrite ? string.Empty : "(r)",
				                     NameSpaceTypeName);
			}
		}

		public int MaximumLength
		{
			get
			{
				return _maximumLength;
			}
			set
			{
				_maximumLength = value;
				OnPropertyChanged("MaximumLength");
			}
		}

		public string NameAndWriteable
		{
			get
			{
				if (CanWrite)
				{
					return BindingPath;
				}
				return String.Format("{0} (r)",
				                     BindingPath);
			}
		}

		public string NameSpaceTypeName
		{
			get
			{
				return string.Format("{0}:{1}",
				                     TypeNamespace,
				                     DataType);
			}
		}

		public bool RenderAsGridTemplateColumn
		{
			get
			{
				return _renderAsGridTemplateColumn;
			}
			set
			{
				_renderAsGridTemplateColumn = value;
				OnPropertyChanged("RenderAsGridTemplateColumn");
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

		public int Width
		{
			get
			{
				return _width;
			}
			set
			{
				_width = value;
				OnPropertyChanged("Width");
			}
		}

		#endregion

		#region Methods

		private void OnPropertyChanged(string PropertyName)
		{
			var h = PropertyChanged;
			if (h == null)
			{
				return;
			}
			h(this,
			  new PropertyChangedEventArgs(PropertyName));
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

	}
}
