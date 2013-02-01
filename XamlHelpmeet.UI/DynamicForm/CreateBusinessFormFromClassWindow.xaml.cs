using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.DynamicForm.DragAndDrop;
using XamlHelpmeet.UI.Editors;
using XamlHelpmeet.UI.Enums;
using XamlHelpmeet.UI.UIControlFactory;

namespace XamlHelpmeet.UI.DynamicForm
{
    /// <summary>
    /// Interaction logic for CreateBusinessFromFromClassWindow.xaml
    /// </summary>
    public partial class CreateBusinessFormFromClassWindow : Window
    {
        #region Constants

        private const string STR_BUSINESSFORM                                     = "Business Form";
        private const string STR_ButtonContentCancelPadding350350GridColumn1Margi = "    <Button Content=\"Cancel\" Padding=\"3.5,0,3.5,0\" Grid.Column=\"1\" Margin=\"3.5\"/>";
        private const string STR_ButtonContentOKPadding350350Margin3              = "    <Button Content=\"OK\" Padding=\"3.5,0,3.5,0\" Margin=\"3\" />";
        private const string STR_ButtonContentOKPadding350350Margin35             = "    <Button Content=\"OK\" Padding=\"3.5,0,3.5,0\" Margin=\"3.5\" />";
        private const string STR_ColumnDefinition                                 = "        <ColumnDefinition />";
        private const string STR_ColumnDefinitionSharedSizeGroupButtons           = "        <ColumnDefinition SharedSizeGroup=\"Buttons\" />";
        private const string STR_ColumnDefinitionWidth                            = "<ColumnDefinition Width=\"{0}\" />{1}";
        private const string STR_ColumnDefinitionWidthAuto                        = "<ColumnDefinition Width=\"Auto\" />";
        private const string STR_DataGridCheckBoxColumnBindingindingHeader        = "<{0}DataGridCheckBoxColumn Binding=\"{{Binding {1}}}\" Header=\"{2}\"/> ";
        private const string STR_DataGridColumnsClose                             = "</{0}DataGrid.Columns>";
        private const string STR_DataGridColumnsOpen                              = "<{0}DataGrid.Columns>";
        private const string STR_DataGridComboBoxColumnBindingindingHeader        = "<{0}DataGridComboBoxColumn Binding=\"{{Binding {1}}}\" Header=\"{2}\"/> ";
        private const string STR_DataGridTemplateColumnCellEditingTemplateClose   = "</{0}DataGridTemplateColumn.CellEditingTemplate> ";
        private const string STR_DataGridTemplateColumnCellEditingTemplateOpen    = "<{0}DataGridTemplateColumn.CellEditingTemplate> ";
        private const string STR_DataGridTemplateColumnCellTemplateClose          = "</{0}DataGridTemplateColumn.CellTemplate> ";
        private const string STR_DataGridTemplateColumnCellTemplateOpen           = "<{0}DataGridTemplateColumn.CellTemplate> ";
        private const string STR_DataGridTemplateColumnClose                      = "</{0}DataGridTemplateColumn> ";
        private const string STR_DataGridTemplateColumnOpen                       = "<{0}DataGridTemplateColumn Header=\"{1}\" SortberPath=\"{2}\"> ";
        private const string STR_DataGridTextColumnBindingindingHeader            = "<{0}DataGridTextColumn Binding=\"{{Binding {1}}}\" Header=\"{2}\"/> ";
        private const string STR_DataGridTextColumnBindingindingStringFormatHeade = "<{0}DataGridTextColumn Binding=\"{{Binding {1}, StringFormat={2}}}\" Header=\"{3}\"/> ";
        private const string STR_DataTemplateClose                                = "</DataTemplate>";
        private const string STR_DataTemplateOpen                                 = "<DataTemplate>";
        private const string STR_GridClose                                        = "</Grid>";
        private const string STR_GridColumn0GridRow0GridColumnSpanText            = " Grid.Column=\"0\" Grid.Row=\"0\" Grid.ColumnSpan=\"{0}\" Text=\"{1}\" ";
        private const string STR_GridColumnDefinitionsClose1                      = "</Grid.ColumnDefinitions>";
        private const string STR_GridColumnDefinitionsClose2                      = "    </Grid.ColumnDefinitions>";
        private const string STR_GridColumnDefinitionsOpen1                       = "<Grid.ColumnDefinitions>";
        private const string STR_GridColumnDefinitionsOpen2                       = "    <Grid.ColumnDefinitions>";
        private const string STR_GridGridColumn0GridRowGridColumnSpanGridIsShared = "<Grid Grid.Column=\"0\" Grid.Row=\"{0}\" Grid.ColumnSpan=\"{1}\" Grid.IsSharedSizeScope=\"true\" HorizontalAlignment=\"Right\">";
        private const string STR_GridGridColumn0GridRowGridColumnSpanHorizontalAl = "<Grid Grid.Column=\"0\" Grid.Row=\"{0}\" Grid.ColumnSpan=\"{1}\" HorizontalAlignment=\"Right\">";
        private const string STR_GridRowDefinitionsClose                          = "</Grid.RowDefinitions>";
        private const string STR_GridRowDefinitionsOpen                           = "<Grid.RowDefinitions>";
        private const string STR_ImageSource                                      = "<Image Source=\"{0}\"/>";
        private const string STR_RowDefinitionHeightAuto                          = "<RowDefinition Height=\"Auto\" />";
        private const string STR_SILVERLIGHTDATAFORM                              = "Silverlight Data Form";
        private const string STR_SILVERLIGHTDATAGRID                              = "Silverlight Data Grid";
        private const string STR_TheFollowingNamespaceDeclarationsMayBeNecessaryF = "<!--The following namespace declarations may be necessary for you to add to the root element of this XAML file.-->";
        private const string STR_TODOAddFormattingConverterForFormat              = "<!-- TODO - Add formatting converter for format: {0} -->";
        private const string STR_WPFDATAGRID                                      = "WPF Data Grid";
        private const string STR_WPFLISTVIEW                                      = "WPF ListView";
        private const string STR_XmlnsclrnamespaceSystemWindowsControlsassemblySy = "<!--xmlns:{0}=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data\"-->";
        private const string STR_Xmlnshttpschemasmicrosoftcomwpf2008toolkit       = "<!--xmlns:{0}=\"http://schemas.microsoft.com/wpf/2008/toolkit\"-->";

        #endregion Constants

        #region Fields

        private readonly ClassEntity _classEntity;
        private string _businessForm = string.Empty;
        private int _numberOfColumnGroups = 2;

        #endregion Fields

        private enum SelectClassberUserControlState
        {
            Minimized,
            Restored
        }

