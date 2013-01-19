using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace XamlHelpmeet.UI.CheckListBox
{
	public class CheckListBoxIndicatorItem
	{
		public ListBoxItem RelatedListBoxItem { get; set; }

		public bool IsSelected { get; set; }

		public double Offset { get; set; }

		public CheckListBoxIndicatorItem(double offset, bool isSelected, ListBoxItem relatedListBoxItem)
		{
			Offset = offset;
			IsSelected = isSelected;
			RelatedListBoxItem = relatedListBoxItem;
		}

		public CheckListBoxIndicatorItem()
		{
			
		}
	}
}
