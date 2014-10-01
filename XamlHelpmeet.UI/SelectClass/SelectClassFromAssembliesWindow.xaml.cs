using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using XamlHelpmeet.Model;

namespace XamlHelpmeet.UI.SelectClass
{
using NLog;

using YoderZone.Extensions.NLog;

/// <summary>
/// Interaction logic for SelectClassFromAssembliesWindow.xaml
/// </summary>
public partial class SelectClassFromAssembliesWindow : Window
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    private readonly CollectionView _assemblyNamespaceClassCollectionView;

    public SelectClassFromAssembliesWindow(ClassInformationList
                                           classInformationList, string nameOfSourceCommand)
    {
        // This call is required by the Windows Form Designer.
        InitializeComponent();

        // Add any initialization after the InitializeComponent() call.
        var cvs = new CollectionViewSource();
        _assemblyNamespaceClassCollectionView =
            CollectionViewSource.GetDefaultView(classInformationList) as
            CollectionView;

        _assemblyNamespaceClassCollectionView.GroupDescriptions.Clear();
        _assemblyNamespaceClassCollectionView.SortDescriptions.Clear();
        _assemblyNamespaceClassCollectionView.GroupDescriptions.Add(
            new PropertyGroupDescription("AssemblyName"));
        _assemblyNamespaceClassCollectionView.GroupDescriptions.Add(
            new PropertyGroupDescription("Namespace"));
        _assemblyNamespaceClassCollectionView.SortDescriptions.Add(
            new SortDescription("AssemblyName", ListSortDirection.Ascending));
        _assemblyNamespaceClassCollectionView.SortDescriptions.Add(
            new SortDescription("Namespace", ListSortDirection.Ascending));
        _assemblyNamespaceClassCollectionView.SortDescriptions.Add(
            new SortDescription("TypeName", ListSortDirection.Ascending));

        this.tvObjects.ItemsSource = _assemblyNamespaceClassCollectionView.Groups;
        this.tbCommandCaption.Text = string.Concat("For ", nameOfSourceCommand);
    }

    public SelectClassFromAssembliesWindow()
    {
        logger.Debug("Entered member.");

        InitializeComponent();
    }

    public ClassInformation SelectedAssemblyNamespaceClass
    {
        get;
        private set;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        DialogResult = false;
    }

    private void btnNext_Click(object sender, RoutedEventArgs e)
    {
        logger.Debug("Entered member.");

        DialogResult = true;
    }

    // NOTE: This is not hooked up in Shifflett code. This window may not be used.
    private void tvObjects_SelectedItemChanged(object sender,
            RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is ClassInformation)
        {
            btnNext.IsEnabled = true;
            SelectedAssemblyNamespaceClass = e.NewValue as ClassInformation;
        }
        else
        {
            btnNext.IsEnabled = false;
            SelectedAssemblyNamespaceClass = null;
        }
    }
}
}