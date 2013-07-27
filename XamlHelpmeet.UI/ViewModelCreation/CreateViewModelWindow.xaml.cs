﻿namespace XamlHelpmeet.UI.ViewModelCreation
{
    #region Imports

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using XamlHelpmeet.Extensions;
    using XamlHelpmeet.Model;
    using XamlHelpmeet.UI.Commands;

    #endregion

    /// <summary>
    ///     Interaction logic for CreateViewModelWindow.xaml.
    /// </summary>
    public partial class CreateViewModelWindow : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        ///     The class entity.
        /// </summary>
        private readonly ClassEntity _classEntity;

        /// <summary>
        ///     Collection of commands.
        /// </summary>
        private readonly ObservableCollection<CreateCommandSource> _commandsCollection =
            new ObservableCollection<CreateCommandSource>();

        /// <summary>
        ///     true if this CreateViewModelWindow is VB.
        /// </summary>
        private readonly bool _isVB;

        /// <summary>
        ///     The create command.
        /// </summary>
        private ICommand _createCommand;

        /// <summary>
        ///     Name of the field.
        /// </summary>
        private string _fieldName = string.Empty;

        /// <summary>
        ///     true if this CreateViewModelWindow has private setter.
        /// </summary>
        private bool _hasPrivateSetter;

        /// <summary>
        ///     true to include, false to exclude the on property changed.
        /// </summary>
        private bool _includeOnPropertyChanged;

        /// <summary>
        ///     true to include, false to exclude the on property changed event handler.
        /// </summary>
        private bool _includeOnPropertyChangedEventHandler = true;

        /// <summary>
        ///     true if this CreateViewModelWindow is property public.
        /// </summary>
        private bool _isPropertyPublic = true;

        /// <summary>
        ///     true if this CreateViewModelWindow is property read only.
        /// </summary>
        private bool _isPropertyReadOnly;

        /// <summary>
        ///     Name of the on property changed method.
        /// </summary>
        private string _onPropertyChangedMethodName = "RaisePropertyChanged";

        /// <summary>
        ///     Name of the property.
        /// </summary>
        private string _propertyName = string.Empty;

        /// <summary>
        ///     The property signature.
        /// </summary>
        private string _propertySignature = "Public Property";

        /// <summary>
        ///     Type of the property.
        /// </summary>
        private string _propertyType = string.Empty;

        /// <summary>
        ///     true to use hungarian notation for private fields.
        /// </summary>
        private bool _useHungarianNotationForPrivateFields;

        /// <summary>
        ///     The view model text.
        /// </summary>
        private string _viewModelText = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the CreateViewModelWindow class.
        /// </summary>
        /// <param name="ClassEntity">
        ///     The class entity.
        /// </param>
        /// <param name="IsVisualBasic">
        ///     true if this CreateViewModelWindow is visual basic.
        /// </param>
        public CreateViewModelWindow(ClassEntity ClassEntity, bool IsVisualBasic)
        {
            this._classEntity = ClassEntity;
            this._isVB = IsVisualBasic;

            string className = ClassEntity.ClassName;
            this._propertyType = className;
            this._propertyName = className;

            this._fieldName = this.IsVB
                                  ? "_obj" + className
                                  : String.Format("_{0}{1}", className.ToLower()[0], className.Substring(1));

            this.DataContext = this;
            this.InitializeComponent();
        }

        #endregion

        #region Properties and Indexers

        /// <summary>
        ///     Gets the class entity.
        /// </summary>
        /// <value>
        ///     The class entity.
        /// </value>
        public ClassEntity ClassEntity
        {
            get { return this._classEntity; }
        }

        /// <summary>
        ///     Gets a collection of commands.
        /// </summary>
        /// <value>
        ///     A Collection of commands.
        /// </value>
        public ObservableCollection<CreateCommandSource> CommandsCollection
        {
            get { return this._commandsCollection; }
        }

        /// <summary>
        ///     Gets the create command.
        /// </summary>
        /// <value>
        ///     The create command.
        /// </value>
        public ICommand CreateCommand
        {
            get
            {
                if (this._createCommand == null)
                {
                    this._createCommand = new RelayCommand(this.CreateExecute, this.CanCreateExecute);
                }
                return this._createCommand;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the expose properties on view model.
        /// </summary>
        /// <value>
        ///     true if expose properties on view model, otherwise false.
        /// </value>
        private bool ExposePropertiesOnViewModel
        {
            get { return this.lbProperteis.SelectedItems.Count > 0; }
        }

        /// <summary>
        ///     Gets or sets the name of the field.
        /// </summary>
        /// <value>
        ///     The name of the field.
        /// </value>
        public string FieldName
        {
            get { return this._fieldName; }
            set
            {
                this._fieldName = value;
                this.OnPropertyChanged("FieldName");
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this CreateViewModelWindow has
        ///     private setter.
        /// </summary>
        /// <value>
        ///     true if this CreateViewModelWindow has private setter, otherwise false.
        /// </value>
        public bool HasPrivateSetter
        {
            get { return this._hasPrivateSetter; }
            set
            {
                this._hasPrivateSetter = value;
                this.OnPropertyChanged("HasPrivateSetter");
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the include on property was
        ///     changed.
        /// </summary>
        /// <value>
        ///     true if include on property changed, otherwise false.
        /// </value>
        public bool IncludeOnPropertyChanged
        {
            get { return this._includeOnPropertyChanged; }
            set
            {
                this._includeOnPropertyChanged = value;
                this.OnPropertyChanged("IncludeOnPropertyChanged");
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the on property changed event
        ///     handler should be included.
        /// </summary>
        /// <value>
        ///     true if include on property changed event handler, otherwise false.
        /// </value>
        public bool IncludeOnPropertyChangedEventHandler
        {
            get { return this._includeOnPropertyChangedEventHandler; }
            set
            {
                this._includeOnPropertyChangedEventHandler = value;
                this.OnPropertyChanged("IncludeOnPropertyChangedEventHandler");
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this CreateViewModelWindow is
        ///     property public.
        /// </summary>
        /// <value>
        ///     true if this CreateViewModelWindow is property public, otherwise false.
        /// </value>
        public bool IsPropertyPublic
        {
            get { return this._isPropertyPublic; }
            set
            {
                this._isPropertyPublic = value;
                this.OnPropertyChanged("IsPropertyPublic");
                this.SetPropertySignature();
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this CreateViewModelWindow is
        ///     property read only.
        /// </summary>
        /// <value>
        ///     true if this CreateViewModelWindow is property read only, otherwise false.
        /// </value>
        public bool IsPropertyReadOnly
        {
            get { return this._isPropertyReadOnly; }
            set
            {
                this._isPropertyReadOnly = value;
                this.OnPropertyChanged("IsPropertyReadOnly");
                this.SetPropertySignature();
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this CreateViewModelWindow is VB.
        /// </summary>
        /// <value>
        ///     true if this CreateViewModelWindow is vb, otherwise false.
        /// </value>
        public bool IsVB
        {
            get { return this._isVB; }
        }

        /// <summary>
        ///     Gets or sets the name of the on property changed method.
        /// </summary>
        /// <value>
        ///     The name of the on property changed method.
        /// </value>
        public string OnPropertyChangedMethodName
        {
            get { return this._onPropertyChangedMethodName; }
            set
            {
                this._onPropertyChangedMethodName = value;
                this.OnPropertyChanged("OnPropertyChangedMethodName");
            }
        }

        /// <summary>
        ///     Gets or sets the name of the property.
        /// </summary>
        /// <value>
        ///     The name of the property.
        /// </value>
        public string PropertyName
        {
            get { return this._propertyName; }
            set
            {
                this._propertyName = value;
                this.OnPropertyChanged("PropertyName");
            }
        }

        /// <summary>
        ///     Gets or sets the property signature.
        /// </summary>
        /// <value>
        ///     The property signature.
        /// </value>
        public string PropertySignature
        {
            get { return this._propertySignature; }
            set
            {
                this._propertySignature = value;
                this.OnPropertyChanged("PropertySignature");
            }
        }

        /// <summary>
        ///     Gets or sets the type of the property.
        /// </summary>
        /// <value>
        ///     The type of the property.
        /// </value>
        public string PropertyType
        {
            get { return this._propertyType; }
            set
            {
                this._propertyType = value;
                this.OnPropertyChanged("PropertyType");
            }
        }

        /// <summary>
        ///     Gets the selected property information collection.
        /// </summary>
        /// <value>
        ///     A Collection of selected property informations.
        /// </value>
        private IEnumerable<PropertyInformation> SelectedPropertyInformationCollection
        {
            get { return from p in this.ClassEntity.PropertyInformation where p.IsSelected orderby p.Name select p; }
        }

        /// <summary>
        ///     Gets the name of the type.
        /// </summary>
        /// <value>
        ///     The name of the type.
        /// </value>
        public string TypeName
        {
            get { return this.ClassEntity.ClassName; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this CreateViewModelWindow use
        ///     hungarian notation for private fields.
        /// </summary>
        /// <value>
        ///     true if use hungarian notation for private fields, otherwise false.
        /// </value>
        public bool UseHungarianNotationForPrivateFields
        {
            get { return this._useHungarianNotationForPrivateFields; }
            set
            {
                this._useHungarianNotationForPrivateFields = value;
                this.OnPropertyChanged("UseHungarianNotationForPrivateFields");
            }
        }

        /// <summary>
        ///     Gets the view model text.
        /// </summary>
        /// <value>
        ///     The view model text.
        /// </value>
        public string ViewModelText
        {
            get { return this._viewModelText; }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        ///     Event inherited from the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods (private)

        /// <summary>
        ///     Determine if we can create execute.
        /// </summary>
        /// <param name="param">
        ///     The parameter.
        /// </param>
        /// <returns>
        ///     true if we can create execute, otherwise false.
        /// </returns>
        private bool CanCreateExecute(object param)
        {
            return
                !(this.PropertyName.IsNullOrEmpty() || this.PropertyType.IsNullOrEmpty()
                  || this.FieldName.IsNullOrEmpty());
        }

        /// <summary>
        ///     Creates c sharp view model text.
        /// </summary>
        private void CreateCSharpViewModelText()
        {
            // NOTE: This method uses multi-line string literals.
            //
            // Modify VB code
            this.PropertyType = this.TranslateVBPropertyToCSharp(this.PropertyType);

            var sb = new StringBuilder(4096);

            if (this.IncludeOnPropertyChanged)
            {
                sb.AppendLine(@"// : System.ComponentModel.INotifyPropertyChanged

// developer, please place the above at the end of your class name
");
            }

            if (this.ExposePropertiesOnViewModel)
            {
                sb.AppendLine(@"

// TODO developers please add your constructors in the below constructor region.
//      be sure to include an overloaded constructor that takes a model type.
");
            }

            sb.AppendLine(this.GetCSharpDeclarations());

            sb.AppendLine(@"#region Events

public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

#endregion");

            sb.AppendLine(this.GetCSharpProperties());

            sb.AppendLine(this.GetCSharpCommandProperties());

            sb.AppendLine(this.GetCSharpConstructors());

            sb.AppendLine(this.GetCSharpMethods());

            if (this.IncludeOnPropertyChangedEventHandler)
            {
                sb.AppendLine(this.GetCSharpINPC());
            }

            this._viewModelText = sb.ToString();
        }

        /// <summary>
        ///     Creates an execute.
        /// </summary>
        /// <param name="param">
        ///     The parameter.
        /// </param>
        private void CreateExecute(object param)
        {
            if (this.CanCreateExecute(param) == false)
            {
                return;
            }

            if (this.IsVB)
            {
                this.CreateVBViewModelText();
            }
            else
            {
                this.CreateCSharpViewModelText();
            }
            this.DialogResult = true;
        }

        /// <summary>
        ///     Creates VB view model text.
        /// </summary>
        private void CreateVBViewModelText()
        {
            var sb = new StringBuilder(4096);

            if (this.IncludeOnPropertyChanged)
            {
                sb.AppendLine("Implements System.ComponentModel.INotifyPropertyChanged");
                sb.AppendLine();
                ;
            }

            sb.AppendLine(this.GetVBDeclarations());

            sb.AppendLine(@"#region "" Events ""

Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#endregion");

            sb.AppendLine(this.GetVBProperties());

            sb.AppendLine(this.GetVBCommandProperties());

            sb.AppendLine(this.GetVBConstructors());

            sb.AppendLine(this.GetVBMethods());

            if (this.IncludeOnPropertyChangedEventHandler)
            {
                sb.AppendLine(this.GetVBINPC());
            }

            this._viewModelText = sb.ToString();
        }

        /// <summary>
        ///     Event handler. Called by CreateViewModelWindow for unloaded events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void CreateViewModelWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            this.cboPropertyType.RemoveHandler(Selector.SelectionChangedEvent,
                                               new SelectionChangedEventHandler(this.cboPropertyType_SelectionChanged));
            this.cboPropertyChangedMethodNames.RemoveHandler(Selector.SelectionChangedEvent,
                                                             new SelectionChangedEventHandler(
                                                                 this.cboPropertyChangedMethodNames_SelectionChanged));
        }

        /// <summary>
        ///     Gets c sharp command properties.
        /// </summary>
        /// <returns>
        ///     The c sharp command properties.
        /// </returns>
        private string GetCSharpCommandProperties()
        {
            var sb = new StringBuilder(4096);

            foreach (var item in (from x in this.CommandsCollection orderby x.CommandName select x))
            {
                bool usesCommandParameter = item.CommandParameterType.IsNullOrWhiteSpace();

                sb.AppendLine(String.Format("public ICommand {0}", item.CommandName));
                sb.AppendLine("{");
                sb.AppendLine("get");
                sb.AppendLine("{");
                sb.AppendLine(String.Format("if ({0} == null)", item.FieldName));
                sb.AppendLine("{");
                sb.AppendFormat("{0} = new ", item.FieldName);

                sb.Append(item.UseRelayCommand ? "RelayCommand" : "DelegateCommand");

                if (usesCommandParameter)
                {
                    sb.AppendFormat("<{0}>", item.CommandParameterType);
                }

                sb.Append("(");

                if (item.ExecuteUseAddressOf)
                {
                    sb.Append(item.ExecuteMethodName);
                }
                else
                {
                    sb.AppendFormat(usesCommandParameter ? "param => {0}(param)" : "() => {0}()", item.ExecuteMethodName);
                }

                if (item.IncludeCanExecuteMethod)
                {
                    sb.Append(", ");

                    if (item.CanExecuteUseAddressOf)
                    {
                        sb.Append(item.CanExecuteMethodName);
                    }
                    else
                    {
                        sb.AppendFormat(usesCommandParameter ? "param => {0}(param)" : "() => {0}()",
                                        item.CanExecuteMethodName);
                    }
                }

                sb.AppendLine(");");
                sb.AppendLine("}");
                sb.AppendLine(String.Format("return {0};", item.FieldName));
                sb.AppendLine("}");
                sb.AppendLine("}");
                sb.AppendLine();
            }

            return String.Format(@"#region Command Properties

{0}

#endregion Create ViewModel Text Methods

", sb);
        }

        /// <summary>
        ///     Gets c sharp constructors.
        /// </summary>
        /// <returns>
        ///     The c sharp constructors.
        /// </returns>
        private string GetCSharpConstructors()
        {
            var sb = new StringBuilder(256);
            sb.AppendLine("#region Constructors");
            sb.AppendLine();
            sb.AppendLine("//TODO developers add your constructors here");
            sb.AppendLine();
            sb.AppendLine("#endregion");
            sb.AppendLine();
            return sb.ToString();
        }

        /// <summary>
        ///     Gets c sharp declarations.
        /// </summary>
        /// <returns>
        ///     The c sharp declarations.
        /// </returns>
        private string GetCSharpDeclarations()
        {
            var sb = new StringBuilder(1024);
            foreach (var obj in (from x in this.CommandsCollection orderby x.CommandName select x))
            {
                sb.AppendFormat("ICommand {0};", obj.FieldName);
            }

            // NOTE: The following uses a multi-line string literal.
            return string.Format(@"#region Declarations

{0}

{1} {2};

#endregion

", sb, this.PropertyType, this.FieldName);
        }

        /// <summary>
        ///     Gets c sharp exposed view model properties.
        /// </summary>
        /// <returns>
        ///     The c sharp exposed view model properties.
        /// </returns>
        private string GetCSharpExposedViewModelProperties()
        {
            var sb = new StringBuilder(4096);
            foreach (var pi in this.SelectedPropertyInformationCollection)
            {
                string typeName = this.TranslateVBPropertyToCSharp(pi.TypeName);

                sb.AppendLine(pi.Name == "Item" && pi.PropertyParameters.Count == 1
                                  ? String.Format("public {0} this[{1}]", typeName, pi.CSParameterString)
                                  : pi.PropertyParameters.Count > 0
                                        ? String.Format("public {0} {1}[{2}]", typeName, pi.Name, pi.CSParameterString)
                                        : String.Format("public {0} {1}", typeName, pi.Name));

                sb.AppendLine("{");

                sb.AppendLine(pi.Name == "Item" && pi.PropertyParameters.Count == 1
                                  ? String.Format("get {{ return {0}[{1}]; }}",
                                                  this.FieldName,
                                                  pi.PropertyParameters[0].ParameterName)
                                  : String.Format("get {{ return {0}.{1}; }}", this.FieldName, pi.Name));

                if (pi.CanWrite)
                {
                    sb.AppendLine("set");
                    sb.AppendLine("{");
                    sb.AppendLine(String.Format("{0}.{1} = value;", this.FieldName, pi.Name));

                    if (this.IncludeOnPropertyChanged)
                    {
                        sb.AppendLine(String.Format("{0}(\"{1}\");", this.OnPropertyChangedMethodName, pi.Name));
                    }
                    sb.AppendLine("}");
                }

                sb.AppendLine("}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Gets c sharp inpc.
        /// </summary>
        /// <returns>
        ///     The c sharp inpc.
        /// </returns>
        [SuppressMessage("Microsoft.Usage", "CA2241:Provide correct arguments to formatting methods")]
        private string GetCSharpINPC()
        {
            return string.Format(@"#region INotifyPropertyChanged Method

protected void {0}(string propertyName)
{{
var handler = this.PropertyChanged;
if (handler != null)
{{
handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
}}
}}

#endregion

", this.OnPropertyChangedMethodName);
        }

        /// <summary>
        ///     Gets c sharp methods.
        /// </summary>
        /// <returns>
        ///     The c sharp methods.
        /// </returns>
        private string GetCSharpMethods()
        {
            var sb = new StringBuilder(4096);

            foreach (var item in (from x in this.CommandsCollection orderby x.CommandName select x))
            {
                bool usesCommandParameter = item.CommandParameterType.IsNotNullOrWhiteSpace();

                if (item.IncludeCanExecuteMethod)
                {
                    sb.AppendLine(usesCommandParameter
                                      ? String.Format("bool {0}({1} param)",
                                                      item.CanExecuteMethodName,
                                                      item.CommandParameterType)
                                      : String.Format("bool {0}()", item.CanExecuteMethodName));

                    sb.AppendLine("{");
                    sb.AppendLine("}");
                }

                sb.AppendLine();

                sb.AppendLine(usesCommandParameter
                                  ? String.Format("void {0}({1} param)",
                                                  item.ExecuteMethodName,
                                                  item.CommandParameterType)
                                  : String.Format("void {0}()", item.ExecuteMethodName));

                sb.AppendLine("{");
                sb.AppendLine("}");
                sb.AppendLine();
            }

            return String.Format(@"#region CommandMethoods

{0}

#endregion

", sb);
        }

        /// <summary>
        ///     Gets c sharp properties.
        /// </summary>
        /// <returns>
        ///     The c sharp properties.
        /// </returns>
        private string GetCSharpProperties()
        {
            // 0 - Visibility
            // 1 - Type
            // 2 - Name
            // 3 - Field name
            // 4 - Set accessor
            string propertyWrapper = @"#region Properties

{0} {1} {2}
{{
get {{ return {3}; }}{4}
}}
";

            // 0 - Visibility
            // 1 - Field name
            // 2 - OnPropertyChanged call
            string setterWrapper = this.IsPropertyReadOnly ? string.Empty : @"
{0}set
{{
{1} = value;{2}
}}
";

            // 0 - OPC Method name
            // 1 - Property name
            string opcWrapper = this.IncludeOnPropertyChanged ? @"
{0}(""{1}"");" : string.Empty;

            string propertyText = string.Format(propertyWrapper,
                                                this.IsPropertyPublic ? "public" : "private",
                                                this.PropertyType,
                                                this.PropertyName,
                                                this.FieldName,
                                                string.Format(setterWrapper,
                                                              this.HasPrivateSetter ? "private " : string.Empty,
                                                              this.FieldName,
                                                              string.Format(opcWrapper,
                                                                            this.OnPropertyChangedMethodName,
                                                                            this.PropertyName)));

            string exposedProperties = this.ExposePropertiesOnViewModel
                                           ? this.GetCSharpExposedViewModelProperties()
                                           : string.Empty;

            return string.Format(@"{0}{1}

#endregion

", propertyText, exposedProperties);
        }

        /// <summary>
        ///     Gets property types.
        /// </summary>
        /// <returns>
        ///     The property types.
        /// </returns>
        private IEnumerable GetPropertyTypes()
        {
            var propertyTypes = new List<string>();

            propertyTypes.Add(this.TypeName);
            propertyTypes.Add(String.Format("List(Of {0})", this.TypeName));
            propertyTypes.Add(String.Format("ObservableCollection(Of {0})", this.TypeName));
            propertyTypes.Add(String.Format("ReadOnlyObservableCollection(Of {0})", this.TypeName));
            propertyTypes.Add(String.Format("IEnumerable(Of {0})", this.TypeName));
            propertyTypes.Add(String.Format("IList(Of {0})", this.TypeName));

            return propertyTypes;
        }

        /// <summary>
        ///     Gets VB command properties.
        /// </summary>
        /// <returns>
        ///     The VB command properties.
        /// </returns>
        private string GetVBCommandProperties()
        {
            var sb = new StringBuilder();

            foreach (var obj in (from x in this.CommandsCollection orderby x.CommandName select x))
            {
                bool usesCommandParameter = obj.CommandParameterType.IsNullOrWhiteSpace();

                sb.AppendLine(String.Format("Public ReadOnly Property {0}() As ICommand", obj.CommandName));
                sb.AppendLine("Get");
                sb.AppendLine(String.Format("if ( {0} Is Nothing Then", obj.FieldName));
                sb.AppendFormat("{0} = New ", obj.FieldName);

                if (obj.UseRelayCommand)
                {
                    sb.Append("RelayCommand");
                }
                else
                {
                    sb.Append("DelegateCommand");
                }

                if (usesCommandParameter)
                {
                    sb.Append(String.Format("(Of {0})", obj.CommandParameterType));
                }

                sb.Append("(");

                if (obj.ExecuteUseAddressOf)
                {
                    sb.AppendFormat("AddressOf {0}", obj.ExecuteMethodName);
                }
                else if (usesCommandParameter)
                {
                    sb.AppendFormat("Sub(param As {0}) {1}(param)", obj.CommandParameterType, obj.ExecuteMethodName);
                }
                else
                {
                    sb.AppendFormat("Sub() {0}()", obj.ExecuteMethodName);
                }

                if (obj.IncludeCanExecuteMethod)
                {
                    sb.Append(", ");

                    if (obj.CanExecuteUseAddressOf)
                    {
                        sb.AppendFormat("AddressOf {0}", obj.CanExecuteMethodName);
                    }
                    else if (usesCommandParameter)
                    {
                        sb.AppendFormat("Function(param as {0}) {1}(param)",
                                        obj.CommandParameterType,
                                        obj.CanExecuteMethodName);
                    }
                    else
                    {
                        sb.AppendFormat("Function() {0}()", obj.CanExecuteMethodName);
                    }
                }
                sb.AppendLine(")");
                sb.AppendLine("}");
                sb.AppendLine(String.Format("Return {0}", obj.FieldName));
                sb.AppendLine("End Get");
                sb.AppendLine("End Property");
            }
            sb.AppendLine();
            sb.AppendLine("#End Region");
            sb.AppendLine();
            return sb.ToString();
        }

        /// <summary>
        ///     Gets VB constructors.
        /// </summary>
        /// <returns>
        ///     The VB constructors.
        /// </returns>
        private string GetVBConstructors()
        {
            var sb = new StringBuilder(1024);

            sb.AppendLine("#Region \" Constructors \"");
            sb.AppendLine();
            sb.AppendLine("Public Sub New()");
            sb.AppendLine();
            sb.AppendLine("End Sub");
            sb.AppendLine();

            if (this.ExposePropertiesOnViewModel && this.PropertyType.IndexOf("(Of") == -1
                && this.PropertyType.IndexOf("<") == -1)
            {
                sb.AppendFormat("Public Sub New({0} As {1})", this.FieldName.Replace("_", ""), this.PropertyType);
                sb.AppendLine();
                sb.AppendFormat("{0} = {1}", this.FieldName, this.FieldName.Replace("_", ""));
                sb.AppendLine();
                sb.AppendLine("End Sub");
                sb.AppendLine();
            }
            sb.AppendLine("#End Region");
            sb.AppendLine();

            return sb.ToString();
        }

        /// <summary>
        ///     Gets VB declarations.
        /// </summary>
        /// <returns>
        ///     The VB declarations.
        /// </returns>
        private string GetVBDeclarations()
        {
            var sb = new StringBuilder(1024);
            foreach (var obj in (from x in this.CommandsCollection orderby x.CommandName select x))
            {
                sb.AppendLine(string.Format("Private {0} As ICommand ", obj.FieldName));
            }
            return String.Format(@"#Region "" Declarations ""

{0}

Private {1} As {2}

#End Region

", sb, this.FieldName, this.PropertyType);
        }

        /// <summary>
        ///     Gets VB exposed view model properties.
        /// </summary>
        /// <returns>
        ///     The VB exposed view model properties.
        /// </returns>
        private string GetVBExposedViewModelProperties()
        {
            var sb = new StringBuilder(4096);
            foreach (var pi in this.SelectedPropertyInformationCollection)
            {
                if (pi.CanWrite)
                {
                    sb.Append("Public Property");
                }
                else
                {
                    sb.Append("Public ReadOnly Property");
                }

                string propertyName = pi.Name;
                if (propertyName == "Error")
                {
                    propertyName = "[Error]";
                }

                sb.AppendLine(String.Format(" {0}({1}) As {2}",
                                            propertyName,
                                            pi.PropertyParameterString(LanguageTypes.VisualBasic),
                                            pi.VBTypeName()));
                sb.AppendLine("Get");

                if (propertyName == "Item" && pi.PropertyParameters.Count == 1)
                {
                    sb.AppendLine(String.Format("Return {0}.{1}({2})",
                                                this.FieldName,
                                                pi.Name,
                                                pi.PropertyParameters[0].ParameterName));
                }
                else
                {
                    sb.AppendLine(String.Format("Return {0}.{1}", this.FieldName, pi.Name));
                }

                sb.AppendLine("End Get");

                if (pi.CanWrite)
                {
                    sb.AppendLine(String.Format("Set(ByVal Value As {0}", pi.VBTypeName()));
                    sb.AppendLine(String.Format("{0}.{1} = Value", this.OnPropertyChangedMethodName, this.PropertyName));

                    if (this.IncludeOnPropertyChanged)
                    {
                        sb.AppendLine(String.Format(@"{0}(""{1}"")", this.OnPropertyChangedMethodName, this.PropertyName));
                    }
                    sb.AppendLine("End Set");
                }

                sb.AppendLine("End Property");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Gets the vbinpc.
        /// </summary>
        /// <returns>
        ///     The vbinpc.
        /// </returns>
        private string GetVBINPC()
        {
            var sb = new StringBuilder();

            sb.AppendLine("#Region \" INotifyProperty Changed Method \"");
            sb.AppendLine();
            sb.AppendFormat("Protected Sub {0}(ByVal strPropertyName As String)", this.OnPropertyChangedMethodName);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("If Me.PropertyChangedEvent IsNot Nothing Then");
            sb.AppendLine(
                          "RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(strPropertyName))");
            sb.AppendLine("End If");
            sb.AppendLine();
            sb.AppendLine("End Sub");
            sb.AppendLine();
            sb.AppendLine("#End Region");

            return sb.ToString();
        }

        /// <summary>
        ///     Gets VB methods.
        /// </summary>
        /// <returns>
        ///     The VB methods.
        /// </returns>
        private string GetVBMethods()
        {
            var sb = new StringBuilder(1024);

            sb.AppendLine("#Region \" Command Methods \"");
            sb.AppendLine();

            foreach (var obj in (from x in this.CommandsCollection orderby x.CommandName select x))
            {
                bool usesCommandParameter = obj.CommandParameterType.IsNotNullOrWhiteSpace();

                if (obj.IncludeCanExecuteMethod)
                {
                    sb.AppendLine();

                    if (usesCommandParameter)
                    {
                        sb.AppendFormat("Private Function {0}(ByVal param As {1}) As Boolean",
                                        obj.CanExecuteMethodName,
                                        obj.CommandParameterType);
                    }
                    else
                    {
                        sb.AppendFormat("Private Function {0}() As Boolean", obj.CanExecuteMethodName);
                    }

                    sb.AppendLine();
                    sb.AppendLine("End Function");
                }

                sb.AppendLine();

                if (usesCommandParameter)
                {
                    sb.AppendFormat("Private Sub {0}(ByVal param As {1})",
                                    obj.ExecuteMethodName,
                                    obj.CommandParameterType);
                    sb.AppendLine();
                    sb.AppendLine("End Sub");
                }
                else
                {
                    sb.AppendFormat("Private Sub {0}()", obj.ExecuteMethodName);
                    sb.AppendLine();
                    sb.AppendLine("End Function");
                }
            }

            sb.AppendLine();
            sb.AppendLine("#End Region");
            sb.AppendLine();

            return sb.ToString();
        }

        /// <summary>
        ///     Gets VB properties.
        /// </summary>
        /// <returns>
        ///     The VB properties.
        /// </returns>
        private string GetVBProperties()
        {
            // 0 - PropertySignature
            // 1 - PropertyName
            // 2 - PropertyType
            // 3 - Field name
            // 4 - Set accessor
            string propertyWrapper = @"Region "" Properties ""

{0} {1}() As {2}
Get
Return {3}
End Get{4}
End Property
";

            // 0 - Visibility
            // 1 - PropertyType
            // 2 - FieldName
            // 3 - OnPropertyChanged call
            string setterWrapper = this.IsPropertyReadOnly ? string.Empty : @"
{0}Set(ByVal Value As {1})
{2} = Value;{3}
End Set
";

            // 0 - OPC Method name
            // 1 - Property name
            string opcWrapper = this.IncludeOnPropertyChanged ? @"
{0}(""{1}"");" : string.Empty;

            string propertyText = string.Format(propertyWrapper,
                                                this.PropertySignature,
                                                this.PropertyName,
                                                this.PropertyType,
                                                this.FieldName,
                                                string.Format(setterWrapper,
                                                              this.HasPrivateSetter ? "Private " : string.Empty,
                                                              this.PropertyType,
                                                              this.FieldName,
                                                              string.Format(opcWrapper,
                                                                            this.OnPropertyChangedMethodName,
                                                                            this.PropertyName)));

            string exposedProperties = this.ExposePropertiesOnViewModel
                                           ? this.GetVBExposedViewModelProperties()
                                           : string.Empty;

            return string.Format(@"{0}
{1}

#End Region

", propertyText, exposedProperties);
        }

        /// <summary>
        ///     Executes the property changed action.
        /// </summary>
        /// <param name="PropertyName">
        ///     Name of the property.
        /// </param>
        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler h = this.PropertyChanged;

            if (h == null)
            {
                return;
            }

            h(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        ///     Sets property signature.
        /// </summary>
        private void SetPropertySignature()
        {
            this.PropertySignature = string.Format("{0} {1}",
                                                   this.IsPropertyPublic ? "Public" : "Private",
                                                   this.IsPropertyReadOnly ? "ReadOnly " : string.Empty);
        }

        /// <summary>
        ///     Translate VB property to c sharp.
        /// </summary>
        /// <param name="VBPropertyName">
        ///     Name of the VB property.
        /// </param>
        /// <returns>
        ///     .
        /// </returns>
        private string TranslateVBPropertyToCSharp(string VBPropertyName)
        {
            if (VBPropertyName.StartsWith("Nullable"))
            {
                VBPropertyName = VBPropertyName.Replace("Nullable(Of ", string.Empty).Replace(")", string.Empty).Trim()
                                 + "?";
            }
            else
            {
                if (VBPropertyName.IndexOf("(Of") != -1)
                {
                    VBPropertyName = VBPropertyName.Replace("(Of ", "<").Replace(")", ">");
                }
            }
            return VBPropertyName;
        }

        /// <summary>
        ///     Event handler. Called by btnAddCommand for click events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void btnAddCommand_Click(object sender, RoutedEventArgs e)
        {
            var frm = new CreateCommandWindow(this.IsVB);

            if (frm.ShowDialog() == true)
            {
                this.CommandsCollection.Add(frm.CreateCommandSource);
            }
        }

        /// <summary>
        ///     Event handler. Called by btnCancel for click events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        ///     Event handler. Called by cboPropertyChangedMethodNames for loaded
        ///     events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void cboPropertyChangedMethodNames_Loaded(object sender, RoutedEventArgs e)
        {
            this.cboPropertyChangedMethodNames.RemoveHandler(Selector.SelectionChangedEvent,
                                                             new SelectionChangedEventHandler(
                                                                 this.cboPropertyType_SelectionChanged));
            this.cboPropertyChangedMethodNames.Items.Add("RaisePropertyChanged");
            this.cboPropertyChangedMethodNames.Items.Add("OnPropertyChanged");
            this.cboPropertyChangedMethodNames.Items.Add("NotifyPropertyChanged");
            this.cboPropertyChangedMethodNames.Items.Add("FirePropertyChanged");
            this.cboPropertyChangedMethodNames.SelectedIndex = -1;
            this.cboPropertyChangedMethodNames.AddHandler(Selector.SelectionChangedEvent,
                                                          new SelectionChangedEventHandler(
                                                              this.cboPropertyType_SelectionChanged));
        }

        /// <summary>
        ///     Event handler. Called by cboPropertyChangedMethodNames for selection
        ///     changed events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void cboPropertyChangedMethodNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cboPropertyChangedMethodNames.SelectedValue == null
                || this.cboPropertyChangedMethodNames.SelectedIndex == -1)
            {
                return;
            }

            this.OnPropertyChangedMethodName = this.cboPropertyChangedMethodNames.SelectedValue.ToString();
        }

        /// <summary>
        ///     Event handler. Called by cboPropertyType for loaded events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void cboPropertyType_Loaded(object sender, RoutedEventArgs e)
        {
            this.cboPropertyType.RemoveHandler(Selector.SelectionChangedEvent,
                                               new SelectionChangedEventHandler(this.cboPropertyType_SelectionChanged));
            this.cboPropertyType.ItemsSource = this.GetPropertyTypes();
            this.cboPropertyType.SelectedIndex = -1;
            this.cboPropertyType.AddHandler(Selector.SelectionChangedEvent,
                                            new SelectionChangedEventHandler(this.cboPropertyType_SelectionChanged));
        }

        /// <summary>
        ///     Event handler. Called by cboPropertyType for selection changed events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void cboPropertyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cboPropertyType.SelectedItem == null || this.cboPropertyType.SelectedIndex == -1)
            {
                return;
            }

            this.PropertyType = this.cboPropertyType.SelectedItem.ToString();
        }

        #endregion
    }
}
