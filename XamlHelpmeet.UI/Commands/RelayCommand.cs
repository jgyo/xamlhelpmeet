namespace XamlHelpmeet.UI.Commands
{
    #region Imports

    using System;
    using System.Windows.Input;

    #endregion

    /// <summary>
    ///     Relay command.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        /// <summary>
        ///     The can execute method.
        /// </summary>
        private readonly Predicate<object> _canExecuteMethod;

        /// <summary>
        ///     The execute method.
        /// </summary>
        private readonly Action<object> _executeMethod;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="ExecuteMethod">
        ///     The execute method.
        /// </param>
        public RelayCommand(Action<object> ExecuteMethod) : this(ExecuteMethod, null) {}

        /// <summary>
        ///     Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="ExecuteMethod">
        ///     The execute method.
        /// </param>
        /// <param name="CanExecuteMethod">
        ///     The can execute method.
        /// </param>
        public RelayCommand(Action<object> ExecuteMethod, Predicate<object> CanExecuteMethod)
        {
            if (ExecuteMethod == null)
            {
                throw new ArgumentNullException("ExecuteMethod", "Delegate commands cannot be null");
            }
            this._canExecuteMethod = CanExecuteMethod;
            this._executeMethod = ExecuteMethod;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its
        ///     current state.
        /// </summary>
        /// <param name="parameter">
        ///     The parameter.
        /// </param>
        /// <returns>
        ///     true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return this._canExecuteMethod == null || this._canExecuteMethod(parameter);
        }

        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should
        ///     execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this._canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (this._canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            this._executeMethod(parameter);
        }

        #endregion

        #region Methods (public)

        /// <summary>
        ///     Raises the can execute changed event.
        /// </summary>
        public void RaiseCanExecuteChangedEvent()
        {
            if (this._canExecuteMethod != null)
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }

        #endregion
    }

    /// <summary>
    ///     Generic Relay Command Class.
    /// </summary>
    /// <typeparam name="T">
    ///     Generic type parameter.
    /// </typeparam>
    public sealed class RelayCommand<T> : ICommand
    {
        #region Fields

        /// <summary>
        ///     The can execute method.
        /// </summary>
        private readonly Predicate<T> _canExecuteMethod;

        /// <summary>
        ///     The execute method.
        /// </summary>
        private readonly Action<T> _executeMethod;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="ExecuteMethod">
        ///     The execute method.
        /// </param>
        public RelayCommand(Action<T> ExecuteMethod) : this(ExecuteMethod, null) {}

        /// <summary>
        ///     Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="ExecuteMethod">
        ///     The execute method.
        /// </param>
        /// <param name="CanExecuteMethod">
        ///     The can execute method.
        /// </param>
        public RelayCommand(Action<T> ExecuteMethod, Predicate<T> CanExecuteMethod)
        {
            if (ExecuteMethod == null)
            {
                throw new ArgumentNullException("ExecuteMethod", "Delegate commands cannot be null");
            }
            this._canExecuteMethod = CanExecuteMethod;
            this._executeMethod = ExecuteMethod;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its
        ///     current state.
        /// </summary>
        /// <param name="parameter">
        ///     The parameter.
        /// </param>
        /// <returns>
        ///     true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return this._canExecuteMethod == null || this._canExecuteMethod((T)parameter);
        }

        /// <summary>
        ///     Occurs when changes occur that affect whether or not the command should
        ///     execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this._canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (this._canExecuteMethod != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            this._executeMethod((T)parameter);
        }

        #endregion

        #region Methods (public)

        /// <summary>
        ///     Raises the can execute changed event.
        /// </summary>
        public void RaiseCanExecuteChangedEvent()
        {
            if (this._canExecuteMethod != null)
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }

        #endregion
    }
}
