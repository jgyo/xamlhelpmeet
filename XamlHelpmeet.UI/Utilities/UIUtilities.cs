using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace XamlHelpmeet.UI.Utilities
{
	public class UIUtilities
	{
		public static MessageBoxResult ShowExceptionMessage(string heading, string message)
		{
			return MessageBox.Show(message, heading, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
		}

		public static MessageBoxResult ShowExceptionMessage(string heading, string message, string footer, string additionalDetails)
		{
#if DEBUG
			return MessageBox.Show(string.Format("{0}{1}{1}{2}{1}{1}{3}", message, Environment.NewLine, footer, additionalDetails), heading, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
#else
			return MessageBox.Show(message,
						heading, 
						MessageBoxButton.OK, 
						MessageBoxImage.Error, 
						MessageBoxResult.OK);

#endif
		}

		public static MessageBoxResult ShowInformationMessage(string heading, string message)
		{
			return MessageBox.Show(message, heading, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
		}

		public static MessageBoxResult ShowInformationMessage(string heading, string message, string footer, string additionalDetails)
		{
#if DEBUG
			return MessageBox.Show(message + Environment.NewLine + Environment.NewLine + footer +Environment.NewLine+Environment.NewLine + additionalDetails, heading, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
#else
			return MessageBox.Show(message, heading, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
#endif
		}

	}
}
