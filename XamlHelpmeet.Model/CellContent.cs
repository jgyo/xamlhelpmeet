using System;
using System.ComponentModel;
using System.Windows.Data;

namespace XamlHelpmeet.Model
{
	[Serializable]
	public class CellContent : INotifyPropertyChanged
	{
		#region Fields

		private int? _width;
		private int? _maximumLength;
		private string _controlLabel;
		private readonly int _column;
		private readonly int _row;
		private BindingMode _bindingMode;
		private string _bindingPath;
		private ControlType _controlType;
		private string _dataType;
		private string _stringFormat;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// 	Initializes a new instance of the CellContent class.
		/// </summary>
		/// <param name="Row">
		/// 	The row.
		/// </param>
		/// <param name="Column">
		/// 	The column.
		/// </param>

		public CellContent(int Row, int Column)
		{
			_row = Row;
			_column = Column;
		}

		/// <summary>
		/// 	Initializes a new instance of the CellContent class.
		/// </summary>
		/// <param name="DataType">
		/// 	Type of the data.
		/// </param>
		/// <param name="Row">
		/// 	The row.
		/// </param>
		/// <param name="Column">
		/// 	The column.
		/// </param>

		public CellContent(string DataType, int Row, int Column)
			: this(Row, Column)
		{
			_dataType = DataType;
		}

		#endregion Constructors

		#region Properties

		public BindingMode BindingMode
		{
			get { return _bindingMode; }
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

		public int Column
		{
			get
			{
				return _column;
			}
		}


		public string ControlLabel
		{
			get { return _controlLabel; }
			set
			{
				_controlLabel = value;
				OnPropertyChanged("ControlLabel");
			}
		}

		public ControlType ControlType
		{
			get { return _controlType; }
			set
			{
				_controlType = value;
				OnPropertyChanged("ControlType");
			}
		}

		public string DataType
		{
			get { return _dataType; }
			set
			{
				_dataType = value;
				OnPropertyChanged("DataType");
			}
		}


		public int? MaximumLength
		{
			get { return _maximumLength; }
			set
			{
				_maximumLength = value;
				OnPropertyChanged("MaximumLength");
			}
		}

		public int Row
		{
			get
			{
				return _row;
			}
		}

		public string StringFormat
		{
			get { return _stringFormat; }
			set
			{
				_stringFormat = value;
				OnPropertyChanged("StringFormat");
			}
		}


		public int? Width
		{
			get { return _width; }
			set
			{
				_width = value;
				OnPropertyChanged("Width");
			}
		}

		#endregion Properties

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string PropertyName)
		{
			var h = PropertyChanged;
			if (h == null)
			{
				return;
			}
			h(this, new PropertyChangedEventArgs(PropertyName));
		}

		#endregion INotifyPropertyChanged Members
	}
}