using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.Commands;
using XamlHelpmeet.Extentions;

namespace XamlHelpmeet.UI.ViewModelCreation
{
	/// <summary>
	/// Interaction logic for CreateViewModelWindow.xaml
	/// </summary>
	public partial class CreateViewModelWindow : Window, INotifyPropertyChanged
	{
		#region Declarations

		private readonly ObservableCollection<CreateCommandSource> _commandsCollection =
			new ObservableCollection<CreateCommandSource>();

		private ClassEntity _classEntity;
		private ICommand _createCommand;
		private string _fieldName = string.Empty;
		private bool _hasPrivateSetter;
		private bool _includeOnPropertyChanged;
		private bool _includeOnPropertyChangedEventHandler = true;
		private bool _isPropertyPublic = true;
		private bool _isPropertyReadOnly;
		private bool _isVB;
		private string _onPropertyChangedMethodName = "RaisePropertyChanged";
		private string _propertyName = string.Empty;
		private string _propertySignature = "Public Property";
		private string _propertyType = string.Empty;
		private bool _useHungarianNotationForPrivateFields;
		private string _viewModelText = string.Empty;

		#endregion Declarations

		#region Events

		/// <summary>
		///     Event inherited from the INotifyPropertyChanged interface
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Events

		#region Command Properties

		public ICommand CreateCommand
		{
			get
			{
				if (_createCommand == null)
				{
					_createCommand = new RelayCommand(CreateExecute, CanCreateExecute);
				}
				return _createCommand;
			}
		}

		#endregion Command Properties

		#region Properties

		public ClassEntity ClassEntity
		{
			get
			{
				return _classEntity;
			}
		}

		public ObservableCollection<CreateCommandSource> CommandsCollection
		{
			get
			{
				return _commandsCollection;
			}
		}

		public string FieldName
		{
			get
			{
				return _fieldName;
			}
			set
			{
				_fieldName = value;
				OnPropertyChanged("FieldName");
			}
		}

		public bool HasPrivateSetter
		{
			get
			{
				return _hasPrivateSetter;
			}
			set
			{
				_hasPrivateSetter = value;
				OnPropertyChanged("HasPrivateSetter");
			}
		}

		public bool IncludeOnPropertyChanged
		{
			get
			{
				return _includeOnPropertyChanged;
			}
			set
			{
				_includeOnPropertyChanged = value;
				OnPropertyChanged("IncludeOnPropertyChanged");
			}
		}

		public bool IncludeOnPropertyChangedEventHandler
		{
			get
			{
				return _includeOnPropertyChangedEventHandler;
			}
			set
			{
				_includeOnPropertyChangedEventHandler = value;
				OnPropertyChanged("IncludeOnPropertyChangedEventHandler");
			}
		}

		public bool IsPropertyPublic
		{
			get
			{
				return _isPropertyPublic;
			}
			set
			{
				_isPropertyPublic = value;
				OnPropertyChanged("IsPropertyPublic");
				SetPropertySignature();
			}
		}

		public bool IsPropertyReadOnly
		{
			get
			{
				return _isPropertyReadOnly;
			}
			set
			{
				_isPropertyReadOnly = value;
				OnPropertyChanged("IsPropertyReadOnly");
				SetPropertySignature();
			}
		}

		public bool IsVB
		{
			get
			{
				return _isVB;
			}
		}

		public string OnPropertyChangedMethodName
		{
			get
			{
				return _onPropertyChangedMethodName;
			}
			set
			{
				_onPropertyChangedMethodName = value;
				OnPropertyChanged("OnPropertyChangedMethodName");
			}
		}

		public string PropertyName
		{
			get
			{
				return _propertyName;
			}
			set
			{
				_propertyName = value;
				OnPropertyChanged("PropertyName");
			}
		}

		public string PropertySignature
		{
			get
			{
				return _propertySignature;
			}
			set
			{
				_propertySignature = value;
				OnPropertyChanged("PropertySignature");
			}
		}

		public string PropertyType
		{
			get
			{
				return _propertyType;
			}
			set
			{
				_propertyType = value;
				OnPropertyChanged("PropertyType");
			}
		}

		public string TypeName
		{
			get
			{
				return ClassEntity.ClassName;
			}
		}

