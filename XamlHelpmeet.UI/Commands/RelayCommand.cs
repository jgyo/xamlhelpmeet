using System;
using System.Windows.Input;

namespace XamlHelpmeet.UI.Commands
{
	public class RelayCommand : ICommand
	{
		private readonly Predicate<object> _canExecuteMethod;
		private readonly Action<object> _executeMethod;

		public RelayCommand(Action<object> ExecuteMethod)
			: this(ExecuteMethod, null)
		{
		}

		public RelayCommand(Action<object> ExecuteMethod, Predicate<object> CanExecuteMethod)
		{
			if (ExecuteMethod == null)
			{
				throw new ArgumentNullException("ExecuteMethod", "Delagate commands cannot be null");
			}
			_canExecuteMethod = CanExecuteMethod;
			_executeMethod = ExecuteMethod;
		}

		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (_canExecuteMethod != null)
				{
					CommandManager.RequerySuggested += value;
				}
			}
			remove
			{
				if (_canExecuteMethod != null)
				{
					CommandManager.RequerySuggested -= value;
				}
			}
		}

		public void RaiseCanExecuteChangedEvent()
		{
			if (_canExecuteMethod != null)
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}

		#region ICommand Members

		public bool CanExecute(object parameter)
		{
			if (_canExecuteMethod == null)
			{
				return true;
			}

			return _canExecuteMethod(parameter);
		}

		public void Execute(object parameter)
		{
			_executeMethod(parameter);
		}

		#endregion ICommand Members
	}

	// ============================================================
	// ============================================================
	// ============================================================
	// ============================================================

	public sealed class RelayCommand<T> : ICommand
	{
		private readonly Predicate<T> _canExecuteMethod;
		private readonly Action<T> _executeMethod;

		public RelayCommand(Action<T> ExecuteMethod)
			: this(ExecuteMethod, null)
		{
		}

		public RelayCommand(Action<T> ExecuteMethod, Predicate<T> CanExecuteMethod)
		{
			if (ExecuteMethod == null)
			{
				throw new ArgumentNullException("ExecuteMethod", "Delagate commands cannot be null");
			}
			_canExecuteMethod = CanExecuteMethod;
			_executeMethod = ExecuteMethod;
		}

		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (_canExecuteMethod != null)
				{
					CommandManager.RequerySuggested += value;
				}
			}
			remove
			{
				if (_canExecuteMethod != null)
				{
					CommandManager.RequerySuggested -= value;
				}
			}
		}

		public void RaiseCanExecuteChangedEvent()
		{
			if (_canExecuteMethod != null)
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}

		#region ICommand Members

		public bool CanExecute(object parameter)
		{
			if (_canExecuteMethod == null)
			{
				return true;
			}

			return _canExecuteMethod((T)parameter);
		}

		public void Execute(object parameter)
		{
			_executeMethod((T)parameter);
		}

		#endregion ICommand Members
	}
}