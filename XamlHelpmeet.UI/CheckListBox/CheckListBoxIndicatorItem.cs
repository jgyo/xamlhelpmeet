// file:	CheckListBox\CheckListBoxIndicatorItem.cs
//
// summary:	Implements the check list box indicator item class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace XamlHelpmeet.UI.CheckListBox
{
	public class CheckListBoxIndicatorItem
	{
        /// <summary>
        ///     Gets or sets the related list box item.
        /// </summary>
        /// <value>
        ///     The related list box item.
        /// </value>
		public ListBoxItem RelatedListBoxItem { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this CheckListBoxIndicatorItem is
        ///     selected.
        /// </summary>
        /// <value>
        ///     true if this CheckListBoxIndicatorItem is selected, otherwise false.
        /// </value>
		public bool IsSelected { get; set; }

        /// <summary>
        ///     Gets or sets the offset.
        /// </summary>
        /// <value>
        ///     The offset.
        /// </value>
		public double Offset { get; set; }

        /// <summary>
        ///     Initializes a new instance of the CheckListBoxIndicatorItem class.
        /// </summary>
        /// <param name="offset">
        ///     The offset.
        /// </param>
        /// <param name="isSelected">
        ///     true if this CheckListBoxIndicatorItem is selected.
        /// </param>
        /// <param name="relatedListBoxItem">
        ///     The related list box item.
        /// </param>
		public CheckListBoxIndicatorItem(double offset, bool isSelected, ListBoxItem relatedListBoxItem)
		{
			Offset = offset;
			IsSelected = isSelected;
			RelatedListBoxItem = relatedListBoxItem;
		}

        /// <summary>
        ///     Initializes a new instance of the CheckListBoxIndicatorItem class.
        /// </summary>
		public CheckListBoxIndicatorItem()
		{
			
		}
	}
}
