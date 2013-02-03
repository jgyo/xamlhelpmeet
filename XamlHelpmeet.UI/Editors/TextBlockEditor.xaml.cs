// file:	Editors\TextBlockEditor.xaml.cs
//
// summary:	Implements the text block editor.xaml class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.CreateBusinessForm;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.UI.Editors
{
    /// <summary>
    ///     Interaction logic for TextBlockEditor.xaml.
    /// </summary>
    /// <seealso cref="T:System.Windows.Controls.UserControl"/>
	public partial class TextBlockEditor : UserControl
	{
        /// <summary>
        ///     Initializes a new instance of the TextBlockEditor class.
        /// </summary>
		public TextBlockEditor()
		{
			InitializeComponent();
            DataContextChanged += TextBlockEditor_DataContextChanged;
		}

        private void InitCBOFormat()
        {
            var cellContent = DataContext as CellContent;
            if (cellContent == null)
                return;
            if (cboStringFormat.ItemsSource == null)
                return;
            cboStringFormat.SelectedItem = cellContent.StringFormat;
        }

        private void TextBlockEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            InitCBOFormat();
        }

        private void cboStringFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            if (cboStringFormat.SelectedValue == null)
            {
                txtStringFormat.Text = string.Empty;
                return;
            }
			txtStringFormat.Text = cboStringFormat.SelectedValue.ToString();
		}

		private void TextBlockEditor_Loaded(object sender, RoutedEventArgs e)
		{
			var binding = new Binding()
			{
				Path = new PropertyPath("BindingPath"),
				Mode = BindingMode.TwoWay,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};

            cboStringFormat.ItemsSource = UIHelpers.GetSampleFormats();
            cboStringFormat.SelectedIndex = -1;

            InitCBOFormat();

			if (CreateBusinessFormWindow.ClassEntity == null || CreateBusinessFormWindow.ClassEntity.PropertyInformation.Count == 0)
			{
				txtBindingPath.Visibility = System.Windows.Visibility.Visible;
				cboBindingPath.Visibility = System.Windows.Visibility.Collapsed;
				txtBindingPath.SetBinding(TextBox.TextProperty, binding);
			}
			else
			{
				txtBindingPath.Visibility = System.Windows.Visibility.Collapsed;
				cboBindingPath.Visibility = System.Windows.Visibility.Visible;
				cboBindingPath.SetBinding(ComboBox.SelectedValueProperty, binding);
				cboBindingPath.ItemsSource = CreateBusinessFormWindow.ClassEntity.PropertyInformation;
			}
		}

        private void FormatChanged(object sender, TextChangedEventArgs e)
        {
            var cellContent = DataContext as CellContent;
            if (cellContent == null)
                return;
            cellContent.StringFormat = txtStringFormat.Text;
        }        
	}
}