		public bool UseHungarianNotationForPrivateFields
		{
			get
			{
				return _useHungarianNotationForPrivateFields;
			}
			set
			{
				_useHungarianNotationForPrivateFields = value;
				OnPropertyChanged("UseHungarianNotationForPrivateFields");
			}
		}

		public string ViewModelText
		{
			get
			{
				return _viewModelText;
			}
		}

		private bool ExposePropertiesOnViewModel
		{
			get
			{
				return lbProperteis.SelectedItems.Count > 0;
			}
		}

		private IEnumerable<PropertyInformation> SelectedPropertyInformationCollection
		{
			get
			{
				return from p in ClassEntity.PropertyInformation
					   where p.IsSelected
					   orderby p.Name
					   select p;
			}
		}

		#endregion Properties

		#region Constructors

		public CreateViewModelWindow(ClassEntity ClassEntity,
									 bool IsVisualBasic)
		{
			_classEntity = ClassEntity;
			_isVB = IsVisualBasic;

			var className = ClassEntity.ClassName;
			_propertyType = className;
			_propertyName = className;

			_fieldName = IsVB ? "_obj" + className :
				String.Format("_{0}{1}", className.ToLower()[0], className.Substring(1));

			DataContext = this;
			InitializeComponent();
		}

		#endregion Constructors

		#region PropertyChanged Methods

		private void OnPropertyChanged(string PropertyName)
		{
			var h = PropertyChanged;

			if (h == null)
				return;

			h(this, new PropertyChangedEventArgs(PropertyName));
		}

		#endregion PropertyChanged Methods

		#region Command Members

		private bool CanCreateExecute(object param)
		{
			return !(PropertyName.IsNullOrEmpty() || PropertyType.IsNullOrEmpty()
				|| FieldName.IsNullOrEmpty());
		}

		private void CreateExecute(object param)
		{
			if (CanCreateExecute(param) == false)
			{
				return;
			}

			if (IsVB)
			{
				CreateVBViewModelText();
			}
			else
			{
				CreateCSharpViewModelText();
			}
			DialogResult = true;
		}

		#endregion Command Members

		#region Methods

		private void btnAddCommand_Click(object sender, RoutedEventArgs e)
		{
			var frm = new CreateCommandWindow(IsVB);

			if (frm.ShowDialog() == true)
			{
				CommandsCollection.Add(frm.CreateCommandSource);
			}
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void cboPropertyType_Loaded(object sender, RoutedEventArgs e)
		{
			cboPropertyType.RemoveHandler(ComboBox.SelectionChangedEvent, new
				SelectionChangedEventHandler(cboPropertyType_SelectionChanged));
			cboPropertyType.ItemsSource = GetPropertyTypes();
			cboPropertyType.SelectedIndex = -1;
			cboPropertyType.AddHandler(ComboBox.SelectionChangedEvent, new
				SelectionChangedEventHandler(cboPropertyType_SelectionChanged));
		}

		private void cboPropertyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cboPropertyType.SelectedItem == null || cboPropertyType.SelectedIndex == -1)
			{
				return;
			}

			PropertyType = cboPropertyType.SelectedItem.ToString();
		}

		private void CreateViewModelWindow_Unloaded(object sender, RoutedEventArgs e)
		{
			cboPropertyType.RemoveHandler(ComboBox.SelectionChangedEvent, new
				SelectionChangedEventHandler(cboPropertyType_SelectionChanged));
			cboPropertyChangedMethodNames.RemoveHandler(ComboBox.SelectionChangedEvent,
				new SelectionChangedEventHandler(cboPropertyChangedMethodNames_SelectionChanged));
		}

		private IEnumerable GetPropertyTypes()
		{
			var propertyTypes = new List<string>();

			propertyTypes.Add(TypeName);
			propertyTypes.Add(String.Format("List(Of {0})", TypeName));
			propertyTypes.Add(String.Format("ObservableCollection(Of {0})", TypeName));
			propertyTypes.Add(String.Format("ReadOnlyObservableCollection(Of {0})", TypeName));
			propertyTypes.Add(String.Format("IEnumerable(Of {0})", TypeName));
			propertyTypes.Add(String.Format("IList(Of {0})", TypeName));

			return propertyTypes;
		}

