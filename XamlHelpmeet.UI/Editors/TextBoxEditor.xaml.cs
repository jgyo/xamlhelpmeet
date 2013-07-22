// file:	Editors\TextBoxEditor.xaml.cs
//
// summary:	Implements the text box editor.xaml class
namespace XamlHelpmeet.UI.Editors
{
    #region Imports

    using System.Windows;
    using System.Windows.Controls;
    using XamlHelpmeet.Model;
    using XamlHelpmeet.UI.CreateBusinessForm;

    #endregion

    /// <summary>
    ///     Interaction logic for TextBoxEditor.xaml.
    /// </summary>
    /// <seealso cref="T:System.Windows.Controls.UserControl" />
    public partial class TextBoxEditor : UserControl
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the TextBoxEditor class.
        /// </summary>
        public TextBoxEditor()
        {
            this.InitializeComponent();

            this.DataContextChanged += this.TextBlockEditor_DataContextChanged;
        }

        #endregion

        #region Methods (private)

        /// <summary>
        ///     Format changed.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Dependency property changed event information.
        /// </param>
        private void FormatChanged(object sender, TextChangedEventArgs e)
        {
            var cellContent = this.DataContext as CellContent;
            if (cellContent == null)
            {
                return;
            }
            cellContent.StringFormat = this.txtStringFormat.Text;
        }

        /// <summary>
        ///     Initializes the combobox format.
        /// </summary>
        private void InitCBOFormat()
        {
            var cellContent = this.DataContext as CellContent;
            if (cellContent == null)
            {
                return;
            }
            if (this.cboStringFormat.ItemsSource == null)
            {
                return;
            }
            this.cboStringFormat.SelectedItem = cellContent.StringFormat;
        }

        /// <summary>
        ///     Event handler. Called by TextBlockEditor for data context changed
        ///     events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Dependency property changed event information.
        /// </param>
        private void TextBlockEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.InitCBOFormat();
        }

        /// <summary>
        ///     Event handler. Called by TextBlockEditor for loaded events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Dependency property changed event information.
        /// </param>
        private void TextBlockEditor_Loaded(object sender, RoutedEventArgs e)
        {
            //var binding = new Binding()
            //{
            //	Path = new PropertyPath("BindingPath"),
            //	Mode = BindingMode.TwoWay,
            //	UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            //         };

            //cboStringFormat.ItemsSource = UIHelpers.GetSampleFormats();
            //cboStringFormat.SelectedIndex = -1;

            this.InitCBOFormat();

            if (CreateBusinessFormWindow.ClassEntity == null
                || CreateBusinessFormWindow.ClassEntity.PropertyInformation.Count == 0)
            {
                this.txtBindingPath.Visibility = Visibility.Visible;
                this.cboBindingPath.Visibility = Visibility.Collapsed;
                //txtBindingPath.SetBinding(TextBox.TextProperty, binding);
            }
            else
            {
                this.txtBindingPath.Visibility = Visibility.Collapsed;
                this.cboBindingPath.Visibility = Visibility.Visible;
                //cboBindingPath.SetBinding(ComboBox.SelectedValueProperty, binding);
                this.cboBindingPath.ItemsSource = CreateBusinessFormWindow.ClassEntity.PropertyInformation;
            }
        }

        /// <summary>
        ///     Event handler. Called by cboStringFormat for selection changed events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Dependency property changed event information.
        /// </param>
        private void cboStringFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cboStringFormat.SelectedValue == null)
            {
                this.txtStringFormat.Text = string.Empty;
                return;
            }
            if (this.txtStringFormat == null)
            {
                return;
            }
            this.txtStringFormat.Text = this.cboStringFormat.SelectedValue.ToString();
        }

        #endregion
    }
}
