using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace XamlHelpmeet.UI
{
	/// <summary>
	/// Interaction logic for AboutWindow.xaml
	/// </summary>
	public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			InitializeComponent();
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (!e.Handled)
				Close();
		}

		private string Version
		{
			get
			{
				var info = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

				return String.Format("{0}.{1}.{2}.{3}",
				                     info.FileMajorPart,
				                     info.FileMinorPart,
				                     info.FileBuildPart,
				                     info.FilePrivatePart);
			}
		}

		private void Window_Loaded_1(object sender, RoutedEventArgs e)
		{
			VersionRun.Text = Version;
		}

		private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
		{
			Process.Start((sender as Hyperlink).ToolTip.ToString());

		}
	}
}
