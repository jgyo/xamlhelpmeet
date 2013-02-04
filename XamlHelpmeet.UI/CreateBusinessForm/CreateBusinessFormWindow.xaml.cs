// file:	CreateBusinessForm\CreateBusinessFormWindow.xaml.cs
//
// summary:	Implements the create business form window.xaml class
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using XamlHelpmeet.Extentions;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.Editors;
using XamlHelpmeet.UI.UIControlFactory;

namespace XamlHelpmeet.UI.CreateBusinessForm
{
    /// <summary>
    ///     Interaction logic for CreateBusinessFormWindow.xaml.
    /// </summary>
    /// <seealso cref="T:System.Windows.Window"/>
    public partial class CreateBusinessFormWindow : Window, INotifyPropertyChanged
    {
        #region Constants

        private const char ROW_COLUMN_KEY_SEPARATOR = ':';

        #endregion

        #region Fields

        private readonly List<ComboBox> _columnHeaderComboBoxCollection;
        private readonly List<TextBlock> _columnHeaderTextBlockCollection;
        private readonly List<GridLength> _columnWidthsCollection;
        private readonly Dictionary<String, CellContent> _gridCellCollection;
        private readonly List<TextBlock> _rowHeaderTextBlockCollection;
        private readonly List<GridLength> _rowHeightsCollection;
        private string _allColumnSize;
        private string _allRowSize;
        private GridLength _columnDefaultSize;
        private string _definedColumns = string.Empty;
        private string _definedRows = string.Empty;
        private int _numberOfColumns;
        private int _numberOfRows;
        private GridLength _rowDefaultSize;

        #endregion

        #region Constructors

        ///// <summary>
        /////     Initializes a new instance of the CreateBusinessFormWindow class.
        ///// </summary>
        //public CreateBusinessFormWindow()
        //{
        //}