		private void SetPropertySignature()
		{
			PropertySignature = string.Format("{0} {1}",
				IsPropertyPublic ? "Public" : "Private",
				IsPropertyReadOnly ? "ReadOnly " : string.Empty);
		}

		private string TranslateVBPropertyToCSharp(string VBPropertyName)
		{
			if (VBPropertyName.StartsWith("Nullable"))
			{
				VBPropertyName = VBPropertyName.Replace("Nullable(Of ", string.Empty).Replace(")", string.Empty).Trim() + "?";
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

		#endregion Methods

		#region Create ViewModel Text Methods

		private void cboPropertyChangedMethodNames_Loaded(object sender, RoutedEventArgs e)
		{
			cboPropertyChangedMethodNames.RemoveHandler(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(cboPropertyType_SelectionChanged));
			cboPropertyChangedMethodNames.Items.Add("RaisePropertyChanged");
			cboPropertyChangedMethodNames.Items.Add("OnPropertyChanged");
			cboPropertyChangedMethodNames.Items.Add("NotifyPropertyChanged");
			cboPropertyChangedMethodNames.Items.Add("FirePropertyChanged");
			cboPropertyChangedMethodNames.SelectedIndex = -1;
			cboPropertyChangedMethodNames.AddHandler(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(cboPropertyType_SelectionChanged));
		}

		private void cboPropertyChangedMethodNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cboPropertyChangedMethodNames.SelectedValue == null || cboPropertyChangedMethodNames.SelectedIndex == -1)
			{
				return;
			}

			OnPropertyChangedMethodName = cboPropertyChangedMethodNames.SelectedValue.ToString();
		}

