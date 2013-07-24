namespace XamlHelpmeet.UI.DynamicForm
{
    #region Imports

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

    #endregion

    /// <summary>
    ///     Interaction logic for CreateBusinessFromFromClassWindow.xaml
    /// </summary>
    public partial class CreateBusinessFormFromClassWindow : Window
    {
        #region Constants

        private const string STR_BUSINESSFORM = "Business Form";

        private const string STR_ButtonContentCancelPadding350350GridColumn1Margi =
            "    <Button Content=\"Cancel\" Padding=\"3.5,0,3.5,0\" Grid.Column=\"1\" Margin=\"3.5\"/>";

        private const string STR_ButtonContentOKPadding350350Margin3 =
            "    <Button Content=\"OK\" Padding=\"3.5,0,3.5,0\" Margin=\"3\" />";

        private const string STR_ButtonContentOKPadding350350Margin35 =
            "    <Button Content=\"OK\" Padding=\"3.5,0,3.5,0\" Margin=\"3.5\" />";

        private const string STR_ColumnDefinition = "        <ColumnDefinition />";

        private const string STR_ColumnDefinitionSharedSizeGroupButtons =
            "        <ColumnDefinition SharedSizeGroup=\"Buttons\" />";

        private const string STR_ColumnDefinitionWidth = "<ColumnDefinition Width=\"{0}\" />{1}";

        private const string STR_ColumnDefinitionWidthAuto = "<ColumnDefinition Width=\"Auto\" />";

        private const string STR_DataGridCheckBoxColumnBindingindingHeader =
            "<{0}DataGridCheckBoxColumn Binding=\"{{Binding {1}}}\" Header=\"{2}\"/> ";

        private const string STR_DataGridColumnsClose = "</{0}DataGrid.Columns>";

        private const string STR_DataGridColumnsOpen = "<{0}DataGrid.Columns>";

        private const string STR_DataGridComboBoxColumnBindingindingHeader =
            "<{0}DataGridComboBoxColumn Binding=\"{{Binding {1}}}\" Header=\"{2}\"/> ";

        private const string STR_DataGridTemplateColumnCellEditingTemplateClose =
            "</{0}DataGridTemplateColumn.CellEditingTemplate> ";

        private const string STR_DataGridTemplateColumnCellEditingTemplateOpen =
            "<{0}DataGridTemplateColumn.CellEditingTemplate> ";

        private const string STR_DataGridTemplateColumnCellTemplateClose = "</{0}DataGridTemplateColumn.CellTemplate> ";

        private const string STR_DataGridTemplateColumnCellTemplateOpen = "<{0}DataGridTemplateColumn.CellTemplate> ";

        private const string STR_DataGridTemplateColumnClose = "</{0}DataGridTemplateColumn> ";

        private const string STR_DataGridTemplateColumnOpen =
            "<{0}DataGridTemplateColumn Header=\"{1}\" SortberPath=\"{2}\"> ";

        private const string STR_DataGridTextColumnBindingindingHeader =
            "<{0}DataGridTextColumn Binding=\"{{Binding {1}}}\" Header=\"{2}\"/> ";

        private const string STR_DataGridTextColumnBindingindingStringFormatHeade =
            "<{0}DataGridTextColumn Binding=\"{{Binding {1}, StringFormat={2}}}\" Header=\"{3}\"/> ";

        private const string STR_DataTemplateClose = "</DataTemplate>";

        private const string STR_DataTemplateOpen = "<DataTemplate>";

        private const string STR_GridClose = "</Grid>";

        private const string STR_GridColumn0GridRow0GridColumnSpanText =
            " Grid.Column=\"0\" Grid.Row=\"0\" Grid.ColumnSpan=\"{0}\" Text=\"{1}\" ";

        private const string STR_GridColumnDefinitionsClose1 = "</Grid.ColumnDefinitions>";

        private const string STR_GridColumnDefinitionsClose2 = "    </Grid.ColumnDefinitions>";

        private const string STR_GridColumnDefinitionsOpen1 = "<Grid.ColumnDefinitions>";

        private const string STR_GridColumnDefinitionsOpen2 = "    <Grid.ColumnDefinitions>";

        private const string STR_GridGridColumn0GridRowGridColumnSpanGridIsShared =
            "<Grid Grid.Column=\"0\" Grid.Row=\"{0}\" Grid.ColumnSpan=\"{1}\" Grid.IsSharedSizeScope=\"true\" HorizontalAlignment=\"Right\">";

        private const string STR_GridGridColumn0GridRowGridColumnSpanHorizontalAl =
            "<Grid Grid.Column=\"0\" Grid.Row=\"{0}\" Grid.ColumnSpan=\"{1}\" HorizontalAlignment=\"Right\">";

        private const string STR_GridRowDefinitionsClose = "</Grid.RowDefinitions>";

        private const string STR_GridRowDefinitionsOpen = "<Grid.RowDefinitions>";

        private const string STR_ImageSource = "<Image Source=\"{0}\"/>";

        private const string STR_RowDefinitionHeightAuto = "<RowDefinition Height=\"Auto\" />";

        private const string STR_SILVERLIGHTDATAFORM = "Silverlight Data Form";

        private const string STR_SILVERLIGHTDATAGRID = "Silverlight Data Grid";

        private const string STR_TheFollowingNamespaceDeclarationsMayBeNecessaryF =
            "<!--The following namespace declarations may be necessary for you to add to the root element of this XAML file.-->";

        private const string STR_TODOAddFormattingConverterForFormat =
            "<!-- TODO - Add formatting converter for format: {0} -->";

        private const string STR_WPFDATAGRID = "WPF Data Grid";

        private const string STR_WPFLISTVIEW = "WPF ListView";

        private const string STR_XmlnsclrnamespaceSystemWindowsControlsassemblySy =
            "<!--xmlns:{0}=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data\"-->";

        private const string STR_Xmlnshttpschemasmicrosoftcomwpf2008toolkit =
            "<!--xmlns:{0}=\"http://schemas.microsoft.com/wpf/2008/toolkit\"-->";

        #endregion

        #region Fields

        public static readonly DependencyProperty ShowFullDynamicFormContentProperty =
            DependencyProperty.Register("ShowFullDynamicFormContent",
                                        typeof(bool),
                                        typeof(Window),
                                        new PropertyMetadata(true));

        private readonly ClassEntity _classEntity;

        private string _businessForm = string.Empty;

        private int _numberOfColumnGroups = 2;

        #endregion

        #region Constructors

        public CreateBusinessFormFromClassWindow(ClassEntity ClassEntity)
        {
            this.InitializeComponent();
            this._classEntity = ClassEntity;
        }

        #endregion

        #region Properties and Indexers

        public string BusinessForm { get { return this._businessForm; } }

        public ClassEntity ClassEntity { get { return this._classEntity; } }

        public int NumberOfColumnGroups
        {
            get { return this._numberOfColumnGroups; }
            set { this._numberOfColumnGroups = value; }
        }

        public UIPlatform PlatformType
        {
            get { return this.ClassEntity.IsSilverlight ? UIPlatform.Silverlight : UIPlatform.WPF; }
        }

        public bool ShowFullDynamicFormContent
        {
            get { return (bool)this.GetValue(ShowFullDynamicFormContentProperty); }
            set { this.SetValue(ShowFullDynamicFormContentProperty, value); }
        }

        #endregion

        #region Methods (private)

        private void ClearAllListBoxFields()
        {
            foreach (var item in this.gridColumnsContainer.Children)
            {
                if (item is ListBox)
                {
                    (item as ListBox).Items.Clear();
                }
            }

            foreach (var item in this.ClassEntity.PropertyInformation)
            {
                item.HasBeenUsed = false;
            }

            var collectionView =
                CollectionViewSource.GetDefaultView(this.ClassEntity.PropertyInformation) as CollectionView;

            if (collectionView == null)
            {
                return;
            }
            collectionView.Refresh();
        }

        private void ClearColumnsExceptFirstColumn(int numberOfColumnGroups)
        {
            if (this._numberOfColumnGroups == numberOfColumnGroups)
            {
                return;
            }

            if (numberOfColumnGroups > this._numberOfColumnGroups)
            {
                for (int i = this._numberOfColumnGroups; i < numberOfColumnGroups; i++)
                {
                    this.gridColumnsContainer.ColumnDefinitions.Insert(
                                                                       this.gridColumnsContainer.ColumnDefinitions.Count
                                                                       - 2,
                                                                       new ColumnDefinition
                                                                       {
                                                                           Width =
                                                                               new GridLength(425,
                                                                                              GridUnitType
                                                                                                  .Pixel),
                                                                           MinWidth = 50
                                                                       });
                    this.gridColumnsContainer.ColumnDefinitions.Insert(
                                                                       this.gridColumnsContainer.ColumnDefinitions.Count
                                                                       - 2,
                                                                       new ColumnDefinition
                                                                       {
                                                                           Width =
                                                                               new GridLength(0,
                                                                                              GridUnitType
                                                                                                  .Auto)
                                                                       });

                    ListBox lb =
                        this.DynamicFormContentListBoxFactory(this.gridColumnsContainer.ColumnDefinitions.Count - 2);
                    this.gridColumnsContainer.Children.Add(lb);

                    var objGridSplitter = new GridSplitter { HorizontalAlignment = HorizontalAlignment.Right };
                    objGridSplitter.SetValue(Grid.ColumnProperty, this.gridColumnsContainer.ColumnDefinitions.Count - 2);
                    this.gridColumnsContainer.Children.Add(objGridSplitter);
                }
            }
            else
            {
                int lastColumnIndexToKeep = (numberOfColumnGroups * 2) - 1;
                var listOfGridSplittersToRemove = new List<GridSplitter>();
                var listOfListBoxesToRemove = new List<ListBox>();

                foreach (var item in this.gridColumnsContainer.Children)
                {
                    if (item is GridSplitter)
                    {
                        if ((int)((item as GridSplitter).GetValue(Grid.ColumnProperty)) > lastColumnIndexToKeep)
                        {
                            listOfGridSplittersToRemove.Add(item as GridSplitter);
                        }
                    }
                    else if (item is ListBox)
                    {
                        if ((int)((item as ListBox).GetValue(Grid.ColumnProperty)) > lastColumnIndexToKeep)
                        {
                            listOfListBoxesToRemove.Add(item as ListBox);
                        }
                    }
                }

                foreach (var obj in listOfGridSplittersToRemove)
                {
                    this.gridColumnsContainer.Children.Remove(obj);
                }

                foreach (var objListBox in listOfListBoxesToRemove)
                {
                    foreach (DynamicFormEditor objDynamicFormEditor in objListBox.Items)
                    {
                        string strPropertyName =
                            (objDynamicFormEditor.DataContext as DynamicFormListBoxContent).BindingPath;

                        foreach (var objPi in this.ClassEntity.PropertyInformation)
                        {
                            if (objPi.Name == strPropertyName)
                            {
                                objPi.HasBeenUsed = false;
                            }
                        }
                    }

                    objListBox.Items.Clear();
                    this.gridColumnsContainer.Children.Remove(objListBox);
                }

                for (int i = this.gridColumnsContainer.ColumnDefinitions.Count - 1; i >= lastColumnIndexToKeep; i--)
                {
                    this.gridColumnsContainer.ColumnDefinitions.RemoveAt(i);
                }

                this.gridColumnsContainer.ColumnDefinitions.Add(new ColumnDefinition
                                                                {
                                                                    Width =
                                                                        new GridLength(0,
                                                                                       GridUnitType
                                                                                           .Auto)
                                                                });
            }

            var collectionView =
                CollectionViewSource.GetDefaultView(this.ClassEntity.PropertyInformation) as CollectionView;

            if (collectionView != null)
            {
                collectionView.Refresh();
            }

            this._numberOfColumnGroups = numberOfColumnGroups;
        }

        private void CreateBusinessForm()
        {
            bool isInsertingTitleRow = !string.IsNullOrEmpty(this.txtFormTitle.Text);
            var columnGroupListBox = new List<ListBox>();

            foreach (var item in this.gridColumnsContainer.Children)
            {
                if (item is ListBox)
                {
                    columnGroupListBox.Add(item as ListBox);
                }
            }

            int numberOfColumns = (columnGroupListBox.Count * 3) - 1;
            int numberOfRows = 0;
            int lastGridRowIndex;

            foreach (var listBox in columnGroupListBox)
            {
                numberOfRows = Math.Max(numberOfRows, listBox.Items.Count);
            }

            if (numberOfColumns == 0 || numberOfRows == 0)
            {
                MessageBox.Show("You do not have any properties added to the layout.",
                                "Invalid Layout",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }

            lastGridRowIndex = numberOfRows;

            var sb = new StringBuilder(10240);

            if (this.chkWrapInBorder.IsChecked.HasValue && this.chkWrapInBorder.IsChecked.Value)
            {
                sb.AppendLine(
                              UIControlFactory.Instance.GetUIControl(UIControlRole.Border, this.PlatformType)
                                              .MakeControlFromDefaults(string.Empty, false, string.Empty));
            }

            sb.AppendLine(
                          UIControlFactory.Instance.GetUIControl(UIControlRole.Grid, this.PlatformType)
                                          .MakeControlFromDefaults(string.Empty, false, string.Empty));
            sb.AppendLine(STR_GridRowDefinitionsOpen);

            if (isInsertingTitleRow)
            {
                sb.AppendLine(STR_RowDefinitionHeightAuto);
                lastGridRowIndex += 1;
            }

            for (int intX = 1; intX <= numberOfRows; intX++)
            {
                sb.AppendLine(STR_RowDefinitionHeightAuto);
            }

            if (this.chkIncludeButtonRow.IsChecked.HasValue && this.chkIncludeButtonRow.IsChecked.Value)
            {
                sb.AppendLine(STR_RowDefinitionHeightAuto);
            }

            sb.AppendLine(STR_GridRowDefinitionsClose);
            sb.AppendLine(STR_GridColumnDefinitionsOpen1);

            for (int intX = 0; intX < columnGroupListBox.Count; intX++)
            {
                sb.AppendFormat(STR_ColumnDefinitionWidth, 100, Environment.NewLine);
                sb.AppendLine(STR_ColumnDefinitionWidthAuto);

                if (intX >= columnGroupListBox.Count - 1)
                {
                    continue;
                }

                // this inserts the spacer column between the groups of columns
                sb.AppendFormat(STR_ColumnDefinitionWidth, 10, Environment.NewLine);
            }

            sb.AppendLine(STR_GridColumnDefinitionsClose1);
            sb.AppendLine();

            if (isInsertingTitleRow)
            {
                sb.AppendLine(
                              UIControlFactory.Instance.GetUIControl(UIControlRole.TextBlock, this.PlatformType)
                                              .MakeControlFromDefaults(
                                                                       string.Format(
                                                                                     STR_GridColumn0GridRow0GridColumnSpanText,
                                                                                     numberOfColumns,
                                                                                     this.txtFormTitle.Text),
                                                                       true,
                                                                       string.Empty));
            }

            int currentRow;

            for (int i = 0; i < columnGroupListBox.Count; i++)
            {
                currentRow = isInsertingTitleRow ? 1 : 0;

                foreach (DynamicFormEditor objDynamicFormEditor in columnGroupListBox[i].Items)
                {
                    var objField = (objDynamicFormEditor.DataContext as DynamicFormListBoxContent);

                    if (!string.IsNullOrEmpty(objField.AssociatedLabel))
                    {
                        sb.AppendLine(UIControlFactory.Instance.MakeLabelWithoutBinding(this.PlatformType,
                                                                                        i * 3,
                                                                                        currentRow,
                                                                                        objField.AssociatedLabel));
                    }

                    currentRow += 1;
                }

                sb.AppendLine();
            }

            sb.AppendLine();

            for (int i = 0; i < columnGroupListBox.Count; i++)
            {
                currentRow = isInsertingTitleRow ? 1 : 0;

                foreach (DynamicFormEditor objDynamicFormEditor in columnGroupListBox[i].Items)
                {
                    var field = objDynamicFormEditor.DataContext as DynamicFormListBoxContent;
                    string bindingPath = string.Concat(this.txtBindingPropertyPrefix.Text, field.BindingPath);

                    switch (field.ControlType)
                    {
                        case DynamicFormControlType.DatePicker:
                            sb.AppendLine(UIControlFactory.Instance.MakeDatePicker(this.PlatformType,
                                                                                   (i * 3) + 1,
                                                                                   currentRow,
                                                                                   bindingPath,
                                                                                   field.Width));
                            break;

                        case DynamicFormControlType.CheckBox:
                            sb.AppendLine(UIControlFactory.Instance.MakeCheckBox(this.PlatformType,
                                                                                 (i * 3) + 1,
                                                                                 currentRow,
                                                                                 field.ControlLabel,
                                                                                 bindingPath,
                                                                                 field.BindingMode));
                            break;

                        case DynamicFormControlType.ComboBox:
                            sb.AppendLine(UIControlFactory.Instance.MakeComboBox(this.PlatformType,
                                                                                 (i * 3) + 1,
                                                                                 currentRow,
                                                                                 bindingPath,
                                                                                 field.BindingMode));
                            break;

                        case DynamicFormControlType.Image:
                            sb.AppendLine(UIControlFactory.Instance.MakeImage(this.PlatformType,
                                                                              (i * 3) + 1,
                                                                              currentRow,
                                                                              bindingPath));
                            break;

                        case DynamicFormControlType.Label:
                            if (this.ClassEntity.IsSilverlight)
                            {
                                sb.AppendLine(this.WriteSilverlightStringFomatComment(field.StringFormat));
                            }

                            sb.AppendLine(UIControlFactory.Instance.MakeLabel(this.PlatformType,
                                                                              (i * 3) + 1,
                                                                              currentRow,
                                                                              bindingPath,
                                                                              field.StringFormat,
                                                                              this.ClassEntity.SilverlightVersion));
                            break;

                        case DynamicFormControlType.TextBlock:
                            if (this.ClassEntity.IsSilverlight)
                            {
                                sb.AppendLine(this.WriteSilverlightStringFomatComment(field.StringFormat));
                            }

                            sb.AppendLine(UIControlFactory.Instance.MakeTextBlock(this.PlatformType,
                                                                                  (i * 3) + 1,
                                                                                  currentRow,
                                                                                  bindingPath,
                                                                                  field.StringFormat,
                                                                                  this.ClassEntity.SilverlightVersion));
                            break;

                        case DynamicFormControlType.TextBox:
                            if (this.ClassEntity.IsSilverlight)
                            {
                                sb.AppendLine(this.WriteSilverlightStringFomatComment(field.StringFormat));
                            }

                            sb.AppendLine(UIControlFactory.Instance.MakeTextBox(this.PlatformType,
                                                                                (i * 3) + 1,
                                                                                currentRow,
                                                                                bindingPath,
                                                                                BindingMode.TwoWay,
                                                                                field.Width,
                                                                                field.MaximumLength,
                                                                                field.StringFormat,
                                                                                field.DataType.StartsWith("Nullable"),
                                                                                this.ClassEntity.SilverlightVersion));
                            break;
                    }

                    currentRow++;
                }

                sb.AppendLine();
            }

            if (this.chkIncludeButtonRow.IsChecked.HasValue && this.chkIncludeButtonRow.IsChecked == true)
            {
                if (!this.ClassEntity.IsSilverlight)
                {
                    sb.AppendFormat(STR_GridGridColumn0GridRowGridColumnSpanGridIsShared,
                                    lastGridRowIndex,
                                    numberOfColumns);
                    sb.AppendLine();
                    sb.AppendLine(STR_GridColumnDefinitionsOpen2);
                    sb.AppendLine(STR_ColumnDefinitionSharedSizeGroupButtons);
                    sb.AppendLine(STR_ColumnDefinitionSharedSizeGroupButtons);
                    sb.AppendLine(STR_GridColumnDefinitionsClose2);
                    sb.AppendLine(STR_ButtonContentOKPadding350350Margin3);
                    sb.AppendLine(STR_ButtonContentCancelPadding350350GridColumn1Margi);
                    sb.AppendLine(STR_GridClose);
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendFormat(STR_GridGridColumn0GridRowGridColumnSpanHorizontalAl,
                                    lastGridRowIndex,
                                    numberOfColumns);
                    sb.AppendLine();
                    sb.AppendLine(STR_GridColumnDefinitionsOpen2);
                    sb.AppendLine(STR_ColumnDefinition);
                    sb.AppendLine(STR_ColumnDefinition);
                    sb.AppendLine(STR_GridColumnDefinitionsClose2);
                    sb.AppendLine(STR_ButtonContentOKPadding350350Margin35);
                    sb.AppendLine(STR_ButtonContentCancelPadding350350GridColumn1Margi);
                    sb.AppendLine(STR_GridClose);
                    sb.AppendLine();
                }
            }

            sb.Replace(" >", ">");
            sb.Replace("    ", " ");
            sb.Replace("   ", " ");
            sb.Replace("  ", " ");
            sb.AppendLine(this.GetCloseTagForControlFromDefaults(UIControlRole.Grid));

            if (this.chkWrapInBorder.IsChecked.HasValue && this.chkWrapInBorder.IsChecked.Value)
            {
                sb.AppendLine(this.GetCloseTagForControlFromDefaults(UIControlRole.Border));
            }

            this._businessForm = sb.ToString();
            this.DialogResult = true;
        }

        private void CreateBusinessFormFromClass_Loaded(object sender, RoutedEventArgs e)
        {
            // Handles Loaded
            this.InitialLayoutOfDynamicForms();
            this.ShowFullDynamicFormContent = true;
            this.Title = string.Concat("Create Business Form For Class: ", this.ClassEntity.ClassName);

            var obj = new List<string>();
            obj.Add(STR_BUSINESSFORM);

            if (this.ClassEntity.IsSilverlight)
            {
                obj.Add(STR_SILVERLIGHTDATAGRID);
                obj.Add(STR_SILVERLIGHTDATAFORM);
            }
            else
            {
                obj.Add(STR_WPFLISTVIEW);
                obj.Add(STR_WPFDATAGRID);
            }

            this.cboSelectObjectToCreate.ItemsSource = obj;
            this.cboSelectObjectToCreate.SelectedIndex = 0;
        }

        private void CreateListView()
        {
            ListBox listBox = null;

            foreach (var obj in this.gridColumnsContainer.Children)
            {
                if (!(obj is ListBox))
                {
                    continue;
                }

                listBox = obj as ListBox;
                break;
            }

            if (listBox == null)
            {
                MessageBox.Show("Unable to get the ListBox used for layout.",
                                "Missing ListBox",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }

            if (listBox.Items.Count == 0)
            {
                MessageBox.Show("You do not have any properties added to the layout.",
                                "Invalid Layout",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }

            var sb = new StringBuilder(10240);
            sb.AppendLine("<ListView>");
            sb.AppendLine("    <ListView.ItemContainerStyle>");
            sb.AppendLine("        <Style TargetType=\"ListViewItem\">");
            sb.AppendLine("            <Setter Property=\"HorizontalContentAlignment\" Value=\"Stretch\" />");
            sb.AppendLine("        </Style>");
            sb.AppendLine("    </ListView.ItemContainerStyle>");
            sb.AppendLine("    <ListView.View>");
            sb.AppendLine("        <GridView>");

            foreach (DynamicFormEditor objDynamicFormEditor in listBox.Items)
            {
                var field = objDynamicFormEditor.DataContext as DynamicFormListBoxContent;
                string bindingPath = string.Concat(this.txtBindingPropertyPrefix.Text, field.BindingPath);

                if (string.IsNullOrEmpty(field.StringFormat))
                {
                    sb.AppendFormat("<GridViewColumn Header=\"{0}\" DisplayMemberBinding=\"{{Binding Path={1}}}\" />",
                                    field.AssociatedLabel,
                                    bindingPath);
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
                                        bindingPath,
                                        field.StringFormat.Replace("{", "\\{").Replace("}", "\\}"));
                        sb.AppendLine();
                    }
                    else
                    {
                        sb.AppendFormat("            <TextBlock Text=\"{{Binding Path={0}, StringFormat={1}}}\" />",
                                        bindingPath,
                                        field.StringFormat.Replace("{", "\\{").Replace("}", "\\}"));
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
            this._businessForm = sb.ToString();
            this.DialogResult = true;
        }

        private void CreateSilverlightDataForm(List<ListBox> columnGroupListBox, int numberOfRows)
        {
            var sb = new StringBuilder(10240);
            sb.AppendLine(string.Empty);
            sb.AppendLine("<!-- Add to your root tag if required");
            sb.AppendLine(string.Empty);
            sb.AppendLine("xmlns:controls=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls\" ");
            sb.AppendLine(
                          "xmlns:dataFormToolkit=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data.DataForm.Toolkit\" ");
            sb.AppendLine(string.Empty);
            sb.AppendLine("-->");
            sb.AppendLine(string.Empty);
            sb.Append("<dataFormToolkit:DataForm  AutoGenerateFields=\"false\" ");

            if (!string.IsNullOrEmpty(this.txtDataFormHeader.Text))
            {
                sb.AppendFormat("Header=\"{0}\" ", this.txtDataFormHeader.Text);
            }

            sb.AppendLine(">");

            StringBuilder sb2 = this.GetDataFormTemplate(columnGroupListBox, numberOfRows);

            // --------------------------------------------------------
            if (this.chkRenderEditTemplate.IsChecked == true)
            {
                sb.Append(sb2);
            }

            if (this.chkRenderReadOnlyTemplate.IsChecked == true)
            {
                sb2.AppendLine();
                sb2.AppendLine();
                sb.Append(sb2.ToString().Replace(".EditTemplate", ".ReadOnlyTemplate"));
            }

            if (this.chkRenderNewItemTemplate.IsChecked == true)
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
            this._businessForm = sb.ToString();
        }

        private void CreateSilverlightDataForm()
        {
            // Setup data to create the form
            var columnGroupListBox = new List<ListBox>();

            foreach (var obj in this.gridColumnsContainer.Children)
            {
                if (obj is ListBox)
                {
                    columnGroupListBox.Add(obj as ListBox);
                }
            }

            int numberOfColumns = columnGroupListBox.Count + 1;
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
                                "Invalid Layout",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }

            //lastGridRowIndex = numberOfRows;

            // Now create the form
            this.CreateSilverlightDataForm(columnGroupListBox, numberOfRows);

            this.DialogResult = true;
        }

        private void CreateSilverlightDataGrid(ListBox listBox)
        {
            bool hasDatePicker = false;
            bool headerHasContent = false;
            var sb = new StringBuilder(10240);
            var sbHeader = new StringBuilder(1024);
            string dataGridTag =
                UIControlFactory.Instance.GetUIControl(UIControlRole.DataGrid, UIPlatform.Silverlight)
                                .MakeControlFromDefaults(string.Empty, false, string.Empty);
            string Namespace = string.Empty;

            if (dataGridTag.Contains(":"))
            {
                Namespace = dataGridTag.Substring(1, dataGridTag.IndexOf(":"));
                sbHeader.AppendLine(STR_TheFollowingNamespaceDeclarationsMayBeNecessaryF);
                sbHeader.AppendLine(string.Format(STR_XmlnsclrnamespaceSystemWindowsControlsassemblySy,
                                                  Namespace.Replace(":", string.Empty)));
                headerHasContent = true;
            }

            sb.AppendLine(dataGridTag);
            sb.AppendFormat(STR_DataGridColumnsOpen, Namespace);
            sb.AppendLine();

            foreach (DynamicFormEditor objDynamicFormEditor in listBox.Items)
            {
                var objField = objDynamicFormEditor.DataContext as DynamicFormListBoxContent;
                string strBindingPath = string.Concat(this.txtBindingPropertyPrefix.Text, objField.BindingPath);

                if (objField.RenderAsGridTemplateColumn || objField.ControlType == DynamicFormControlType.Image
                    || objField.ControlType == DynamicFormControlType.ComboBox
                    || objField.ControlType == DynamicFormControlType.DatePicker)
                {
                    this.CreateSilverlightDataGridControl(ref hasDatePicker, sb, Namespace, objField, strBindingPath);
                }
                else
                {
                    switch (objField.ControlType)
                    {
                        case DynamicFormControlType.CheckBox:
                            sb.AppendFormat(
                                            "<{0}DataGridCheckBoxColumn Header=\"{1}\" Binding=\"{{Binding {2}}}\" SortberPath=\"{2}\" /> ",
                                            Namespace,
                                            objField.AssociatedLabel,
                                            strBindingPath);
                            break;

                        case DynamicFormControlType.Label:
                        case DynamicFormControlType.TextBlock:
                            sb.AppendLine(
                                          string.Format(
                                                        "<{0}DataGridTextColumn IsReadOnly=\"true\" Header=\"{1}\" Binding=\"{{Binding {2}}}\" SortberPath=\"{2}\" />",
                                                        Namespace,
                                                        objField.AssociatedLabel,
                                                        strBindingPath));
                            break;

                        case DynamicFormControlType.TextBox:
                            sb.AppendLine(
                                          string.Format(
                                                        "<{0}DataGridTextColumn Header=\"{1}\" Binding=\"{{Binding {2}}}\" SortberPath=\"{2}\" />",
                                                        Namespace,
                                                        objField.AssociatedLabel,
                                                        strBindingPath));
                            break;

                        default:
                            break;
                    }

                    sb.AppendLine();
                }
            }

            sb.AppendFormat(STR_DataGridColumnsClose, Namespace);
            sb.AppendLine(this.GetCloseTagForControlFromDefaults(UIControlRole.DataGrid));
            sb.AppendLine();
            sb.Replace(" >", ">");
            sb.Replace("    ", " ");
            sb.Replace("   ", " ");
            sb.Replace("  ", " ");

            if (headerHasContent && hasDatePicker)
            {
                sbHeader.AppendLine(
                                    "<!--xmlns:controls=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls\"-->");
            }
            else if (!headerHasContent && hasDatePicker)
            {
                sbHeader.AppendLine(STR_TheFollowingNamespaceDeclarationsMayBeNecessaryF);
                sbHeader.AppendLine(
                                    "<!--xmlns:controls=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls\"-->");
                headerHasContent = true;
            }

            this._businessForm = headerHasContent ? string.Concat(sbHeader.ToString(), sb.ToString()) : sb.ToString();
        }

        private void CreateSilverlightDataGrid()
        {
            ListBox listBox = null;

            foreach (var item in this.gridColumnsContainer.Children)
            {
                if (!(item is ListBox))
                {
                    continue;
                }

                listBox = item as ListBox;
                break;
            }

            if (listBox == null)
            {
                MessageBox.Show("Unable to get the ListBox used for layout.",
                                "Missing ListBox",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }

            if (listBox.Items.Count == 0)
            {
                MessageBox.Show("You do not have any properties added to the layout.",
                                "Invalid Layout",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }

            this.CreateSilverlightDataGrid(listBox);
            this.DialogResult = true;
        }

        private void CreateSilverlightDataGridControl(ref bool hasDatePicker,
                                                      StringBuilder sb,
                                                      string Namespace,
                                                      DynamicFormListBoxContent field,
                                                      string bindingPath)
        {
            //const string STR_DataTemplateOpen = "<DataTemplate>";
            //const string STR_DataTemplateClose = "</DataTemplate>";

            switch (field.ControlType)
            {
                case DynamicFormControlType.DatePicker:
                    hasDatePicker = true;
                    sb.Append(this.GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine(this.WriteSilverlightStringFomatComment(field.StringFormat));
                    sb.AppendLine(UIControlFactory.Instance.MakeTextBlock(UIPlatform.Silverlight,
                                                                          null,
                                                                          null,
                                                                          bindingPath,
                                                                          field.StringFormat,
                                                                          this.ClassEntity.SilverlightVersion));

                    sb.Append(this.GetCenterField(Namespace));

                    sb.AppendLine(this.WriteSilverlightStringFomatComment(field.StringFormat));
                    sb.AppendLine(UIControlFactory.Instance.MakeDatePicker(UIPlatform.Silverlight,
                                                                           null,
                                                                           null,
                                                                           bindingPath,
                                                                           field.Width));

                    sb.Append(this.GetFieldStop(Namespace));
                    break;

                case DynamicFormControlType.CheckBox:
                    sb.Append(this.GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine(UIControlFactory.Instance.MakeCheckBox(UIPlatform.Silverlight,
                                                                         null,
                                                                         null,
                                                                         string.Empty,
                                                                         bindingPath,
                                                                         BindingMode.TwoWay));

                    sb.Append(this.GetFieldStop(Namespace));
                    break;

                case DynamicFormControlType.ComboBox:
                    sb.Append(this.GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine("<!-- Bind Silverlight ComboBox in code after its ItemsSource has been loaded -->");
                    sb.AppendLine(UIControlFactory.Instance.MakeComboBox(UIPlatform.Silverlight,
                                                                         null,
                                                                         null,
                                                                         bindingPath,
                                                                         BindingMode.TwoWay));

                    sb.Append(this.GetFieldStop(Namespace));
                    break;

                case DynamicFormControlType.Image:
                    sb.Append(this.GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine(string.Format(STR_ImageSource, field.BindingPath));

                    sb.Append(this.GetFieldStop(Namespace));
                    break;

                case DynamicFormControlType.Label:
                    sb.Append(this.GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine(this.WriteSilverlightStringFomatComment(field.StringFormat));
                    sb.AppendLine(UIControlFactory.Instance.MakeLabel(UIPlatform.Silverlight,
                                                                      null,
                                                                      null,
                                                                      field.AssociatedLabel,
                                                                      field.StringFormat,
                                                                      this.ClassEntity.SilverlightVersion));

                    sb.Append(this.GetFieldStop(Namespace));
                    break;

                case DynamicFormControlType.TextBlock:
                    sb.Append(this.GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine(this.WriteSilverlightStringFomatComment(field.StringFormat));
                    sb.AppendLine(UIControlFactory.Instance.MakeTextBlock(UIPlatform.Silverlight,
                                                                          null,
                                                                          null,
                                                                          field.BindingPath,
                                                                          field.StringFormat,
                                                                          this.ClassEntity.SilverlightVersion));

                    sb.Append(this.GetFieldStop(Namespace));
                    break;

                case DynamicFormControlType.TextBox:
                    sb.Append(this.GetFieldStart(Namespace, field.AssociatedLabel, bindingPath));

                    sb.AppendLine(this.WriteSilverlightStringFomatComment(field.StringFormat));
                    sb.AppendLine(UIControlFactory.Instance.MakeTextBlock(UIPlatform.Silverlight,
                                                                          null,
                                                                          null,
                                                                          bindingPath,
                                                                          field.StringFormat,
                                                                          this.ClassEntity.SilverlightVersion));

                    sb.Append(this.GetCenterField(Namespace));

                    sb.AppendLine(this.WriteSilverlightStringFomatComment(field.StringFormat));
                    sb.AppendLine(UIControlFactory.Instance.MakeTextBox(UIPlatform.Silverlight,
                                                                        null,
                                                                        null,
                                                                        bindingPath,
                                                                        BindingMode.TwoWay,
                                                                        field.Width,
                                                                        field.MaximumLength,
                                                                        string.Empty,
                                                                        field.DataType.StartsWith("Nullable"),
                                                                        this.ClassEntity.SilverlightVersion));

                    sb.Append(this.GetFieldStop(Namespace));
                    break;
            }
        }

        private void CreateWPFDataGrid()
        {
            ListBox objListBox = null;

            foreach (var obj in this.gridColumnsContainer.Children)
            {
                if (!(obj is ListBox))
                {
                    continue;
                }

                objListBox = obj as ListBox;
                break;
            }

            if (objListBox == null)
            {
                MessageBox.Show("Unable to get the ListBox used for layout.",
                                "Missing ListBox",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }

            if (objListBox.Items.Count == 0)
            {
                MessageBox.Show("You do not have any properties added to the layout.",
                                "Invalid Layout",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }

            var sb = new StringBuilder(10240);
            string strDataGridTag =
                UIControlFactory.Instance.GetUIControl(UIControlRole.DataGrid, UIPlatform.WPF)
                                .MakeControlFromDefaults(string.Empty, false, string.Empty);
            string strDataGridNamespace = string.Empty;

            if (strDataGridTag.Contains(":"))
            {
                strDataGridNamespace = strDataGridTag.Substring(1, strDataGridTag.IndexOf(":"));
                sb.AppendLine(
                              "<!--The following namespace declaration may be necessary for you to add to the root element of this XAML file.-->");
                sb.AppendLine(string.Format(STR_Xmlnshttpschemasmicrosoftcomwpf2008toolkit,
                                            strDataGridNamespace.Replace(":", string.Empty)));
            }

            sb.AppendLine(strDataGridTag);
            sb.AppendFormat(STR_DataGridColumnsOpen, strDataGridNamespace);
            sb.AppendLine();

            foreach (DynamicFormEditor objDynamicFormEditor in objListBox.Items)
            {
                var objField = objDynamicFormEditor.DataContext as DynamicFormListBoxContent;
                string strBindingPath = string.Concat(this.txtBindingPropertyPrefix.Text, objField.BindingPath);

                switch (objField.ControlType)
                {
                    case DynamicFormControlType.DatePicker:
                        sb.AppendLine(string.Format(STR_DataGridTemplateColumnOpen,
                                                    strDataGridNamespace,
                                                    objField.AssociatedLabel,
                                                    strBindingPath));
                        sb.AppendLine(string.Format(STR_DataGridTemplateColumnCellTemplateOpen, strDataGridNamespace));
                        sb.AppendLine(STR_DataTemplateOpen);
                        sb.AppendLine(UIControlFactory.Instance.MakeTextBlock(UIPlatform.WPF,
                                                                              null,
                                                                              null,
                                                                              objField.BindingPath,
                                                                              "{0:d}",
                                                                              this.ClassEntity.SilverlightVersion));
                        sb.AppendLine(STR_DataTemplateClose);
                        sb.AppendLine(string.Format(STR_DataGridTemplateColumnCellTemplateClose, strDataGridNamespace));
                        sb.AppendLine(string.Format(STR_DataGridTemplateColumnCellEditingTemplateOpen,
                                                    strDataGridNamespace));
                        sb.AppendLine(STR_DataTemplateOpen);
                        sb.AppendLine(UIControlFactory.Instance.MakeDatePicker(UIPlatform.WPF,
                                                                               null,
                                                                               null,
                                                                               objField.BindingPath,
                                                                               objField.Width));
                        sb.AppendLine(STR_DataTemplateClose);
                        sb.AppendLine(string.Format(STR_DataGridTemplateColumnCellEditingTemplateClose,
                                                    strDataGridNamespace));
                        sb.AppendLine(string.Format(STR_DataGridTemplateColumnClose, strDataGridNamespace));
                        break;

                    case DynamicFormControlType.CheckBox:
                        sb.AppendFormat(STR_DataGridCheckBoxColumnBindingindingHeader,
                                        strDataGridNamespace,
                                        strBindingPath,
                                        objField.AssociatedLabel);
                        break;

                    case DynamicFormControlType.ComboBox:
                        sb.AppendFormat(STR_DataGridComboBoxColumnBindingindingHeader,
                                        strDataGridNamespace,
                                        strBindingPath,
                                        objField.AssociatedLabel);
                        break;

                    case DynamicFormControlType.Image:
                        break;

                        // will be added in the future when this ColumnType is added to the DataGrid
                    case DynamicFormControlType.Label:
                    case DynamicFormControlType.TextBlock:
                    case DynamicFormControlType.TextBox:

                        if (string.IsNullOrEmpty(objField.StringFormat))
                        {
                            sb.AppendFormat(STR_DataGridTextColumnBindingindingHeader,
                                            strDataGridNamespace,
                                            strBindingPath,
                                            objField.AssociatedLabel);
                        }
                        else
                        {
                            sb.AppendFormat(STR_DataGridTextColumnBindingindingStringFormatHeade,
                                            strDataGridNamespace,
                                            strBindingPath,
                                            objField.StringFormat.Replace("{", "\\{").Replace("}", "\\}"),
                                            objField.AssociatedLabel);
                        }
                        break;
                }

                sb.AppendLine();
            }

            sb.AppendFormat(STR_DataGridColumnsClose, strDataGridNamespace);
            sb.AppendLine(this.GetCloseTagForControlFromDefaults(UIControlRole.DataGrid));
            sb.AppendLine();
            sb.Replace(" >", ">");
            sb.Replace("    ", " ");
            sb.Replace("   ", " ");
            sb.Replace("  ", " ");
            this._businessForm = sb.ToString();
            this.DialogResult = true;
        }

        private ListBox DynamicFormContentListBoxFactory(int intGridColumn)
        {
            var lb = new ListBox
                     {
                         HorizontalAlignment = HorizontalAlignment.Stretch,
                         HorizontalContentAlignment = HorizontalAlignment.Stretch,
                         VerticalAlignment = VerticalAlignment.Stretch,
                         Background = new SolidColorBrush(Colors.WhiteSmoke)
                     };
            lb.SetValue(DragDropHelper.IsDragSourceProperty, true);
            lb.SetValue(DragDropHelper.IsDropTargetProperty, true);
            lb.SetValue(DragDropHelper.DragDropTemplateProperty, this.FindResource("dynamicFormDragDropDataTemplate"));
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
            return string.Format("</{0}>",
                                 UIControlFactory.Instance.GetUIControl(enumUIControlRole,
                                                                        this.ClassEntity.IsSilverlight
                                                                            ? UIPlatform.Silverlight
                                                                            : UIPlatform.WPF).ControlType);
        }

        private string GetDataFormField(int currentRow, int currentColumn, DynamicFormEditor dynamicFormEditor)
        {
            var sb = new StringBuilder(1024);
            var field = dynamicFormEditor.DataContext as DynamicFormListBoxContent;
            string bindingPath = string.Concat(this.txtBindingPropertyPrefix.Text, field.BindingPath);

            sb.AppendFormat("<dataFormToolkit:DataField Grid.Row=\"{0}\" Grid.Column=\"{1}\" ",
                            currentRow,
                            currentColumn);

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

            string strStringFormatNotice = this.WriteSilverlightStringFomatComment(field.StringFormat);

            switch (field.ControlType)
            {
                case DynamicFormControlType.DatePicker:
                    sb.AppendLine(UIControlFactory.Instance.MakeDatePicker(this.PlatformType,
                                                                           null,
                                                                           null,
                                                                           bindingPath,
                                                                           field.Width));
                    break;

                case DynamicFormControlType.CheckBox:
                    sb.AppendLine(UIControlFactory.Instance.MakeCheckBox(this.PlatformType,
                                                                         null,
                                                                         null,
                                                                         field.ControlLabel,
                                                                         bindingPath,
                                                                         field.BindingMode));
                    break;

                case DynamicFormControlType.ComboBox:
                    sb.AppendLine(UIControlFactory.Instance.MakeComboBox(this.PlatformType,
                                                                         null,
                                                                         null,
                                                                         bindingPath,
                                                                         field.BindingMode));
                    break;

                case DynamicFormControlType.Image:
                    sb.AppendLine(UIControlFactory.Instance.MakeImage(this.PlatformType, null, null, bindingPath));
                    break;

                case DynamicFormControlType.Label:
                    if (this.ClassEntity.IsSilverlight && !string.IsNullOrEmpty(strStringFormatNotice))
                    {
                        sb.AppendLine(strStringFormatNotice);
                    }

                    sb.AppendLine(UIControlFactory.Instance.MakeLabel(this.PlatformType,
                                                                      null,
                                                                      null,
                                                                      bindingPath,
                                                                      field.StringFormat,
                                                                      this.ClassEntity.SilverlightVersion));
                    break;

                case DynamicFormControlType.TextBlock:

                    if (this.ClassEntity.IsSilverlight && !string.IsNullOrEmpty(strStringFormatNotice))
                    {
                        sb.AppendLine(strStringFormatNotice);
                    }

                    sb.AppendLine(UIControlFactory.Instance.MakeTextBlock(this.PlatformType,
                                                                          null,
                                                                          null,
                                                                          bindingPath,
                                                                          field.StringFormat,
                                                                          this.ClassEntity.SilverlightVersion));
                    break;

                case DynamicFormControlType.TextBox:

                    if (this.ClassEntity.IsSilverlight && !string.IsNullOrEmpty(strStringFormatNotice))
                    {
                        sb.AppendLine(strStringFormatNotice);
                    }

                    sb.AppendLine(UIControlFactory.Instance.MakeTextBox(this.PlatformType,
                                                                        null,
                                                                        null,
                                                                        bindingPath,
                                                                        BindingMode.TwoWay,
                                                                        field.Width,
                                                                        field.MaximumLength,
                                                                        field.StringFormat,
                                                                        field.DataType.StartsWith("Nullable"),
                                                                        this.ClassEntity.SilverlightVersion));
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
            sb.AppendLine(
                          UIControlFactory.Instance.GetUIControl(UIControlRole.Grid, this.PlatformType)
                                          .MakeControlFromDefaults(string.Empty, false, string.Empty));
            sb.AppendLine(STR_GridRowDefinitionsOpen);

            for (int intX = 1; intX <= numberOfRows; intX++)
            {
                sb.AppendLine(STR_RowDefinitionHeightAuto);
            }

            sb.AppendLine(STR_GridRowDefinitionsClose);
            sb.AppendLine(STR_GridColumnDefinitionsOpen1);

            for (int intX = 0; intX < columnGroupListBox.Count; intX++)
            {
                sb.AppendLine("<ColumnDefinition />");

                if (intX >= columnGroupListBox.Count - 1)
                {
                    continue;
                }

                // this inserts the spacer column between the groups of columns
                sb.AppendFormat(STR_ColumnDefinitionWidth, 10, Environment.NewLine);
            }

            sb.AppendLine(STR_GridColumnDefinitionsClose1);
            sb.AppendLine();

            int currentRow;
            int currentColumn;

            for (int i = 0; i < columnGroupListBox.Count; i++)
            {
                currentRow = 0;
                currentColumn = i * 2;

                foreach (DynamicFormEditor dynamicFormEditor in columnGroupListBox[i].Items)
                {
                    sb.AppendLine(this.GetDataFormField(currentRow++, currentColumn, dynamicFormEditor));
                }
            }

            sb.AppendLine(this.GetCloseTagForControlFromDefaults(UIControlRole.Grid));
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

        private void InitialLayoutOfDynamicForms()
        {
            var objCollectionView =
                CollectionViewSource.GetDefaultView(this._classEntity.PropertyInformation) as CollectionView;
            objCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("HasBeenUsed"));
            objCollectionView.SortDescriptions.Add(new SortDescription("HasBeenUsed", ListSortDirection.Ascending));
            objCollectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            this.gridColumnsContainer.ColumnDefinitions.Add(new ColumnDefinition
                                                            {
                                                                Width =
                                                                    new GridLength(425,
                                                                                   GridUnitType
                                                                                       .Pixel),
                                                                MinWidth = 50
                                                            });
            this.gridColumnsContainer.ColumnDefinitions.Add(new ColumnDefinition
                                                            {
                                                                Width =
                                                                    new GridLength(0,
                                                                                   GridUnitType
                                                                                       .Auto)
                                                            });

            ListBox lb = this.DynamicFormContentListBoxFactory(0);
            this.gridColumnsContainer.Children.Add(lb);
            this.gridColumnsContainer.Children.Add(new GridSplitter { HorizontalAlignment = HorizontalAlignment.Right });
            this.txtNumberOfColumnGroups.Text = "1";
            this.txtNumberOfColumnGroupsDataForm.Text = "1";
            this.NumberOfColumnGroups = 1;
            this.lbFields.ItemsSource = this.ClassEntity.PropertyInformation;
        }

        private string WriteSilverlightStringFomatComment(string strStringFormat)
        {
            if (this.ClassEntity.SilverlightVersion.StartsWith("3") && !string.IsNullOrEmpty(strStringFormat))
            {
                return string.Format(STR_TODOAddFormattingConverterForFormat, strStringFormat);
            }
            return string.Empty;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) { this.DialogResult = false; }

        private void btnClearnAllFields_Click(object sender, RoutedEventArgs e) { this.ClearAllListBoxFields(); }

        private void btnCreateForm_Click(object sender, RoutedEventArgs e)
        {
            this.txtBindingPropertyPrefix.Text = this.txtBindingPropertyPrefix.Text.Trim();

            if (this.txtBindingPropertyPrefix.Text.Length > 0 && !this.txtBindingPropertyPrefix.Text.EndsWith("."))
            {
                this.txtBindingPropertyPrefix.Text += ".";
            }

            switch (this.cboSelectObjectToCreate.SelectedValue as string)
            {
                case STR_BUSINESSFORM:
                    this.CreateBusinessForm();
                    break;

                case STR_WPFLISTVIEW:
                    this.CreateListView();
                    break;

                case STR_WPFDATAGRID:
                    this.CreateWPFDataGrid();
                    break;

                case STR_SILVERLIGHTDATAGRID:
                    this.CreateSilverlightDataGrid();
                    break;

                case STR_SILVERLIGHTDATAFORM:
                    this.CreateSilverlightDataForm();
                    break;

                default:
                    MessageBox.Show(
                                    string.Format("Selection {0}, not implemented",
                                                  this.cboSelectObjectToCreate.SelectedValue),
                                    "Not Implemented",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation,
                                    MessageBoxResult.OK);
                    break;
            }
        }

        private void cboSelectObjectToCreate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cboSelectObjectToCreate == null || this.wpBusinessForm == null
                || this.cboSelectObjectToCreate.SelectedIndex == -1)
            {
                return;
            }

            this.wpBusinessForm.Visibility = Visibility.Collapsed;
            this.wpListView.Visibility = Visibility.Collapsed;
            this.wpWPFDataGrid.Visibility = Visibility.Collapsed;
            this.wpSilverlightDataGrid.Visibility = Visibility.Collapsed;
            this.wpSilverlightDataForm.Visibility = Visibility.Collapsed;

            switch (this.cboSelectObjectToCreate.SelectedValue as string)
            {
                case STR_BUSINESSFORM:
                    this.wpBusinessForm.Visibility = Visibility.Visible;
                    break;

                case STR_WPFLISTVIEW:
                    this.wpListView.Visibility = Visibility.Visible;
                    break;

                case STR_WPFDATAGRID:
                    this.wpWPFDataGrid.Visibility = Visibility.Visible;
                    break;

                case STR_SILVERLIGHTDATAGRID:
                    this.wpSilverlightDataGrid.Visibility = Visibility.Visible;
                    break;

                case STR_SILVERLIGHTDATAFORM:
                    this.wpSilverlightDataForm.Visibility = Visibility.Visible;
                    break;

                default:
                    throw new Exception("Unexpected condition.");
            }

            this.ClearAllListBoxFields();
            this.ClearColumnsExceptFirstColumn(1);
        }

        private void hlJaime_Click(object sender, RoutedEventArgs e)
        {
            var objHyperlink = sender as Hyperlink;
            var psi = new ProcessStartInfo { FileName = objHyperlink.NavigateUri.AbsoluteUri, UseShellExecute = true };
            Process.Start(psi);
        }

        private void txtNumberOfColumnGroups_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int intNumberOfColumnGroups;

                if (int.TryParse((sender as TextBox).Text, out intNumberOfColumnGroups) && intNumberOfColumnGroups >= 1)
                {
                    this.ClearColumnsExceptFirstColumn(intNumberOfColumnGroups);
                }
                else
                {
                    MessageBox.Show(
                                    "The number of column groups must be an integer greater than or equal to one, please reenter.",
                                    "Invalid Data",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation);
                }
            }
        }

        #endregion

        #region Nested type: SelectClassberUserControlState

        private enum SelectClassberUserControlState
        {
            Minimized,

            Restored
        }

        #endregion
    }
}
