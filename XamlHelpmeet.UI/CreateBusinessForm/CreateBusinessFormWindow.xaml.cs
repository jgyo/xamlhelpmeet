// file:	CreateBusinessForm\CreateBusinessFormWindow.xaml.cs
//
// summary:	Implements the create business form window.xaml class
namespace XamlHelpmeet.UI.CreateBusinessForm
{
    #region Imports

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
    using XamlHelpmeet.Extensions;
    using XamlHelpmeet.Model;
    using XamlHelpmeet.UI.Editors;
    using XamlHelpmeet.UI.UIControlFactory;

    #endregion

    /// <summary>
    ///     Interaction logic for CreateBusinessFormWindow.xaml.
    /// </summary>
    /// <seealso cref="T:System.Windows.Window" />
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

        /// <summary>
        ///     Initializes a new instance of the CreateBusinessFormWindow class.
        /// </summary>
        /// <param name="ClassEntity">
        ///     The class entity.
        /// </param>
        public CreateBusinessFormWindow(ClassEntity ClassEntity)
        {
            // Here's the call that the form designer forces us to call.
            this.InitializeComponent();

            this.ColumnDefaultSize = new GridLength(0, GridUnitType.Auto);
            this.RowDefaultSize = new GridLength(0, GridUnitType.Auto);

            this._columnHeaderComboBoxCollection = new List<ComboBox>();
            this._columnHeaderTextBlockCollection = new List<TextBlock>();
            this._columnWidthsCollection = new List<GridLength>();
            this._gridCellCollection = new Dictionary<string, CellContent>();
            this._rowHeaderTextBlockCollection = new List<TextBlock>();
            this._rowHeightsCollection = new List<GridLength>();

            this.BusinessForm = string.Empty;
            CreateBusinessFormWindow.ClassEntity = ClassEntity;
        }

        #endregion

        #region Properties and Indexers

        public string AllColumnSize
        {
            get { return this._allColumnSize; }
            set
            {
                if (this._allColumnSize == value)
                {
                    return;
                }
                this._allColumnSize = value;
                this.OnPropertyChanged("AllColumnSize");
            }
        }

        public string AllRowSize
        {
            get { return this._allRowSize; }
            set
            {
                if (this._allRowSize == value)
                {
                    return;
                }
                this._allRowSize = value;
                this.OnPropertyChanged("AllRowSize");
            }
        }

        /// <summary>
        ///     Gets the business form.
        /// </summary>
        /// <value>
        ///     The business form.
        /// </value>
        public string BusinessForm { get; private set; }

        /// <summary>
        ///     Gets the class entity.
        /// </summary>
        /// <value>
        ///     The class entity.
        /// </value>
        public static ClassEntity ClassEntity { get; private set; }

