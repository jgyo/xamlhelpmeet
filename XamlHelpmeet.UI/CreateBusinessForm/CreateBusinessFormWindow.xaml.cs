// file:    CreateBusinessForm\CreateBusinessFormWindow.xaml.cs
//
// summary: Implements the create business form window.xaml class

namespace XamlHelpmeet.UI.CreateBusinessForm
{
#region Imports

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using NLog;

using XamlHelpmeet.Extensions;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.Editors;
using XamlHelpmeet.UI.UIControlFactory;

using YoderZone.Extensions.NLog;

#endregion

/// <summary>
///     Interaction logic for CreateBusinessFormWindow.xaml.
/// </summary>
/// <seealso cref="T:System.Windows.Window" />
public partial class CreateBusinessFormWindow : Window,
    INotifyPropertyChanged
{
    #region Constants

    private const char ROW_COLUMN_KEY_SEPARATOR = ':';

    #endregion

    #region Static Fields

    private static readonly Logger logger = SettingsHelper.CreateLogger();

    #endregion

    #region Fields

    private readonly List<ComboBox> _columnHeaderComboBoxCollection;

    private readonly List<TextBlock> _columnHeaderTextBlockCollection;

    private readonly List<GridLength> _columnWidthsCollection;

    private readonly Dictionary<String, CellContent> _gridCellCollection;

    private readonly List<TextBlock> _rowHeaderTextBlockCollection;

    private readonly List<GridLength> _rowHeightsCollection;

    private int _numberOfColumns;

    private int _numberOfRows;

    private string allColumnWidth;

    private string allRowHeight;

    private GridLength defaultColumnWidth;

    private GridLength defaultRowHeight;

    private int definedColumns;

    private int definedRows;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    ///     Initializes a new instance of the CreateBusinessFormWindow class.
    /// </summary>
    /// <param name="ClassEntity">
    ///     The class entity.
    /// </param>
    public CreateBusinessFormWindow(ClassEntity ClassEntity)
    {
        logger.Debug("Entered member.");

        // Here's the call that the form designer forces us to call.
        this.InitializeComponent();

        this.DefaultColumnWidth = new GridLength(0, GridUnitType.Auto);
        this.DefaultRowHeight = new GridLength(0, GridUnitType.Auto);

        this._columnHeaderComboBoxCollection = new List<ComboBox>();
        this._columnHeaderTextBlockCollection = new List<TextBlock>();
        this._columnWidthsCollection = new List<GridLength>();
        this._gridCellCollection = new Dictionary<string, CellContent>();
        this._rowHeaderTextBlockCollection = new List<TextBlock>();
        this._rowHeightsCollection = new List<GridLength>();

        this.BusinessForm = string.Empty;
        CreateBusinessFormWindow.ClassEntity = ClassEntity;
        this.Loaded += this.CreateBusinessFormWindow_Loaded;
    }

    #endregion

    #region Public Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region Public Properties

    /// <summary>
    ///     Gets the class entity.
    /// </summary>
    /// <value>
    ///     The class entity.
    /// </value>
    public static ClassEntity ClassEntity { get; private set; }

    public string AllColumnWidth
    {
        get
        {
            return this.allColumnWidth;
        }
        set
        {
            if (this.allColumnWidth == value)
            {
                return;
            }
            this.allColumnWidth = value;
            this.OnPropertyChanged("AllColumnWidth");
            this.UpdateAllRowsOrColumns(value, false);
        }
    }

    public string AllRowHeight
    {
        get
        {
            return this.allRowHeight;
        }
        set
        {
            if (this.allRowHeight == value)
            {
                return;
            }
            this.allRowHeight = value;
            this.OnPropertyChanged("AllRowHeight");
            this.UpdateAllRowsOrColumns(value, true);
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
    ///     Gets a collection of column header combo boxes.
    /// </summary>
    /// <value>
    ///     A Collection of column header combo boxes.
    /// </value>
    public List<ComboBox> ColumnHeaderComboBoxCollection
    {
        get
        {
            return this._columnHeaderComboBoxCollection;
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
            return this._columnHeaderTextBlockCollection;
        }
    }

    /// <summary>
    ///     Gets or sets the column size POP up.
    /// </summary>
    /// <value>
    ///     The column size POP up.
    /// </value>
    public Popup ColumnSizePopUp { get; set; }

    /*    /// <summary>
        ///     Gets or sets the column size popup timer.
        /// </summary>
        /// <value>
        ///     The column size popup timer.
        /// </value>
        public DispatcherTimer ColumnSizePopupTimer { get; set; }*/

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
            return this._columnWidthsCollection;
        }
    }

    /// <summary>
    ///     Gets or sets the size of the column default.
    /// </summary>
    /// <value>
    ///     The size of the column default.
    /// </value>
    public GridLength DefaultColumnWidth
    {
        get
        {
            return this.defaultColumnWidth;
        }
        set
        {
            if (this.defaultColumnWidth == value)
            {
                return;
            }
            this.defaultColumnWidth = value;
            this.OnPropertyChanged("DefaultColumnWidth");
            if (value.IsAuto)
            {
                this.AllColumnWidth = "Auto";
            }
            else if (value.IsStar)
            {
                this.AllColumnWidth = "*";
            }
            else
            {
                this.AllColumnWidth = value.Value.ToString(CultureInfo.InvariantCulture);
            }
        }
    }

    /// <summary>
    ///     Gets or sets the size of the row default.
    /// </summary>
    /// <value>
    ///     The size of the row default.
    /// </value>
    public GridLength DefaultRowHeight
    {
        get
        {
            return this.defaultRowHeight;
        }
        set
        {
            if (this.defaultRowHeight == value)
            {
                return;
            }
            this.defaultRowHeight = value;
            this.OnPropertyChanged("DefaultRowHeight");
            if (value.IsAuto)
            {
                this.AllRowHeight = "Auto";
            }
            else if (value.IsStar)
            {
                this.AllRowHeight = "*";
            }
            else
            {
                this.AllRowHeight = value.Value.ToString(CultureInfo.InvariantCulture);
            }
        }
    }

    public int DefinedColumns
    {
        get
        {
            return this.definedColumns;
        }
        set
        {
            if (this.definedColumns == value)
            {
                return;
            }
            this.definedColumns = value;
            this.OnPropertyChanged("DefinedColumns");
            this.SetRowOrColumnNumber(value, false);
        }
    }

    public int DefinedRows
    {
        get
        {
            return this.definedRows;
        }
        set
        {
            if (this.definedRows == value)
            {
                return;
            }
            this.definedRows = value;
            this.OnPropertyChanged("DefinedRows");
            this.SetRowOrColumnNumber(value, true);
        }
    }

    /// <summary>
    ///     Gets a collection of CellContent objects.
    /// </summary>
    /// <value>
    ///     A Collection of grid cells.
    /// </value>
    public Dictionary<String, CellContent> GridCellCollection
    {
        get
        {
            return this._gridCellCollection;
        }
    }

    /// <summary>
    ///     Gets or sets the number of columns.
    /// </summary>
    /// <value>
    ///     The total number of columns.
    /// </value>
    public int NumberOfColumns
    {
        get
        {
            return this._numberOfColumns;
        }
        set
        {
            Contract.Requires<ArgumentOutOfRangeException>(value >= 1);
            if (this._numberOfColumns == value)
            {
                return;
            }
            this._numberOfColumns = value;
            this.OnPropertyChanged("NumberOfColumns");
            this.DefinedColumns = value;
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
        get
        {
            return this._numberOfRows;
        }
        set
        {
            Contract.Requires<ArgumentOutOfRangeException>(value >= 1);
            if (this._numberOfRows == value)
            {
                return;
            }
            this._numberOfRows = value;
            this.OnPropertyChanged("NumberOfRows");
            this.DefinedRows = value;
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
            return this._rowHeaderTextBlockCollection;
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
            return this._rowHeightsCollection;
        }
    }

    /// <summary>
    ///     Gets or sets the row size POP up.
    /// </summary>
    /// <value>
    ///     The row size POP up.
    /// </value>
    public Popup RowSizePopUp { get; set; }

    #endregion

    #region Methods

    // + Create Column Header Row
    private void AddColumnHeaders()
    {
        logger.Debug("Entered member.");

        for (int columnIndex = 1;
                columnIndex < this.gridLayout.ColumnDefinitions.Count;
                columnIndex++)
        {
            // Create and add the combo box.
            var comboBox = new ComboBox
            {
                FontSize = 10,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10),
                IsTabStop = false,
                Tag = columnIndex - 1
            };

            // Create a list of names from the enumeration
            string[] names = Enum.GetNames(typeof(ControlType));
            Array.Sort(names);

            // Create the values for the comboBox
            string lastString = string.Empty;

            // Get rid of the None option and add a select option.
            for (int j = 0; j < names.Length; j++)
            {
                if (lastString == string.Empty)
                {
                    lastString = "Select";
                }

                if (names[j] == "None")
                {
                    names[j] = lastString;
                    break;
                }

                string currentString = names[j];
                names[j] = lastString;
                lastString = currentString;
            }

            comboBox.ItemsSource = names;

            comboBox.SelectedValue = "Select";

            comboBox.AddHandler(
                Selector.SelectionChangedEvent,
                new SelectionChangedEventHandler(this.cboColumnHeader_SelectionChanged));
            this.ColumnHeaderComboBoxCollection.Add(comboBox);

            // Create the root element of the editor.
            var stackPanel = new StackPanel();

            // comboBox goes into a StackPanel.
            stackPanel.Children.Add(comboBox);

            // Create and add a text block to show the width of the column.
            var textBlock = new TextBlock
            {
                Tag = columnIndex - 1,
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Center,
                Text =
                this.ParseGridLength(
                    this.ColumnWidthsCollection[columnIndex - 1]),
                ToolTip = "Right click to edit this column's size"
            };

            textBlock.AddHandler(
                MouseRightButtonDownEvent,
                new MouseButtonEventHandler(this.ColumnTextBlock_MouseRightButtonDown));
            this.ColumnHeaderTextBlockCollection.Add(textBlock);

            stackPanel.Children.Add(textBlock);

            // Set the column property of the stack panel.
            stackPanel.SetValue(Grid.ColumnProperty, columnIndex);

            // StackPanel into the grid.
            this.gridLayout.Children.Add(stackPanel);
        }
    }

    // + Create cells for rows 1 and above.
    private void AddRowHeadersAndData()
    {
        logger.Debug("Entered member.");

        // Iterate through the rows (row 0 done above).
        for (int rowsIndex = 1; rowsIndex < this.NumberOfRows + 1; rowsIndex++)
        {
            // Iterate through the columns.
            for (int columnIndex = 0; columnIndex < this.NumberOfColumns + 1;
                    columnIndex++)
            {
                // + Column 0 is a Row Header
                if (columnIndex == 0)
                {
                    // Row size text box.
                    var textBlock = new TextBlock
                    {
                        Tag = rowsIndex - 1,
                        Margin = new Thickness(5),
                        HorizontalAlignment =
                        HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Text =
                        this.ParseGridLength(
                            this.RowHeightsCollection[
                                rowsIndex - 1]),
                        ToolTip =
                        "Right click to edit this row's size"
                    };

                    // Right Mouse Button Down event
                    textBlock.AddHandler(
                        MouseRightButtonDownEvent,
                        new MouseButtonEventHandler(this.RowTextBlock_MouseRightButtonDownEvent));

                    // Set the row property.
                    textBlock.SetValue(Grid.RowProperty, rowsIndex);

                    // Set the column property.
                    textBlock.SetValue(Grid.ColumnProperty, columnIndex);

                    // Add to the RowHeaderTextBlockCollection.
                    this.RowHeaderTextBlockCollection.Add(textBlock);

                    // tb to the Grid.
                    this.gridLayout.Children.Add(textBlock);
                }
                else
                {
                    // Else clause takes care of columns 1 and above.
                    if (!this.GridCellCollection.ContainsKey(
                                this.MakeKey(rowsIndex, columnIndex)))
                    {
                        this.GridCellCollection.Add(
                            this.MakeKey(rowsIndex, columnIndex),
                            new CellContent(rowsIndex, columnIndex));
                    }

                    var gridCellEditor = new GridCellEditor();
                    gridCellEditor.SetValue(Grid.RowProperty, rowsIndex);
                    gridCellEditor.SetValue(Grid.ColumnProperty, columnIndex);

                    // The DataContext of the gridCellEditor is set to the
                    // CellContent object for this row and column. This should
                    // allow the SelectedItem of the combo box to bind to the
                    // ControlType property of the CellContent object.
                    gridCellEditor.DataContext =
                        this.GridCellCollection[this.MakeKey(rowsIndex, columnIndex)];

                    // cell editor into the Grid.
                    this.gridLayout.Children.Add(gridCellEditor);
                }
            }
        }
    }

    private void ColumnTextBlock_MouseRightButtonDown(object sender,
            MouseButtonEventArgs e)
    {
        var textBlock = sender as TextBlock;
        this.ColumnSizePopUp = this.FindResource("columnPopUp") as Popup;

        Popup columnSizePopUp = this.ColumnSizePopUp;
        if (columnSizePopUp == null)
        {
            return;
        }

        if (textBlock != null)
        {
            columnSizePopUp.Tag = textBlock.Tag;
            columnSizePopUp.StaysOpen = true;
            columnSizePopUp.PlacementTarget = textBlock;
        }

        columnSizePopUp.VerticalOffset = -5;
        columnSizePopUp.IsOpen = true;

        /*
            this.ColumnSizePopupTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            this.ColumnSizePopupTimer.Start();
        */
    }

    // ++ Create a Control
    private string ControlFactory(CellContent cellContent)
    {
        logger.Debug("Entered member.");

        var uiPlatform = UIPlatform.WPF;

        if (ClassEntity != null && ClassEntity.IsSilverlight)
        {
            uiPlatform = UIPlatform.Silverlight;
        }

        int columnIndex = cellContent.Column - 1;
        int rowIndex = cellContent.Row - 1;

        if (cellContent.BindingPath.IsNullOrEmpty())
        {
            cellContent.BindingPath = "CHANGEME";
        }

        switch (cellContent.ControlType)
        {
            case ControlType.CheckBox:
                return UIControlFactory.Instance.MakeCheckBox(
                           uiPlatform,
                           columnIndex,
                           rowIndex,
                           cellContent.ControlLabel,
                           cellContent.BindingPath,
                           cellContent.BindingMode);
            case ControlType.ComboBox:
                return UIControlFactory.Instance.MakeComboBox(
                           uiPlatform,
                           columnIndex,
                           rowIndex,
                           cellContent.BindingPath,
                           cellContent.BindingMode);
            case ControlType.Image:
                return UIControlFactory.Instance.MakeImage(
                           uiPlatform,
                           columnIndex,
                           rowIndex,
                           cellContent.BindingPath);
            case ControlType.Label:
                return UIControlFactory.Instance.MakeLabelWithoutBinding(
                           uiPlatform,
                           columnIndex,
                           rowIndex,
                           cellContent.ControlLabel);
            case ControlType.TextBlock:
                return UIControlFactory.Instance.MakeTextBlock(
                           uiPlatform,
                           columnIndex,
                           rowIndex,
                           cellContent.BindingPath,
                           cellContent.StringFormat,
                           ClassEntity == null ? string.Empty : ClassEntity.SilverlightVersion);
            case ControlType.TextBox:
                return UIControlFactory.Instance.MakeTextBox(
                           uiPlatform,
                           columnIndex,
                           rowIndex,
                           cellContent.BindingPath,
                           cellContent.BindingMode,
                           cellContent.Width,
                           cellContent.MaximumLength,
                           cellContent.StringFormat,
                           cellContent.DataType.StartsWith("Nullable"),
                           ClassEntity == null ? string.Empty : ClassEntity.SilverlightVersion);
            case ControlType.DatePicker:
                return UIControlFactory.Instance.MakeDatePicker(
                           uiPlatform,
                           columnIndex,
                           rowIndex,
                           cellContent.BindingPath,
                           cellContent.Width);
            default:

                // No match
                return string.Empty;
        }
    }

    private void CreateBusinessFormWindow_Loaded(object sender,
            RoutedEventArgs e)
    {
        // Create default row and column.
        this.DefinedColumns = 1;
        this.DefinedRows = 1;
    }

    private int GetIntFromKey(string arg)
    {
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(arg));
        logger.Debug("Entered member.");

        string[] rowColumnArray = arg.Split(ROW_COLUMN_KEY_SEPARATOR);
        if (rowColumnArray.Length < 2)
        {
            throw new InvalidOperationException("rolColumnArray split error.");
        }

        // CHECK: Is this still marked by Code Contracts?
        return int.Parse(rowColumnArray[1]);
    }

