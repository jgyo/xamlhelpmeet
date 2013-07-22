namespace XamlHelpmeet.UI.Editors
{
    #region Imports

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using XamlHelpmeet.UI.Utilities;

    #endregion

    /// <summary>
    ///     Interaction logic for DynamicFormCheckBoxEditor.xaml.
    /// </summary>
    public partial class DynamicFormCheckBoxEditor : UserControl
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the DynamicFormCheckBoxEditor class.
        /// </summary>
        public DynamicFormCheckBoxEditor() { this.InitializeComponent(); }

        #endregion

        #region Methods (private)

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

        #endregion
    }
}