        /// <summary>
        ///     Initializes a new instance of the CreateBusinessFormWindow class.
        /// </summary>
        /// <param name="ClassEntity">
        ///     The class entity.
        /// </param>
        public CreateBusinessFormWindow(ClassEntity ClassEntity)
        {
            // Here's the call that the form designer forces us to call.
            InitializeComponent();

            ColumnDefaultSize = new GridLength(0, GridUnitType.Auto);
            RowDefaultSize = new GridLength(0, GridUnitType.Auto);

            _columnHeaderComboBoxCollection = new List<ComboBox>();
            _columnHeaderTextBlockCollection = new List<TextBlock>();
            _columnWidthsCollection = new List<GridLength>();
            _gridCellCollection = new Dictionary<string, CellContent>();
            _rowHeaderTextBlockCollection = new List<TextBlock>();
            _rowHeightsCollection = new List<GridLength>();

            BusinessForm = string.Empty;
            CreateBusinessFormWindow.ClassEntity = ClassEntity;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the class entity.
        /// </summary>
        /// <value>
        ///     The class entity.
        /// </value>
        public static ClassEntity ClassEntity
        {
            get;
            private set;
        }

        public string AllColumnSize
        {
            get { return _allColumnSize; }
            set
            {
                if (_allColumnSize == value)
                    return;
                _allColumnSize = value;
                OnPropertyChanged("AllColumnSize");
            }
        }

        public string AllRowSize
        {
            get { return _allRowSize; }
            set
            {
                if (_allRowSize == value)
                    return;
                _allRowSize = value;
                OnPropertyChanged("AllRowSize");
            }
        }

        /// <summary>
        ///     Gets the business form.
        /// </summary>
        /// <value>
        ///     The business form.
        /// </value>
        public string BusinessForm
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the size of the column default.
        /// </summary>
        /// <value>
        /// The size of the column default.
        /// </value>
        public GridLength ColumnDefaultSize
        {
            get
            {
                return _columnDefaultSize;
            }
            set
            {
                if (_columnDefaultSize == value)
                    return;
                _columnDefaultSize = value;
                OnPropertyChanged("ColumnDefaultSize");
                if (value.IsAuto)
                    AllColumnSize = "Auto";
                else if (value.IsStar)
                    AllColumnSize = "*";
                else
                    AllColumnSize = value.Value.ToString();
            }
        }

        /// <summary>
        ///     Gets a collection of column header combo boxes.
        /// </summary>
        /// <value>
        ///     A Collection of column header combo boxes.
        /// </value>
        public List<ComboBox> ColumnHeaderComboBoxCollection
        {
            get
            {
                return _columnHeaderComboBoxCollection;
            }
        }

        /// <summary>
        ///     Gets a collection of column header text blocks.
        /// </summary>
        /// <value>
        ///     A Collection of column header text blocks.
        /// </value>
        public List<TextBlock> ColumnHeaderTextBlockCollection
        {
            get
            {
                return _columnHeaderTextBlockCollection;
            }
        }

        /// <summary>
        ///     Gets or sets the column size POP up.
        /// </summary>
        /// <value>
        ///     The column size POP up.
        /// </value>
        public Popup ColumnSizePopUp
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets the column size popup timer.
        /// </summary>
        /// <value>
        ///     The column size popup timer.
        /// </value>
        public DispatcherTimer ColumnSizePopupTimer
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets a collection of column widths.
        /// </summary>
        /// <value>
        ///     A Collection of column widths.
        /// </value>
        public List<GridLength> ColumnWidthsCollection
        {
            get
            {
                return _columnWidthsCollection;
            }
        }

        public string DefinedColumns
        {
            get { return _definedColumns; }
            set
            {
                if (_definedColumns == value)
                    return;
                _definedColumns = value;
                OnPropertyChanged("DefinedColumns");
            }
        }

        public string DefinedRows
        {
            get
            {
                return _definedRows;
            }
            set
            {
                if (_definedRows == value)
                    return;
                _definedRows = value;
                OnPropertyChanged("DefinedRows");
            }
        }

        /// <summary>
        ///     Gets a collection of grid cells.
        /// </summary>
        /// <value>
        ///     A Collection of grid cells.
        /// </value>
        public Dictionary<String, CellContent> GridCellCollection
        {
            get
            {
                return _gridCellCollection;
            }
        }

        /// <summary>
        /// Gets or sets the number of columns.
        /// </summary>
        /// <value>
        /// The total number of columns.
        /// </value>
        public int NumberOfColumns
        {
            get
            {
                return _numberOfColumns;
            }
            set
            {
                if (_numberOfColumns == value)
                    return;
                _numberOfColumns = value;
                OnPropertyChanged("NumberOfColumns");
                DefinedColumns = (value - 1).ToString();
            }
        }

        /// <summary>
        /// Gets or sets the number of rows.
        /// </summary>
        /// <value>
        /// The total number of rows.
        /// </value>
        public int NumberOfRows
        {
            get
            {
                return _numberOfRows;
            }
            set
            {
                if (_numberOfRows == value)
                    return;
                _numberOfRows = value;
                OnPropertyChanged("NumberOfRows");
                DefinedRows = (value - 1).ToString();
            }
        }

        /// <summary>
        /// Gets or sets the size of the row default.
        /// </summary>
        /// <value>
        /// The size of the row default.
        /// </value>
        public GridLength RowDefaultSize
        {
            get
            {
                return _rowDefaultSize;
            }
            set
            {
                if (_rowDefaultSize == value)
                    return;
                _rowDefaultSize = value;
                OnPropertyChanged("RowDefaultSize");
                if (value.IsAuto)
                    AllRowSize = "Auto";
                else if (value.IsStar)
                    AllRowSize = "*";
                else
                    AllRowSize = value.Value.ToString();
            }
        }

        /// <summary>
        ///     Gets a collection of row header text blocks.
        /// </summary>
        /// <value>
        ///     A Collection of row header text blocks.
        /// </value>
        public List<TextBlock> RowHeaderTextBlockCollection
        {
            get
            {
                return _rowHeaderTextBlockCollection;
            }
        }

        /// <summary>
        ///     Gets a collection of row heights.
        /// </summary>
        /// <value>
        ///     A Collection of row heights.
        /// </value>
        public List<GridLength> RowHeightsCollection
        {
            get
            {
                return _rowHeightsCollection;
            }
        }

        /// <summary>
        ///     Gets or sets the row size POP up.
        /// </summary>
        /// <value>
        ///     The row size POP up.
        /// </value>
        public Popup RowSizePopUp
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets the row size popup timer.
        /// </summary>
        /// <value>
        ///     The row size popup timer.
        /// </value>
        public DispatcherTimer RowSizePopupTimer
        {
            get;
            set;
        }

        #endregion

        #region Event Handlers

        private void btnAllColumnsAuto_Click(object sender, RoutedEventArgs e)
        {
            SetAllColumnWidths(0, GridUnitType.Auto);
        }

        private void btnAllColumnsStar_Click(object sender, RoutedEventArgs e)
        {
            SetAllColumnWidths(1, GridUnitType.Star);
        }

        private void btnAllRowsAuto_Click(object sender, RoutedEventArgs e)
        {
            SetAllRowsHeights(0, GridUnitType.Auto);
        }

        private void btnAllRowsStar_Click(object sender, RoutedEventArgs e)
        {
            SetAllRowsHeights(1, GridUnitType.Star);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // BUG: Investigate exception: DialogResult can be set only after
            // a window has been opened as a dialog.
            DialogResult = false;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder(10240);
            sb.AppendLine("<Grid>");
            sb.AppendLine("\t<Grid.RowDefinitions>");

            var skipFirst = true;

            foreach (var obj in RowHeightsCollection)
            {
                if (skipFirst)
                {
                    skipFirst = false;
                    continue;
                }
                var height = obj.IsStar ? "*" : obj.IsAuto ? "Auto" : obj.Value.ToString();
                sb.AppendFormat("\t\t<RowDefinition Height=\"{0}\" />\r\n", height);
            }

            sb.AppendLine("\t</Grid.RowDefinitions>");
            sb.AppendLine("\t<Grid.ColumnDefinitions>");

            skipFirst = true;
            foreach (var obj in ColumnWidthsCollection)
            {
                if (skipFirst)
                {
                    skipFirst = false;
                    continue;
                }
                var width = obj.IsStar ? "*" : obj.IsAuto ? "Auto" : obj.Value.ToString();
                sb.AppendFormat("\t\t<ColumnDefinition Width=\"{0}\" />\r\n", width);
            }

            sb.AppendLine("\t</Grid.ColumnDefinitions>\r\n");

            for (int columnIndex = 0; columnIndex < gridLayout.ColumnDefinitions.Count; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < gridLayout.RowDefinitions.Count; rowIndex++)
                {
                    if (GridCellCollection.ContainsKey(MakeKey(rowIndex, columnIndex)) && GridCellCollection[MakeKey(rowIndex, columnIndex)].ControlType != ControlType.None)
                    {
                        var obj = GridCellCollection[MakeKey(rowIndex, columnIndex)];

                        if (ClassEntity != null && ClassEntity.IsSilverlight && obj.StringFormat.IsNotNullOrEmpty())
                        {
                            sb.AppendFormat("\t<!-- TODO - Add formatting converter for format: {0} -->", obj.StringFormat);
                        }
                        sb.AppendLine(ControlFactory(obj));
                    }
                }
                sb.AppendLine();
            }

            sb.Replace(" >", ">");
            sb.Replace("    ", " ");
            sb.Replace("   ", " ");
            sb.Replace("  ", " ");
            sb.AppendLine("</Grid>");
            BusinessForm = sb.ToString();

            if (BusinessForm.IndexOf("CHAGEME") > -1)
            {
                BusinessForm = string.Concat("\r\n\t<!-- Search for and change all instances of CHANGEME -->\r\n", BusinessForm);
            }
            DialogResult = true;
        }

