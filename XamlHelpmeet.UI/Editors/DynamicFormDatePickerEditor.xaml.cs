using System.Windows.Controls;

namespace XamlHelpmeet.UI.Editors
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// Interaction logic for DynamicFormDatePickerEditor.xaml
/// </summary>
public partial class DynamicFormDatePickerEditor : UserControl
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    public DynamicFormDatePickerEditor()
    {
        logger.Debug("Entered member.");

        InitializeComponent();
    }
}
}