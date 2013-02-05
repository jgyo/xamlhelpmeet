using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XamlHelpmeet.UI.UIControlFactory;

namespace XamlHelpmeet.UI
{
    /// <summary>
    /// Interaction logic for UIControlDefaultsWindow.xaml
    /// </summary>
    public partial class UIControlDefaultsWindow : Window
    {
        public UIControlDefaultsWindow()
        {
            InitializeComponent();
        }

        private void btnAddNewUIProperty_Click(object sender, RoutedEventArgs e)
        {
            if (this.bdrContainer.DataContext == null)
                return;

            var uiProperty = new UIProperty("ChangeMe", "ChangeMe");
            (this.bdrContainer.DataContext as UIControl).ControlProperties.Add(uiProperty);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnDeleteUIProperty_Click(object sender, RoutedEventArgs e)
        {
            if (bdrContainer.DataContext == null)
                return;

            var uiProperty = (sender as Button).DataContext as UIProperty;

            if (MessageBox.Show(String.Format("Are you sure you want to delete this Control Property: \r\nName: {0}\r\nValue: {1}", uiProperty.PropertyName, uiProperty.PropertyValue), "Delete Control Property?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
                return;

            (this.bdrContainer.DataContext as UIControl).ControlProperties.Remove(uiProperty);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            UIControlFactory.UIControlFactory.Instance.Save(true);
            DialogResult = true;
        }

        private void lbControls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.lbControls == null || this.lbControls.SelectedItem == null)
                return;

            var uiControl = this.lbControls.SelectedItem as UIControl;

            switch (uiControl.ControlRole)
            {
                case UIControlRole.Border:
                case UIControlRole.Grid:
                    this.chkGenerateControlName.IsEnabled = false;
                    break;

                default:
                    this.chkGenerateControlName.IsEnabled = true;
                    break;
            }

            switch (uiControl.ControlRole)
            {
                case UIControlRole.TextBox:
                    this.chkIncludeTargetNullValueForNullableBindings.IsEnabled = true;
                    break;

                default:
                    this.chkIncludeTargetNullValueForNullableBindings.IsEnabled = false;
                    break;
            }

            switch (uiControl.ControlRole)
            {
                case UIControlRole.DataGrid:
                case UIControlRole.Border:
                case UIControlRole.Grid:
                case UIControlRole.Image:
                case UIControlRole.Label:
                case UIControlRole.TextBlock:
                    this.chkIncludeNotifyOnValidationError.IsEnabled = false;
                    this.chkIncludeValidatesOnDataErrors.IsEnabled = false;
                    this.chkIncludeValidatesOnExceptions.IsEnabled = false;
                    break;

                case UIControlRole.CheckBox:
                case UIControlRole.ComboBox:
                case UIControlRole.TextBox:
                case UIControlRole.DatePicker:
                    this.chkIncludeNotifyOnValidationError.IsEnabled = true;
                    this.chkIncludeValidatesOnDataErrors.IsEnabled = true;
                    this.chkIncludeValidatesOnExceptions.IsEnabled = true;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("ControlRole", (this.lbControls.SelectedItem as UIControl).ControlRole, "Programmer did not program this value.");
            }
        }

        private void UIControlDefaultsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var uiControls = UIControlFactory.UIControlFactory.Instance.UIControls;
            var collectionView = CollectionViewSource.GetDefaultView(uiControls) as CollectionView;
            collectionView.GroupDescriptions.Clear();
            collectionView.SortDescriptions.Clear();
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Platform"));
            collectionView.SortDescriptions.Add(new SortDescription("Platform", ListSortDirection.Ascending));
            collectionView.SortDescriptions.Add(new SortDescription("ControlRoleName", ListSortDirection.Ascending));
            this.lbControls.ItemsSource = uiControls;
            this.lbControls.SelectedIndex = 0;
        }
    }
}