        private void btnPopupColumnAutoSize_Click(object sender, RoutedEventArgs e)
        {
            var columnIndex = int.Parse(ColumnSizePopUp.Tag.ToString());
            ColumnWidthsCollection[columnIndex] = new GridLength(0, GridUnitType.Auto);
            ColumnHeaderTextBlockCollection[columnIndex].Text = ParseGridLength(ColumnWidthsCollection[columnIndex]);
            ColumnSizePopUp.IsOpen = false;
        }

        private void btnPopupColumnStarSize_Click(object sender, RoutedEventArgs e)
        {
            var columnIndex = int.Parse(ColumnSizePopUp.Tag.ToString());
            ColumnWidthsCollection[columnIndex] = new GridLength(1, GridUnitType.Star);
            ColumnHeaderTextBlockCollection[columnIndex].Text = ParseGridLength(ColumnWidthsCollection[columnIndex]);
            ColumnSizePopUp.IsOpen = false;
        }

        private void btnPopupRowAutoSize_Click(object sender, RoutedEventArgs e)
        {
            var rowIndex = int.Parse(RowSizePopUp.Tag.ToString());
            RowHeightsCollection[rowIndex] = new GridLength(0, GridUnitType.Auto);
            RowHeaderTextBlockCollection[rowIndex].Text = ParseGridLength(RowHeightsCollection[rowIndex]);
            RowSizePopUp.IsOpen = false;
        }

