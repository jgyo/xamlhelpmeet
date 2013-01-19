using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using XamlHelpmeet.UI.DynamicForm;
using XamlHelpmeet.UI.Enums;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.UI.Editors
{
	/// <summary>
	/// Interaction logic for DynamicFormEditor.xaml
	/// </summary>
	public partial class DynamicFormEditor : UserControl
	{
		private void cboControlType_Loaded(object sender,
		                                   RoutedEventArgs e)
		{
			var cbo = sender as ComboBox;
			var ary = Enum.GetNames(typeof(DynamicFormControlType));
			Array.Sort(ary);
			cbo.ItemsSource = ary;
			SetRenderInDataColunnTemplateVisibility();
		}

		private void cboControlType_SelectionChanged(object sender,
		                                             SelectionChangedEventArgs e)
		{
			if (gridDynamicFormControlEditor == null)
			{
				// this happens on load because we are wired up in XAML
				return;
			}
			if (gridDynamicFormControlEditor.Children != null)
			{
				gridDynamicFormControlEditor.Children.Clear();
			}
			var isSilverlight = chkRenderInDataColumnTemplate.Visibility == Visibility.Visible;
			chkRenderInDataColumnTemplate.IsEnabled = true;
			var controlType = (DynamicFormControlType)Enum.Parse(typeof(DynamicFormControlType),
			                             (sender as ComboBox).SelectedValue.ToString());
			switch (controlType)
			{
				case DynamicFormControlType.CheckBox:
					gridDynamicFormControlEditor.Children.Add(new DynamicFormCheckBoxEditor());
					break;
				case DynamicFormControlType.ComboBox:
					gridDynamicFormControlEditor.Children.Add(new DynamicFormComboBoxEditor());

					if (isSilverlight)
					{
						chkRenderInDataColumnTemplate.IsChecked = true;
						chkRenderInDataColumnTemplate.IsEnabled = false;
					}
					break;
				case DynamicFormControlType.Image:
					gridDynamicFormControlEditor.Children.Add(new DynamicFormTextBlockEditor());
					if (isSilverlight)
					{
						chkRenderInDataColumnTemplate.IsChecked = true;
						chkRenderInDataColumnTemplate.IsEnabled = false;
					}
						break;
				case DynamicFormControlType.Label:
				case DynamicFormControlType.TextBlock:
					gridDynamicFormControlEditor.Children.Add(new DynamicFormTextBlockEditor());
					break;
				case DynamicFormControlType.TextBox:
					gridDynamicFormControlEditor.Children.Add(new DynamicFormTextBoxEditor());
					break;
				case DynamicFormControlType.DatePicker:
					gridDynamicFormControlEditor.Children.Add(new DynamicFormDatePickerEditor());
					break;
				default:
					throw new ArgumentOutOfRangeException("ControlType", controlType, 
					"The programmer did not program this enum value.");
			}
		}

		private void cboDescriptionViewerPosition_Loaded(object sender, RoutedEventArgs e)
		{
			var cbo = sender as ComboBox;
			var ary = new string[]
			{ 
				"Auto", 
				"BesideContent", 
				"BesideLabel"
			};

			cbo.ItemsSource = ary;
		}

		private void cboLabelPosition_Loaded(object sender, RoutedEventArgs e)
		{
			var cbo = sender as ComboBox;
			var ary = new string[]
			{ 
				"Auto", 
				"Left", 
				"Top"
			};

			cbo.ItemsSource = ary;
		}

		private void DynamicFormEditor_Loaded(object sender, RoutedEventArgs e)
		{
			SetSilverlightDataFormFieldsVisibility();
		}

		private void SetRenderInDataColunnTemplateVisibility()
		{
			var obj = UIHelpers.FindAnscestorWindow(this) as CreateBusinessFormFromClassWindow;
			if (obj == null)
				return;

			if (!obj.ClassEntity.IsSilverlight || obj.cboSelectObjectToCreate.SelectedValue.ToString() != 
			"Silverlight Data Grid")
			{
				return;
			}
			chkRenderInDataColumnTemplate.Visibility = Visibility.Visible;
		}

		private void SetSilverlightDataFormFieldsVisibility()
		{
			gridSilverlightDataFormFields.Visibility = Visibility.Collapsed;

			var obj = UIHelpers.FindAnscestorWindow(this) as CreateBusinessFormFromClassWindow;

			if (obj == null || !obj.ClassEntity.IsSilverlight || obj.cboSelectObjectToCreate.SelectedValue.
			ToString() != "Silverlight Data Form")
			{
				return;
			}

			gridSilverlightDataFormFields.Visibility = Visibility.Visible;
		}
	}
}
