using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using XamlHelpmeet.Model;
using XamlHelpmeet.UI.Editors;
using XamlHelpmeet.UI.Utilities;

namespace XamlHelpmeet.UI.DynamicForm.DragAndDrop
{
	public class DynamicFormUtilities
	{
		public static FrameworkElement FindAncestor(Type AncestorType,
										  Visual Visual)
		{
			while (Visual != null && !AncestorType.IsInstanceOfType(Visual))
			{
				Visual = (Visual)VisualTreeHelper.GetParent(Visual);
			}
			return Visual as FrameworkElement;
		}

		public static FrameworkElement GetItemContainer(ItemsControl ItemsControl,
														Visual BottomMostVisual)
		{
			FrameworkElement itemContainer = null;

			if (ItemsControl != null && BottomMostVisual != null && ItemsControl.Items.Count >= 1)
			{
				var firstContainer = ItemsControl.ItemContainerGenerator.ContainerFromIndex(0);

				if (firstContainer == null)
				{
					return itemContainer;		// retyrn null;
				}

				itemContainer = DynamicFormUtilities.FindAncestor(firstContainer.GetType(),
																  BottomMostVisual);

				if (itemContainer == null || itemContainer.DataContext == null)
				{
					return itemContainer;
				}

				var itemContainerVerify = ItemsControl.ItemContainerGenerator.ContainerFromItem(itemContainer.
				DataContext) as FrameworkElement;

				if (itemContainer == itemContainerVerify)
				{
					return null;
				}
			}

			return itemContainer;
		}

		public static bool HasVerticalOrientation(FrameworkElement ItemContainer)
		{
			if (ItemContainer != null)
			{
				var panel = VisualTreeHelper.GetParent(ItemContainer) as Panel;
				var stackPanel = panel as StackPanel;

				if (stackPanel != null)
				{
					return stackPanel.Orientation == Orientation.Vertical;
				}

				var wrapPanel = panel as WrapPanel;
				if (wrapPanel != null)
				{
					return wrapPanel.Orientation == Orientation.Vertical;
				}
			}

			// Default orientation is vertical
			return true;
		}

		public static void InsertItemInItemsControl(ItemsControl ItemsControl,
													object ItemToInsert,
													int InsertionIndex)
		{
			if (ItemToInsert == null)
			{
				return;
			}

			var itemsSource = ItemsControl.ItemsSource;

			if (itemsSource != null && ItemToInsert is DynamicFormEditor)
			{
			}
			else if (itemsSource == null && ItemToInsert is PropertyInformation)
			{
				ItemsControl.Items.Insert(InsertionIndex, UIHelpers.DynamicFormEditorFactory(ItemToInsert as
				PropertyInformation));
			}
			else if (itemsSource == null && ItemToInsert is DynamicFormEditor)
			{
				ItemsControl.Items.Insert(InsertionIndex,
										  ItemToInsert);
			}
		}

		public static bool IsInFirstHalf(FrameworkElement Container,
										 Point ClickedPoint,
										 bool HasVerticalOrientation)
		{
			if (HasVerticalOrientation)
			{
				return ClickedPoint.Y < (Container.ActualHeight / 2);
			}

			return ClickedPoint.X < (Container.ActualWidth / 2);
		}

		public static bool IsMovementBigEnough(Point InitialMousePosition, Point CurrentMousePosition)
		{
			return (Math.Abs((double)CurrentMousePosition.X - InitialMousePosition.X) >= SystemParameters.
			MinimumHorizontalDragDistance) || (Math.Abs((double)CurrentMousePosition.Y - InitialMousePosition
			.Y) >= SystemParameters.MinimumVerticalDragDistance);
		}

		public static int RemoveItemFromItemsControl(ItemsControl ItemsControl, object ItemToRemove)
		{
			if (ItemToRemove == null)
			{
				return -1;
			}

			var indexToBeRemoved = ItemsControl.Items.IndexOf(ItemToRemove);

			if (indexToBeRemoved == -1)
			{
				return -1;
			}

			// We have the final index now.

			var itemsSource = ItemsControl.ItemsSource;

			if (itemsSource != null && itemsSource.GetType().IsGenericType)
			{
				((PropertyInformation)ItemToRemove).HasBeenUsed = true;
				var collectionView = CollectionViewSource.GetDefaultView(ItemsControl.ItemsSource) as
				CollectionView;
				if (collectionView != null)
				{
					collectionView.Refresh();
				}
				return indexToBeRemoved;
			}

			if (itemsSource == null)
			{
				ItemsControl.Items.RemoveAt(indexToBeRemoved);
			}
			return indexToBeRemoved;
		}
	}
}