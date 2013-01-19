using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.UI.Editors
{
	/// <summary>
	/// Interaction logic for DynamicFormComboBoxEditor.xaml
	/// </summary>
	public partial class DynamicFormComboBoxEditor : UserControl
	{
		public DynamicFormComboBoxEditor()
		{
			InitializeComponent();
		}

		private void cboBindingMode_Loaded(object sender, RoutedEventArgs e)
		{
			var cbo = sender as ComboBox;
			if (cbo.ItemsSource != null)
			{
				return;
			}
			cbo.ItemsSource = UIHelpers.GetSortedEnumNames(typeof(BindingMode));
		}
	}
}