        #region Dependency Properties

        public static readonly DependencyProperty ShowFullDynamicFormContentProperty =
            DependencyProperty.Register("ShowFullDynamicFormContent", typeof(bool),
            typeof(Window), new PropertyMetadata(true));

        #endregion Dependency Properties

        #region Properties

        public string BusinessForm
        {
            get
            {
                return _businessForm;
            }
        }

        public ClassEntity ClassEntity
        {
            get
            {
                return _classEntity;
            }
        }

        public int NumberOfColumnGroups
        {
            get
            {
                return _numberOfColumnGroups;
            }
            set
            {
                _numberOfColumnGroups = value;
            }
        }

        public UIPlatform PlatformType
        {
            get
            {
                if (ClassEntity.IsSilverlight)
                    return UIPlatform.Silverlight;

                return UIPlatform.WPF;
            }
        }

        public bool ShowFullDynamicFormContent
        {
            get
            {
                return (bool)GetValue(ShowFullDynamicFormContentProperty);
            }
            set
            {
                SetValue(ShowFullDynamicFormContentProperty, value);
            }
        }

        #endregion Properties

        #region Constructors

        public CreateBusinessFormFromClassWindow(ClassEntity ClassEntity)
        {
            InitializeComponent();
            _classEntity = ClassEntity;
        }

        #endregion Constructors

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnClearnAllFields_Click(object sender, RoutedEventArgs e)
        {
            ClearAllListBoxFields();
        }

        private void btnCreateForm_Click(object sender, RoutedEventArgs e)
        {
            txtBindingPropertyPrefix.Text = txtBindingPropertyPrefix.Text.Trim();

            if (txtBindingPropertyPrefix.Text.Length > 0 && !txtBindingPropertyPrefix.Text.EndsWith("."))
            {
                txtBindingPropertyPrefix.Text += ".";
            }

            switch (cboSelectObjectToCreate.SelectedValue as string)
            {
                case STR_BUSINESSFORM:
                    CreateBusinessForm();
                    break;

                case STR_WPFLISTVIEW:
                    CreateListView();
                    break;

                case STR_WPFDATAGRID:
                    CreateWPFDataGrid();
                    break;

                case STR_SILVERLIGHTDATAGRID:
                    CreateSilverlightDataGrid();
                    break;

                case STR_SILVERLIGHTDATAFORM:
                    CreateSilverlightDataForm();
                    break;

                default:
                    MessageBox.Show(string.Format("Selection {0}, not implemented",
                        cboSelectObjectToCreate.SelectedValue), "Not Implemented",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation,
                        MessageBoxResult.OK);
                    break;
            }
        }

        private void cboSelectObjectToCreate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSelectObjectToCreate == null || wpBusinessForm == null ||
                cboSelectObjectToCreate.SelectedIndex == -1)
            {
                return;
            }

            wpBusinessForm.Visibility = System.Windows.Visibility.Collapsed;
            wpListView.Visibility = System.Windows.Visibility.Collapsed;
            wpWPFDataGrid.Visibility = System.Windows.Visibility.Collapsed;
            wpSilverlightDataGrid.Visibility = System.Windows.Visibility.Collapsed;
            wpSilverlightDataForm.Visibility = System.Windows.Visibility.Collapsed;

            switch (cboSelectObjectToCreate.SelectedValue as string)
            {
                case STR_BUSINESSFORM:
                    wpBusinessForm.Visibility = System.Windows.Visibility.Visible;
                    break;

                case STR_WPFLISTVIEW:
                    wpListView.Visibility = System.Windows.Visibility.Visible;
                    break;

                case STR_WPFDATAGRID:
                    wpWPFDataGrid.Visibility = System.Windows.Visibility.Visible;
                    break;

                case STR_SILVERLIGHTDATAGRID:
                    wpSilverlightDataGrid.Visibility = System.Windows.Visibility.Visible;
                    break;

                case STR_SILVERLIGHTDATAFORM:
                    wpSilverlightDataForm.Visibility = System.Windows.Visibility.Visible;
                    break;

                default:
                    throw new Exception("Unexpected condition.");
            }

