namespace XamlHelpmeet.UI.Controls
{
#region Imports

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using XamlHelpmeet.UI.Commands;

#endregion

/// <summary>
///     Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
///
///     Step 1a) Using this custom control in a XAML file that exists in the
///     current project.
///     Add this XmlNamespace attribute to the root element of the markup file
///     where it is
///     to be used:
///     xmlns:MyNamespace="clr-namespace:XamlHelpmeet.UI.Controls"
///
///     Step 1b) Using this custom control in a XAML file that exists in a
///     different project.
///     Add this XmlNamespace attribute to the root element of the markup file
///     where it is
///     to be used:
///     xmlns:MyNamespace="clr-namespace:XamlHelpmeet.UI.Controls;assembly=XamlHelpmeet.UI.Controls"
///     You will also need to add a project reference from the project where the
///     XAML file lives
///     to this project and Rebuild to avoid compilation errors:
///     Right click on the target project in the Solution Explorer and
///     "Add Reference"->"Projects"->[Browse to and select this project]
///
///     Step 2)
///     Go ahead and use your control in the XAML file.
///     <MyNamespace:Counter />
/// </summary>
public class Counter : Control
{
    #region Static Fields

    public static readonly DependencyProperty CountProperty =
        DependencyProperty.Register(
            "Count",
            typeof(int),
            typeof(Counter),
            new FrameworkPropertyMetadata(1, null, CoerceCountProperty));

    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register(
            "Maximum",
            typeof(int),
            typeof(Counter),
            new FrameworkPropertyMetadata(99, OnMaximumPropertyChanged,
                                          CoerceMaximumProperty));

    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register(
            "Minimum",
            typeof(int),
            typeof(Counter),
            new FrameworkPropertyMetadata(1, OnMinimumPropertyChanged));

    #endregion

    #region Fields

    private RelayCommand decrementCommand;

    private RelayCommand incrementCommand;

    #endregion

    #region Constructors and Destructors

    static Counter()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(Counter),
            new FrameworkPropertyMetadata(typeof(Counter)));
    }

    #endregion

    #region Public Properties

    public int Count
    {
        get
        {
            return (int)this.GetValue(CountProperty);
        }
        set
        {
            this.SetValue(CountProperty, value);
        }
    }

    public RelayCommand DecrementCommand
    {
        get
        {
            return this.decrementCommand
                   ?? (this.decrementCommand =
                           new RelayCommand(this.Decrement, this.CanDecrement));
        }
    }

    public RelayCommand IncrementCommand
    {
        get
        {
            return this.incrementCommand
                   ?? (this.incrementCommand =
                           new RelayCommand(this.Increment, this.CanIncrement));
        }
    }

    public int Maximum
    {
        get
        {
            return (int)this.GetValue(MaximumProperty);
        }
        set
        {
            this.SetValue(MaximumProperty, value);
        }
    }

    public int Minimum
    {
        get
        {
            return (int)this.GetValue(MinimumProperty);
        }
        set
        {
            this.SetValue(MinimumProperty, value);
        }
    }

    #endregion

    #region Methods

    private static object CoerceCountProperty(DependencyObject d,
            object basevalue)
    {
        int i = basevalue is int ? (int)basevalue : 0;
        var counter = d as Counter;
        if (counter == null)
        {
            return basevalue;
        }

        if (i < counter.Minimum)
        {
            i = counter.Minimum;
        }

        if (i > counter.Maximum)
        {
            i = counter.Maximum;
        }

        return i;
    }

    private static object CoerceMaximumProperty(DependencyObject d,
            object basevalue)
    {
        var counter = d as Counter;
        int i = basevalue is int ? (int)basevalue : 0;

        if (counter == null)
        {
            return basevalue;
        }

        if (counter.Minimum > i)
        {
            i = counter.Minimum;
        }

        return i;
    }

    private static void OnMaximumPropertyChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        var counter = d as Counter;
        if (counter == null)
        {
            return;
        }

        if (counter.Count > counter.Maximum)
        {
            counter.Count = counter.Maximum;
        }
    }

    private static void OnMinimumPropertyChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        var counter = d as Counter;
        if (counter == null)
        {
            return;
        }

        if (counter.Count < counter.Minimum)
        {
            counter.Count = counter.Minimum;
        }

        if (counter.Maximum < counter.Minimum)
        {
            counter.Maximum = counter.Minimum;
        }
    }

    /// <summary>
    /// Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement"/> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)"/>.
    /// </summary>
    /// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
    protected override void OnPropertyChanged(
        DependencyPropertyChangedEventArgs e)
    {
        switch (e.Property.Name)
        {
            case "Count":
            case "Maximum":
            case "Minimum":
                var dispatcherTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
                dispatcherTimer.Tick += (o, a) =>
                {
                    IncrementCommand.RaiseCanExecuteChangedEvent();
                    DecrementCommand.RaiseCanExecuteChangedEvent();
                };
                dispatcherTimer.Start();
                break;
        }
        base.OnPropertyChanged(e);
    }

    private bool CanDecrement(object obj)
    {
        return this.Count > this.Minimum;
    }

    private bool CanIncrement(object obj)
    {
        return this.Count < this.Maximum;
    }

    private void Decrement(object obj)
    {
        this.SetCurrentValue(CountProperty, this.Count - 1);
    }

    private void Increment(object obj)
    {
        this.SetCurrentValue(CountProperty, this.Count + 1);
    }

    #endregion
}
}