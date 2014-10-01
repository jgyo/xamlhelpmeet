namespace XamlHelpmeet.UI.About
{
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// Interaction logic for AboutWindow.xaml
/// </summary>
public partial class AboutWindow : Window
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    public AboutWindow()
    {
        logger.Debug("Entered member.");

        InitializeComponent();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        logger.Debug("Entered member.");

        if (!e.Handled)
        { this.Close(); }
    }

    private string Version
    {
        get
        {
            var info = FileVersionInfo.GetVersionInfo(
                           Assembly.GetExecutingAssembly().Location);

            return String.Format("{0}.{1}.{2}.{3}",
                                 info.FileMajorPart,
                                 info.FileMinorPart,
                                 info.FileBuildPart,
                                 info.FilePrivatePart);
        }
    }

    private void Window_Loaded_1(object sender, RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        this.VersionRun.Text = this.Version;
    }

    private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        Process.Start((sender as Hyperlink).ToolTip.ToString());

    }
}
}
