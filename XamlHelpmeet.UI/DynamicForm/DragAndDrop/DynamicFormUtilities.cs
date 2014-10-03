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
using System.Diagnostics.Contracts;

using NLog;

using YoderZone.Extensions.NLog;

public class DynamicFormUtilities
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

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

        if (ItemsControl != null && BottomMostVisual != null &&
                ItemsControl.Items.Count >= 1)
        {
            var firstContainer =
                ItemsControl.ItemContainerGenerator.ContainerFromIndex(0);

            if (firstContainer == null)
            {
                return itemContainer;       // retyrn null;
            }

            itemContainer = DynamicFormUtilities.FindAncestor(
                                firstContainer.GetType(),
                                BottomMostVisual);

            if (itemContainer == null || itemContainer.DataContext == null)
            {
                return itemContainer;
            }

            var itemContainerVerify =
                ItemsControl.ItemContainerGenerator.ContainerFromItem(itemContainer.
                        DataContext) as FrameworkElement;

            if (itemContainer != itemContainerVerify)
            {
                return null;
            }
        }

        return itemContainer;
    }

    public static bool HasVerticalOrientation(FrameworkElement ItemContainer)
    {
        logger.Debug("Entered member.");

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

    public static void InsertItemInItemsControl(ItemsControl itemsControl,
            object itemToInsert,
            int insertionIndex)
    {
        if (itemToInsert == null)
        {
            return;
        }

        var itemsSource = itemsControl.ItemsSource;

        if (itemsSource != null && itemToInsert is DynamicFormEditor)
        {
            var propertyName = ((itemToInsert as DynamicFormEditor).DataContext
                                as DynamicFormListBoxContent).BindingPath;
            var collectionView = itemsControl.ItemsSource as CollectionView;

            if (collectionView != null)
            {
                foreach (PropertyInformation item in collectionView)
                {
                    if (item.Name == propertyName)
                    {
                        item.HasBeenUsed = true;
                        break;
                    }
                    collectionView.Refresh();
                }
            }
        }
        else if (itemsSource == null && itemToInsert is PropertyInformation)
        {
            // this occurs when dragging from the left side field list
            // to the form fields listings
            itemsControl.Items.Insert(insertionIndex,
                                      UIHelpers.DynamicFormEditorFactory(itemToInsert as
                                              PropertyInformation));
        }
        else if (itemsSource == null && itemToInsert is DynamicFormEditor)
        {
            itemsControl.Items.Insert(insertionIndex,
                                      itemToInsert);
        }
    }

    public static bool IsInFirstHalf(FrameworkElement container,
                                     Point clickedPoint,
                                     bool hasVerticalOrientation)
    {
        Contract.Requires<ArgumentNullException>(clickedPoint != null);
        Contract.Requires<ArgumentNullException>(container != null);
        if (hasVerticalOrientation)
        {
            return clickedPoint.Y < (container.ActualHeight / 2);
        }

        return clickedPoint.X < (container.ActualWidth / 2);
    }

    public static bool IsMovementBigEnough(Point InitialMousePosition,
                                           Point CurrentMousePosition)
    {
        return (Math.Abs((double)CurrentMousePosition.X - InitialMousePosition.X)
                >= SystemParameters.
                MinimumHorizontalDragDistance) ||
               (Math.Abs((double)CurrentMousePosition.Y - InitialMousePosition
                         .Y) >= SystemParameters.MinimumVerticalDragDistance);
    }

    public static int RemoveItemFromItemsControl(ItemsControl ItemsControl,
            object ItemToRemove)
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
            var collectionView = CollectionViewSource.GetDefaultView(
                                     ItemsControl.ItemsSource) as
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