        /// <summary>
        ///     Gets or sets the size of the column default.
        /// </summary>
        /// <value>
        ///     The size of the column default.
        /// </value>
        public GridLength ColumnDefaultSize
        {
            get { return this._columnDefaultSize; }
            set
            {
                if (this._columnDefaultSize == value)
                {
                    return;
                }
                this._columnDefaultSize = value;
                this.OnPropertyChanged("ColumnDefaultSize");
                if (value.IsAuto)
                {
                    this.AllColumnSize = "Auto";
                }
                else if (value.IsStar)
                {
                    this.AllColumnSize = "*";
                }
                else
                {
                    this.AllColumnSize = value.Value.ToString();
                }
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
            get { return this._columnHeaderComboBoxCollection; }
        }

        /// <summary>
        ///     Gets a collection of column header text blocks.
        /// </summary>
        /// <value>
        ///     A Collection of column header text blocks.
        /// </value>
        public List<TextBlock> ColumnHeaderTextBlockCollection
        {
            get { return this._columnHeaderTextBlockCollection; }
        }

        /// <summary>
        ///     Gets or sets the column size POP up.
        /// </summary>
        /// <value>
        ///     The column size POP up.
        /// </value>
        public Popup ColumnSizePopUp { get; set; }

        /// <summary>
        ///     Gets or sets the column size popup timer.
        /// </summary>
        /// <value>
        ///     The column size popup timer.
        /// </value>
        public DispatcherTimer ColumnSizePopupTimer { get; set; }

        /// <summary>
        ///     Gets a collection of column widths.
        /// </summary>
        /// <value>
        ///     A Collection of column widths.
        /// </value>
        public List<GridLength> ColumnWidthsCollection
        {
            get { return this._columnWidthsCollection; }
        }

        public string DefinedColumns
        {
            get { return this._definedColumns; }
            set
            {
                if (this._definedColumns == value)
                {
                    return;
                }
                this._definedColumns = value;
                this.OnPropertyChanged("DefinedColumns");
            }
        }

        public string DefinedRows
        {
            get { return this._definedRows; }
            set
            {
                if (this._definedRows == value)
                {
                    return;
                }
                this._definedRows = value;
                this.OnPropertyChanged("DefinedRows");
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
            get { return this._gridCellCollection; }
        }

        /// <summary>
        ///     Gets or sets the number of columns.
        /// </summary>
        /// <value>
        ///     The total number of columns.
        /// </value>
        public int NumberOfColumns
        {
            get { return this._numberOfColumns; }
            set
            {
                if (this._numberOfColumns == value)
                {
                    return;
                }
                this._numberOfColumns = value;
                this.OnPropertyChanged("NumberOfColumns");
                this.DefinedColumns = (value - 1).ToString();
            }
        }

        /// <summary>
        ///     Gets or sets the number of rows.
        /// </summary>
        /// <value>
        ///     The total number of rows.
        /// </value>
        public int NumberOfRows
        {
            get { return this._numberOfRows; }
            set
            {
                if (this._numberOfRows == value)
                {
                    return;
                }
                this._numberOfRows = value;
                this.OnPropertyChanged("NumberOfRows");
                this.DefinedRows = (value - 1).ToString();
            }
        }

        /// <summary>
        ///     Gets or sets the size of the row default.
        /// </summary>
        /// <value>
        ///     The size of the row default.
        /// </value>
        public GridLength RowDefaultSize
        {
            get { return this._rowDefaultSize; }
            set
            {
                if (this._rowDefaultSize == value)
                {
                    return;
                }
                this._rowDefaultSize = value;
                this.OnPropertyChanged("RowDefaultSize");
                if (value.IsAuto)
                {
                    this.AllRowSize = "Auto";
                }
                else if (value.IsStar)
                {
                    this.AllRowSize = "*";
                }
                else
                {
                    this.AllRowSize = value.Value.ToString();
                }
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
            get { return this._rowHeaderTextBlockCollection; }
        }

        /// <summary>
        ///     Gets a collection of row heights.
        /// </summary>
        /// <value>
        ///     A Collection of row heights.
        /// </value>
        public List<GridLength> RowHeightsCollection
        {
            get { return this._rowHeightsCollection; }
        }

        /// <summary>
        ///     Gets or sets the row size POP up.
        /// </summary>
        /// <value>
        ///     The row size POP up.
        /// </value>
        public Popup RowSizePopUp { get; set; }

        /// <summary>
        ///     Gets or sets the row size popup timer.
        /// </summary>
        /// <value>
        ///     The row size popup timer.
        /// </value>
        public DispatcherTimer RowSizePopupTimer { get; set; }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods (private)

        private void AddColumnHeaders()
        {
            // Starting in column 1 (because 0 row and column are used for size
            // configuration) add elements to the column headers.
            for (int i = 1; i < this.gridLayout.ColumnDefinitions.Count; i++)
            {
                // Create and add the combo box.
                var cbo = new ComboBox
                          {
                              FontSize = 10,
                              VerticalAlignment = VerticalAlignment.Top,
                              Margin = new Thickness(10),
                              IsTabStop = false,
                              Tag = i
                          };

                // Create a list of names from the enumeration
                string[] ary = Enum.GetNames(typeof(ControlType));
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

                cbo.AddHandler(Selector.SelectionChangedEvent,
                               new SelectionChangedEventHandler(this.cboColumnHeader_SelectionChanged));
                this.ColumnHeaderComboBoxCollection.Add(cbo);

                // Create the root element of the editor.
                var sp = new StackPanel();

                // cbo goes into a StackPanel.
                sp.Children.Add(cbo);

                // Create and add a text block to show the width of the column.
                var tb = new TextBlock
                         {
                             Tag = i - 1,
                             Margin = new Thickness(5),
                             HorizontalAlignment = HorizontalAlignment.Center,
                             Text = this.ParseGridLength(this.ColumnWidthsCollection[i]),
                             ToolTip = "Right click to edit this column's size"
                         };
                tb.AddHandler(MouseRightButtonDownEvent,
                              new MouseButtonEventHandler(this.ColumnTextBlock_MouseRightButtonDown));
                this.ColumnHeaderTextBlockCollection.Add(tb);

                // tb goes into the StackPanel
                sp.Children.Add(tb);

                // Set the column property of the stack panel.
                sp.SetValue(Grid.ColumnProperty, i);

                // StackPanel into the grid.
                this.gridLayout.Children.Add(sp);
            }
        }

        private void AddRowHeadersAndData()
        {
            // Iterate through the rows (row 0 done above).
            for (int rowsIndex = 1; rowsIndex < this.NumberOfRows; rowsIndex++)
            {
                // Iterate through the columns.
                for (int columnIndex = 0; columnIndex < this.NumberOfColumns; columnIndex++)
                {
                    // If in the first column, add the size info.
                    if (columnIndex == 0)
                    {
                        // Row size text box.
                        var tb = new TextBlock
                                 {
                                     Tag = rowsIndex - 1,
                                     Margin = new Thickness(5),
                                     HorizontalAlignment = HorizontalAlignment.Center,
                                     VerticalAlignment = VerticalAlignment.Center,
                                     Text = this.ParseGridLength(this.RowHeightsCollection[rowsIndex]),
                                     ToolTip = "Right click to edit this row's size"
                                 };

                        // Right Mouse Button Down event
                        tb.AddHandler(MouseRightButtonDownEvent,
                                      new MouseButtonEventHandler(this.RowTextBlock_MouseRightButtonDownEvent));

                        // Set the row property.
                        tb.SetValue(Grid.RowProperty, rowsIndex);

                        // Set the column property.
                        tb.SetValue(Grid.ColumnProperty, columnIndex);

                        // Add to the RowHeaderTextBlockCollection.
                        this.RowHeaderTextBlockCollection.Add(tb);

                        // tb to the Grid.
                        this.gridLayout.Children.Add(tb);
                    }
                    else
                    {
                        // Else clause takes care of columns 1 and above.

                        // if the GridCellCollection does not have the row/column key, add it.
                        if (!this.GridCellCollection.ContainsKey(this.MakeKey(rowsIndex, columnIndex)))
                        {
                            this.GridCellCollection.Add(this.MakeKey(rowsIndex, columnIndex),
                                                        new CellContent(rowsIndex, columnIndex));
                        }

                        var gridCellEditor = new GridCellEditor();
                        gridCellEditor.SetValue(Grid.RowProperty, rowsIndex);
                        gridCellEditor.SetValue(Grid.ColumnProperty, columnIndex);

                        // The DataContext of the gridCellEditor is set to the
                        // CellContent object for this row and column. This should
                        // allow the SelectedItem of the combo box to bind to the
                        // ControlType property of the CellContent object.
                        gridCellEditor.DataContext = this.GridCellCollection[this.MakeKey(rowsIndex, columnIndex)];

                        // cell editor into the Grid.
                        this.gridLayout.Children.Add(gridCellEditor);
                    }
                }
            }
        }

        private void ColumnTextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBlock;
            this.ColumnSizePopUp = this.FindResource("columnPopUp") as Popup;

            this.ColumnSizePopUp.Tag = tb.Tag;
            this.ColumnSizePopUp.StaysOpen = true;
            this.ColumnSizePopUp.PlacementTarget = tb;
            this.ColumnSizePopUp.VerticalOffset = -5;
            this.ColumnSizePopUp.IsOpen = true;

            this.ColumnSizePopupTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            this.ColumnSizePopupTimer.Start();
        }

        private string ControlFactory(CellContent obj)
        {
            var uiPlatform = UIPlatform.WPF;

            if (ClassEntity != null && ClassEntity.IsSilverlight)
            {
                uiPlatform = UIPlatform.Silverlight;
            }

            int columnIndex = obj.Column - 1;
            int rowIndex = obj.Row - 1;

            if (obj.BindingPath.IsNullOrEmpty())
            {
                obj.BindingPath = "CHANGEME";
            }

            switch (obj.ControlType)
            {
                case ControlType.CheckBox:
                    return UIControlFactory.Instance.MakeCheckBox(uiPlatform,
                                                                  columnIndex,
                                                                  rowIndex,
                                                                  obj.ControlLabel,
                                                                  obj.BindingPath,
                                                                  obj.BindingMode);
                case ControlType.ComboBox:
                    return UIControlFactory.Instance.MakeComboBox(uiPlatform,
                                                                  columnIndex,
                                                                  rowIndex,
                                                                  obj.BindingPath,
                                                                  obj.BindingMode);
                case ControlType.Image:
                    return UIControlFactory.Instance.MakeImage(uiPlatform, columnIndex, rowIndex, obj.BindingPath);
                case ControlType.Label:
                    return UIControlFactory.Instance.MakeLabelWithoutBinding(uiPlatform,
                                                                             columnIndex,
                                                                             rowIndex,
                                                                             obj.ControlLabel);
                case ControlType.TextBlock:
                    return UIControlFactory.Instance.MakeTextBlock(uiPlatform,
                                                                   columnIndex,
                                                                   rowIndex,
                                                                   obj.BindingPath,
                                                                   obj.StringFormat,
                                                                   ClassEntity == null
                                                                       ? string.Empty
                                                                       : ClassEntity.SilverlightVersion);
                case ControlType.TextBox:
                    return UIControlFactory.Instance.MakeTextBox(uiPlatform,
                                                                 columnIndex,
                                                                 rowIndex,
                                                                 obj.BindingPath,
                                                                 obj.BindingMode,
                                                                 obj.Width,
                                                                 obj.MaximumLength,
                                                                 obj.StringFormat,
                                                                 obj.DataType.StartsWith("Nullable"),
                                                                 ClassEntity == null
                                                                     ? string.Empty
                                                                     : ClassEntity.SilverlightVersion);
                case ControlType.DatePicker:
                    return UIControlFactory.Instance.MakeDatePicker(uiPlatform,
                                                                    columnIndex,
                                                                    rowIndex,
                                                                    obj.BindingPath,
                                                                    obj.Width);
                default:

                    // No match
                    return string.Empty;
            }
        }

        private int GetIntFromKey(string arg)
        {
            string[] rowColumnArray = arg.Split(ROW_COLUMN_KEY_SEPARATOR);
            return int.Parse(rowColumnArray[1]);
        }

        private void LayoutGrid()
        {
            this.ResetLayout();

            this.MakeColumnsAndRows();

            this.AddColumnHeaders();

            this.AddRowHeadersAndData();
        }

        private void MakeColumnsAndRows()
        {
            // For each NumberOfColumns add a column definition.
            for (int i = 0; i < this.NumberOfColumns; i++)
            {
                this.gridLayout.ColumnDefinitions.Add(new ColumnDefinition
                                                      {
                                                          Width =
                                                              new GridLength(0, GridUnitType.Auto),
                                                          MinWidth = 75
                                                      });
            }

            // for each NumberOfRows add a row definition.
            for (int i = 0; i < this.NumberOfRows; i++)
            {
                this.gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            }

            // This adds alternating color to each row
            for (int i = 0; i < this.gridLayout.RowDefinitions.Count; i++)
            {
                if (i % 2 != 0)
                {
                    var rectangle = new Rectangle
                                    {
                                        Fill = new SolidColorBrush(Colors.WhiteSmoke),
                                        VerticalAlignment = VerticalAlignment.Stretch,
                                        HorizontalAlignment = HorizontalAlignment.Stretch
                                    };
                    rectangle.SetValue(Grid.RowProperty, i);
                    rectangle.SetValue(Grid.ColumnSpanProperty, this.NumberOfColumns);
                    this.gridLayout.Children.Add(rectangle);
                }
            }
        }

        private string MakeKey(int RowIndex, int ColumnIndex)
        {
            return string.Format("{0}{1}{2}", RowIndex, ROW_COLUMN_KEY_SEPARATOR, ColumnIndex);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler h = this.PropertyChanged;
            if (h == null)
            {
                return;
            }
            h(this, new PropertyChangedEventArgs(propertyName));
        }

        private string ParseGridLength(GridLength obj)
        {
            if (obj.IsAuto)
            {
                return "Auto";
            }
            return obj.IsStar ? "Star" : obj.Value.ToString();
        }

        private void ResetLayout()
        {
            // Remove all previous handlers
            foreach (var cbo in this.ColumnHeaderComboBoxCollection)
            {
                cbo.RemoveHandler(Selector.SelectionChangedEvent,
                                  new SelectionChangedEventHandler(this.cboColumnHeader_SelectionChanged));
            }

            this.ColumnHeaderTextBlockCollection.Clear();
            this.RowHeaderTextBlockCollection.Clear();
            this.ColumnHeaderComboBoxCollection.Clear();
            this.gridLayout.Children.Clear();
            this.gridLayout.ColumnDefinitions.Clear();
            this.gridLayout.RowDefinitions.Clear();
        }

        private bool RightSizeRowsOrColumns(int index, bool DoRows)
        {
            int numberOfItems;
            List<GridLength> itemsCollection;
            GridLength defaultSize;

            if (DoRows)
            {
                numberOfItems = this.NumberOfRows;
                itemsCollection = this.RowHeightsCollection;
                defaultSize = this.RowDefaultSize;
            }
            else
            {
                numberOfItems = this.NumberOfColumns;
                itemsCollection = this.ColumnWidthsCollection;
                defaultSize = this.ColumnDefaultSize;
            }

            index++;

            // User removed one or more rows or columns.
            if (index < numberOfItems)
            {
                var removeList = new List<string>();
                foreach (var s in this.GridCellCollection.Keys)
                {
                    if (this.GetIntFromKey(s) > index)
                    {
                        removeList.Add(s);
                    }
                }
                foreach (var s in removeList)
                {
                    this.GridCellCollection.Remove(s);
                }
                itemsCollection.RemoveRange(index, numberOfItems - index);
            }
            if (DoRows)
            {
                this.NumberOfRows = index;
            }
            else
            {
                this.NumberOfColumns = index;
            }
            while (itemsCollection.Count < index)
            {
                itemsCollection.Add(defaultSize);
            }
            this.LayoutGrid();
            return true;
        }

        private void RowTextBlock_MouseRightButtonDownEvent(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBlock;
            this.RowSizePopUp = this.FindResource("rowPopUp") as Popup;

            this.RowSizePopUp.Tag = tb.Tag;
            this.RowSizePopUp.StaysOpen = true;
            this.RowSizePopUp.PlacementTarget = tb;
            this.RowSizePopUp.VerticalOffset = -5;
            this.RowSizePopUp.IsOpen = true;

            this.RowSizePopupTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            this.RowSizePopupTimer.Start();
        }

        private void SetAllColumnWidths(int length, GridUnitType defaultType)
        {
            this.ColumnDefaultSize = new GridLength(length, defaultType);

            for (int i = 1; i < this.gridLayout.ColumnDefinitions.Count; i++)
            {
                this.ColumnWidthsCollection[i] = this.ColumnDefaultSize;
                this.ColumnHeaderTextBlockCollection[i - 1].Text = this.ParseGridLength(this.ColumnDefaultSize);
            }

            this.txtColumnSize.Text = string.Empty;
        }

        private void SetAllRowsHeights(int height, GridUnitType defaultType)
        {
            this.RowDefaultSize = new GridLength(height, defaultType);

            for (int i = 1; i < this.gridLayout.RowDefinitions.Count; i++)
            {
                this.RowHeightsCollection[i] = this.RowDefaultSize;
                this.RowHeaderTextBlockCollection[i - 1].Text = this.ParseGridLength(this.RowDefaultSize);
            }

            this.txtRowSize.Text = string.Empty;
        }

        private void SetDimension(TextBox txt, bool doRow)
        {
            int index;
            if (txt.Text.IsNullOrWhiteSpace())
            {
                return;
            }

            int dimension;
            if (!(int.TryParse(txt.Text, out dimension) && dimension >= 0))
            {
                MessageBox.Show("The dimention must be an integer greater than or equal to zero, please reenter.",
                                "Invalid Data",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }

            if (txt.Tag == null)
            {
                for (int i = 0; i < this.NumberOfRows - 1; i++)
                {
                    txt.Tag = i;
                    this.SetDimension(txt, doRow);
                }
                txt.Tag = null;
                return;
            }
            index = (int)txt.Tag;

            List<TextBlock> TextBlockCollection;
            List<GridLength> GridLengthCollection;

            if (doRow)
            {
                TextBlockCollection = this.RowHeaderTextBlockCollection;
                GridLengthCollection = this.RowHeightsCollection;
            }
            else
            {
                TextBlockCollection = this.ColumnHeaderTextBlockCollection;
                GridLengthCollection = this.ColumnWidthsCollection;
            }

            TextBlockCollection[index].Text = dimension.ToString();
            GridLengthCollection[index] = new GridLength(dimension);

            //txt.Text = string.Empty;
        }

        private bool SetRowOrColumnNumber(TextBox tb)
        {
            bool? DoRows = tb.Name.Contains("Rows") ? true : tb.Name.Contains("Columns") ? false : (bool?)null;

            if (DoRows == null)
            {
                return false;
            }

            int index = -1;
            if (int.TryParse(tb.Text, out index) == false || index < 1 || index > 50)
            {
                MessageBox.Show(
                                "Row count and column count must be entered between 1 and 50. Please reenter and try again.",
                                "Invalid Data",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return true;
            }

            if (!((bool)DoRows && this.NumberOfColumns < 2))
            {
                return this.RightSizeRowsOrColumns(index, (bool)DoRows);
            }

            tb.Text = string.Empty;
            MessageBox.Show(
                            "Column count must be configured before rows. Please enter a column count between 1 and 50 first, and press ENTER. A row count may then be specified.",
                            "Workflow Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Exclamation);
            return true;
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
                gridLengthCollection = this.RowHeightsCollection;
                textBlockCollection = this.RowHeaderTextBlockCollection;
                definitionCount = this.gridLayout.RowDefinitions.Count - 1;
            }
            else
            {
                gridLengthCollection = this.ColumnWidthsCollection;
                textBlockCollection = this.ColumnHeaderTextBlockCollection;
                definitionCount = this.gridLayout.ColumnDefinitions.Count - 1;
            }

            string warning = RowUpdate ? "row height" : "column width";
            var tb = sender as TextBox;
            int length;

            if (!int.TryParse(tb.Text, out length) || length < 0)
            {
                MessageBox.Show(
                                String.Format(
                                              "The {0} must be an integer greater than or equal to zero, please reenter.",
                                              warning),
                                "Invalid Data",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }

            var defaultSize = new GridLength(length, GridUnitType.Pixel);

            if (RowUpdate)
            {
                this.RowDefaultSize = defaultSize;
            }
            else
            {
                this.ColumnDefaultSize = defaultSize;
            }

            for (int i = 0; i < definitionCount; i++)
            {
                gridLengthCollection[i] = defaultSize;
                textBlockCollection[i].Text = this.ParseGridLength(defaultSize);
            }
        }

        private void btnAllColumnsAuto_Click(object sender, RoutedEventArgs e)
        {
            this.SetAllColumnWidths(0, GridUnitType.Auto);
        }

        private void btnAllColumnsStar_Click(object sender, RoutedEventArgs e)
        {
            this.SetAllColumnWidths(1, GridUnitType.Star);
        }

        private void btnAllRowsAuto_Click(object sender, RoutedEventArgs e)
        {
            this.SetAllRowsHeights(0, GridUnitType.Auto);
        }

        private void btnAllRowsStar_Click(object sender, RoutedEventArgs e)
        {
            this.SetAllRowsHeights(1, GridUnitType.Star);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // BUG: Investigate exception: DialogResult can be set only after
            // a window has been opened as a dialog.
            this.DialogResult = false;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder(10240);
            sb.AppendLine("<Grid>");
            sb.AppendLine("\t<Grid.RowDefinitions>");

            bool skipFirst = true;

            foreach (var obj in this.RowHeightsCollection)
            {
                if (skipFirst)
                {
                    skipFirst = false;
                    continue;
                }
                string height = obj.IsStar ? "*" : obj.IsAuto ? "Auto" : obj.Value.ToString();
                sb.AppendFormat("\t\t<RowDefinition Height=\"{0}\" />\r\n", height);
            }

            sb.AppendLine("\t</Grid.RowDefinitions>");
            sb.AppendLine("\t<Grid.ColumnDefinitions>");

            skipFirst = true;
            foreach (var obj in this.ColumnWidthsCollection)
            {
                if (skipFirst)
                {
                    skipFirst = false;
                    continue;
                }
                string width = obj.IsStar ? "*" : obj.IsAuto ? "Auto" : obj.Value.ToString();
                sb.AppendFormat("\t\t<ColumnDefinition Width=\"{0}\" />\r\n", width);
            }

            sb.AppendLine("\t</Grid.ColumnDefinitions>\r\n");

            for (int columnIndex = 0; columnIndex < this.gridLayout.ColumnDefinitions.Count; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < this.gridLayout.RowDefinitions.Count; rowIndex++)
                {
                    if (this.GridCellCollection.ContainsKey(this.MakeKey(rowIndex, columnIndex))
                        && this.GridCellCollection[this.MakeKey(rowIndex, columnIndex)].ControlType != ControlType.None)
                    {
                        CellContent obj = this.GridCellCollection[this.MakeKey(rowIndex, columnIndex)];

                        if (ClassEntity != null && ClassEntity.IsSilverlight && obj.StringFormat.IsNotNullOrEmpty())
                        {
                            sb.AppendFormat("\t<!-- TODO - Add formatting converter for format: {0} -->",
                                            obj.StringFormat);
                        }
                        sb.AppendLine(this.ControlFactory(obj));
                    }
                }
                sb.AppendLine();
            }

            sb.Replace(" >", ">");
            sb.Replace("    ", " ");
            sb.Replace("   ", " ");
            sb.Replace("  ", " ");
            sb.AppendLine("</Grid>");
            this.BusinessForm = sb.ToString();

            if (this.BusinessForm.IndexOf("CHAGEME") > -1)
            {
                this.BusinessForm = string.Concat("\r\n\t<!-- Search for and change all instances of CHANGEME -->\r\n",
                                                  this.BusinessForm);
            }
            this.DialogResult = true;
        }

        private void btnPopupColumnAutoSize_Click(object sender, RoutedEventArgs e)
        {
            int columnIndex = int.Parse(this.ColumnSizePopUp.Tag.ToString());
            this.ColumnWidthsCollection[columnIndex] = new GridLength(0, GridUnitType.Auto);
            this.ColumnHeaderTextBlockCollection[columnIndex].Text =
                this.ParseGridLength(this.ColumnWidthsCollection[columnIndex]);
            this.ColumnSizePopUp.IsOpen = false;
        }

        private void btnPopupColumnStarSize_Click(object sender, RoutedEventArgs e)
        {
            int columnIndex = int.Parse(this.ColumnSizePopUp.Tag.ToString());
            this.ColumnWidthsCollection[columnIndex] = new GridLength(1, GridUnitType.Star);
            this.ColumnHeaderTextBlockCollection[columnIndex].Text =
                this.ParseGridLength(this.ColumnWidthsCollection[columnIndex]);
            this.ColumnSizePopUp.IsOpen = false;
        }

        private void btnPopupRowAutoSize_Click(object sender, RoutedEventArgs e)
        {
            int rowIndex = int.Parse(this.RowSizePopUp.Tag.ToString());
            this.RowHeightsCollection[rowIndex] = new GridLength(0, GridUnitType.Auto);
            this.RowHeaderTextBlockCollection[rowIndex].Text = this.ParseGridLength(this.RowHeightsCollection[rowIndex]);
            this.RowSizePopUp.IsOpen = false;
        }

        private void btnPopupRowStarSize_Click(object sender, RoutedEventArgs e)
        {
            int rowIndex = int.Parse(this.RowSizePopUp.Tag.ToString());
            this.RowHeightsCollection[rowIndex] = new GridLength(1, GridUnitType.Star);
            this.RowHeaderTextBlockCollection[rowIndex].Text = this.ParseGridLength(this.RowHeightsCollection[rowIndex]);
            this.RowSizePopUp.IsOpen = false;
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

            for (int rowIndex = 1; rowIndex < this.NumberOfRows; rowIndex++)
            {
                this.GridCellCollection[this.MakeKey(rowIndex, columnIndex)].ControlType = controlType;
            }

            this.LayoutGrid();
        }

        private void txtAllColumnsWidth_KeyPress(object sender, KeyEventArgs e)
        {
            this.UpdateAllRowsOrColumns(sender, e, false);
        }

        private void txtAllRowsHeight_KeyPress(object sender, KeyEventArgs e)
        {
            this.UpdateAllRowsOrColumns(sender, e, true);
        }

        private void txtNumberOfRowsOrColumns_KeyPress(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox) || e.Key != Key.Enter)
            {
                return;
            }
            if (this.SetRowOrColumnNumber(sender as TextBox))
            {
                e.Handled = true;
            }
        }

        private void txtPopupColumnWidth_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            this.SetDimension(sender as TextBox, false);
            this.ColumnSizePopUp.IsOpen = false;
        }

        private void txtPopupRowHeight_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            e.Handled = true;
            this.SetDimension(sender as TextBox, true);
            this.RowSizePopUp.IsOpen = false;
        }

        #endregion
    }
}
