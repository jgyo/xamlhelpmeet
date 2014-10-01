using System.Windows.Controls;

namespace XamlHelpmeet.UI.Editors
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// Interaction logic for LabelEditor.xaml
/// </summary>
public partial class LabelEditor : UserControl
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    public LabelEditor()
    {
        logger.Debug("Entered member.");

        InitializeComponent();
    }
}
}