using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.Editors;

namespace XamlHelpmeet.UI.DynamicForm.DragAndDrop
{
	public class DragDropHelper
	{
		#region Declarations

		private static DragDropHelper _instance;

		private readonly DataFormat _format = DataFormats.GetDataFormat("DragDropItemsControl");

		#endregion Declarations

		#region Properties

		private static DragDropHelper Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DragDropHelper();
				}
				return _instance;
			}
		}

		private DraggedAdorner DraggedAdorner
		{
			get;
			set;
		}

		private object DraggedData
		{
			get;
			set;
		}

		private DataFormat Format
		{
			get
			{
				return _format;
			}
		}

		private bool HasVerticalOrientation
		{
			get;
			set;
		}

		private Point InitialMousePosition
		{
			get;
			set;
		}

		private InsertionAdorner InsertionAdorner
		{
			get;
			set;
		}

		private int InsertionIndex
		{
			get;
			set;
		}

		private bool IsInFirstHalf
		{
			get;
			set;
		}

		private FrameworkElement SourceItemContainer
		{
			get;
			set;
		}

		private ItemsControl SourceItemsControl
		{
			get;
			set;
		}

		private FrameworkElement TargetItemContainer
		{
			get;
			set;
		}

		private ItemsControl TargetItemsControl
		{
			get;
			set;
		}

		private Window TopWindow
		{
			get;
			set;
		}

		#endregion Properties

		#region Dependency Properties

		public static DependencyProperty DragDropTemplateProperty = DependencyProperty.RegisterAttached("DragDropTemplate", typeof(DataTemplate), typeof(DragDropHelper), new UIPropertyMetadata(null));

		public static DependencyProperty IsDragSourceProperty = DependencyProperty.RegisterAttached("IsDragSource", typeof(bool), typeof(DragDropHelper), new UIPropertyMetadata(false, DragDropHelper.IsDragSourceChanged));

		public static DependencyProperty IsDropTargetProperty = DependencyProperty.RegisterAttached("IsDropTarget", typeof(bool), typeof(DragDropHelper), new UIPropertyMetadata(false, DragDropHelper.IsDropTargetChanged));

		#endregion Dependency Properties

		#region Methods

		public static DataTemplate GetDragDropTemplate(DependencyObject obj)
		{
			return (DataTemplate)obj.GetValue(DragDropTemplateProperty);
		}

		public static bool GetIsDragSource(DependencyObject obj)
		{
			return (bool)obj.GetValue(DragDropHelper.IsDragSourceProperty);
		}

		public static bool GetIsDragTarget(DependencyObject obj)
		{
			return (bool)obj.GetValue(DragDropHelper.IsDropTargetProperty);
		}

		public static void SetDragDropTemplate(DependencyObject obj,
											   DataTemplate value)
		{
			obj.SetValue(DragDropHelper.DragDropTemplateProperty,
						 value);
		}

		public static void SetIsDragSource(DependencyObject obj,
										   bool value)
		{
			obj.SetValue(DragDropHelper.IsDragSourceProperty,
						 value);
		}

		public static void SetIsDropTarget(DependencyObject obj,
										   bool value)
		{
			obj.SetValue(DragDropHelper.IsDropTargetProperty,
						 value);

		}

		private static void IsDragSourceChanged(DependencyObject d,
												DependencyPropertyChangedEventArgs e)
		{
			var dragSource = d as ItemsControl;

			if (dragSource == null)
			{
				return;
			}

			if (e.NewValue.Equals(true))
			{
				dragSource.PreviewMouseLeftButtonDown += DragDropHelper.Instance.DragSource_PreviewMouseLeftButtonDown;
				dragSource.PreviewMouseLeftButtonUp += DragDropHelper.Instance.DragSource_PreviewMouseLeftButtonUp;
				dragSource.PreviewMouseMove += DragDropHelper.Instance.DragSource_PreviewMouseMove;
			}
			else
			{
				dragSource.PreviewMouseLeftButtonDown -= DragDropHelper.Instance.DragSource_PreviewMouseLeftButtonDown;
				dragSource.PreviewMouseLeftButtonUp -= DragDropHelper.Instance.DragSource_PreviewMouseLeftButtonUp;
				dragSource.PreviewMouseMove -= DragDropHelper.Instance.DragSource_PreviewMouseMove;
			}
		}

		private static void IsDropTargetChanged(DependencyObject d,
												DependencyPropertyChangedEventArgs e)
		{
			var dropTarget = d as ItemsControl;

			if (dropTarget == null)
			{
				return;
			}

			if (e.NewValue.Equals(true))
			{
				dropTarget.AllowDrop = true;
				dropTarget.PreviewDrop += DragDropHelper.Instance.DropTarget_PreviewDrop;
				dropTarget.PreviewDragEnter += DragDropHelper.Instance.DropTarget_PreviewDragEnter;
				dropTarget.PreviewDragOver += DragDropHelper.Instance.DropTarget_PreviewDragOver;
				dropTarget.PreviewDragLeave += DragDropHelper.Instance.DropTarget_PreviewDragLeave;
			}
			else
			{
				dropTarget.AllowDrop = false;
				dropTarget.PreviewDrop -= DragDropHelper.Instance.DropTarget_PreviewDrop;
				dropTarget.PreviewDragEnter -= DragDropHelper.Instance.DropTarget_PreviewDragEnter;
				dropTarget.PreviewDragOver -= DragDropHelper.Instance.DropTarget_PreviewDragOver;
				dropTarget.PreviewDragLeave -= DragDropHelper.Instance.DropTarget_PreviewDragLeave;
			}
		}

		private void CreateInsertionAdorner()
		{
			if (TargetItemContainer != null)
			{
				InsertionAdorner = new InsertionAdorner(HasVerticalOrientation,
														IsInFirstHalf,
														TargetItemContainer,
														AdornerLayer.GetAdornerLayer(TargetItemContainer));
			}
		}

		private void DecideDropTarget(DragEventArgs e)
		{
			var targetItemsControlCount = TargetItemsControl.Items.Count;
			var draggedItem = e.Data.GetData(Format.Name);

			if (IsDropDataTypeAllowed(draggedItem))
			{
				if (targetItemsControlCount > 0)
				{
					HasVerticalOrientation = DynamicFormUtilities.HasVerticalOrientation(TargetItemsControl.
					ItemContainerGenerator.ContainerFromIndex(0) as FrameworkElement);

					TargetItemContainer = DynamicFormUtilities.GetItemContainer(TargetItemsControl, e.
																				OriginalSource as Visual);

					if (TargetItemContainer != null)
					{
						IsInFirstHalf = DynamicFormUtilities.IsInFirstHalf(TargetItemContainer,
																			e.GetPosition(TargetItemContainer),
																			HasVerticalOrientation);
						InsertionIndex = TargetItemsControl.ItemContainerGenerator.IndexFromContainer(
						TargetItemContainer);

						if (!IsInFirstHalf)
						{
							InsertionIndex++;
						}
					}
					else
					{
						TargetItemContainer = TargetItemsControl.ItemContainerGenerator.ContainerFromIndex(
						targetItemsControlCount - 1) as FrameworkElement;
						IsInFirstHalf = false;
						InsertionIndex = targetItemsControlCount;
					}
				}
				else
				{
					TargetItemContainer = null;
					InsertionIndex = 0;
				}
			}
			else
			{
				TargetItemContainer = null;
				InsertionIndex = -1;
				e.Effects = DragDropEffects.None;
			}
		}

		private void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var fe = DynamicFormUtilities.GetItemContainer(sender as ItemsControl, e.OriginalSource as
															Visual) as FrameworkElement;

			if (fe != null && fe.DataContext is PropertyInformation && (fe.DataContext as PropertyInformation
			).HasBeenUsed)
			{
				e.Handled = true;
				return;
			}

			InitialMousePosition = e.GetPosition(TopWindow);
			SourceItemsControl = (ItemsControl)sender;
			TopWindow = DynamicFormUtilities.FindAncestor(typeof(Window),
														  SourceItemsControl) as Window;
			SourceItemContainer = DynamicFormUtilities.GetItemContainer(SourceItemsControl, e.OriginalSource
																		as Visual);

			if (SourceItemContainer != null)
			{
				DraggedData = SourceItemContainer.DataContext;
			}
		}

		private void DragSource_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			DraggedData = null;
		}

		private void DragSource_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (DraggedData == null || !DynamicFormUtilities.IsMovementBigEnough(InitialMousePosition, e.GetPosition(TopWindow)))
			{
				return;
			}

			var data = new DataObject(Format.Name, DraggedData);
			var previousAllowDrop = TopWindow.AllowDrop;
			TopWindow.AllowDrop = true;
			TopWindow.DragEnter += TopWindow_DragEnter;
			TopWindow.DragOver += TopWindow_DragOver;
			TopWindow.DragLeave += TopWindow_DragLeave;

			var effects = DragDrop.DoDragDrop(sender as DependencyObject, data, DragDropEffects.Move);

			TopWindow.AllowDrop = previousAllowDrop;
			TopWindow.DragEnter -= TopWindow_DragEnter;
			TopWindow.DragOver -= TopWindow_DragOver;
			TopWindow.DragLeave -= TopWindow_DragLeave;
			DraggedData = null;
		}

		private void DropTarget_PreviewDragEnter(object sender, DragEventArgs e)
		{
			TargetItemsControl = sender as ItemsControl;
			DecideDropTarget(e);

			if (e.Data.GetData(Format.Name) == null)
			{
				return;
			}

			ShowDraggedAdorner(e.GetPosition(TopWindow));
			CreateInsertionAdorner();

			e.Handled = true;
		}

		private void DropTarget_PreviewDragLeave(object sender, DragEventArgs e)
		{
			if (e.Data.GetData(Format.Name) != null)
			{
				RemoveInsertionAdorner();
			}
			e.Handled = true;
		}

		private void DropTarget_PreviewDragOver(object sender, DragEventArgs e)
		{
			DecideDropTarget(e);

			if (e.Data.GetData(Format.Name) != null)
			{
				ShowDraggedAdorner(e.GetPosition(TopWindow));
				UpdateInsertionAdorner();
			}
			e.Handled = true;
		}

		private void DropTarget_PreviewDrop(object sender, DragEventArgs e)
		{
			var draggedItem = e.Data.GetData(Format.Name);
			var indexRemoved = -1;

			if (draggedItem != null)
			{
				if ((e.Effects & DragDropEffects.Move) != DragDropEffects.None)
				{
					indexRemoved = DynamicFormUtilities.RemoveItemFromItemsControl(SourceItemsControl, draggedItem);
				}

				if (indexRemoved != -1 && SourceItemsControl == TargetItemsControl && indexRemoved < InsertionIndex)
				{
					InsertionIndex--;
				}

				DynamicFormUtilities.InsertItemInItemsControl(TargetItemsControl, draggedItem, InsertionIndex);
				RemoveDraggedAdorner();
				RemoveInsertionAdorner();
			}
			e.Handled = true;
		}
		private bool IsDropDataTypeAllowed(object DraggedItem)
		{
			if (SourceItemsControl != TargetItemsControl)
			{
				return DraggedItem is DynamicFormEditor || DraggedItem is PropertyInformation;
			}

			// SourceItemsControl is the TargetItemsControl
			return SourceItemsControl.ItemsSource is ListCollectionView || DraggedItem is PropertyInformation
				? false : DraggedItem is DynamicFormEditor;
		}

		private void RemoveDraggedAdorner()
		{
			if (DraggedAdorner == null)
			{
				return;
			}
			DraggedAdorner.Detach();
			DraggedAdorner = null;
		}

		private void RemoveInsertionAdorner()
		{
			if (InsertionAdorner == null)
			{
				return;
			}
			InsertionAdorner.Detach();
			InsertionAdorner = null;
		}

		private void ShowDraggedAdorner(Point CurrentPosition)
		{
			if (DraggedAdorner == null)
			{
				DraggedAdorner = new DraggedAdorner(DraggedData, DragDropHelper.GetDragDropTemplate(SourceItemsControl),
					SourceItemContainer, AdornerLayer.GetAdornerLayer(SourceItemsControl));
			}

			DraggedAdorner.SetPosition(CurrentPosition.X - InitialMousePosition.X,
				CurrentPosition.Y - InitialMousePosition.Y);
		}

		private void TopWindow_DragEnter(object sender, DragEventArgs e)
		{
			ShowDraggedAdorner(e.GetPosition(TopWindow));
			e.Effects = DragDropEffects.None;
			e.Handled = true;
		}

		private void TopWindow_DragLeave(object sender, DragEventArgs e)
		{
			RemoveDraggedAdorner();
			e.Handled = true;
		}

		private void TopWindow_DragOver(object sender, DragEventArgs e)
		{
			ShowDraggedAdorner(e.GetPosition(TopWindow));
			e.Effects = DragDropEffects.Move;
			e.Handled = true;
		}

		private void UpdateInsertionAdorner()
		{
			if (InsertionAdorner == null)
			{
				return;
			}
			InsertionAdorner.IsInFirstHalf = IsInFirstHalf;
			InsertionAdorner.InvalidateVisual();
		}

		#endregion Methods
	}
}