        private void btnPopupRowStarSize_Click(object sender, RoutedEventArgs e)
        {
            var rowIndex = int.Parse(RowSizePopUp.Tag.ToString());
            RowHeightsCollection[rowIndex] = new GridLength(1, GridUnitType.Star);
            RowHeaderTextBlockCollection[rowIndex].Text = ParseGridLength(RowHeightsCollection[rowIndex]);
            RowSizePopUp.IsOpen = false;
        }

        private void cboColumnHeader_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbo = sender as ComboBox;
            var columnIndex = (int)cbo.Tag;

            if (cbo.SelectedValue.ToString() == "Select")
            {
                return;
            }

            var controlType = (ControlType)Enum.Parse(typeof(ControlType), cbo.SelectedValue.ToString());

            for (int rowIndex = 1; rowIndex < NumberOfRows; rowIndex++)
            {
                GridCellCollection[MakeKey(rowIndex, columnIndex)].ControlType = controlType;
            }

            LayoutGrid();
        }

        private void ColumnTextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBlock;
            ColumnSizePopUp = FindResource("columnPopUp") as Popup;

            ColumnSizePopUp.Tag = tb.Tag;
            ColumnSizePopUp.StaysOpen = true;
            ColumnSizePopUp.PlacementTarget = tb;
            ColumnSizePopUp.VerticalOffset = -5;
            ColumnSizePopUp.IsOpen = true;

