using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace XamlHelpmeet.UI.CheckListBox
{
    /// <summary>
    /// 	CheckListBox class.
    /// </summary>
    /// <seealso cref="T:System.Windows.Controls.ContentControl"/>
    [TemplatePart(Name = "PART_IndicatorList", Type = typeof(ItemsControl))]
    public class CheckListBox : ContentControl
    {
        /// <summary>
        /// 	The brush property of the checkbox.
        /// </summary>
        public static readonly DependencyProperty CheckBrushProperty = DependencyProperty.Register("CheckBrush",
            typeof(Brush), typeof(CheckListBox), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>
        /// 	The brush stroke thickness property of the check list box.
        /// </summary>
        public static readonly DependencyProperty CheckBrushStrokeThicknessProperty = DependencyProperty
            .Register("CheckBrushStrokeThickness", typeof(double), typeof(CheckListBox), new PropertyMetadata(2.0));

        /// <summary>
        /// 	The height width property of the check list box.
        /// </summary>
        public static readonly DependencyProperty CheckHeightWidthProperty = DependencyProperty
            .Register("CheckHeightWidth", typeof(double), typeof(CheckListBox), new PropertyMetadata(13.0));

        private ObservableCollection<CheckListBoxIndicatorItem> _indicatorOffsets;
        private ListBox _listBox;
        private ItemsControl _objIndicatorList;

        static CheckListBox()
        {
            // This OverrideMetadata call tells the system that this element wants
            // to provide a style that is different than its base class.
            // This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckListBox), new FrameworkPropertyMetadata(typeof(CheckListBox)));
        }

        /// <summary>
        /// 	Initializes a new instance of the CheckListBox class.
        /// </summary>
        public CheckListBox()
        {
            SizeChanged += CheckListBox_SizeChanged;
            Loaded += ListBoxSelectedItemIndicator_Loaded;
            Unloaded += ListBoxSelectedItemIndicator_Unloaded;
            AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(CheckBox_Clicked));
        }

        /// <summary>
        /// 	Gets or sets the check brush.
        /// </summary>
        /// <value>
        /// 	The check brush.
        /// </value>
        [Description("Brush used to paint the check inside the checkbox.  Defaults to black.")]
        [Category("Custom")]
        public Brush CheckBrush
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (Brush)GetValue(CheckBrushProperty);
            }
            set
            {
                SetValue(CheckBrushProperty, value);
            }
        }

        /// <summary>
        /// 	Gets or sets the check brush stroke thickness.
        /// </summary>
        /// <value>
        /// 	The check brush stroke thickness.
        /// </value>
        [Description("Stroke thickness for the check inside the checkbox.  Defaults to 2.")]
        [Category("Custom")]
        public double CheckBrushStrokeThickness
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (double)GetValue(CheckBrushStrokeThicknessProperty);
            }
            set
            {
                SetValue(CheckBrushStrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// 	Gets or sets the width and height of the check list box.
        /// </summary>
        /// <value>
        /// 	The width and height of the check.
        /// </value>
        [Description("Size of CheckBox.  CheckBox is rendered in a square so this value is the height and width of the CheckBox.  Default value is 13.")]
        [Category("Custom")]
        public double CheckHeightWidth
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (double)GetValue(CheckHeightWidthProperty);
            }
            set
            {
                SetValue(CheckHeightWidthProperty, value);
            }
        }

        /// <summary>
        /// 	Overrides the corresponding method from the System.Windows.FrameworkElement base
        /// 	type.
        /// </summary>
        /// <seealso cref="M:System.Windows.FrameworkElement.OnApplyTemplate()"/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // when the template is applied, this give the developer the
            // oppurtunity to get references to name objects in the control template.
            // in our case, we need a reference to the ItemsControl that holds
            // the indicator arrows.
            //
            // what your control does in the absence of an expected object in the
            // control template is up to the control develeper. In my case here,
            // without the items control, we are dead in the water.

            // remember that custom controls are supposed to be Lookless.  Meaning
            // the visual and code are highly decoupled.  Any designer using Blend
            // fully expects to be able edit the control template anyway they want.
            // My using the "PART_" naming convention, you indicate that this
            // object is probably necessary for the conrol to work, but this is not
            // true in all cases.

            _objIndicatorList = GetTemplateChild("PART_IndicatorList") as ItemsControl;

            if (_objIndicatorList == null)
            {
                throw new Exception("Hey!  The PART_IndicatorList is missing from the template or is not an ItemsControl.  Sorry but this ItemsControl is required.");
            }
        }

        /// <summary>
        /// 	Overrides the corresponding method from the
        /// 	System.Windows.Controls.ContentControl base type.
        /// </summary>
        /// <seealso cref="M:System.Windows.Controls.ContentControl.OnContentChanged(Object,Object)"/>
        protected override void OnContentChanged(Object oldContent, Object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            // this is our insurance policy that the developer does not add
            // content that is not a ListBox.
            if (newContent == null || newContent is ListBox)
            {
                // this ensures that our reference to the child ListBox is always correct
                // or nothing.  if the child ListBox is removed, our reference is set to
                // Nothing. if the child ListBox is swapped out, our reference is set to
                // the newContent.
                _listBox = Content as ListBox;

                if (_indicatorOffsets != null && _indicatorOffsets.Count > 0)
                {
                    _indicatorOffsets.Clear();
                }
                return;
            }

            throw new NotSupportedException("Invalid content.  CheckListBox only accepts a ListBox control as its content.");
        }

        private void CheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            var cb = e.OriginalSource as CheckBox;

            if (cb == null)
            {
                return;
            }

            var objCheckListBoxIndicatorItem = cb.DataContext as CheckListBoxIndicatorItem;

            if (objCheckListBoxIndicatorItem == null)
            {
                return;
            }

            objCheckListBoxIndicatorItem.RelatedListBoxItem.IsSelected = cb.IsChecked ?? false;
        }

        private void CheckListBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateIndicators();
        }

        private void ListBox_ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // if the user is scrolling horizontality, no reason to run any of our
            // attached behavior code
            if (e.VerticalChange == 0)
            {
                return;
            }

            UpdateIndicators();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateIndicators();
        }

        private void ListBoxSelectedItemIndicator_Loaded(object sender, RoutedEventArgs e)
        {
            if (_objIndicatorList == null)
            {
                // remember how much "fun" tabs were be in VB and Access?  Well...
                //
                // this is here because when you place a custom control in a tab,
                // it loads the control once before it runs OnApplyTemplate
                // when the TabItem its in gets clicked (focus), OnApplyTemplate
                // runs then Loaded runs agin.
                return;
            }

            _indicatorOffsets = new ObservableCollection<CheckListBoxIndicatorItem>();
            _objIndicatorList.ItemsSource = _indicatorOffsets;

            // How cool are routed events!  We can listen into the child ListBoxes
            // activities and act accordingly.
            AddHandler(ListBox.SelectionChangedEvent, new SelectionChangedEventHandler(ListBox_SelectionChanged));
            AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(ListBox_ScrollViewer_ScrollChanged));
            UpdateIndicators();
        }

        private void ListBoxSelectedItemIndicator_Unloaded(object sender, RoutedEventArgs e)
        {
            RemoveHandler(ListBox.SelectionChangedEvent, new SelectionChangedEventHandler(ListBox_SelectionChanged));
            RemoveHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(ListBox_ScrollViewer_ScrollChanged));
        }

        private void UpdateIndicators()
        {
            // This is the awesome procedure that Josh Smith authored with a
            // few modifications
            if (_indicatorOffsets == null)
            {
                return;
            }

            if (_listBox == null)
            {
                return;
            }

            if (_indicatorOffsets.Count > 0)
            {
                _indicatorOffsets.Clear();
            }

            var objGen = _listBox.ItemContainerGenerator;

            if (objGen.Status != GeneratorStatus.ContainersGenerated)
            {
                return;
            }

            foreach (var objSelectedItem in _listBox.Items)
            {
                var lbi = objGen.ContainerFromItem(objSelectedItem) as ListBoxItem;

                if (lbi == null)
                {
                    continue;
                }

                var objTransform = lbi.TransformToAncestor(_listBox);
                var pt = objTransform.Transform(new Point(0, 0));
                var dblOffset = pt.Y + (lbi.ActualHeight / 2) - (CheckHeightWidth / 2);
                _indicatorOffsets.Add(new CheckListBoxIndicatorItem(dblOffset,
                                          lbi.IsSelected,
                                          lbi));
            }
        }
    }
}