namespace XamlHelpmeet.UI.Editors
{
    #region Imports

    using System;
    using System.Windows;
    using System.Windows.Controls;
    using XamlHelpmeet.UI.DynamicForm;
    using XamlHelpmeet.UI.Enums;
    using XamlHelpmeet.UI.Utilities;

    #endregion

    /// <summary>
    ///     Interaction logic for DynamicFormEditor.xaml.
    /// </summary>
    public partial class DynamicFormEditor : UserControl
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the DynamicFormEditor class.
        /// </summary>
        public DynamicFormEditor()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods (private)

        /// <summary>
        ///     Event handler. Called by DynamicFormEditor for loaded events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void DynamicFormEditor_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetSilverlightDataFormFieldsVisibility();
        }

        /// <summary>
        ///     Sets render in data column template visibility.
        /// </summary>
        private void SetRenderInDataColumnTemplateVisibility()
        {
            var obj = UIHelpers.FindAnscestorWindow(this) as CreateBusinessFormFromClassWindow;
            if (obj == null)
            {
                return;
            }

            if (!obj.ClassEntity.IsSilverlight
                || obj.cboSelectObjectToCreate.SelectedValue.ToString() != "Silverlight Data Grid")
            {
                return;
            }
            this.chkRenderInDataColumnTemplate.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Sets silverlight data form fields visibility.
        /// </summary>
        private void SetSilverlightDataFormFieldsVisibility()
        {
            this.gridSilverlightDataFormFields.Visibility = Visibility.Collapsed;

            var obj = UIHelpers.FindAnscestorWindow(this) as CreateBusinessFormFromClassWindow;

            if (obj == null || !obj.ClassEntity.IsSilverlight
                || obj.cboSelectObjectToCreate.SelectedValue.ToString() != "Silverlight Data Form")
            {
                return;
            }

            this.gridSilverlightDataFormFields.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Event handler. Called by cboControlType for loaded events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void cboControlType_Loaded(object sender, RoutedEventArgs e)
        {
            var cbo = sender as ComboBox;
            string[] ary = Enum.GetNames(typeof(DynamicFormControlType));
            Array.Sort(ary);
            cbo.ItemsSource = ary;
            this.SetRenderInDataColumnTemplateVisibility();
        }

        /// <summary>
        ///     Event handler. Called by cboControlType for selection changed events.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void cboControlType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.gridDynamicFormControlEditor == null)
            {
                // this happens on load because we are wired up in XAML. 
                return;
            }
            if (this.gridDynamicFormControlEditor.Children != null)
            {
                this.gridDynamicFormControlEditor.Children.Clear();
            }
            bool isSilverlight = this.chkRenderInDataColumnTemplate.Visibility == Visibility.Visible;
            this.chkRenderInDataColumnTemplate.IsEnabled = true;
            var controlType =
                (DynamicFormControlType)
                Enum.Parse(typeof(DynamicFormControlType), (sender as ComboBox).SelectedValue.ToString());
            switch (controlType)
            {
                case DynamicFormControlType.CheckBox:
                    this.gridDynamicFormControlEditor.Children.Add(new DynamicFormCheckBoxEditor());
                    break;
                case DynamicFormControlType.ComboBox:
                    this.gridDynamicFormControlEditor.Children.Add(new DynamicFormComboBoxEditor());

                    if (isSilverlight)
                    {
                        this.chkRenderInDataColumnTemplate.IsChecked = true;
                        this.chkRenderInDataColumnTemplate.IsEnabled = false;
                    }
                    break;
                case DynamicFormControlType.Image:
                    this.gridDynamicFormControlEditor.Children.Add(new DynamicFormTextBlockEditor());
                    if (isSilverlight)
                    {
                        this.chkRenderInDataColumnTemplate.IsChecked = true;
                        this.chkRenderInDataColumnTemplate.IsEnabled = false;
                    }
                    break;
                case DynamicFormControlType.Label:
                case DynamicFormControlType.TextBlock:
                    this.gridDynamicFormControlEditor.Children.Add(new DynamicFormTextBlockEditor());
                    break;
                case DynamicFormControlType.TextBox:
                    this.gridDynamicFormControlEditor.Children.Add(new DynamicFormTextBoxEditor());
                    break;
                case DynamicFormControlType.DatePicker:
                    this.gridDynamicFormControlEditor.Children.Add(new DynamicFormDatePickerEditor());
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ControlType",
                                                          controlType,
                                                          "The programmer did not program this enum value.");
            }
        }

        /// <summary>
        ///     Event handler. Called by cboDescriptionViewerPosition for loaded
        ///     events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void cboDescriptionViewerPosition_Loaded(object sender, RoutedEventArgs e)
        {
            var cbo = sender as ComboBox;
            var ary = new[] { "Auto", "BesideContent", "BesideLabel" };

            cbo.ItemsSource = ary;
        }

        /// <summary>
        ///     Event handler. Called by cboLabelPosition for loaded events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void cboLabelPosition_Loaded(object sender, RoutedEventArgs e)
        {
            var cbo = sender as ComboBox;
            var ary = new[] { "Auto", "Left", "Top" };

            cbo.ItemsSource = ary;
        }

        #endregion
    }
}
