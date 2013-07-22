namespace XamlHelpmeet.UI.Editors
{
    #region Imports

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using XamlHelpmeet.Model;
    using XamlHelpmeet.UI.DynamicForm;
    using XamlHelpmeet.UI.Utilities;

    #endregion

    /// <summary>
    ///     Interaction logic for DynamicFormTextBoxEditor.xaml.
    /// </summary>
    public partial class DynamicFormTextBoxEditor : UserControl
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the DynamicFormTextBoxEditor class.
        /// </summary>
        public DynamicFormTextBoxEditor()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods (private)

        /// <summary>
        ///     Event handler. Called by DynamicFormTextBoxEditor for unloaded events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void DynamicFormTextBoxEditor_Unloaded(object sender, RoutedEventArgs e)
        {
            this.cboStringFormat.RemoveHandler(Selector.SelectionChangedEvent,
                                               new SelectionChangedEventHandler(this.cboStringFormat_SelectionChanged));
        }

        /// <summary>
        ///     Event handler. Called by cboBindingMode for loaded events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void cboBindingMode_Loaded(object sender, RoutedEventArgs e)
        {
            var cbo = sender as ComboBox;

            if (cbo.ItemsSource != null)
            {
                return;
            }

            cbo.ItemsSource = UIHelpers.GetSortedEnumNames(typeof(BindingMode));
        }

        /// <summary>
        ///     Event handler. Called by cboStringFormat for loaded events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void cboStringFormat_Loaded(object sender, RoutedEventArgs e)
        {
            var cbo = sender as ComboBox;

            if (cbo.ItemsSource != null)
            {
                return;
            }

            this.cboStringFormat.RemoveHandler(Selector.SelectionChangedEvent,
                                               new SelectionChangedEventHandler(this.cboStringFormat_SelectionChanged));
            this.cboStringFormat.ItemsSource = UIHelpers.GetSampleFormats();
            this.cboStringFormat.SelectedIndex = -1;
            this.cboStringFormat.AddHandler(Selector.SelectionChangedEvent,
                                            new SelectionChangedEventHandler(this.cboStringFormat_SelectionChanged));
        }

        /// <summary>
        ///     Event handler. Called by cboStringFormat for selection changed events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void cboStringFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cboStringFormat.SelectedItem == null || this.cboStringFormat.SelectedIndex == -1)
            {
                return;
            }

            (this.cboStringFormat.DataContext as DynamicFormListBoxContent).StringFormat =
                (this.cboStringFormat.SelectedItem as SampleFormat).StringFormat;
        }

        #endregion
    }
}
