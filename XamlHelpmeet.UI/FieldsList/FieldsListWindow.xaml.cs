using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.UIControlFactory;

namespace XamlHelpmeet.UI
{
	/// <summary>
	/// 	Interaction logic for FieldsListWindow.xaml.
	/// </summary>
	/// <seealso cref="T:System.Windows.Window"/>
	public partial class FieldsListWindow : Window
	{
		#region Fields

		private readonly ClassEntity _classEntity;
		private DataObject _dataObject;
		private double _saveHeight;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// 	Initializes a new instance of the FieldsListWindow class.
		/// </summary>
		/// <param name="ClassEntity">
		/// 	The class entity.
		/// </param>
		public FieldsListWindow(ClassEntity ClassEntity)
		{
			InitializeComponent();

			// Add other initialization code here
			_classEntity = ClassEntity;
		}

		public FieldsListWindow()
		{
			InitializeComponent();
		}

		#endregion Constructors

		#region Methods
		private void btnCollapseExpand_click(object sender, RoutedEventArgs e)
		{
			var btn = sender as Button;

			if (btn.Content.ToString() == "Collapse")
			{
				_saveHeight = Height;
				Height = 97;
				btn.Content = "Expand";
			}
			else
			{
				Height = _saveHeight;
				Content = "Collapse";
			}
		}

		private void cboControlType_Loaded(object sender, RoutedEventArgs e)
		{
			var cbo = sender as ComboBox;

			cbo.ItemsSource = (from d in Enum.GetValues(typeof(ControlType)).OfType<ControlType>()
							   where d.ToString() != "None"
							   orderby d.ToString()
							   select d.ToString()).ToArray<string>();
		}

		private string GetControlsForField(PropertyInformation pi)
		{
			var uiPlatform = UIPlatform.WPF;

			if (_classEntity.IsSilverlight)
			{
				uiPlatform = UIPlatform.Silverlight;
			}

			int? columnIndex = null;
			int? rowIndex = null;

			if (pi.FieldListIncludGridAttachedProperties)
			{
				columnIndex = 0;
				rowIndex = 0;
			}

			// Initialize the return value.
			var resultString = string.Empty;

			if ((rdoLabelAndControl.IsChecked ?? false) || (rdoLabelOnly.IsChecked ?? false))
			{
				resultString = rdoLabelAndControl.IsChecked ?? false
					? string.Concat(UIControlFactory.UIControlFactory.Instance.MakeLabelWithoutBinding(uiPlatform, columnIndex, rowIndex, pi.LabelText), Environment.NewLine)
					: UIControlFactory.UIControlFactory.Instance.MakeLabelWithoutBinding(uiPlatform, columnIndex, rowIndex, pi.LabelText);
			}

			if (!((rdoLabelAndControl.IsChecked ?? false) || (rdoControlOnly.IsChecked ?? false)))
				return resultString;

			// Construct xaml for the control type.
			switch (pi.FieldListControlType)
			{
				case ControlType.CheckBox:
					return string.Concat(resultString, UIControlFactory.UIControlFactory.Instance.MakeCheckBox(uiPlatform, columnIndex, rowIndex, string.Empty, pi.Name, BindingMode.TwoWay));
				case ControlType.ComboBox:
					resultString = string.Concat(resultString, UIControlFactory.UIControlFactory.Instance.MakeComboBox(uiPlatform, columnIndex, rowIndex, pi.Name, BindingMode.TwoWay));
					if (_classEntity.IsSilverlight)
						return string.Concat("<!-- Bind Silverlight ComboBox in code after its ItemsSource has been loaded. -->\r\n", resultString);
					return resultString;
				case ControlType.Image:
					return string.Concat(resultString, UIControlFactory.UIControlFactory.Instance.MakeImage(uiPlatform, columnIndex, rowIndex, pi.Name));
				case ControlType.Label:
					return string.Concat(resultString, UIControlFactory.UIControlFactory.Instance.MakeLabel(uiPlatform, columnIndex, rowIndex, pi.Name, pi.StringFormat, _classEntity.SilverlightVersion));
				case ControlType.TextBlock:
					return string.Concat(resultString, UIControlFactory.UIControlFactory.Instance.MakeTextBlock(uiPlatform, columnIndex, rowIndex, pi.Name, pi.StringFormat, _classEntity.SilverlightVersion));
				case ControlType.TextBox:
					return string.Concat(resultString, UIControlFactory.UIControlFactory.Instance.MakeTextBox(uiPlatform, columnIndex, rowIndex, pi.Name, BindingMode.TwoWay, null, null, pi.StringFormat, pi.TypeName.StartsWith("Nullable"), _classEntity.SilverlightVersion));
				case ControlType.DatePicker:
					return string.Concat(resultString, UIControlFactory.UIControlFactory.Instance.MakeDatePicker(uiPlatform, columnIndex, rowIndex, pi.Name, null));
				default:
					throw new ArgumentOutOfRangeException("pi.FieldListControlType", "Sorry, but the program does not know the current value of the parameter.");
			}
		}

		// Watchs the MouseDown events and initializes the data object for a drag and drop operation.
		private void TextBlockDrag_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
				_dataObject = null;
		}

		// While the mouse is pressed down watches the MouseMove event to handle dragging effects.
		// Stops if the mouse is released, and will not run if the data object is null.
		private void TextBlockDrag_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Released || _dataObject != null)
				return;

			var tb = sender as TextBlock;
			_dataObject = new DataObject(DataFormats.Text, GetControlsForField(tb.DataContext as PropertyInformation));
			DragDrop.DoDragDrop(tb, _dataObject, DragDropEffects.Copy);
		}
		#endregion
	}
}