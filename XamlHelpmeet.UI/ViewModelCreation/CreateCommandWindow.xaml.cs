using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
 using XamlHelpmeet.Extensions;
using XamlHelpmeet.UI.Commands;

namespace XamlHelpmeet.UI.ViewModelCreation
{
	/// <summary>
	/// Interaction logic for CreateCommandWindow.xaml
	/// </summary>
	public partial class CreateCommandWindow : Window, INotifyPropertyChanged
	{
		#region Fields

		private bool _autoAppendExecute;
		private string _canExecuteMethodName;
		private string _commandName;
		private string _commandParameterType;
		private ICommand _createCommand;
		private CreateCommandSource _createCommandSource;
		private string _executeMethodName;
		private string _fieldName;
		private bool _isVB;

		#endregion Fields

		#region Properties

		public bool AutoAppendExecute
		{
			get
			{
				return _autoAppendExecute;
			}
			set
			{
				_autoAppendExecute = value;
				OnPropertyChanged("AutoAppendExecute");
				UIControlFactory.UIControlFactory.Instance.UIControls.AutoAppendExecute = _autoAppendExecute;
			}
		}

		public string CanExecuteMethodName
		{
			get
			{
				return _canExecuteMethodName;
			}
			set
			{
				_canExecuteMethodName = value;
				OnPropertyChanged("CanExecuteMethodName");
			}
		}

		public string CommandName
		{
			get
			{
				return _commandName;
			}
			set
			{
				_commandName = value;
				OnPropertyChanged("CommandName");
				SetCommandMethodNames();
			}
		}

		public string CommandParameterType
		{
			get
			{
				return _commandParameterType;
			}
			set
			{
				_commandParameterType = value;
				OnPropertyChanged("CommandParameterType");
			}
		}

		public ICommand CreateCommand
		{
			get
			{
				if (_createCommand == null)
					_createCommand = new RelayCommand(CreateExecute, CanCreateExecute);
				return _createCommand;
			}
			set
			{
				_createCommand = value;
			}
		}

		public CreateCommandSource CreateCommandSource
		{
			get
			{
				return _createCommandSource;
			}
			set
			{
				_createCommandSource = value;
				OnPropertyChanged("CreateCommandSource");
			}
		}

		public string ExecuteMethodName
		{
			get
			{
				return _executeMethodName;
			}
			set
			{
				_executeMethodName = value;
				OnPropertyChanged("ExecuteMethodName");
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

		public bool IsVB
		{
			get
			{
				return _isVB;
			}
		}

		#endregion Properties

		#region Methods

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private bool CanCreateExecute(object obj)
		{
			if (_commandName.IsNullOrEmpty())
				return false;

			if (_fieldName.IsNullOrEmpty())
				return false;

			if (_executeMethodName == null)
				return false;

			if ((chkIncludeCanExecuteMethod.IsChecked == true) && _canExecuteMethodName.IsNullOrEmpty())
				return false;

			return true;
		}

		private void cboCommandName_Loaded(object sender, RoutedEventArgs e)
		{
			cboCommandName.RemoveHandler(ComboBox.SelectionChangedEvent, new 
				SelectionChangedEventHandler(cboCommandNameSelectionChanged));
			cboCommandName.ItemsSource = GetCommandNames();
			cboCommandName.SelectedIndex = -1;
			cboCommandName.AddHandler(ComboBox.SelectionChangedEvent, new
				SelectionChangedEventHandler(cboCommandNameSelectionChanged));
		}

		private void CreateExecute(object obj)
		{
			UIControlFactory.UIControlFactory.Instance.Save(false);
			CreateCommandSource = new CreateCommandSource(rdoCanExecuteUseAddressOf
				.IsChecked.Value, rdoExecuteUseAddressOf.IsChecked.Value,
				chkIncludeCanExecuteMethod.IsChecked.Value, rdoRelayCommand
				.IsChecked.Value, CanExecuteMethodName, CommandName,
				CommandParameterType, ExecuteMethodName, FieldName);
			DialogResult = true;
		}

		private IList<string> GetCommandNames()
		{
			var obj = new List<string>();
			obj.Add("New");
			obj.Add("Save");
			obj.Add("Update");
			obj.Add("Delete");
			obj.Add("Insert");
			obj.Add("Select");
			obj.Add("Remove");
			obj.Add("Add");
			obj.Add("Lookup");
			obj.Add("Create");
			obj.Add("Modify");
			obj.Add("Extract");
			obj.Add("Next");
			obj.Add("Last");
			obj.Add("Previous");
			obj.Add("First");
			obj.Add("Stop");
			obj.Add("Cancel");
			obj.Sort();
			return obj;
		}

		private void SetCommandMethodNames()
		{
			var commandName = CommandName;

			if (IsVB)
				FieldName = String.Format("_cmd{0}", commandName);
			else
				FieldName = String.Format("_{0}{1}", commandName.ToLower()[0], commandName.Substring(1));

			commandName = commandName.Replace("Command", string.Empty);

			if (AutoAppendExecute)
			{
				ExecuteMethodName = String.Format("{0}Execute", commandName);
				CanExecuteMethodName = String.Format("Can{0}Execute", commandName);
			}
			else
			{
				ExecuteMethodName = commandName;
				CanExecuteMethodName = String.Format("Can{0}", commandName);
			}
		}

		#endregion Methods

		#region Constructors

		public CreateCommandWindow(bool isVB)
		{
			_isVB = isVB;

			if (isVB)
				_commandParameterType = "Object";
			else
				_commandParameterType = "object";

			_autoAppendExecute = UIControlFactory.UIControlFactory.Instance.UIControls.AutoAppendExecute;
			DataContext = this;
			InitializeComponent();
		}

		public CreateCommandWindow()
		{
			InitializeComponent();
		}

		#endregion Constructors

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			var h=PropertyChanged;
			if (h == null)
				return;

			h(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion INotifyPropertyChanged Members

		private void cboCommandNameSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cboCommandName.SelectedItem == null || cboCommandName.SelectedIndex == -1)
				return;

			CommandName = String.Format("{0}Command", cboCommandName.SelectedItem);
		}
	}
}