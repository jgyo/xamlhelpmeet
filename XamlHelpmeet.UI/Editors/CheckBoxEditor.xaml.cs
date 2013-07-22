namespace XamlHelpmeet.UI.Editors
{
    #region Imports

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using XamlHelpmeet.UI.CreateBusinessForm;

    #endregion

    /// <summary>
    ///     Interaction logic for CheckBoxEditor.xaml.
    /// </summary>
    public partial class CheckBoxEditor : UserControl
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the CheckBoxEditor class.
        /// </summary>
        public CheckBoxEditor()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods (private)

        /// <summary>
        ///     Event handler. Called by CheckBoxEditor for loaded events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void CheckBoxEditor_Loaded(object sender, RoutedEventArgs e)
        {
            var binding = new Binding
                          {
                              Path = new PropertyPath("BindingPath"),
                              Mode = BindingMode.TwoWay,
                              UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                          };

            if (CreateBusinessFormWindow.ClassEntity == null
                || CreateBusinessFormWindow.ClassEntity.PropertyInformation.Count == 0)
            {
                this.txtBindingPath.Visibility = Visibility.Visible;
                this.cboBindingPath.Visibility = Visibility.Collapsed;
                this.txtBindingPath.SetBinding(TextBox.TextProperty, binding);
            }
            else
            {
                this.txtBindingPath.Visibility = Visibility.Collapsed;
                this.cboBindingPath.Visibility = Visibility.Visible;
                this.cboBindingPath.SetBinding(Selector.SelectedValueProperty, binding);
                this.cboBindingPath.ItemsSource = CreateBusinessFormWindow.ClassEntity.PropertyInformation;
            }
        }

        #endregion
    }
}