            ClearAllListBoxFields();
            ClearColumnsExceptFirstColumn(1);
        }

        private void ClearAllListBoxFields()
        {
            foreach (var item in gridColumnsContainer.Children)
            {
                if (item is ListBox)
                {
                    (item as ListBox).Items.Clear();
                }
            }

            foreach (var item in ClassEntity.PropertyInformation)
            {
                item.HasBeenUsed = false;
            }

            var collectionView = CollectionViewSource
                .GetDefaultView(ClassEntity.PropertyInformation) as CollectionView;

            if (collectionView == null)
            {
                return;
            }
            collectionView.Refresh();
        }

        private void ClearColumnsExceptFirstColumn(int numberOfColumnGroups)
        {
            if (_numberOfColumnGroups == numberOfColumnGroups)
                return;

            if (numberOfColumnGroups > _numberOfColumnGroups)
            {
                for (var i = _numberOfColumnGroups; i < numberOfColumnGroups; i++)
                {
                    gridColumnsContainer.ColumnDefinitions.Insert(gridColumnsContainer
                        .ColumnDefinitions.Count - 2, new ColumnDefinition()
                    {
                        Width = new GridLength(425, GridUnitType.Pixel),
                        MinWidth = 50
                    });
                    gridColumnsContainer.ColumnDefinitions.Insert(gridColumnsContainer
                        .ColumnDefinitions.Count - 2, new ColumnDefinition()
                    {
                        Width = new GridLength(0, GridUnitType.Auto)
                    });

                    var lb = DynamicFormContentListBoxFactory(gridColumnsContainer
                        .ColumnDefinitions.Count - 2);
                    gridColumnsContainer.Children.Add(lb);

                    var objGridSplitter = new GridSplitter()
                    {
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Right
                    };
                    objGridSplitter.SetValue(Grid.ColumnProperty, gridColumnsContainer
                        .ColumnDefinitions.Count - 2);
                    gridColumnsContainer.Children.Add(objGridSplitter);
                }
            }
            else
            {
                var lastColumnIndexToKeep = (numberOfColumnGroups * 2) - 1;
                var listOfGridSplittersToRemove = new List<GridSplitter>();
                var listOfListBoxesToRemove = new List<ListBox>();

                foreach (var item in gridColumnsContainer.Children)
                {
                    if (item is GridSplitter)
                    {
                        if ((int)((item as GridSplitter).GetValue(Grid.ColumnProperty))
                            > lastColumnIndexToKeep)
                        {
                            listOfGridSplittersToRemove.Add(item as GridSplitter);
                        }
                    }
                    else if (item is ListBox)
                    {
                        if ((int)((item as ListBox).GetValue(Grid.ColumnProperty))
                            > lastColumnIndexToKeep)
                        {
                            listOfListBoxesToRemove.Add(item as ListBox);
                        }
                    }
                }

                foreach (var obj in listOfGridSplittersToRemove)
                {
                    gridColumnsContainer.Children.Remove(obj);
                }

                foreach (var objListBox in listOfListBoxesToRemove)
                {
                    foreach (DynamicFormEditor objDynamicFormEditor in objListBox.Items)
                    {
                        var strPropertyName = (objDynamicFormEditor.DataContext
                            as DynamicFormListBoxContent).BindingPath;

                        foreach (var objPi in ClassEntity.PropertyInformation)
                        {
                            if (objPi.Name == strPropertyName)
                            {
                                objPi.HasBeenUsed = false;
                            }
                        }
                    }

                    objListBox.Items.Clear();
                    gridColumnsContainer.Children.Remove(objListBox);
                }

                for (var i = gridColumnsContainer.ColumnDefinitions.Count - 1;
                    i >= lastColumnIndexToKeep; i--)
                {
                    gridColumnsContainer.ColumnDefinitions.RemoveAt(i);
                }

                gridColumnsContainer.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(0, GridUnitType.Auto)
                });
            }

            var collectionView = CollectionViewSource.GetDefaultView(ClassEntity
                .PropertyInformation) as CollectionView;

            if (collectionView != null)
            {
                collectionView.Refresh();
            }

            _numberOfColumnGroups = numberOfColumnGroups;
        }

        private void CreateBusinessForm()
        {
            var isInsertingTitleRow = !string.IsNullOrEmpty(txtFormTitle.Text);
            var columnGroupListBox = new List<ListBox>();

            foreach (var item in gridColumnsContainer.Children)
            {
                if (item is ListBox)
                {
                    columnGroupListBox.Add(item as ListBox);
                }
            }

            var numberOfColumns = (columnGroupListBox.Count * 3) - 1;
            int numberOfRows = 0;
            int lastGridRowIndex;

            foreach (var listBox in columnGroupListBox)
            {
                numberOfRows = Math.Max(numberOfRows, listBox.Items.Count);
            }

            if (numberOfColumns == 0 || numberOfRows == 0)
            {
                MessageBox.Show("You do not have any properties added to the layout.",
                    "Invalid Layout", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            lastGridRowIndex = numberOfRows;

            var sb = new StringBuilder(10240);

            if (chkWrapInBorder.IsChecked.HasValue && chkWrapInBorder.IsChecked
                .Value == true)
            {
                sb.AppendLine(UIControlFactory.UIControlFactory.Instance
                    .GetUIControl(UIControlRole.Border, PlatformType)
                    .MakeControlFromDefaults(string.Empty, false, string.Empty));
            }

            sb.AppendLine(UIControlFactory.UIControlFactory.Instance
                .GetUIControl(UIControlRole.Grid, PlatformType)
                .MakeControlFromDefaults(string.Empty, false, string.Empty));
            sb.AppendLine(STR_GridRowDefinitionsOpen);

            if (isInsertingTitleRow)
            {
                sb.AppendLine(STR_RowDefinitionHeightAuto);
                lastGridRowIndex += 1;
            }

            for (var intX = 1; intX <= numberOfRows; intX++)
            {
                sb.AppendLine(STR_RowDefinitionHeightAuto);
            }

            if (chkIncludeButtonRow.IsChecked.HasValue && chkIncludeButtonRow
                .IsChecked.Value == true)
            {
                sb.AppendLine(STR_RowDefinitionHeightAuto);
            }

            sb.AppendLine(STR_GridRowDefinitionsClose);
            sb.AppendLine(STR_GridColumnDefinitionsOpen1);

            for (var intX = 0; intX < columnGroupListBox.Count; intX++)
            {
                sb.AppendFormat(STR_ColumnDefinitionWidth, 100,
                    Environment.NewLine);
                sb.AppendLine(STR_ColumnDefinitionWidthAuto);

                if (intX >= columnGroupListBox.Count - 1)
                    continue;

                // this inserts the spacer column between the groups of columns
                sb.AppendFormat(STR_ColumnDefinitionWidth, 10,
                    Environment.NewLine);
            }

            sb.AppendLine(STR_GridColumnDefinitionsClose1);
            sb.AppendLine();

            if (isInsertingTitleRow)
            {
                sb.AppendLine(UIControlFactory.UIControlFactory.Instance
                    .GetUIControl(UIControlRole.TextBlock, PlatformType)
                    .MakeControlFromDefaults(
                    string.Format(STR_GridColumn0GridRow0GridColumnSpanText,
                    numberOfColumns, txtFormTitle.Text), true, string.Empty));
            }

            int currentRow;

            for (var i = 0; i < columnGroupListBox.Count; i++)
            {
                currentRow = isInsertingTitleRow ? 1 : 0;

                foreach (DynamicFormEditor objDynamicFormEditor in columnGroupListBox[i].Items)
                {
                    var objField = (objDynamicFormEditor.DataContext as DynamicFormListBoxContent);

                    if (!string.IsNullOrEmpty(objField.AssociatedLabel))
                    {
                        sb.AppendLine(UIControlFactory.UIControlFactory.Instance
                            .MakeLabelWithoutBinding(PlatformType, i * 3, currentRow,
                            objField.AssociatedLabel));
                    }

                    currentRow += 1;
                }

                sb.AppendLine();
            }

            sb.AppendLine();

            for (var i = 0; i < columnGroupListBox.Count; i++)
            {
                currentRow = isInsertingTitleRow ? 1 : 0;

                foreach (DynamicFormEditor objDynamicFormEditor in columnGroupListBox[i].Items)
                {
                    var field = objDynamicFormEditor.DataContext as
                        DynamicFormListBoxContent;
                    var bindingPath = string.Concat(txtBindingPropertyPrefix.Text,
                        field.BindingPath);

                    switch (field.ControlType)
                    {
                        case DynamicFormControlType.DatePicker:
                            sb.AppendLine(UIControlFactory.UIControlFactory.Instance
                                .MakeDatePicker(PlatformType, (i * 3) + 1, currentRow,
                                bindingPath, field.Width));
                            break;

                        case DynamicFormControlType.CheckBox:
                            sb.AppendLine(UIControlFactory.UIControlFactory.Instance
                                .MakeCheckBox(PlatformType, (i * 3) + 1, currentRow,
                                field.ControlLabel, bindingPath, field.BindingMode));
                            break;

                        case DynamicFormControlType.ComboBox:
                            sb.AppendLine(UIControlFactory.UIControlFactory.Instance
                                .MakeComboBox(PlatformType, (i * 3) + 1, currentRow,
                                bindingPath, field.BindingMode));
                            break;

                        case DynamicFormControlType.Image:
                            sb.AppendLine(UIControlFactory.UIControlFactory.Instance
                                .MakeImage(PlatformType, (i * 3) + 1, currentRow,
                                bindingPath));
                            break;

                        case DynamicFormControlType.Label:
                            if (ClassEntity.IsSilverlight)
                            {
                                sb.AppendLine(WriteSilverlightStringFomatComment(
                                    field.StringFormat));
                            }

                            sb.AppendLine(UIControlFactory.UIControlFactory.Instance
                                .MakeLabel(PlatformType, (i * 3) + 1, currentRow,
                                bindingPath, field.StringFormat, ClassEntity
                                .SilverlightVersion));
                            break;

                        case DynamicFormControlType.TextBlock:
                            if (ClassEntity.IsSilverlight)
                            {
                                sb.AppendLine(WriteSilverlightStringFomatComment(
                                    field.StringFormat));
                            }

                            sb.AppendLine(UIControlFactory.UIControlFactory.Instance
                                .MakeTextBlock(PlatformType, (i * 3) + 1, currentRow,
                                bindingPath, field.StringFormat, ClassEntity
                                .SilverlightVersion));
                            break;

                        case DynamicFormControlType.TextBox:
                            if (ClassEntity.IsSilverlight)
                            {
                                sb.AppendLine(WriteSilverlightStringFomatComment(
                                    field.StringFormat));
                            }

                            sb.AppendLine(UIControlFactory.UIControlFactory.Instance
                                .MakeTextBox(PlatformType, (i * 3) + 1, currentRow,
                                bindingPath, BindingMode.TwoWay, field.Width,
                                field.MaximumLength, field.StringFormat, field
                                .DataType.StartsWith("Nullable"), ClassEntity
                                .SilverlightVersion));
                            break;
                    }

                    currentRow++;
                }

                sb.AppendLine();
            }

            if (chkIncludeButtonRow.IsChecked.HasValue && chkIncludeButtonRow
                .IsChecked == true)
            {
                if (!ClassEntity.IsSilverlight)
                {
                    sb.AppendFormat(
                        STR_GridGridColumn0GridRowGridColumnSpanGridIsShared,
                        lastGridRowIndex, numberOfColumns);
                    sb.AppendLine();
                    sb.AppendLine(STR_GridColumnDefinitionsOpen2);
                    sb.AppendLine(STR_ColumnDefinitionSharedSizeGroupButtons);
                    sb.AppendLine(STR_ColumnDefinitionSharedSizeGroupButtons);
                    sb.AppendLine(STR_GridColumnDefinitionsClose2);
                    sb.AppendLine(
                        STR_ButtonContentOKPadding350350Margin3);
                    sb.AppendLine(
                        STR_ButtonContentCancelPadding350350GridColumn1Margi);
                    sb.AppendLine(STR_GridClose);
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendFormat(
                        STR_GridGridColumn0GridRowGridColumnSpanHorizontalAl,
                        lastGridRowIndex, numberOfColumns);
                    sb.AppendLine();
                    sb.AppendLine(STR_GridColumnDefinitionsOpen2);
                    sb.AppendLine(STR_ColumnDefinition);
                    sb.AppendLine(STR_ColumnDefinition);
                    sb.AppendLine(STR_GridColumnDefinitionsClose2);
                    sb.AppendLine(
                        STR_ButtonContentOKPadding350350Margin35);
                    sb.AppendLine(
                        STR_ButtonContentCancelPadding350350GridColumn1Margi);
                    sb.AppendLine(STR_GridClose);
                    sb.AppendLine();
                }
            }

            sb.Replace(" >", ">");
            sb.Replace("    ", " ");
            sb.Replace("   ", " ");
            sb.Replace("  ", " ");
            sb.AppendLine(GetCloseTagForControlFromDefaults(UIControlRole.Grid));

            if (chkWrapInBorder.IsChecked.HasValue && chkWrapInBorder.IsChecked.Value == true)
            {
                sb.AppendLine(GetCloseTagForControlFromDefaults(UIControlRole.Border));
            }

            _businessForm = sb.ToString();
            DialogResult = true;
        }

        private void CreateBusinessFormFromClass_Loaded(object sender, RoutedEventArgs e)
        {
            // Handles Loaded
            InitialLayoutOfDynamicForms();
            ShowFullDynamicFormContent = true;
            Title = string.Concat("Create Business Form For Class: ",
                ClassEntity.ClassName);

            var obj = new List<string>();
            obj.Add(STR_BUSINESSFORM);

            if (ClassEntity.IsSilverlight)
            {
                obj.Add(STR_SILVERLIGHTDATAGRID);
                obj.Add(STR_SILVERLIGHTDATAFORM);
            }
            else
            {
                obj.Add(STR_WPFLISTVIEW);
                obj.Add(STR_WPFDATAGRID);
            }

            cboSelectObjectToCreate.ItemsSource = obj;
            cboSelectObjectToCreate.SelectedIndex = 0;
        }

        private void CreateListView()
        {
            ListBox listBox = null;

            foreach (Object obj in gridColumnsContainer.Children)
            {
                if (!(obj is ListBox))
                    continue;

                listBox = obj as ListBox;
                break;
            }

            if (listBox == null)
            {
                MessageBox.Show("Unable to get the ListBox used for layout.",
                    "Missing ListBox", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (listBox.Items.Count == 0)
            {
                MessageBox.Show("You do not have any properties added to the layout.",
                    "Invalid Layout", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var sb = new StringBuilder(10240);
            sb.AppendLine("<ListView>");
            sb.AppendLine("    <ListView.ItemContainerStyle>");
            sb.AppendLine("        <Style TargetType=\"ListViewItem\">");
            sb.AppendLine(
                "            <Setter Property=\"HorizontalContentAlignment\" Value=\"Stretch\" />");
            sb.AppendLine("        </Style>");
            sb.AppendLine("    </ListView.ItemContainerStyle>");
            sb.AppendLine("    <ListView.View>");
            sb.AppendLine("        <GridView>");

            foreach (DynamicFormEditor objDynamicFormEditor in listBox.Items)
            {
                var field = objDynamicFormEditor.DataContext as DynamicFormListBoxContent;
                var bindingPath = string.Concat(txtBindingPropertyPrefix.Text, field.BindingPath);

                if (string.IsNullOrEmpty(field.StringFormat))
                {
                    sb.AppendFormat(
                        "<GridViewColumn Header=\"{0}\" DisplayMemberBinding=\"{{Binding Path={1}}}\" />",
                        field.AssociatedLabel, bindingPath);
                }
                else
                {
                    sb.AppendFormat("<GridViewColumn Header=\"{0}\" >", field.AssociatedLabel);
                    sb.AppendLine();
                    sb.AppendLine("    <GridViewColumn.CellTemplate>");
                    sb.AppendLine("        <DataTemplate>");

                    if (field.DataType.Contains("Decimal") || field.DataType.Contains("Double")
                        || field.DataType.Contains("Integer"))
                    {
                        sb.AppendFormat(
                            "            <TextBlock TextAlignment=\"Right\" Text=\"{{Binding Path={0}, StringFormat={1}}}\" />",
                            bindingPath, field.StringFormat.Replace("{", "\\{").Replace("}", "\\}"));
                        sb.AppendLine();
                    }
                    else
                    {
                        sb.AppendFormat(
                            "            <TextBlock Text=\"{{Binding Path={0}, StringFormat={1}}}\" />",
                            bindingPath, field.StringFormat.Replace("{", "\\{").Replace("}", "\\}"));
                        sb.AppendLine();
                    }

                    sb.AppendLine("        </DataTemplate>");
                    sb.AppendLine("    </GridViewColumn.CellTemplate>");
                    sb.AppendLine("</GridViewColumn>");
                }

                sb.AppendLine();
            }

            sb.AppendLine("        </GridView>");
            sb.AppendLine("    </ListView.View>");
            sb.AppendLine("</ListView>");
            sb.Replace(" >", ">");
            sb.Replace("    ", " ");
            sb.Replace("   ", " ");
            sb.Replace("  ", " ");
            _businessForm = sb.ToString();
            DialogResult = true;
        }

        private void CreateSilverlightDataForm(List<ListBox> columnGroupListBox, int numberOfRows)
        {
            var sb = new StringBuilder(10240);
            sb.AppendLine(string.Empty);
            sb.AppendLine("<!-- Add to your root tag if required");
            sb.AppendLine(string.Empty);
            sb.AppendLine(
                "xmlns:controls=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls\" ");
            sb.AppendLine(
                "xmlns:dataFormToolkit=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data.DataForm.Toolkit\" ");
            sb.AppendLine(string.Empty);
            sb.AppendLine("-->");
            sb.AppendLine(string.Empty);
            sb.Append("<dataFormToolkit:DataForm  AutoGenerateFields=\"false\" ");

            if (!string.IsNullOrEmpty(txtDataFormHeader.Text))
            {
                sb.AppendFormat("Header=\"{0}\" ", txtDataFormHeader.Text);
            }

            sb.AppendLine(">");

            var sb2 = GetDataFormTemplate(columnGroupListBox, numberOfRows);

            // --------------------------------------------------------
            if (chkRenderEditTemplate.IsChecked == true)
            {
                sb.Append(sb2.ToString());
            }

            if (chkRenderReadOnlyTemplate.IsChecked == true)
            {
                sb2.AppendLine();
                sb2.AppendLine();
                sb.Append(sb2.ToString().Replace(".EditTemplate", ".ReadOnlyTemplate"));
            }

            if (chkRenderNewItemTemplate.IsChecked == true)
            {
                sb2.AppendLine();
                sb2.AppendLine();
                sb.Append(sb2.ToString().Replace(".EditTemplate", ".NewItemTemplate"));
            }

            sb.Append("</dataFormToolkit:DataForm>");
            sb.Replace(" >", ">");
            sb.Replace("    ", " ");
            sb.Replace("   ", " ");
            sb.Replace("  ", " ");
            _businessForm = sb.ToString();
        }

        private void CreateSilverlightDataForm()
        {
            // Setup data to create the form
            var columnGroupListBox = new List<ListBox>();

            foreach (Object obj in gridColumnsContainer.Children)
            {
                if (obj is ListBox)
                {
                    columnGroupListBox.Add(obj as ListBox);
                }
            }

            var numberOfColumns = columnGroupListBox.Count + 1;
            int numberOfRows = 0;

            //int lastGridRowIndex;

            foreach (var lb in columnGroupListBox)
            {
                numberOfRows = Math.Max(numberOfRows, lb.Items.Count);
            }

            // Check that the user has given us what we need to create the form
            if (numberOfColumns == 0 || numberOfRows == 0)
            {
                MessageBox.Show("You do not have any properties added to the layout.",
                    "Invalid Layout", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            //lastGridRowIndex = numberOfRows;

            // Now create the form
            CreateSilverlightDataForm(columnGroupListBox, numberOfRows);

            DialogResult = true;
        }

        private void CreateSilverlightDataGrid(ListBox listBox)
        {
            var hasDatePicker = false;
            var headerHasContent = false;
            var sb = new StringBuilder(10240);
            var sbHeader = new StringBuilder(1024);
            var dataGridTag = UIControlFactory.UIControlFactory.Instance.GetUIControl(UIControlRole.DataGrid, UIPlatform.Silverlight).MakeControlFromDefaults(string.Empty, false, string.Empty);
            var Namespace = string.Empty;

            if (dataGridTag.Contains(":"))
            {
                Namespace = dataGridTag.Substring(1, dataGridTag.IndexOf(":"));
                sbHeader.AppendLine(STR_TheFollowingNamespaceDeclarationsMayBeNecessaryF);
                sbHeader.AppendLine(string.Format(STR_XmlnsclrnamespaceSystemWindowsControlsassemblySy, Namespace.Replace(":", string.Empty)));
                headerHasContent = true;
            }

            sb.AppendLine(dataGridTag);
            sb.AppendFormat(STR_DataGridColumnsOpen, Namespace);
            sb.AppendLine();

            foreach (DynamicFormEditor objDynamicFormEditor in listBox.Items)
            {
                var objField = objDynamicFormEditor.DataContext as DynamicFormListBoxContent;
                var strBindingPath = string.Concat(txtBindingPropertyPrefix.Text, objField.BindingPath);

                if (objField.RenderAsGridTemplateColumn || objField.ControlType == DynamicFormControlType.Image || objField.ControlType == DynamicFormControlType.ComboBox || objField.ControlType == DynamicFormControlType.DatePicker)
                {
                    CreateSilverlightDataGridControl(ref hasDatePicker, sb, Namespace, objField, strBindingPath);
                }
                else
                {
                    switch (objField.ControlType)
                    {
                        case DynamicFormControlType.CheckBox:
                            sb.AppendFormat("<{0}DataGridCheckBoxColumn Header=\"{1}\" Binding=\"{{Binding {2}}}\" SortberPath=\"{2}\" /> ", Namespace, objField.AssociatedLabel, strBindingPath);
                            break;

                        case DynamicFormControlType.Label:
                        case DynamicFormControlType.TextBlock:
                            sb.AppendLine(string.Format("<{0}DataGridTextColumn IsReadOnly=\"true\" Header=\"{1}\" Binding=\"{{Binding {2}}}\" SortberPath=\"{2}\" />", Namespace, objField.AssociatedLabel, strBindingPath));
                            break;

                        case DynamicFormControlType.TextBox:
                            sb.AppendLine(string.Format("<{0}DataGridTextColumn Header=\"{1}\" Binding=\"{{Binding {2}}}\" SortberPath=\"{2}\" />", Namespace, objField.AssociatedLabel, strBindingPath));
                            break;

                        default:
                            break;
                    }

                    sb.AppendLine();
                }
            }

            sb.AppendFormat(STR_DataGridColumnsClose, Namespace);
            sb.AppendLine(GetCloseTagForControlFromDefaults(UIControlRole.DataGrid));
            sb.AppendLine();
            sb.Replace(" >", ">");
            sb.Replace("    ", " ");
            sb.Replace("   ", " ");
            sb.Replace("  ", " ");

            if (headerHasContent && hasDatePicker)
            {
                sbHeader.AppendLine("<!--xmlns:controls=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls\"-->");
            }
            else if (!headerHasContent && hasDatePicker)
            {
                sbHeader.AppendLine(STR_TheFollowingNamespaceDeclarationsMayBeNecessaryF);
                sbHeader.AppendLine("<!--xmlns:controls=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls\"-->");
                headerHasContent = true;
            }

            _businessForm = headerHasContent ? string.Concat(sbHeader.ToString(), sb.ToString()) : sb.ToString();
        }

        private void CreateSilverlightDataGrid()
        {
            ListBox listBox = null;

            foreach (var item in gridColumnsContainer.Children)
            {
                if (!(item is ListBox))
                    continue;

                listBox = item as ListBox;
                break;
            }

            if (listBox == null)
            {
                MessageBox.Show("Unable to get the ListBox used for layout.", "Missing ListBox", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (listBox.Items.Count == 0)
            {
                MessageBox.Show("You do not have any properties added to the layout.", "Invalid Layout", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            CreateSilverlightDataGrid(listBox);
            DialogResult = true;
        }

        private void CreateSilverlightDataGridControl(ref bool hasDatePicker, StringBuilder sb, string Namespace, DynamicFormListBoxContent field, string bindingPath)
        {
            //const string STR_DataTemplateOpen = "<DataTemplate>";
            //const string STR_DataTemplateClose = "</DataTemplate>";

            switch (field.ControlType)
            {
                case DynamicFormControlType.DatePicker:
                    hasDatePicker = true;
                    sb.Append(GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine(WriteSilverlightStringFomatComment(field.StringFormat));
                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeTextBlock(UIPlatform.Silverlight, null, null, bindingPath, field.StringFormat, ClassEntity.SilverlightVersion));

                    sb.Append(GetCenterField(Namespace));

                    sb.AppendLine(WriteSilverlightStringFomatComment(field.StringFormat));
                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeDatePicker(UIPlatform.Silverlight, null, null, bindingPath, field.Width));

                    sb.Append(GetFieldStop(Namespace));
                    break;

                case DynamicFormControlType.CheckBox:
                    sb.Append(GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeCheckBox(UIPlatform.Silverlight, null, null, string.Empty, bindingPath, BindingMode.TwoWay));

                    sb.Append(GetFieldStop(Namespace));
                    break;

                case DynamicFormControlType.ComboBox:
                    sb.Append(GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine("<!-- Bind Silverlight ComboBox in code after its ItemsSource has been loaded -->");
                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeComboBox(UIPlatform.Silverlight, null, null, bindingPath, BindingMode.TwoWay));

                    sb.Append(GetFieldStop(Namespace));
                    break;

                case DynamicFormControlType.Image:
                    sb.Append(GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine(string.Format(STR_ImageSource, field.BindingPath));

                    sb.Append(GetFieldStop(Namespace));
                    break;

                case DynamicFormControlType.Label:
                    sb.Append(GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine(WriteSilverlightStringFomatComment(field.StringFormat));
                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeLabel(UIPlatform.Silverlight, null, null, field.AssociatedLabel, field.StringFormat, ClassEntity.SilverlightVersion));

                    sb.Append(GetFieldStop(Namespace));
                    break;

                case DynamicFormControlType.TextBlock:
                    sb.Append(GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine(WriteSilverlightStringFomatComment(field.StringFormat));
                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeTextBlock(UIPlatform.Silverlight, null, null, field.BindingPath, field.StringFormat, ClassEntity.SilverlightVersion));

                    sb.Append(GetFieldStop(Namespace));
                    break;

                case DynamicFormControlType.TextBox:
                    sb.Append(GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine(WriteSilverlightStringFomatComment(field.StringFormat));
                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeTextBlock(UIPlatform.Silverlight, null, null, bindingPath, field.StringFormat, ClassEntity.SilverlightVersion));

                    sb.Append(GetCenterField(Namespace));

                    sb.AppendLine(WriteSilverlightStringFomatComment(field.StringFormat));
                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeTextBox(UIPlatform.Silverlight, null, null, bindingPath, BindingMode.TwoWay, field.Width, field.MaximumLength, string.Empty, field.DataType.StartsWith("Nullable"), ClassEntity.SilverlightVersion));

                    sb.Append(GetFieldStop(Namespace));
                    break;
            }
        }

        private void CreateWPFDataGrid()
        {
            ListBox objListBox = null;

            foreach (Object obj in gridColumnsContainer.Children)
            {
                if (!(obj is ListBox))
                    continue;

                objListBox = obj as ListBox;
                break;
            }

            if (objListBox == null)
            {
                MessageBox.Show("Unable to get the ListBox used for layout.", "Missing ListBox", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (objListBox.Items.Count == 0)
            {
                MessageBox.Show("You do not have any properties added to the layout.", "Invalid Layout", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var sb = new StringBuilder(10240);
            var strDataGridTag = UIControlFactory.UIControlFactory.Instance.GetUIControl(UIControlRole.DataGrid, UIPlatform.WPF).MakeControlFromDefaults(string.Empty, false, string.Empty);
            var strDataGridNamespace = string.Empty;

            if (strDataGridTag.Contains(":"))
            {
                strDataGridNamespace = strDataGridTag.Substring(1, strDataGridTag.IndexOf(":"));
                sb.AppendLine("<!--The following namespace declaration may be necessary for you to add to the root element of this XAML file.-->");
                sb.AppendLine(string.Format(STR_Xmlnshttpschemasmicrosoftcomwpf2008toolkit, strDataGridNamespace.Replace(":", string.Empty)));
            }

            sb.AppendLine(strDataGridTag);
            sb.AppendFormat(STR_DataGridColumnsOpen, strDataGridNamespace);
            sb.AppendLine();

            foreach (DynamicFormEditor objDynamicFormEditor in objListBox.Items)
            {
                var objField = objDynamicFormEditor.DataContext as DynamicFormListBoxContent;
                var strBindingPath = string.Concat(txtBindingPropertyPrefix.Text, objField.BindingPath);

                switch (objField.ControlType)
                {
                    case DynamicFormControlType.DatePicker:
                        sb.AppendLine(string.Format(STR_DataGridTemplateColumnOpen, strDataGridNamespace, objField.AssociatedLabel, strBindingPath));
                        sb.AppendLine(string.Format(STR_DataGridTemplateColumnCellTemplateOpen, strDataGridNamespace));
                        sb.AppendLine(STR_DataTemplateOpen);
                        sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeTextBlock(UIPlatform.WPF, null, null, objField.BindingPath, "{0:d}", ClassEntity.SilverlightVersion));
                        sb.AppendLine(STR_DataTemplateClose);
                        sb.AppendLine(string.Format(STR_DataGridTemplateColumnCellTemplateClose, strDataGridNamespace));
                        sb.AppendLine(string.Format(STR_DataGridTemplateColumnCellEditingTemplateOpen, strDataGridNamespace));
                        sb.AppendLine(STR_DataTemplateOpen);
                        sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeDatePicker(UIPlatform.WPF, null, null, objField.BindingPath, objField.Width));
                        sb.AppendLine(STR_DataTemplateClose);
                        sb.AppendLine(string.Format(STR_DataGridTemplateColumnCellEditingTemplateClose, strDataGridNamespace));
                        sb.AppendLine(string.Format(STR_DataGridTemplateColumnClose, strDataGridNamespace));
                        break;

                    case DynamicFormControlType.CheckBox:
                        sb.AppendFormat(STR_DataGridCheckBoxColumnBindingindingHeader, strDataGridNamespace, strBindingPath, objField.AssociatedLabel);
                        break;

                    case DynamicFormControlType.ComboBox:
                        sb.AppendFormat(STR_DataGridComboBoxColumnBindingindingHeader, strDataGridNamespace, strBindingPath, objField.AssociatedLabel);
                        break;

                    case DynamicFormControlType.Image:
                        break;

                    // will be added in the future when this ColumnType is added to the DataGrid
                    case DynamicFormControlType.Label:
                    case DynamicFormControlType.TextBlock:
                    case DynamicFormControlType.TextBox:

                        if (string.IsNullOrEmpty(objField.StringFormat))
                        {
                            sb.AppendFormat(STR_DataGridTextColumnBindingindingHeader, strDataGridNamespace, strBindingPath, objField.AssociatedLabel);
                        }
                        else
                        {
                            sb.AppendFormat(STR_DataGridTextColumnBindingindingStringFormatHeade, strDataGridNamespace, strBindingPath, objField.StringFormat.Replace("{", "\\{").Replace("}", "\\}"), objField.AssociatedLabel);
                        }
                        break;
                }

                sb.AppendLine();
            }

            sb.AppendFormat(STR_DataGridColumnsClose, strDataGridNamespace);
            sb.AppendLine(GetCloseTagForControlFromDefaults(UIControlRole.DataGrid));
            sb.AppendLine();
            sb.Replace(" >", ">");
            sb.Replace("    ", " ");
            sb.Replace("   ", " ");
            sb.Replace("  ", " ");
            _businessForm = sb.ToString();
            DialogResult = true;
        }

        private ListBox DynamicFormContentListBoxFactory(int intGridColumn)
        {
            var lb = new ListBox()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.WhiteSmoke)
            };
            lb.SetValue(DragDropHelper.IsDragSourceProperty, true);
            lb.SetValue(DragDropHelper.IsDropTargetProperty, true);
            lb.SetValue(DragDropHelper.DragDropTemplateProperty, FindResource("dynamicFormDragDropDataTemplate"));
            lb.ToolTip = "Drag properties here to create layout.";
            lb.SetValue(Grid.ColumnProperty, intGridColumn);
            return lb;
        }

        private string GetCenterField(string Namespace)
        {
            var sb = new StringBuilder(512);
            sb.AppendLine(STR_DataTemplateClose);
            sb.AppendLine(string.Format(STR_DataGridTemplateColumnCellTemplateClose, Namespace));
            sb.AppendLine(string.Format(STR_DataGridTemplateColumnCellEditingTemplateOpen, Namespace));
            sb.AppendLine(STR_DataTemplateOpen);
            return sb.ToString();
        }

        private string GetCloseTagForControlFromDefaults(UIControlRole enumUIControlRole)
        {
            return string.Format("</{0}>", UIControlFactory.UIControlFactory.Instance.GetUIControl(enumUIControlRole, ClassEntity.IsSilverlight ? UIPlatform.Silverlight : UIPlatform.WPF).ControlType);
        }

        private string GetDataFormField(int currentRow,
            int currentColumn, DynamicFormEditor dynamicFormEditor)
        {
            var sb = new StringBuilder(1024);
            var field = dynamicFormEditor.DataContext as DynamicFormListBoxContent;
            var bindingPath = string.Concat(txtBindingPropertyPrefix.Text, field.BindingPath);

            sb.AppendFormat("<dataFormToolkit:DataField Grid.Row=\"{0}\" Grid.Column=\"{1}\" ", currentRow, currentColumn);

            if (!string.IsNullOrEmpty(field.FieldDescription))
            {
                sb.AppendFormat("Description=\"{0}\" ", field.FieldDescription);
                sb.AppendFormat("DescriptionViewerPosition=\"{0}\" ", field.DescriptionViewerPosition);
            }

            if (!string.IsNullOrEmpty(field.AssociatedLabel))
            {
                sb.AppendFormat("Label=\"{0}\" ", field.AssociatedLabel);
                sb.AppendFormat("LabelPosition=\"{0}\" ", field.LabelPosition);
            }

            sb.AppendLine(">");

            var strStringFormatNotice = WriteSilverlightStringFomatComment(field.StringFormat);

            switch (field.ControlType)
            {
                case DynamicFormControlType.DatePicker:
                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeDatePicker(PlatformType, null, null, bindingPath, field.Width));
                    break;

                case DynamicFormControlType.CheckBox:
                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeCheckBox(PlatformType, null, null, field.ControlLabel, bindingPath, field.BindingMode));
                    break;

                case DynamicFormControlType.ComboBox:
                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeComboBox(PlatformType, null, null, bindingPath, field.BindingMode));
                    break;

                case DynamicFormControlType.Image:
                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeImage(PlatformType, null, null, bindingPath));
                    break;

                case DynamicFormControlType.Label:
                    if (ClassEntity.IsSilverlight && !string.IsNullOrEmpty(strStringFormatNotice))
                    {
                        sb.AppendLine(strStringFormatNotice);
                    }

                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeLabel(PlatformType, null, null, bindingPath, field.StringFormat, ClassEntity.SilverlightVersion));
                    break;

                case DynamicFormControlType.TextBlock:

                    if (ClassEntity.IsSilverlight && !string.IsNullOrEmpty(strStringFormatNotice))
                    {
                        sb.AppendLine(strStringFormatNotice);
                    }

                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeTextBlock(PlatformType, null, null, bindingPath, field.StringFormat, ClassEntity.SilverlightVersion));
                    break;

                case DynamicFormControlType.TextBox:

                    if (ClassEntity.IsSilverlight && !string.IsNullOrEmpty(strStringFormatNotice))
                    {
                        sb.AppendLine(strStringFormatNotice);
                    }

                    sb.AppendLine(UIControlFactory.UIControlFactory.Instance.MakeTextBox(PlatformType, null, null, bindingPath, BindingMode.TwoWay, field.Width, field.MaximumLength, field.StringFormat, field.DataType.StartsWith("Nullable"), ClassEntity.SilverlightVersion));
                    break;
            }

            sb.AppendLine("</dataFormToolkit:DataField>");
            return sb.ToString();
        }

        private StringBuilder GetDataFormTemplate(List<ListBox> columnGroupListBox, int numberOfRows)
        {
            // ----------------------------------------------------------------
            // this builds a separate stringbuilder that will be used to create
            //  the three templates as required.

            var sb = new StringBuilder(10240);
            sb.AppendLine("<dataFormToolkit:DataForm.EditTemplate>");
            sb.AppendLine(STR_DataTemplateOpen);
            sb.AppendLine(UIControlFactory.UIControlFactory.Instance
                .GetUIControl(UIControlRole.Grid, PlatformType).MakeControlFromDefaults(
                string.Empty, false, string.Empty));
            sb.AppendLine(STR_GridRowDefinitionsOpen);

            for (var intX = 1; intX <= numberOfRows; intX++)
            {
                sb.AppendLine(STR_RowDefinitionHeightAuto);
            }

            sb.AppendLine(STR_GridRowDefinitionsClose);
            sb.AppendLine(STR_GridColumnDefinitionsOpen1);

            for (var intX = 0; intX < columnGroupListBox.Count; intX++)
            {
                sb.AppendLine("<ColumnDefinition />");

                if (intX >= columnGroupListBox.Count - 1)
                    continue;

                // this inserts the spacer column between the groups of columns
                sb.AppendFormat(STR_ColumnDefinitionWidth, 10,
                    Environment.NewLine);
            }

            sb.AppendLine(STR_GridColumnDefinitionsClose1);
            sb.AppendLine();

            int currentRow;
            int currentColumn;

            for (var i = 0; i < columnGroupListBox.Count; i++)
            {
                currentRow = 0;
                currentColumn = i * 2;

                foreach (DynamicFormEditor dynamicFormEditor in columnGroupListBox[i].Items)
                {
                    sb.AppendLine(GetDataFormField(currentRow++, currentColumn, dynamicFormEditor));
                }
            }

            sb.AppendLine(GetCloseTagForControlFromDefaults(UIControlRole.Grid));
            sb.AppendLine(STR_DataTemplateClose);
            sb.AppendLine("</dataFormToolkit:DataForm.EditTemplate>");
            return sb;
        }

        private string GetFieldStart(string Namespace, string AssociatedLabel, string bindingPath)
        {
            var sb = new StringBuilder(512);
            sb.AppendLine(string.Format(STR_DataGridTemplateColumnOpen, Namespace, AssociatedLabel, bindingPath));
            sb.AppendLine(string.Format(STR_DataGridTemplateColumnCellTemplateOpen, Namespace));
            sb.AppendLine(STR_DataTemplateOpen);
            return sb.ToString();
        }

        private string GetFieldStop(string Namespace)
        {
            var sb = new StringBuilder(512);

            sb.AppendLine(STR_DataTemplateClose);
            sb.AppendLine(string.Format(STR_DataGridTemplateColumnCellEditingTemplateClose, Namespace));
            sb.AppendLine(string.Format(STR_DataGridTemplateColumnClose, Namespace));
            return sb.ToString();
        }

        private void hlJaime_Click(object sender, RoutedEventArgs e)
        {
            var objHyperlink = sender as Hyperlink;
            var psi = new ProcessStartInfo()
            {
                FileName = objHyperlink.NavigateUri.AbsoluteUri,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void InitialLayoutOfDynamicForms()
        {
            var objCollectionView = CollectionViewSource.GetDefaultView(_classEntity.PropertyInformation) as CollectionView;
            objCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("HasBeenUsed"));
            objCollectionView.SortDescriptions.Add(new SortDescription("HasBeenUsed", ListSortDirection.Ascending));
            objCollectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            gridColumnsContainer.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(425, GridUnitType.Pixel),
                MinWidth = 50
            });
            gridColumnsContainer.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(0, GridUnitType.Auto)
            });

            var lb = DynamicFormContentListBoxFactory(0);
            gridColumnsContainer.Children.Add(lb);
            gridColumnsContainer.Children.Add(new GridSplitter()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right
            });
            txtNumberOfColumnGroups.Text = "1";
            txtNumberOfColumnGroupsDataForm.Text = "1";
            NumberOfColumnGroups = 1;
            lbFields.ItemsSource = ClassEntity.PropertyInformation;
        }

        private void txtNumberOfColumnGroups_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int intNumberOfColumnGroups;

                if (int.TryParse((sender as TextBox).Text, out intNumberOfColumnGroups) && intNumberOfColumnGroups >= 1)
                {
                    ClearColumnsExceptFirstColumn(intNumberOfColumnGroups);
                }
                else
                {
                    MessageBox.Show("The number of column groups must be an integer greater than or equal to one, please reenter.", "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private string WriteSilverlightStringFomatComment(string strStringFormat)
        {
            if (ClassEntity.SilverlightVersion.StartsWith("3") && !string.IsNullOrEmpty(strStringFormat))
            {
                return string.Format(STR_TODOAddFormattingConverterForFormat, strStringFormat);
            }
            return string.Empty;
        }
    }
}