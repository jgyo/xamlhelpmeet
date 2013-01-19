using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.DynamicForm;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.UI.Editors
{
	/// <summary>
	/// Interaction logic for DynamicFormTextBoxEditor.xaml
	/// </summary>
	public partial class DynamicFormTextBoxEditor : UserControl
	{
		public DynamicFormTextBoxEditor()
		{
			InitializeComponent();
		}

		private void cboStringFormat_Loaded(object sender, RoutedEventArgs e)
		{
			var cbo = sender as ComboBox;

			if (cbo.ItemsSource != null)
				return;

			cboStringFormat.RemoveHandler(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(cboStringFormat_SelectionChanged));
			cboStringFormat.ItemsSource = UIHelpers.GetSampleFormats();
			cboStringFormat.SelectedIndex = -1;
			cboStringFormat.AddHandler(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(cboStringFormat_SelectionChanged));
		}

		private void cboBindingMode_Loaded(object sender, RoutedEventArgs e)
		{
			var cbo = sender as ComboBox;

			if (cbo.ItemsSource != null)
				return;

			cbo.ItemsSource = UIHelpers.GetSortedEnumNames(typeof(BindingMode));
		}

		private void cboStringFormat_SelectionChanged(
			object sender, SelectionChangedEventArgs e)
		{
			if (cboStringFormat.SelectedItem == null || cboStringFormat.SelectedIndex==-1)
				return;

			(cboStringFormat.DataContext as DynamicFormListBoxContent).StringFormat =
				(cboStringFormat.SelectedItem as SampleFormat).StringFormat;
		}

		private void DynamicFormTextBoxEditor_Unloaded(
			object sender, RoutedEventArgs e)
		{
			cboStringFormat.RemoveHandler(ComboBox.SelectionChangedEvent,
				new SelectionChangedEventHandler(cboStringFormat_SelectionChanged));
		}
	}
}