            ColumnSizePopupTimer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            ColumnSizePopupTimer.Start();
        }

        private void OnPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h == null)
                return;
            h(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RowTextBlock_MouseRightButtonDownEvent(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBlock;
            RowSizePopUp = FindResource("rowPopUp") as Popup;

            RowSizePopUp.Tag = tb.Tag;
            RowSizePopUp.StaysOpen = true;
            RowSizePopUp.PlacementTarget = tb;
            RowSizePopUp.VerticalOffset = -5;
            RowSizePopUp.IsOpen = true;

            RowSizePopupTimer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            RowSizePopupTimer.Start();
        }

        private void txtAllColumnsWidth_KeyPress(object sender,
            KeyEventArgs e)
        {
            UpdateAllRowsOrColumns(sender, e, false);
        }

        private void txtAllRowsHeight_KeyPress(object sender,
            KeyEventArgs e)
        {
            UpdateAllRowsOrColumns(sender, e, true);
        }

        private void txtNumberOfRowsOrColumns_KeyPress(object sender,
            KeyEventArgs e)
        {
            if (!(sender is TextBox) || e.Key != Key.Enter)
            {
                return;
            }
            if (SetRowOrColumnNumber(sender as TextBox))
                e.Handled = true;
        }

        private void txtPopupColumnWidth_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            SetDimention(sender as TextBox, false);
            ColumnSizePopUp.IsOpen = false;
        }

        private void txtPopupRowHeight_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            e.Handled = true;
            SetDimention(sender as TextBox, true);
            RowSizePopUp.IsOpen = false;
        }

        private void UpdateAllRowsOrColumns(object sender, KeyEventArgs e, bool RowUpdate)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            e.Handled = true;

            List<GridLength> gridLengthCollection;
            List<TextBlock> textBlockCollection;
            int definitionCount;

            if (RowUpdate)
            {
                gridLengthCollection = RowHeightsCollection;
                textBlockCollection = RowHeaderTextBlockCollection;
                definitionCount = gridLayout.RowDefinitions.Count - 1;
            }
            else
            {
                gridLengthCollection = ColumnWidthsCollection;
                textBlockCollection = ColumnHeaderTextBlockCollection;
                definitionCount = gridLayout.ColumnDefinitions.Count - 1;
            }

            var warning = RowUpdate ? "row height" : "column width";
            var tb = sender as TextBox;
            int length;

            if (!int.TryParse(tb.Text, out length) || length < 0)
            {
                MessageBox.Show(String.Format("The {0} must be an integer greater than or equal to zero, please reenter.",
                    warning), "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var defaultSize = new GridLength(length, GridUnitType.Pixel);

            if (RowUpdate)
                RowDefaultSize = defaultSize;
            else
                ColumnDefaultSize = defaultSize;

            for (int i = 0; i < definitionCount; i++)
            {
                gridLengthCollection[i] = defaultSize;
                textBlockCollection[i].Text = ParseGridLength(defaultSize);
            }
        }

        #endregion

        #region Methods

        private void AddColumnHeaders()
        {
            // Starting in column 1 (because 0 row and column are used for size
            // configuration) add elements to the column headers.
            for (int i = 1; i < gridLayout.ColumnDefinitions.Count; i++)
            {
                // Create and add the combo box.
                var cbo = new ComboBox()
                {
                    FontSize = 10,
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    Margin = new Thickness(10),
                    IsTabStop = false,
                    Tag = i
                };

                // Create a list of names from the enumeration
                var ary = Enum.GetNames(typeof(ControlType));
                Array.Sort(ary);

                // Create the values for the comboBox
                string nextString = string.Empty;
                string currentString = string.Empty;

                for (int j = 0; j < ary.Length; j++)
                {
                    if (nextString == string.Empty)
                    {
                        nextString = "Select";
                    }

                    if (ary[j] == "None")
                    {
                        ary[j] = nextString;
                        break;
                    }
                    currentString = ary[j];
                    ary[j] = nextString;
                    nextString = currentString;
                }
                cbo.ItemsSource = ary;

                // Needs to follow above loop to have something to select
                cbo.SelectedValue = "Select";

                cbo.AddHandler(ComboBox.SelectionChangedEvent,
                    new SelectionChangedEventHandler(cboColumnHeader_SelectionChanged));
                ColumnHeaderComboBoxCollection.Add(cbo);

                // Create the root element of the editor.
                var sp = new StackPanel();

                // cbo goes into a StackPanel.
                sp.Children.Add(cbo);

                // Create and add a text block to show the width of the column.
                var tb = new TextBlock()
                {
                    Tag = i - 1,
                    Margin = new Thickness(5),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    Text = ParseGridLength(ColumnWidthsCollection[i]),
                    ToolTip = "Right click to edit this column's size"
                };
                tb.AddHandler(TextBlock.MouseRightButtonDownEvent,
                    new MouseButtonEventHandler(ColumnTextBlock_MouseRightButtonDown));
                ColumnHeaderTextBlockCollection.Add(tb);

                // tb goes into the StackPanel
                sp.Children.Add(tb);

                // Set the column property of the stack panel.
                sp.SetValue(Grid.ColumnProperty, i);

                // StackPanel into the grid.
                gridLayout.Children.Add(sp);
            }
        }

        private void AddRowHeadersAndData()
        {
            // Iterate through the rows (row 0 done above).
            for (int rowsIndex = 1; rowsIndex < NumberOfRows; rowsIndex++)
            {
                // Iterate through the columns.
                for (int columnIndex = 0; columnIndex < NumberOfColumns; columnIndex++)
                {
                    // If in the first column, add the size info.
                    if (columnIndex == 0)
                    {
                        // Row size text box.
                        var tb = new TextBlock()
                        {
                            Tag = rowsIndex - 1,
                            Margin = new Thickness(5),
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                            VerticalAlignment = System.Windows.VerticalAlignment.Center,
                            Text = ParseGridLength(RowHeightsCollection[rowsIndex]),
                            ToolTip = "Right click to edit this row's size"
                        };

                        // Right Mouse Button Down event
                        tb.AddHandler(TextBlock.MouseRightButtonDownEvent,
                            new MouseButtonEventHandler(RowTextBlock_MouseRightButtonDownEvent));

                        // Set the row property.
                        tb.SetValue(Grid.RowProperty, rowsIndex);

                        // Set the column property.
                        tb.SetValue(Grid.ColumnProperty, columnIndex);

                        // Add to the RowHeaderTextBlockCollection.
                        RowHeaderTextBlockCollection.Add(tb);

                        // tb to the Grid.
                        gridLayout.Children.Add(tb);
                    }
                    else
                    {
                        // Else clause takes care of columns 1 and above.

                        // if the GridCellCollection does not have the row/column key, add it.
                        if (!GridCellCollection.ContainsKey(MakeKey(rowsIndex, columnIndex)))
                        {
                            GridCellCollection.Add(MakeKey(rowsIndex, columnIndex), new CellContent(rowsIndex, columnIndex));
                        }

                        var gridCellEditor = new GridCellEditor();
                        gridCellEditor.SetValue(Grid.RowProperty, rowsIndex);
                        gridCellEditor.SetValue(Grid.ColumnProperty, columnIndex);

                        // The DataContext of the gridCellEditor is set to the
                        // CellContent object for this row and column. This should
                        // allow the SelectedItem of the combo box to bind to the
                        // ControlType property of the CellContent object.
                        gridCellEditor.DataContext = GridCellCollection[MakeKey(rowsIndex, columnIndex)];

                        // cell editor into the Grid.
                        gridLayout.Children.Add(gridCellEditor);
                    }
                }
            }
        }

        private string ControlFactory(CellContent obj)
        {
            var uiPlatform = UIPlatform.WPF;

            if (ClassEntity != null && ClassEntity.IsSilverlight)
            {
                uiPlatform = UIPlatform.Silverlight;
            }

            var columnIndex = obj.Column - 1;
            var rowIndex = obj.Row - 1;

            if (obj.BindingPath.IsNullOrEmpty())
            {
                obj.BindingPath = "CHANGEME";
            }

            switch (obj.ControlType)
            {
                case ControlType.CheckBox:
                    return UIControlFactory.UIControlFactory.Instance
                        .MakeCheckBox(uiPlatform, columnIndex, rowIndex,
                        obj.ControlLabel, obj.BindingPath, obj.BindingMode);
                case ControlType.ComboBox:
                    return UIControlFactory.UIControlFactory.Instance
                        .MakeComboBox(uiPlatform, columnIndex, rowIndex,
                        obj.BindingPath, obj.BindingMode);
                case ControlType.Image:
                    return UIControlFactory.UIControlFactory.Instance
                        .MakeImage(uiPlatform, columnIndex, rowIndex, obj.BindingPath);
                case ControlType.Label:
                    return UIControlFactory.UIControlFactory.Instance
                        .MakeLabelWithoutBinding(uiPlatform, columnIndex, rowIndex,
                        obj.ControlLabel);
                case ControlType.TextBlock:
                    return UIControlFactory.UIControlFactory.Instance
                        .MakeTextBlock(uiPlatform, columnIndex, rowIndex,
                        obj.BindingPath, obj.StringFormat,
                        ClassEntity == null ? string.Empty :
                        ClassEntity.SilverlightVersion);
                case ControlType.TextBox:
                    return UIControlFactory.UIControlFactory.Instance
                        .MakeTextBox(uiPlatform, columnIndex, rowIndex, obj.BindingPath,
                        obj.BindingMode, obj.Width, obj.MaximumLength, obj.StringFormat,
                        obj.DataType.StartsWith("Nullable"),
                        ClassEntity == null ? string.Empty :
                        ClassEntity.SilverlightVersion);
                case ControlType.DatePicker:
                    return UIControlFactory.UIControlFactory.Instance
                        .MakeDatePicker(uiPlatform, columnIndex, rowIndex,
                        obj.BindingPath, obj.Width);
                default:

                    // No match
                    return string.Empty;
            }
        }

        private int GetIntFromKey(string arg)
        {
            var rowColumnArray = arg.Split(ROW_COLUMN_KEY_SEPARATOR);
            return int.Parse(rowColumnArray[1]);
        }

        private void LayoutGrid()
        {
            ResetLayout();

            MakeColumnsAndRows();

            AddColumnHeaders();

            AddRowHeadersAndData();
        }

        private void MakeColumnsAndRows()
        {
            // For each NumberOfColumns add a column definition.
            for (int i = 0; i < NumberOfColumns; i++)
            {
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(0, GridUnitType.Auto),
                    MinWidth = 75
                });
            }

            // for each NumberOfRows add a row definition.
            for (int i = 0; i < NumberOfRows; i++)
            {
                gridLayout.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(0, GridUnitType.Auto)
                });
            }

            // This adds alternating color to each row
            for (int i = 0; i < gridLayout.RowDefinitions.Count; i++)
            {
                if (i % 2 != 0)
                {
                    var rectangle = new Rectangle()
                    {
                        Fill = new SolidColorBrush(Colors.WhiteSmoke),
                        VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
                    };
                    rectangle.SetValue(Grid.RowProperty, i);
                    rectangle.SetValue(Grid.ColumnSpanProperty, NumberOfColumns);
                    gridLayout.Children.Add(rectangle);
                }
            }
        }

        private string MakeKey(int RowIndex, int ColumnIndex)
        {
            return string.Format("{0}{1}{2}", RowIndex, ROW_COLUMN_KEY_SEPARATOR, ColumnIndex);
        }

        private string ParseGridLength(GridLength obj)
        {
            if (obj.IsAuto)
            {
                return "Auto";
            }
            if (obj.IsStar)
            {
                return "Star";
            }
            return obj.Value.ToString();
        }

        private void ResetLayout()
        {
            // Remove all previous handlers
            foreach (var cbo in ColumnHeaderComboBoxCollection)
            {
                cbo.RemoveHandler(ComboBox.SelectionChangedEvent,
                    new SelectionChangedEventHandler(cboColumnHeader_SelectionChanged));
            }

            ColumnHeaderTextBlockCollection.Clear();
            RowHeaderTextBlockCollection.Clear();
            ColumnHeaderComboBoxCollection.Clear();
            gridLayout.Children.Clear();
            gridLayout.ColumnDefinitions.Clear();
            gridLayout.RowDefinitions.Clear();
        }

        private bool RightSizeRowsOrColumns(int index, bool DoRows)
        {
            int numberOfItems;
            List<GridLength> itemsCollection;
            GridLength defaultSize;

            if (DoRows)
            {
                numberOfItems = NumberOfRows;
                itemsCollection = RowHeightsCollection;
                defaultSize = RowDefaultSize;
            }
            else
            {
                numberOfItems = NumberOfColumns;
                itemsCollection = ColumnWidthsCollection;
                defaultSize = ColumnDefaultSize;
            }

            index++;

            // User removed one or more rows or columns.
            if (index < numberOfItems)
            {
                var removeList = new List<string>();
                foreach (var s in GridCellCollection.Keys)
                {
                    if (GetIntFromKey(s) > index)
                        removeList.Add(s);
                }
                foreach (var s in removeList)
                {
                    GridCellCollection.Remove(s);
                }
                itemsCollection.RemoveRange(index, numberOfItems - index);
            }
            if (DoRows)
            {
                NumberOfRows = index;
            }
            else
            {
                NumberOfColumns = index;
            }
            while (itemsCollection.Count < index)
            {
                itemsCollection.Add(defaultSize);
            }
            LayoutGrid();
            return true;
        }

        private void SetAllColumnWidths(int length, GridUnitType defaultType)
        {
            ColumnDefaultSize = new GridLength(length, defaultType);

            for (int i = 1; i < gridLayout.ColumnDefinitions.Count; i++)
            {
                ColumnWidthsCollection[i] = ColumnDefaultSize;
                ColumnHeaderTextBlockCollection[i - 1].Text = ParseGridLength(ColumnDefaultSize);
            }

            txtColumnSize.Text = string.Empty;
        }

        private void SetAllRowsHeights(int height, GridUnitType defaultType)
        {
            RowDefaultSize = new GridLength(height, defaultType);

            for (int i = 1; i < gridLayout.RowDefinitions.Count; i++)
            {
                RowHeightsCollection[i] = RowDefaultSize;
                RowHeaderTextBlockCollection[i - 1].Text = ParseGridLength(RowDefaultSize);
            }

            txtRowSize.Text = string.Empty;
        }

        private void SetDimention(TextBox txt, bool doRow)
        {
            int index;
            if (txt.Text.IsNullOrWhiteSpace())
                return;

            int dimention;
            if (!(int.TryParse(txt.Text, out dimention) && dimention >= 0))
            {
                MessageBox.Show("The dimention must be an integer greater than or equal to zero, please reenter.", "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (txt.Tag == null)
            {
                for (int i = 0; i < NumberOfRows - 1; i++)
                {
                    txt.Tag = i;
                    SetDimention(txt, doRow);
                }
                txt.Tag = null;
                return;
            }
            else
            {
                index = (int)txt.Tag;
            }

            List<TextBlock> TextBlockCollection;
            List<GridLength> GridLengthCollection;

            if (doRow)
            {
                TextBlockCollection = RowHeaderTextBlockCollection;
                GridLengthCollection = RowHeightsCollection;
            }
            else
            {
                TextBlockCollection = ColumnHeaderTextBlockCollection;
                GridLengthCollection = ColumnWidthsCollection;
            }

            TextBlockCollection[index].Text = dimention.ToString();
            GridLengthCollection[index] = new GridLength(dimention);

            //txt.Text = string.Empty;
        }

        private bool SetRowOrColumnNumber(TextBox tb)
        {
            bool? DoRows = tb.Name.Contains("Rows") ? true :
                tb.Name.Contains("Columns") ? false : (bool?)null;

            if (DoRows == null)
            {
                return false;
            }

            var index = -1;
            if (int.TryParse(tb.Text, out index) == false || index < 1 || index > 50)
            {
                MessageBox.Show("Row count and column count must be entered between 1 and 50. Please reenter and try again.",
                    "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return true;
            }

            if (!((bool)DoRows && NumberOfColumns < 2))
                return RightSizeRowsOrColumns(index, (bool)DoRows);

            tb.Text = string.Empty;
            MessageBox.Show("Column count must be configured before rows. Please enter a column count between 1 and 50 first, and press ENTER. A row count may then be specified.",
                "Workflow Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return true;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}