		private void CreateCSharpViewModelText()
		{
			// NOTE: This method uses multiline string literals.
			//
			// Modify VB code
			PropertyType = TranslateVBPropertyToCSharp(PropertyType);

			var sb = new StringBuilder(4096);

			if (IncludeOnPropertyChanged)
				sb.AppendLine(@"// : System.ComponentModel.INotifyPropertyChanged

// developer, please place the above at the end of your class name
");

			if (ExposePropertiesOnViewModel)
				sb.AppendLine(@"

// TODO developers please add your constructors in the below constructor region.
//      be sure to include an overloaded constructor that takes a model type.
");

			sb.AppendLine(GetCSharpDeclarations());

			sb.AppendLine(@"#region Events

public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

#regionend");

			sb.AppendLine(GetCSharpProperties());

			sb.AppendLine(GetCSharpCommandProperties());

			sb.AppendLine(GetCSharpConstructors());

			sb.AppendLine(GetCSharpMethods());

			if (IncludeOnPropertyChangedEventHandler)
				sb.AppendLine(GetCSharpINPC());

			_viewModelText = sb.ToString();
		}

		private void CreateVBViewModelText()
		{
			var sb = new StringBuilder(4096);

			if (IncludeOnPropertyChanged)
			{
				sb.AppendLine("Implements System.ComponentModel.INotifyPropertyChanged");
				sb.AppendLine();
				;
			}

			sb.AppendLine(GetVBDeclarations());

			sb.AppendLine(@"#Region "" Events ""

Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region");

			sb.AppendLine(GetVBProperties());

			sb.AppendLine(GetVBCommandProperties());

			sb.AppendLine(GetVBConstructors());

			sb.AppendLine(GetVBMethods());

			if (IncludeOnPropertyChangedEventHandler)
				sb.AppendLine(GetVBINPC());

			_viewModelText = sb.ToString();
		}

		private string GetVBCommandProperties()
		{
			var sb = new StringBuilder();

			foreach (var obj in (from x in CommandsCollection
								 orderby x.CommandName
								 select x))
			{
				var usesCommandParameter = obj.CommandParameterType.IsNullOrWhiteSpace();

				sb.AppendLine(String.Format("Public ReadOnly Property {0}() As ICommand", obj.CommandName));
				sb.AppendLine("Get");
				sb.AppendLine(String.Format("if ( {0} Is Nothing Then", obj.FieldName));
				sb.Append(String.Format("{0} = New ", obj.FieldName));

				if (obj.UseRelayCommand)
					sb.Append("RelayCommand");
				else
					sb.Append("DelegateCommand");

				if (usesCommandParameter)
					sb.Append(String.Format("(Of {0})", obj.CommandParameterType));

				sb.Append("(");

				if (obj.ExecuteUseAddressOf)
					sb.Append(String.Format("AddressOf {0}", obj.ExecuteMethodName));
				else
					if (usesCommandParameter)
						sb.Append(String.Format("Sub(param As {0}) {1}(param)", obj.CommandParameterType, obj.ExecuteMethodName));
					else
						sb.Append(String.Format("Sub() {0}()", obj.ExecuteMethodName));

				if (obj.IncludeCanExecuteMethod)
				{
					sb.Append(", ");

					if (obj.CanExecuteUseAddressOf)
						sb.Append(String.Format("AddressOf {0}", obj.CanExecuteMethodName));
					else
						if (usesCommandParameter)
							sb.Append(String.Format("Function(param as {0}) {1}(param)", obj.CommandParameterType, obj.CanExecuteMethodName));
						else
							sb.Append(String.Format("Function() {0}()", obj.CanExecuteMethodName));
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

		private string GetVBConstructors()
		{
			var sb = new StringBuilder(1024);

			sb.AppendLine("#Region \" Constructors \"");
			sb.AppendLine();
			sb.AppendLine("Public Sub New()");
			sb.AppendLine();
			sb.AppendLine("End Sub");
			sb.AppendLine();

			if (ExposePropertiesOnViewModel && PropertyType.IndexOf("(Of") == -1 && PropertyType.IndexOf("<") == -1)
			{
				sb.AppendFormat("Public Sub New({0} As {1})", FieldName.Replace("_", ""), PropertyType);
				sb.AppendLine();
				sb.AppendFormat("{0} = {1}", FieldName, FieldName.Replace("_", ""));
				sb.AppendLine();
				sb.AppendLine("End Sub");
				sb.AppendLine();
			}
			sb.AppendLine("#End Region");
			sb.AppendLine();

			return sb.ToString();
		}

		private string GetVBDeclarations()
		{
			var sb= new StringBuilder(1024);
			foreach (var obj in (from x in CommandsCollection
								 orderby x.CommandName
								 select x))
			{
				sb.AppendLine(string.Format("Private {0} As ICommand ", obj.FieldName));
			}
			return String.Format(@"#Region "" Declarations ""

{0}

Private {1} As {2}

#End Region

", sb, FieldName, PropertyType);
		}

		private string GetVBExposedViewModelProperties()
		{
			var sb = new StringBuilder(4096);
			foreach (var pi in SelectedPropertyInformationCollection)
			{
				if (pi.CanWrite)
				{
					sb.Append("Public Property");
				}
				else
				{
					sb.Append("Public ReadOnly Property");
				}

				var propertyName = pi.Name;
				if (propertyName == "Error")
				{
					propertyName = "[Error]";
				}

				sb.AppendLine(String.Format(" {0}({1}) As {2}", propertyName, pi.PropertyParameterString(LanguageTypes.VisualBasic), pi.VBTypeName()));
				sb.AppendLine("Get");

				if (propertyName == "Item" && pi.PropertyParameters.Count == 1)
				{
					sb.AppendLine(String.Format("Return {0}.{1}({2})", FieldName, pi.Name, pi.PropertyParameters[0].ParameterName));
				}
				else
				{
					sb.AppendLine(String.Format("Return {0}.{1}", FieldName, pi.Name));
				}

				sb.AppendLine("End Get");

				if (pi.CanWrite)
				{
					sb.AppendLine(String.Format("Set(ByVal Value As {0}", pi.VBTypeName()));
					sb.AppendLine(String.Format("{0}.{1} = Value", OnPropertyChangedMethodName, PropertyName));

					if (IncludeOnPropertyChanged)
					{
						sb.AppendLine(String.Format(@"{0}(""{1}"")", OnPropertyChangedMethodName, PropertyName));
					}
					sb.AppendLine("End Set");
				}

				sb.AppendLine("End Property");
				sb.AppendLine();
			}
			return sb.ToString();
		}

		private string GetVBINPC()
		{
			var sb = new StringBuilder();

			sb.AppendLine("#Region \" INotifyProperty Changed Method \"");
			sb.AppendLine();
			sb.AppendFormat("Protected Sub {0}(ByVal strPropertyName As String)", OnPropertyChangedMethodName);
			sb.AppendLine();
			sb.AppendLine();
			sb.AppendLine("If Me.PropertyChangedEvent IsNot Nothing Then");
			sb.AppendLine("RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(strPropertyName))");
			sb.AppendLine("End If");
			sb.AppendLine();
			sb.AppendLine("End Sub");
			sb.AppendLine();
			sb.AppendLine("#End Region");

			return sb.ToString();
		}

		private string GetVBMethods()
		{
			var sb = new StringBuilder(1024);

			sb.AppendLine("#Region \" Command Methods \"");
			sb.AppendLine();

			foreach (var obj in (from x in CommandsCollection
								 orderby x.CommandName
								 select x))
			{
				var usesCommandParameter = obj.CommandParameterType.IsNotNullOrWhiteSpace();

				if (obj.IncludeCanExecuteMethod)
				{
					sb.AppendLine();

					if (usesCommandParameter)
					{
						sb.AppendFormat("Private Function {0}(ByVal param As {1}) As Boolean", obj.CanExecuteMethodName, obj.CommandParameterType);
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
					sb.AppendFormat("Private Sub {0}(ByVal param As {1})", obj.ExecuteMethodName, obj.CommandParameterType);
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

		private string GetVBProperties()
		{
			// 0 - PropertySignature
			// 1 - PropertyName
			// 2 - PropertyType
			// 3 - Field name
			// 4 - Set accessor
			var propertyWrapper = @"Region "" Properties ""

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
			var setterWrapper = IsPropertyReadOnly ? string.Empty : @"
{0}Set(ByVal Value As {1})
{2} = Value;{3}
End Set
";

			// 0 - OPC Method name
			// 1 - Property name
			var opcWrapper = IncludeOnPropertyChanged ? @"
{0}(""{1}"");" : string.Empty;

			var propertyText = string.Format(propertyWrapper,
				PropertySignature,
				PropertyName,
				PropertyType,
				FieldName,
				string.Format(setterWrapper,
				HasPrivateSetter ? "Private " : string.Empty,
				PropertyType,
				FieldName,
				string.Format(opcWrapper,
				OnPropertyChangedMethodName,
				PropertyName)));

			string exposedProperties = ExposePropertiesOnViewModel ? GetVBExposedViewModelProperties() :
				string.Empty;

			return string.Format(@"{0}
{1}

#End Region

", propertyText, exposedProperties);
		}

		private string GetCSharpCommandProperties()
		{
			var sb = new StringBuilder(4096);

			foreach (var item in (from x in CommandsCollection
								  orderby x.CommandName
								  select x))
			{
				var usesCommandParameter = item.CommandParameterType.IsNullOrWhiteSpace();

				sb.AppendLine(String.Format("public ICommand {0}", item.CommandName));
				sb.AppendLine("{");
				sb.AppendLine("get");
				sb.AppendLine("{");
				sb.AppendLine(String.Format("if ({0} == null)", item.FieldName));
				sb.AppendLine("{");
				sb.Append(String.Format("{0} = new ", item.FieldName));

				sb.Append(item.UseRelayCommand ? "RelayCommand" : "DelegateCommand");

				if (usesCommandParameter)
					sb.AppendFormat("<{0}>", item.CommandParameterType);

				sb.Append("(");

				if (item.ExecuteUseAddressOf)
					sb.Append(item.ExecuteMethodName);
				else
					sb.AppendFormat(usesCommandParameter ? "param => {0}(param)" : "() => {0}()", item.ExecuteMethodName);

				if (item.IncludeCanExecuteMethod)
				{
					sb.Append(", ");

					if (item.CanExecuteUseAddressOf)
						sb.Append(item.CanExecuteMethodName);
					else
						sb.AppendFormat(usesCommandParameter ? "param => {0}(param)" : "() => {0}()", item.CanExecuteMethodName);
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

		private string GetCSharpDeclarations()
		{
			var sb = new StringBuilder(1024);
			foreach (var obj in (from x in CommandsCollection
								 orderby x.CommandName
								 select x))
			{
				sb.AppendFormat(string.Format("ICommand {0};", obj.FieldName));
			}

			// NOTE: The following uses a multiline string literal.
			return string.Format(@"#region Declarations

{0}

{1} {2};

#endregion

", sb, PropertyType, FieldName);
		}

		private string GetCSharpExposedViewModelProperties()
		{
			var sb = new StringBuilder(4096);
			foreach (var pi in SelectedPropertyInformationCollection)
			{
				var typeName = TranslateVBPropertyToCSharp(pi.TypeName);

				sb.AppendLine(pi.Name == "Item" && pi.PropertyParameters.Count == 1 ?
					String.Format("public {0} this[{1}]", typeName, pi.CSParameterString) :
					pi.PropertyParameters.Count > 0 ?
					String.Format("public {0} {1}[{2}]", typeName, pi.Name, pi.CSParameterString) :
					String.Format("public {0} {1}", typeName, pi.Name));

				sb.AppendLine("{");

				sb.AppendLine(pi.Name == "Item" && pi.PropertyParameters.Count == 1 ?
					String.Format("get {{ return {0}[{1}]; }}", FieldName, pi.PropertyParameters[0].ParameterName) :
					String.Format("get {{ return {0}.{1}; }}", FieldName, pi.Name));

				if (pi.CanWrite)
				{
					sb.AppendLine("set");
					sb.AppendLine("{");
					sb.AppendLine(String.Format("{0}.{1} = value;", FieldName, pi.Name));

					if (IncludeOnPropertyChanged)
					{
						sb.AppendLine(String.Format("{0}(\"{1}\");", OnPropertyChangedMethodName, pi.Name));
					}
					sb.AppendLine("}");
				}

				sb.AppendLine("}");
				sb.AppendLine();
			}
			return sb.ToString();
		}

		private string GetCSharpINPC()
		{
			return String.Format(@"#region INotifyProperty Changed Method

protected void {0}(string propertyName)
{
var handler = this.PropertyChanged;
if (handler != null)
{
handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
}
}

#endregion

", OnPropertyChangedMethodName);
		}

		private string GetCSharpMethods()
		{
			var sb = new StringBuilder(4096);

			foreach (var item in (from x in CommandsCollection
								  orderby x.CommandName
								  select x))
			{
				var usesCommandParameter = item.CommandParameterType.IsNotNullOrWhiteSpace();

				if (item.IncludeCanExecuteMethod)
				{
					sb.AppendLine(usesCommandParameter ?
						String.Format("bool {0}({1} param)", item.CanExecuteMethodName, item.CommandParameterType) :
						String.Format("bool {0}()", item.CanExecuteMethodName));

					sb.AppendLine("{");
					sb.AppendLine("}");
				}

				sb.AppendLine();

				sb.AppendLine(usesCommandParameter ?
					String.Format("void {0}({1} param)", item.ExecuteMethodName, item.CommandParameterType) :
					String.Format("void {0}()", item.ExecuteMethodName));

				sb.AppendLine("{");
				sb.AppendLine("}");
				sb.AppendLine();
			}

			return String.Format(@"#region CommandMethoods

{0}

#endregion

", sb);
		}

		private string GetCSharpProperties()
		{
			// 0 - Visibility
			// 1 - Type
			// 2 - Name
			// 3 - Field name
			// 4 - Set accessor
			var propertyWrapper = @"region Properties

{0} {1} {2}
{{
get {{ return {3}; }}{4}
}}
";

			// 0 - Visibility
			// 1 - Field name
			// 2 - OnPropertyChanged call
			var setterWrapper = IsPropertyReadOnly ? string.Empty : @"
{0}set
{{
{1} = value;{2}
}}
";

			// 0 - OPC Method name
			// 1 - Property name
			var opcWrapper = IncludeOnPropertyChanged ? @"
{0}(""{1}"");" : string.Empty;

			var propertyText = string.Format(propertyWrapper,
				IsPropertyPublic ? "public" : "private",
				PropertyType,
				PropertyName,
				FieldName,
				string.Format(setterWrapper,
				HasPrivateSetter ? "private " : string.Empty,
				FieldName,
				string.Format(opcWrapper,
				OnPropertyChangedMethodName,
				PropertyName)));

			string exposedProperties = ExposePropertiesOnViewModel ? GetCSharpExposedViewModelProperties() :
				string.Empty;

			return string.Format(@"{0}{1}

#endregion

", propertyText, exposedProperties);
		}

		#endregion
	}
}