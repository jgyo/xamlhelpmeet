// file:	Editors\GridCellEditor.xaml.cs
//
// summary:	Implements the grid cell editor.xaml class
using System;
using System.Windows;
using System.Windows.Controls;
using XamlHelpmeet.Model;

namespace XamlHelpmeet.UI.Editors
{
    /// <summary>
    ///     Interaction logic for GridCellEditor.xaml.
    /// </summary>
    /// <seealso cref="T:System.Windows.Controls.UserControl"/>
    public partial class GridCellEditor : UserControl
    {
        #region Fields

        private CellContent _cellContentFromDataContext;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the GridCellEditor class.
        /// </summary>
        public GridCellEditor()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void _cellContent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ControlType":
                    ControlTypeChanged();
                    break;
            }
        }

        private void ControlTypeChanged()
        {
            var controlType = _cellContentFromDataContext == null
                ? ControlType.None
                : _cellContentFromDataContext.ControlType;

            if (editorContent == null)
                return;

            editorContent.Children.Clear();

            switch (controlType)
            {
                case ControlType.CheckBox:
                    editorContent.Children.Add(new CheckBoxEditor());
                    break;

                case ControlType.ComboBox:
                    editorContent.Children.Add(new ComboBoxEditor());
                    break;

                case ControlType.Image:
                    editorContent.Children.Add(new ImageEditor());
                    break;

                case ControlType.Label:
                    editorContent.Children.Add(new LabelEditor());
                    break;

                case ControlType.TextBlock:
                    editorContent.Children.Add(new TextBlockEditor());
                    break;

                case ControlType.TextBox:
                    editorContent.Children.Add(new TextBoxEditor());
                    break;

                case ControlType.DatePicker:
                    editorContent.Children.Add(new DatePickerEditor());
                    break;

                case ControlType.None:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("ControlType",
                        controlType, "This enum value has not been anticipated.");
            }
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_cellContentFromDataContext != null)
            {
                _cellContentFromDataContext.PropertyChanged -= _cellContent_PropertyChanged;
            }

            _cellContentFromDataContext = DataContext as CellContent;
            if (_cellContentFromDataContext != null)
                _cellContentFromDataContext.PropertyChanged += _cellContent_PropertyChanged;
            ControlTypeChanged();
        }

        #endregion
    }
}