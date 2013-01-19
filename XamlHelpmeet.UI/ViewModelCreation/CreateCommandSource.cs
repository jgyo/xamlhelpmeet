using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlHelpmeet.UI.ViewModelCreation
{
	public class CreateCommandSource
	{
		#region Properties

		public bool CanExecuteUseAddressOf
		{
			get;
			private set;
		}
		public bool ExecuteUseAddressOf
		{
			get;
			private set;
		}
		public bool IncludeCanExecuteMethod
		{
			get;
			private set;
		}
		public bool UseRelayCommand
		{
			get;
			private set;
		}
		public string CanExecuteMethodName
		{
			get;
			private set;
		}
		public string CommandName
		{
			get;
			private set;
		}
		public string CommandParameterType
		{
			get;
			private set;
		}
		public string ExecuteMethodName
		{
			get;
			private set;
		}
		public string FieldName
		{
			get;
			private set;
		}

		#endregion
		public CreateCommandSource(bool CanExecuteUseAddressOf, bool ExecuteUseAddressOf, bool IncludeCanExecuteMethod, bool UseRelayCommand, string CanExecuteMethodName, string CommandName, string CommandParameterType, string ExecuteMethodName, string FieldName)
		{
			this.CanExecuteUseAddressOf = CanExecuteUseAddressOf;
			this.ExecuteUseAddressOf = ExecuteUseAddressOf;
			this.IncludeCanExecuteMethod = IncludeCanExecuteMethod;
			this.UseRelayCommand = UseRelayCommand;
			this.CanExecuteMethodName = CanExecuteMethodName;
			this.CommandName = CommandName;
			this.CommandParameterType = CommandParameterType;
			this.ExecuteMethodName = ExecuteMethodName;
			this.FieldName = FieldName;

		}

		public override string ToString()
		{
			return CommandName;
		}
	}
}
