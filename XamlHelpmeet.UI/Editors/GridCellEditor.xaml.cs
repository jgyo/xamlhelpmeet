using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using XamlHelpmeet.Model;

namespace XamlHelpmeet.UI.Editors
{
	/// <summary>
	/// Interaction logic for GridCellEditor.xaml
	/// </summary>
	public partial class GridCellEditor : UserControl
	{
		public GridCellEditor()
		{
			InitializeComponent();
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (gridCellEditor == null)
			{
				// this happens on load because we are wired in XAML
				return;
			}

			if (gridCellEditor.Children != null)
			{
				gridCellEditor.Children.Clear();
			}

			var controlType = (ControlType)Enum.Parse(typeof(ControlType),
				(sender as ComboBox).SelectedValue.ToString());

			switch (controlType)
			{
				case ControlType.CheckBox:
					gridCellEditor.Children.Add(new CheckBoxEditor());
					break;
				case ControlType.ComboBox:
					gridCellEditor.Children.Add(new ComboBoxEditor());
					break;
				case ControlType.Image:
					gridCellEditor.Children.Add(new ImageEditor());
					break;
				case ControlType.Label:
					gridCellEditor.Children.Add(new LabelEditor());
					break;
				case ControlType.TextBlock:
					gridCellEditor.Children.Add(new TextBlockEditor());
					break;
				case ControlType.TextBox:
					gridCellEditor.Children.Add(new TextBoxEditor());
					break;
				case ControlType.DatePicker:
					gridCellEditor.Children.Add(new DatePickerEditor());
					break;
				default:
					throw new ArgumentOutOfRangeException("ControlType",
						controlType, "This enum value has not been anticipated.");
			}
		}
	}
}
