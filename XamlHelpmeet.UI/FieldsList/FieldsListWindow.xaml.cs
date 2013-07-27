namespace XamlHelpmeet.UI
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using XamlHelpmeet.Model;
    using XamlHelpmeet.UI.UIControlFactory;

    #endregion

    /// <summary>
    /// Interaction logic for FieldsListWindow.xaml.
    /// </summary>
    /// <seealso cref="T:System.Windows.Window"/>
    public partial class FieldsListWindow
    {
        #region Fields

        /// <summary>
        /// The class entity.
        /// </summary>
        private readonly ClassEntity _classEntity;

        /// <summary>
        /// The data object.
        /// </summary>
        private DataObject _dataObject;

        /// <summary>
        /// Height of the save.
        /// </summary>
        private double _saveHeight;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FieldsListWindow class.
        /// </summary>
        /// <param name="ClassEntity">
        /// The class entity.
        /// </param>
        public FieldsListWindow(ClassEntity ClassEntity)
        {
            this.InitializeComponent();

            // Add other initialization code here
            this._classEntity = ClassEntity;
        }

        /// <summary>
        /// Initializes a new instance of the FieldsListWindow class.
        /// </summary>
        public FieldsListWindow() { this.InitializeComponent(); }

        #endregion

        #region Methods (private)

        /// <summary>
        /// Gets controls for field.
        /// </summary>
        /// <exception cref="InvalidProgramException">
        /// Thrown when an Invalid Program error condition occurs.
        /// </exception>
        /// <param name="pi">
        /// The pi.
        /// </param>
        /// <returns>
        /// The controls for field.
        /// </returns>
        private string GetControlsForField(PropertyInformation pi)
        {
            var uiPlatform = UIPlatform.WPF;

            if (this._classEntity.IsSilverlight)
            {
                uiPlatform = UIPlatform.Silverlight;
            }

            int? columnIndex = null;
            int? rowIndex = null;

            if (pi.FieldListIncludeGridAttachedProperties)
            {
                columnIndex = 0;
                rowIndex = 0;
            }

            // Initialize the return value.
            string resultString = string.Empty;

            if ((this.rdoLabelAndControl.IsChecked ?? false) || (this.rdoLabelOnly.IsChecked ?? false))
            {
                resultString = UIControlFactory.UIControlFactory.Instance.MakeLabelWithoutBinding(uiPlatform,
                                                                                                  columnIndex,
                                                                                                  rowIndex,
                                                                                                  pi.LabelText);
                if (this.rdoLabelAndControl.IsChecked ?? false)
                {
                    resultString += Environment.NewLine;
                }
            }

            if ((!(this.rdoLabelAndControl.IsChecked ?? false)) && (!(this.rdoControlOnly.IsChecked ?? false)))
            {
                return resultString;
            }

            switch (pi.FieldListControlType)
            {
                case ControlType.CheckBox:
                    return string.Concat(resultString,
                                         UIControlFactory.UIControlFactory.Instance.MakeCheckBox(uiPlatform,
                                                                                                 columnIndex,
                                                                                                 rowIndex,
                                                                                                 string.Empty,
                                                                                                 pi.Name,
                                                                                                 BindingMode.TwoWay));
                case ControlType.ComboBox:
                    resultString = string.Concat(resultString,
                                                 UIControlFactory.UIControlFactory.Instance.MakeComboBox(uiPlatform,
                                                                                                         columnIndex,
                                                                                                         rowIndex,
                                                                                                         pi.Name,
                                                                                                         BindingMode
                                                                                                             .TwoWay));
                    return this._classEntity.IsSilverlight
                               ? string.Concat(
                                               "<!-- Bind Silverlight ComboBox in code after its ItemsSource has been loaded. -->\r\n",
                                               resultString)
                               : resultString;
                case ControlType.Image:
                    return string.Concat(resultString,
                                         UIControlFactory.UIControlFactory.Instance.MakeImage(uiPlatform,
                                                                                              columnIndex,
                                                                                              rowIndex,
                                                                                              pi.Name));
                case ControlType.Label:
                    return string.Concat(resultString,
                                         UIControlFactory.UIControlFactory.Instance.MakeLabel(uiPlatform,
                                                                                              columnIndex,
                                                                                              rowIndex,
                                                                                              pi.Name,
                                                                                              pi.StringFormat,
                                                                                              this._classEntity
                                                                                                  .SilverlightVersion));
                case ControlType.TextBlock:
                    return string.Concat(resultString,
                                         UIControlFactory.UIControlFactory.Instance.MakeTextBlock(uiPlatform,
                                                                                                  columnIndex,
                                                                                                  rowIndex,
                                                                                                  pi.Name,
                                                                                                  pi.StringFormat,
                                                                                                  this._classEntity
                                                                                                      .SilverlightVersion));
                case ControlType.TextBox:
                    return string.Concat(resultString,
                                         UIControlFactory.UIControlFactory.Instance.MakeTextBox(uiPlatform,
                                                                                                columnIndex,
                                                                                                rowIndex,
                                                                                                pi.Name,
                                                                                                BindingMode.TwoWay,
                                                                                                null,
                                                                                                null,
                                                                                                pi.StringFormat,
                                                                                                pi.TypeName.StartsWith(
                                                                                                                       "Nullable"),
                                                                                                this._classEntity
                                                                                                    .SilverlightVersion));
                case ControlType.DatePicker:
                    return string.Concat(resultString,
                                         UIControlFactory.UIControlFactory.Instance.MakeDatePicker(uiPlatform,
                                                                                                   columnIndex,
                                                                                                   rowIndex,
                                                                                                   pi.Name,
                                                                                                   null));
                default:
                    throw new InvalidProgramException("Sorry, but the program does not know the current value of pi.FieldListControlType.");
            }
        }

        // TODO: Why doesn't the combo box get set??

        /// <summary>
        /// Watchs the MouseDown events and initializes the data object for a drag
        /// and drop operation.
        /// </summary>
        /// <param name="sender">
        /// Source of the event.
        /// </param>
        /// <param name="e">
        /// Mouse button event information.
        /// </param>
        private void TextBlockDrag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this._dataObject = null;
            }
        }

        /// <summary>
        /// While the mouse is pressed down watches the MouseMove event to handle
        /// dragging effects. Stops if the mouse is released, and will not run if the
        /// data object is null.
        /// </summary>
        /// <param name="sender">
        /// Source of the event.
        /// </param>
        /// <param name="e">
        /// Mouse button event information.
        /// </param>
        private void TextBlockDrag_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || this._dataObject != null)
            {
                return;
            }

            var tb = sender as TextBlock;
            Debug.Assert(tb != null, "tb != null");
            this._dataObject = new DataObject(DataFormats.Text,
                                              this.GetControlsForField(tb.DataContext as PropertyInformation));
            DragDrop.DoDragDrop(tb, this._dataObject, DragDropEffects.Copy);
        }

        /// <summary>
        /// Event handler. Called by btnCollapseExpand for click events.
        /// </summary>
        /// <param name="sender">
        /// Source of the event.
        /// </param>
        /// <param name="e">
        /// Mouse button event information.
        /// </param>
        // ReSharper disable once IdentifierTypo
        private void btnCollapseExpand_click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            Debug.Assert(btn != null, "btn != null");
            if (btn.Content.ToString() == "Collapse")
            {
                this._saveHeight = this.Height;
                this.Height = 97;
                btn.Content = "Expand";
            }
            else
            {
                this.Height = this._saveHeight;
                btn.Content = "Collapse";
            }
        }

        /// <summary>
        /// Event handler. Called by cboControlType for loaded events.
        /// </summary>
        /// <param name="sender">
        /// Source of the event.
        /// </param>
        /// <param name="e">
        /// Mouse button event information.
        /// </param>
        private void cboControlType_Loaded(object sender, RoutedEventArgs e)
        {
            var cbo = sender as ComboBox;

            Debug.Assert(cbo != null, "cbo != null");
            cbo.ItemsSource = (from d in Enum.GetValues(typeof(ControlType)).OfType<ControlType>()
                               where d.ToString() != "None"
                               orderby d.ToString()
                               select d.ToString()).ToArray<string>();

            // In the C# implementation of these to Loaded event handlers
            // the FieldListControlType was not being set right when initialized.
            // Window_Loaded ran first. It had a loop with the IF construct below
            // to set the value, but because the cbo was created later, the cbo
            // did not have any membmers, so the selected item was set to -1.
            // Once the query ran, the selected item became the first item,
            // and this set the FieldListControlType of the bound item to be
            // a checkbox no matter what type it was.
            
            var item = cbo.DataContext as PropertyInformation;
            Debug.Assert(item != null, "item != null");
            if (item.TypeName.Contains("Boolean"))
            {
                cbo.SelectedValue = "CheckBox";
                // item.FieldListControlType = ControlType.CheckBox;
            }
            else if (item.TypeName.Contains("Date"))
            {
                cbo.SelectedValue = "DatePicker";
                // item.FieldListControlType = ControlType.DatePicker;
            }
            else
            {
                cbo.SelectedValue = "TextBox";
                // item.FieldListControlType = ControlType.TextBox;
            }
        }



        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = string.Format("Fields List for Class: {0}", this._classEntity.ClassName);
            this.lbFields.ItemsSource = _classEntity.PropertyInformation;
        }

    }
}
