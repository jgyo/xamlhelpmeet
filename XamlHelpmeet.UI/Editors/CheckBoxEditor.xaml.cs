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
	/// Interaction logic for CheckBoxEditor.xaml
	/// </summary>
	public partial class CheckBoxEditor : UserControl
	{
		public CheckBoxEditor()
		{
			InitializeComponent();
		}

		private void CheckBoxEditor_Loaded(object sender, RoutedEventArgs e)
		{
			var binding = new Binding()
			{
				Path = new PropertyPath("BindingPath"),
				Mode = BindingMode.TwoWay,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};

			if (CreateBusinessFormWindow.ClassEntity == null ||
				CreateBusinessFormWindow.ClassEntity.PropertyInformation.Count==0)
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
	}
}
