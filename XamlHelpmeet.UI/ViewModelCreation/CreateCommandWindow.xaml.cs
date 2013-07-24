namespace XamlHelpmeet.UI.ViewModelCreation
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using XamlHelpmeet.Extensions;
    using XamlHelpmeet.UI.Commands;
    using XamlHelpmeet.UI.UIControlFactory;

    #endregion

    /// <summary>
    ///     Interaction logic for CreateCommandWindow.xaml
    /// </summary>
    public partial class CreateCommandWindow : Window, INotifyPropertyChanged
    {
        #region Fields

        private readonly bool _isVB;

        private bool _autoAppendExecute;

        private string _canExecuteMethodName;

        private string _commandName;

        private string _commandParameterType;

        private ICommand _createCommand;

        private CreateCommandSource _createCommandSource;

        private string _executeMethodName;

        private string _fieldName;

        #endregion

        #region Constructors

        public CreateCommandWindow(bool isVB)
        {
            this._isVB = isVB;

            if (isVB)
            {
                this._commandParameterType = "Object";
            }
            else
            {
                this._commandParameterType = "object";
            }

            this._autoAppendExecute = UIControlFactory.Instance.UIControls.AutoAppendExecute;
            this.DataContext = this;
            this.InitializeComponent();
        }

        public CreateCommandWindow() { this.InitializeComponent(); }

        #endregion

        #region Properties and Indexers

        public bool AutoAppendExecute
        {
            get { return this._autoAppendExecute; }
            set
            {
                this._autoAppendExecute = value;
                this.OnPropertyChanged("AutoAppendExecute");
                UIControlFactory.Instance.UIControls.AutoAppendExecute = this._autoAppendExecute;
            }
        }

        public string CanExecuteMethodName
        {
            get { return this._canExecuteMethodName; }
            set
            {
                this._canExecuteMethodName = value;
                this.OnPropertyChanged("CanExecuteMethodName");
            }
        }

        public string CommandName
        {
            get { return this._commandName; }
            set
            {
                this._commandName = value;
                this.OnPropertyChanged("CommandName");
                this.SetCommandMethodNames();
            }
        }

        public string CommandParameterType
        {
            get { return this._commandParameterType; }
            set
            {
                this._commandParameterType = value;
                this.OnPropertyChanged("CommandParameterType");
            }
        }

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
            set { this._createCommand = value; }
        }

        public CreateCommandSource CreateCommandSource
        {
            get { return this._createCommandSource; }
            set
            {
                this._createCommandSource = value;
                this.OnPropertyChanged("CreateCommandSource");
            }
        }

        public string ExecuteMethodName
        {
            get { return this._executeMethodName; }
            set
            {
                this._executeMethodName = value;
                this.OnPropertyChanged("ExecuteMethodName");
            }
        }

        public string FieldName
        {
            get { return this._fieldName; }
            set
            {
                this._fieldName = value;
                this.OnPropertyChanged("FieldName");
            }
        }

        public bool IsVB { get { return this._isVB; } }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods (private)

        private bool CanCreateExecute(object obj)
        {
            if (this._commandName.IsNullOrEmpty())
            {
                return false;
            }

            if (this._fieldName.IsNullOrEmpty())
            {
                return false;
            }

            if (this._executeMethodName == null)
            {
                return false;
            }

            return (this.chkIncludeCanExecuteMethod.IsChecked != true) || !this._canExecuteMethodName.IsNullOrEmpty();
        }

        private void CreateExecute(object obj)
        {
            UIControlFactory.Instance.Save(false);
            this.CreateCommandSource = new CreateCommandSource(this.rdoCanExecuteUseAddressOf.IsChecked.Value,
                                                               this.rdoExecuteUseAddressOf.IsChecked.Value,
                                                               this.chkIncludeCanExecuteMethod.IsChecked.Value,
                                                               this.rdoRelayCommand.IsChecked.Value,
                                                               this.CanExecuteMethodName,
                                                               this.CommandName,
                                                               this.CommandParameterType,
                                                               this.ExecuteMethodName,
                                                               this.FieldName);
            this.DialogResult = true;
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler h = this.PropertyChanged;
            if (h == null)
            {
                return;
            }

            h(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetCommandMethodNames()
        {
            string commandName = this.CommandName;

            if (this.IsVB)
            {
                this.FieldName = String.Format("_cmd{0}", commandName);
            }
            else
            {
                this.FieldName = String.Format("_{0}{1}", commandName.ToLower()[0], commandName.Substring(1));
            }

            commandName = commandName.Replace("Command", string.Empty);

            if (this.AutoAppendExecute)
            {
                this.ExecuteMethodName = String.Format("{0}Execute", commandName);
                this.CanExecuteMethodName = String.Format("Can{0}Execute", commandName);
            }
            else
            {
                this.ExecuteMethodName = commandName;
                this.CanExecuteMethodName = String.Format("Can{0}", commandName);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) { this.DialogResult = false; }

        private void cboCommandNameSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cboCommandName.SelectedItem == null || this.cboCommandName.SelectedIndex == -1)
            {
                return;
            }

            this.CommandName = String.Format("{0}Command", this.cboCommandName.SelectedItem);
        }

        private void cboCommandName_Loaded(object sender, RoutedEventArgs e)
        {
            this.cboCommandName.RemoveHandler(Selector.SelectionChangedEvent,
                                              new SelectionChangedEventHandler(this.cboCommandNameSelectionChanged));
            this.cboCommandName.ItemsSource = this.GetCommandNames();
            this.cboCommandName.SelectedIndex = -1;
            this.cboCommandName.AddHandler(Selector.SelectionChangedEvent,
                                           new SelectionChangedEventHandler(this.cboCommandNameSelectionChanged));
        }

        #endregion
    }
}
