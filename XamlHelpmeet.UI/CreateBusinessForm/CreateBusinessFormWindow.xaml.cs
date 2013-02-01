﻿using System;
using System.Collections.Generic;
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
    /// Interaction logic for CreateBusinessFormWindow.xaml
    /// </summary>
    public partial class CreateBusinessFormWindow : Window
    {
        #region Constants

        private const int COLUMNOFFSET = 1;
        private const char ROW_COLUMN_KEY_SEPARATOR = ':';
        private const int ROWOFFSET = 1;

        #endregion Constants

        #region Fields

        private readonly List<ComboBox> _columnHeaderComboBoxCollection;
        private readonly List<TextBlock> _columnHeaderTextBlockCollection;
        private readonly List<GridLength> _columnWidthsCollection;
        private readonly Dictionary<String, CellContent> _gridCellCollection;
        private readonly List<TextBlock> _rowHeaderTextBlockCollection;
        private readonly List<GridLength> _rowHeightsCollection;

        #endregion Fields

        #region Constructors

        public CreateBusinessFormWindow()
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
            RightSizeRowsOrColumns(2, false);
            RightSizeRowsOrColumns(4, true);
        }

        public CreateBusinessFormWindow(ClassEntity ClassEntity)
            : this()
        {
            CreateBusinessFormWindow.ClassEntity = ClassEntity;
        }

        #endregion Constructors

        #region Properties

        public static ClassEntity ClassEntity
        {
            get;
            private set;
        }

        public string BusinessForm
        {
            get;
            private set;
        }

        public GridLength ColumnDefaultSize
        {
            get;
            set;
        }

        public List<ComboBox> ColumnHeaderComboBoxCollection
        {
            get
            {
                return _columnHeaderComboBoxCollection;
            }
        }

        public List<TextBlock> ColumnHeaderTextBlockCollection
        {
            get
            {
                return _columnHeaderTextBlockCollection;
            }
        }

        public Popup ColumnSizePopUp
        {
            get;
            set;
        }

        public DispatcherTimer ColumnSizePopupTimer
        {
            get;
            set;
        }

        public List<GridLength> ColumnWidthsCollection
        {
            get
            {
                return _columnWidthsCollection;
            }
        }

        public Dictionary<String, CellContent> GridCellCollection
        {
            get
            {
                return _gridCellCollection;
            }
        }

        public int NumberOfColumns
        {
            get;
            set;
        }

        public int NumberOfRows
        {
            get;
            set;
        }

        public GridLength RowDefaultSize
        {
            get;
            set;
        }

        public List<TextBlock> RowHeaderTextBlockCollection
        {
            get
            {
                return _rowHeaderTextBlockCollection;
            }
        }

        public List<GridLength> RowHeightsCollection
        {
            get
            {
                return _rowHeightsCollection;
            }
        }

        public Popup RowSizePopUp
        {
            get;
            set;
        }

        public DispatcherTimer RowSizePopupTimer
        {
            get;
            set;
        }

        #endregion Properties

        #region Popup Methods

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

        private void ColumnTextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBlock;
            ColumnSizePopUp = FindResource("columnSizePopUp") as Popup;

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

        private void SetColumnWidth(TextBox txt)
        {
            var columnIndex = int.Parse(ColumnSizePopUp.Tag.ToString());
            int width;

            if (int.TryParse(txt.Text, out width) && width >= 0)
            {
                ColumnHeaderTextBlockCollection[columnIndex].Text = width.ToString();
                ColumnWidthsCollection[columnIndex] = new GridLength(width);
                txt.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("The column width must be an integer greater than or equal to zero, please reenter.", "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void SetRowHeight(TextBox txt)
        {
            var rowIndex = int.Parse(RowSizePopUp.Tag.ToString());
            int height;

            if (int.TryParse(txt.Text, out height) && height >= 0)
            {
                RowHeaderTextBlockCollection[rowIndex].Text = height.ToString();
                RowHeightsCollection[rowIndex] = new GridLength(height);
                txt.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("The row height must be an integer greater than or equal to zero, please reenter.", "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void txtPopupColumnWidth_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            SetColumnWidth(sender as TextBox);
        }

        private void txtPopupRowHeight_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            var txt = sender as TextBox;
            SetRowHeight(txt);
        }

        #endregion Popup Methods

        #region Methods

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

            //var skipFirst = true;	// looks like a kludge

            foreach (var obj in RowHeightsCollection)
            {
                //if (skipFirst)		// This looks like a kludge to force one-basedness
                //{						// in the collection. If that's what it is, we
                //	skipFirst = false;	// rather use the whole collection, and use
                //}						// zero-basedness, since that is how C# is
                //else					// designed to be used.
                //{
                var height = obj.IsStar ? "*" : obj.IsAuto ? "Auto" : obj.Value.ToString();
                sb.AppendFormat("\t\t<RowDefinition Height=\"{0}\" />\r\n", height);

                //}
            }

            sb.AppendLine("\t</Grid.RowDefinitions>");
            sb.AppendLine("\t<Grid.ColumnDefinitions>");

            //skipFirst = true;

            foreach (var obj in ColumnWidthsCollection)
            {
                //if (skipFirst)
                //{
                //	skipFirst = false;
                //}
                //else
                //{
                var width = obj.IsStar ? "*" : obj.IsAuto ? "Auto" : obj.Value.ToString();
                sb.AppendFormat("\t\t<ColumnDefinition Width=\"{0}\" />\r\n", width);

                //}
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

        private string ControlFactory(CellContent obj)
        {
            var uiPlatform = UIPlatform.WPF;

            if (ClassEntity != null && ClassEntity.IsSilverlight)
            {
                uiPlatform = UIPlatform.Silverlight;
            }

            // CHECK: See if the offset can be purged
            var columnIndex = obj.Column - COLUMNOFFSET;	// Here is more of the column kludge
            var rowIndex = obj.Row - ROWOFFSET;

            if (obj.BindingPath.IsNullOrEmpty())
            {
                obj.BindingPath = "CHANGEME";
            }

            switch (obj.ControlType)
            {
                case ControlType.CheckBox:
                    return UIControlFactory.UIControlFactory.Instance
                        .MakeDatePicker(uiPlatform, columnIndex, rowIndex,
                        obj.BindingPath, obj.Width);
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
                        obj.BindingPath, obj.StringFormat, ClassEntity.SilverlightVersion);
                case ControlType.TextBox:
                    return UIControlFactory.UIControlFactory.Instance
                        .MakeTextBox(uiPlatform, columnIndex, rowIndex, obj.BindingPath,
                        obj.BindingMode, obj.Width, obj.MaximumLength, obj.StringFormat,
                        obj.DataType.StartsWith("Nullable"),
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
            // NOTE: This method is buggy and needs review.

            // Remove all previous handlers
            foreach (var cbo in ColumnHeaderComboBoxCollection)
            {
                cbo.RemoveHandler(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(cboColumnHeader_SelectionChanged));
            }

            ColumnHeaderTextBlockCollection.Clear();

            // ColumnHeaderTextBlockCollection.Add(new TextBlock());	// This must be a dummy
            RowHeaderTextBlockCollection.Clear();

            // RowHeaderTextBlockCollection.Add(new TextBlock());	// Ditto
            ColumnHeaderComboBoxCollection.Clear();
            gridLayout.Children.Clear();
            gridLayout.ColumnDefinitions.Clear();
            gridLayout.RowDefinitions.Clear();

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

            // Starting in column 1 because 0 row and column are used for size configuration
            for (int i = 1; i < gridLayout.ColumnDefinitions.Count; i++)
            {
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

                // Populate the combobox
                foreach (var s in ary)
                {
                    cbo.Items.Add(s == "None" ? (object)"Select" : (object)s);
                }

                // Needs to follow above loop to have something to select
                cbo.SelectedValue = "Select";

                cbo.AddHandler(ComboBox.SelectionChangedEvent,
                    new SelectionChangedEventHandler(cboColumnHeader_SelectionChanged));
                ColumnHeaderComboBoxCollection.Add(cbo);

                var sp = new StackPanel();

                // cbo goes into a StackPanel.
                sp.Children.Add(cbo);

                var tb = new TextBlock()
                {
                    Tag = i,
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

            // Iterate through the rows (row 0 done above).
            for (int rowsIndex = 1; rowsIndex < NumberOfRows; rowsIndex++)
            {
                // Iterate through the columns.
                for (int columnIndex = 0; columnIndex < NumberOfColumns; columnIndex++)
                {
                    // If the first column:
                    if (columnIndex == 0)
                    {
                        // Row size text box.
                        var tb = new TextBlock()
                        {
                            Tag = rowsIndex,
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
                        gridCellEditor.DataContext = GridCellCollection[MakeKey(rowsIndex, columnIndex)];

                        // cell editor into the Grid.
                        gridLayout.Children.Add(gridCellEditor);
                    }
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

        private bool SetRowOrColumnNumber(TextBox tb)
        {
            var index = -1;
            if (int.TryParse(tb.Text, out index) == false || index < 1 || index > 50)
            {
                MessageBox.Show("The number of rows or columns must be equal to or less than 50. Please reenter.",
                    "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            bool? DoRows = tb.Name.Contains("Rows") ? true :
                tb.Name.Contains("Columns") ? false : (bool?)null;

            if (DoRows == null)
            {
                return false;
            }

            return RightSizeRowsOrColumns(index, (bool)DoRows);
        }

        private void txtAllColumnsWidth_KeyPress(object sender,
            KeyEventArgs e)
        {
            UpdateAllRowsOrColumns(sender, e.Key, false);
        }

        private void txtAllRowsHeight_KeyPress(object sender,
            KeyEventArgs e)
        {
            UpdateAllRowsOrColumns(sender, e.Key, true);
        }

        // This method handles only the Enter key.

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

        private void UpdateAllRowsOrColumns(object sender, Key Key, bool RowUpdate)
        {
            if (Key != Key.Enter)
            {
                return;
            }

            List<GridLength> gridLengthCollection;
            List<TextBlock> textBlockCollection;
            int definitionCount;

            if (RowUpdate)
            {
                gridLengthCollection = RowHeightsCollection;
                textBlockCollection = RowHeaderTextBlockCollection;
                definitionCount = gridLayout.RowDefinitions.Count;
            }
            else
            {
                gridLengthCollection = ColumnWidthsCollection;
                textBlockCollection = ColumnHeaderTextBlockCollection;
                definitionCount = gridLayout.ColumnDefinitions.Count;
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

        #endregion Methods

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (e.Handled)
                return;
            e.Handled = true;
            if (textBox == txtColumnSize)
            {
                SetColumnWidth(textBox);
                return;
            }
            if (textBox == txtRowSize)
            {
                SetRowHeight(textBox);
                return;
            }
            if (textBox == txtNumberOfRows || textBox == txtNumberOfColumns)
            {
                SetRowOrColumnNumber(textBox);
                return;
            }
            e.Handled = false;
        }
    }
}