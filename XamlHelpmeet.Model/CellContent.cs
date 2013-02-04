// file:	CellContent.cs
//
// summary:	Implements the cell content class
using System;
using System.ComponentModel;
using System.Windows.Data;

namespace XamlHelpmeet.Model
{
    /// <summary>
    ///     Cell content.
    /// </summary>
    /// <seealso cref="T:System.ComponentModel.INotifyPropertyChanged"/>
	[Serializable]
	public class CellContent : INotifyPropertyChanged
	{
		#region Fields

		private int? _width;
		private int? _maximumLength;
        private string _controlLabel = string.Empty;
		private readonly int _column;
		private readonly int _row;
		private BindingMode _bindingMode;
		private string _bindingPath = string.Empty;
		private ControlType _controlType;
		private string _dataType = string.Empty;
		private string _stringFormat = string.Empty;

		#endregion Fields

		#region Constructors

        /// <summary>
        ///     Default constructor is required to use as a new data context in XAML.
        /// </summary>
        public CellContent()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the CellContent class.
        /// </summary>
        /// <param name="Row">
        ///     The row.
        /// </param>
        /// <param name="Column">
        ///     The column.
        /// </param>
		public CellContent(int Row, int Column)
		{
			_row = Row;
			_column = Column;
		}

        /// <summary>
        ///     Initializes a new instance of the CellContent class.
        /// </summary>
        /// <param name="DataType">
        ///     Type of the data.
        /// </param>
        /// <param name="Row">
        ///     The row.
        /// </param>
        /// <param name="Column">
        ///     The column.
        /// </param>
		public CellContent(string DataType, int Row, int Column)
			: this(Row, Column)
		{
			_dataType = DataType;
		}

		#endregion Constructors

		#region Properties

        /// <summary>
        ///     Gets or sets the binding mode.
        /// </summary>
        /// <value>
        ///     The binding mode.
        /// </value>
		public BindingMode BindingMode
		{
			get { return _bindingMode; }
			set
			{
				_bindingMode = value;
				OnPropertyChanged("BindingMode");
			}
		}

        /// <summary>
        ///     Gets or sets the binding path.
        /// </summary>
        /// <value>
        ///     The binding path.
        /// </value>
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

        /// <summary>
        ///     Gets the column index.
        /// </summary>
        /// <value>
        ///     The column index.
        /// </value>
		public int Column
		{
			get
			{
				return _column;
			}
		}

        /// <summary>
        ///     Gets or sets the control label.
        /// </summary>
        /// <value>
        ///     The control label.
        /// </value>
		public string ControlLabel
		{
			get { return _controlLabel; }
			set
			{
				_controlLabel = value;
				OnPropertyChanged("ControlLabel");
			}
		}

        /// <summary>
        ///     Gets or sets the type of the control.
        /// </summary>
        /// <value>
        ///     The type of the control.
        /// </value>
		public ControlType ControlType
		{
			get { return _controlType; }
			set
			{
				_controlType = value;
				OnPropertyChanged("ControlType");
			}
		}

        /// <summary>
        ///     Gets or sets the type of the data.
        /// </summary>
        /// <value>
        ///     The type of the data.
        /// </value>
		public string DataType
		{
			get { return _dataType; }
			set
			{
				_dataType = value;
				OnPropertyChanged("DataType");
			}
		}

        /// <summary>
        ///     Gets or sets the maximum length
        /// </summary>
        /// <value>
        ///     The maximum length.
        /// </value>
		public int? MaximumLength
		{
			get { return _maximumLength; }
			set
			{
				_maximumLength = value;
				OnPropertyChanged("MaximumLength");
			}
		}

        /// <summary>
        ///     Gets the row index.
        /// </summary>
        /// <value>
        ///     The row.
        /// </value>
		public int Row
		{
			get
			{
				return _row;
			}
		}

        /// <summary>
        ///     Gets or sets the string format.
        /// </summary>
        /// <value>
        ///     The string format.
        /// </value>
		public string StringFormat
		{
			get { return _stringFormat; }
			set
			{
				_stringFormat = value;
				OnPropertyChanged("StringFormat");
			}
		}

        /// <summary>
        ///     Gets or sets the width.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
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

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
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