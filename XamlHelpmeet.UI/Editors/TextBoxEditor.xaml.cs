using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XamlHelpmeet.UI.CreateBusinessForm;

namespace XamlHelpmeet.UI.Editors
{
	/// <summary>
	/// Interaction logic for TextBoxEditor.xaml
	/// </summary>
	public partial class TextBoxEditor : UserControl
	{
		public TextBoxEditor()
		{
			InitializeComponent();
		}

		// This method should not be necessary. With binding this can be accomplished automatically.
		//private void cboStringFormats_SelectionChanged(object sender, SelectionChangedEventArgs e)
		//{
		//	txtStringFormat.Text = cboStringFormat.SelectedValue.ToString();
		//}

		private void TextBlockEditor_Loaded(object sender, RoutedEventArgs e)
		{
			var binding = new Binding()
			{
				Path = new PropertyPath("BindingPath"),
				Mode = BindingMode.TwoWay,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};

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

			//cboStringFormat.AddHandler(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(cboStringFormat_SelectionChanged));
		}

		//private void TextBlockEditor_Unloaded(object sender, RoutedEventArgs e)
		//{
		//	cboStringFormat.RemoveHandler(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(cboStringFormat_SelectionChanged));
		//}
	}
}