    private void LayoutGrid()
    {
        logger.Debug("Entered member.");

        this.ResetLayout();

        this.MakeColumnsAndRows();

        this.AddColumnHeaders();

        this.AddRowHeadersAndData();
    }

    private void MakeColumnsAndRows()
    {
        logger.Debug("Entered member.");

        // For each NumberOfColumns add a column definition.
        for (int columnIndex = 0; columnIndex < this.NumberOfColumns + 1;
                columnIndex++)
        {
            this.gridLayout.ColumnDefinitions.Add(
                new ColumnDefinition
            {
                Width = new GridLength(0, GridUnitType.Auto),
                MinWidth = 75
            });
        }

        // for each NumberOfRows add a row definition.
        for (int rowIndex = 0; rowIndex < this.NumberOfRows + 1; rowIndex++)
        {
            this.gridLayout.RowDefinitions.Add(
                new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
        }

        // This adds alternating color to each row
        for (int rowIndex = 0; rowIndex < this.gridLayout.RowDefinitions.Count;
                rowIndex++)
        {
            if (rowIndex % 2 == 0)
            {
                continue;
            }

            var rectangle = new Rectangle
            {
                Fill = new SolidColorBrush(Colors.WhiteSmoke),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            rectangle.SetValue(Grid.RowProperty, rowIndex);
            rectangle.SetValue(Grid.ColumnSpanProperty, this.NumberOfColumns);
            this.gridLayout.Children.Add(rectangle);
        }
    }

    private string MakeKey(int rowIndex, int columnIndex)
    {
        logger.Debug("Entered member.");

        return string.Format("{0}{1}{2}", rowIndex, ROW_COLUMN_KEY_SEPARATOR,
                             columnIndex);
    }

    private void OnPropertyChanged(string propertyName)
    {
        logger.Debug("Entered member.");

        PropertyChangedEventHandler h = this.PropertyChanged;
        if (h == null)
        {
            return;
        }

        h(this, new PropertyChangedEventArgs(propertyName));
    }

    private string ParseGridLength(GridLength gridLength)
    {
        logger.Debug("Entered member.");

        if (gridLength.IsAuto)
        {
            return "Auto";
        }

        return gridLength.IsStar
               ? "Star"
               : gridLength.Value.ToString(CultureInfo.InvariantCulture);
    }

    private void ResetLayout()
    {
        logger.Debug("Entered member.");

        // Remove all previous handlers
        foreach (var comboBox in this.ColumnHeaderComboBoxCollection)
        {
            comboBox.RemoveHandler(
                Selector.SelectionChangedEvent,
                new SelectionChangedEventHandler(this.cboColumnHeader_SelectionChanged));
        }

        this.ColumnHeaderTextBlockCollection.Clear();
        this.RowHeaderTextBlockCollection.Clear();
        this.ColumnHeaderComboBoxCollection.Clear();
        this.gridLayout.Children.Clear();
        this.gridLayout.ColumnDefinitions.Clear();
        this.gridLayout.RowDefinitions.Clear();
    }

    private void RightSizeRowsOrColumns(int count, bool doRows)
    {
        logger.Debug("Entered member.");

        int numberOfItems;
        List<GridLength> itemsCollection;
        GridLength defaultSize;

        if (doRows) // Do Rows
        {
            numberOfItems = this.NumberOfRows;
            itemsCollection = this.RowHeightsCollection;
            defaultSize = this.DefaultRowHeight;
        }
        else // Do Columns
        {
            numberOfItems = this.NumberOfColumns;
            itemsCollection = this.ColumnWidthsCollection;
            defaultSize = this.DefaultColumnWidth;
        }

        // If user removed one or more rows or columns.
        if (count < numberOfItems)
        {
            List<string> removeList =
                this.GridCellCollection.Keys.Where(s => this.GetIntFromKey(s) > count)
                .ToList();
            foreach (var s in removeList)
            {
                this.GridCellCollection.Remove(s);
            }

            itemsCollection.RemoveRange(count, numberOfItems - count);
        }

        if (doRows)
        {
            this.NumberOfRows = count;
        }
        else
        {
            this.NumberOfColumns = count;
        }

        while (itemsCollection.Count < count)
        {
            itemsCollection.Add(defaultSize);
        }

        this.LayoutGrid();
    }

    private void RowTextBlock_MouseRightButtonDownEvent(object sender,
            MouseButtonEventArgs e)
    {
        var tb = sender as TextBlock;
        this.RowSizePopUp = this.FindResource("rowPopUp") as Popup;

        Popup rowSizePopUp = this.RowSizePopUp;
        if (rowSizePopUp != null)
        {
            if (tb != null)
            {
                rowSizePopUp.Tag = tb.Tag;
                rowSizePopUp.StaysOpen = true;
                rowSizePopUp.PlacementTarget = tb;
            }
            rowSizePopUp.VerticalOffset = -5;
            rowSizePopUp.IsOpen = true;
        }

        /*        this.RowSizePopupTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            this.RowSizePopupTimer.Start();*/
    }

    private void SetAllColumnWidths(int length, GridUnitType defaultType)
    {
        logger.Debug("Entered member.");

        this.DefaultColumnWidth = new GridLength(length, defaultType);

        for (int i = 0; i < this.gridLayout.ColumnDefinitions.Count; i++)
        {
            this.ColumnWidthsCollection[i] = this.DefaultColumnWidth;
            this.ColumnHeaderTextBlockCollection[i].Text =
                this.ParseGridLength(this.DefaultColumnWidth);
        }
    }

    private void SetAllRowsHeights(int height, GridUnitType defaultType)
    {
        logger.Debug("Entered member.");

        this.DefaultRowHeight = new GridLength(height, defaultType);

        for (int i = 0; i < this.gridLayout.RowDefinitions.Count; i++)
        {
            this.RowHeightsCollection[i] = this.DefaultRowHeight;
            this.RowHeaderTextBlockCollection[i].Text =
                this.ParseGridLength(this.DefaultRowHeight);
        }
    }

    private void SetRowOrColumnNumber(int count, bool doRows)
    {
        Contract.Assume(count > 0 && count <= 50);
        logger.Debug("Entered member.");

        if (doRows && this.NumberOfColumns == 0)
        {
            this.RightSizeRowsOrColumns(1, false);
        }

        this.RightSizeRowsOrColumns(count, doRows);
    }

    private void SetWidthOrHeight(TextBox txt, bool doRow)
    {
        logger.Debug("Entered member.");

        if (txt.Text.IsNullOrWhiteSpace())
        {
            logger.Debug("Text is null or white space.");
            return;
        }
        logger.Trace("txt.Text: {0}", txt.Text);

        int size;
        if (!(int.TryParse(txt.Text, out size) && size >= 0))
        {
            throw new InvalidOperationException("size is invalid.");
        }
        logger.Trace("size: {0}", size);

        // if the textBox's Tag is not set to a number
        // This branch occurs once for each new window.
        if (txt.Tag == null)
        {
            logger.Debug("txt.Tag is null");
            // iterate once for each row.
            for (int i = 0; i < this.NumberOfRows; i++)
            {
                // set the tag to the current index.
                txt.Tag = i;
                // then set the dimension of the textBox according to the value of doRow.
                // Note: This call is recursive.
                // The recursion is stopped in the next iteration since in it Tag will not be null.
                this.SetWidthOrHeight(txt, doRow);
            }

            // Reset the Tag back to null.
            txt.Tag = null;
            return;
        }

        var index = (int)txt.Tag;
        if (index < 0)
        {
            throw new InvalidOperationException("txt.Tag content is < 0");
        }

        if (doRow && index >= this.NumberOfRows)
        {
            throw new InvalidOperationException("txt.Tag content is >= NumberOfRows");
        }

        if (!doRow && index >= this.NumberOfColumns)
        {
            throw new InvalidOperationException("txt.Tag content is >= NumberOfColumns");
        }

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

        TextBlockCollection[index].Text = size.ToString(
                                              CultureInfo.InvariantCulture);
        GridLengthCollection[index] = new GridLength(size);

        //txt.Text = string.Empty;
    }

    private void UpdateAllRowsOrColumns(string width, bool doRows)
    {
        List<GridLength> gridLengthCollection;
        List<TextBlock> textBlockCollection;
        int definitionCount;

        if (doRows)
        {
            gridLengthCollection = this.RowHeightsCollection;
            textBlockCollection = this.RowHeaderTextBlockCollection;
            definitionCount = this.gridLayout.RowDefinitions.Count;
        }
        else
        {
            gridLengthCollection = this.ColumnWidthsCollection;
            textBlockCollection = this.ColumnHeaderTextBlockCollection;
            definitionCount = this.gridLayout.ColumnDefinitions.Count;
        }

        double length = 0.0;
        var unitType = GridUnitType.Pixel;

        if (width.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
        {
            unitType = GridUnitType.Auto;
        }
        else if (width.Equals("*", StringComparison.InvariantCulture))
        {
            unitType = GridUnitType.Star;
        }
        else
        {
            length = double.Parse(width);
        }

        var defaultSize = new GridLength(length, unitType);

        if (doRows)
        {
            this.DefaultRowHeight = defaultSize;
        }
        else
        {
            this.DefaultColumnWidth = defaultSize;
        }

        for (int index = 0; index < definitionCount; index++)
        {
            gridLengthCollection[index] = defaultSize;
            textBlockCollection[index].Text = this.ParseGridLength(defaultSize);
        }
    }

    private void btnAllColumnsAuto_Click(object sender, RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        this.SetAllColumnWidths(0, GridUnitType.Auto);
    }

    private void btnAllColumnsStar_Click(object sender, RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        this.SetAllColumnWidths(1, GridUnitType.Star);
    }

    private void btnAllRowsAuto_Click(object sender, RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        this.SetAllRowsHeights(0, GridUnitType.Auto);
    }

    private void btnAllRowsStar_Click(object sender, RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        this.SetAllRowsHeights(1, GridUnitType.Star);
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        this.DialogResult = false;
    }

    // ++ Create the form
    private void btnCreate_Click(object sender, RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        var sb = new StringBuilder(10240);
        sb.AppendLine("<Grid>");

        // + RowDefinitions
        if(this.RowHeightsCollection.Count > 1)
        {
            sb.AppendLine("\t<Grid.RowDefinitions>");

            foreach (string height in this.RowHeightsCollection.Select(
                         obj => obj.IsStar
                         ? "*"
                         : obj.IsAuto
                         ? "Auto"
                         : obj.Value.ToString(CultureInfo.InvariantCulture)))
            {
                sb.AppendFormat("\t\t<RowDefinition Height=\"{0}\" />\r\n", height);
            }

            sb.AppendLine("\t</Grid.RowDefinitions>");
        }

        // + ColumnDefinitions
        if(this.ColumnWidthsCollection.Count > 1)
        {
            sb.AppendLine("\t<Grid.ColumnDefinitions>");

            foreach (string width in this.ColumnWidthsCollection.Select(
                         obj => obj.IsStar
                         ? "*"
                         : obj.IsAuto
                         ? "Auto"
                         : obj.Value.ToString(CultureInfo.InvariantCulture)))
            {
                sb.AppendFormat("\t\t<ColumnDefinition Width=\"{0}\" />\r\n", width);
            }

            sb.AppendLine("\t</Grid.ColumnDefinitions>\r\n");
        }

        // + Controls
        for (var columnIndex = 1;
                columnIndex < this.gridLayout.ColumnDefinitions.Count;
                columnIndex++)
        {
            bool addNewLine = false;
            for (int rowIndex = 1; rowIndex < this.gridLayout.RowDefinitions.Count;
                    rowIndex++)
            {
                CellContent value;
                var key = this.MakeKey(rowIndex, columnIndex);
                if (!this.GridCellCollection.TryGetValue(key, out value)
                        || value.ControlType == ControlType.None)
                {
                    continue;
                }

                addNewLine = true;

                if (ClassEntity != null && ClassEntity.IsSilverlight
                        && value.StringFormat.IsNotNullOrEmpty())
                {
                    sb.AppendFormat(
                        "\t<!-- TODO - Add formatting converter for format: {0} -->",
                        value.StringFormat);
                }

                sb.AppendLine(this.ControlFactory(value));
            }

            if (addNewLine)
            {
                sb.AppendLine();
            }
        }

        sb.Replace(" >", ">");
        sb.Replace("    ", " ");
        sb.Replace("   ", " ");
        sb.Replace("  ", " ");
        sb.AppendLine("</Grid>");
        this.BusinessForm = sb.ToString();

        if (this.BusinessForm.IndexOf("CHANGEME", StringComparison.Ordinal) > -1)
        {
            this.BusinessForm =
                string.Concat(
                    "\r\n\t<!-- Search for and change all instances of CHANGEME -->\r\n",
                    this.BusinessForm);
        }

        this.DialogResult = true;
    }

    // + Event handler for clicking on the Auto button of COLUMN pup up.
    private void btnPopupColumnAutoSize_Click(object sender,
            RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        var columnIndex = (int)this.ColumnSizePopUp.Tag;
        logger.Trace("columnIndex: {0}", columnIndex);

        Contract.Assume(columnIndex >= 0);
        this.ColumnWidthsCollection[columnIndex] = new GridLength(0,
                GridUnitType.Auto);
        this.ColumnHeaderTextBlockCollection[columnIndex].Text =
            this.ParseGridLength(this.ColumnWidthsCollection[columnIndex]);
        this.ColumnSizePopUp.IsOpen = false;
    }

    // + Event handler for clicking on the Star button of COLUMN pop up.
    private void btnPopupColumnStarSize_Click(object sender,
            RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        var columnIndex = (int)this.ColumnSizePopUp.Tag;
        logger.Trace("columnIndex: {0}", columnIndex);

        Contract.Assume(columnIndex >= 0);
        this.ColumnWidthsCollection[columnIndex] = new GridLength(1,
                GridUnitType.Star);
        this.ColumnHeaderTextBlockCollection[columnIndex].Text =
            this.ParseGridLength(this.ColumnWidthsCollection[columnIndex]);
        this.ColumnSizePopUp.IsOpen = false;
    }

    // + Event handler for clicking on the Auto button of ROW pop up
    private void btnPopupRowAutoSize_Click(object sender, RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        var rowIndex = (int)this.RowSizePopUp.Tag;
        logger.Trace("rowIndex: {0}", rowIndex);

        Contract.Assume(rowIndex >= 0);
        this.RowHeightsCollection[rowIndex] = new GridLength(0,
                GridUnitType.Auto);
        this.RowHeaderTextBlockCollection[rowIndex].Text =
            this.ParseGridLength(this.RowHeightsCollection[rowIndex]);
        this.RowSizePopUp.IsOpen = false;
    }

    // + Event handler for clicking on the Star button of ROW pop up
    private void btnPopupRowStarSize_Click(object sender, RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        var rowIndex = (int)this.RowSizePopUp.Tag;
        logger.Trace("rowIndex: {0}", rowIndex);

        Contract.Assume(rowIndex >= 0);
        this.RowHeightsCollection[rowIndex] = new GridLength(1,
                GridUnitType.Star);
        this.RowHeaderTextBlockCollection[rowIndex].Text =
            this.ParseGridLength(this.RowHeightsCollection[rowIndex]);
        this.RowSizePopUp.IsOpen = false;
    }

    private void cboColumnHeader_SelectionChanged(object sender,
            SelectionChangedEventArgs e)
    {
        logger.Debug("Entered member.");

        var comboBox = sender as ComboBox;
        if (comboBox == null)
        {
            logger.Error("Sender is not a ComboBox.");
            throw new ArgumentException("sender is not a ComboBox.");
        }

        var columnIndex = (int)comboBox.Tag;

        if (comboBox.SelectedValue.ToString() == "Select")
        {
            return;
        }

        var controlType =
            (ControlType)Enum.Parse(typeof(ControlType),
                                    comboBox.SelectedValue.ToString());

        for (int rowIndex = 0; rowIndex < this.NumberOfRows; rowIndex++)
        {
            this.GridCellCollection[this.MakeKey(rowIndex + 1,
                                                 columnIndex + 1)].ControlType =
                                                         controlType;
        }

        this.LayoutGrid();
    }

    private void txtPopupColumnWidth_KeyPress(object sender, KeyEventArgs e)
    {
        logger.Debug("Entered member.");

        if (e.Key != Key.Enter)
        {
            return;
        }

        this.SetWidthOrHeight(sender as TextBox, false);
        this.ColumnSizePopUp.IsOpen = false;
    }

    private void txtPopupRowHeight_KeyPress(object sender, KeyEventArgs e)
    {
        logger.Debug("Entered member.");

        if (e.Key != Key.Enter)
        {
            return;
        }
        e.Handled = true;
        this.SetWidthOrHeight(sender as TextBox, true);
        this.RowSizePopUp.IsOpen = false;
    }

    #endregion
}
}