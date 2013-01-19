// file:	DynamicForm\DragAndDrop\DraggedAdorner.cs
//
// summary:	Implements the dragged adorner class
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace XamlHelpmeet.UI.DynamicForm.DragAndDrop
{
	/// <summary>
	/// 	Dragged adorner.
	/// </summary>
	/// <seealso cref="T:System.Windows.Documents.Adorner"/>
	public class DraggedAdorner : Adorner
	{
		#region Properties

		private double Left
		{
			get;
			set;
		}

		private double Top
		{
			get;
			set;
		}

		private AdornerLayer AdornerLayer
		{
			get;
			set;
		}

		private ContentPresenter ContentPresenter
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets the number of visual child elements within this element.
		/// </summary>
		/// <seealso cref="P:System.Windows.FrameworkElement.VisualChildrenCount"/>
		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		#endregion Properties

		#region Constructors

		/// <summary>
		/// 	Initializes a new instance of the DraggedAdorner class.
		/// </summary>
		/// <param name="DragDropData">
		/// 	Information describing the drag drop.
		/// </param>
		/// <param name="DataTemplate">
		/// 	The data template.
		/// </param>
		/// <param name="AdornedElement">
		/// 	The adorned element.
		/// </param>
		/// <param name="AdornerLayer">
		/// 	The adorner layer.
		/// </param>
		public DraggedAdorner(object DragDropData, DataTemplate DataTemplate, UIElement AdornedElement, AdornerLayer AdornerLayer)
			: base(AdornedElement)
		{
			this.AdornerLayer = AdornerLayer;
			ContentPresenter = new ContentPresenter()
			{
				Content = DragDropData,
				ContentTemplate = DataTemplate,
				Opacity = 0.7
			};
			this.AdornerLayer.Add(this);
		}

		#endregion

		#region Methods

		/// <summary>
		/// 	Detaches this DraggedAdorner.
		/// </summary>
		public void Detach()
		{
			AdornerLayer.Remove(this);
		}

		/// <summary>
		/// 	Returns a <see cref="T:System.Windows.Media.Transform" /> for the adorner, based
		/// 	on the transform that is currently applied to the adorned element.
		/// </summary>
		/// <seealso cref="M:System.Windows.Documents.Adorner.GetDesiredTransform(GeneralTransform)"/>
		public override GeneralTransform GetDesiredTransform(GeneralTransform Transform)
		{
			var result = new GeneralTransformGroup();

			result.Children.Add(base.GetDesiredTransform(Transform));
			result.Children.Add(new TranslateTransform(Left, Top));
			return result;
		}

		/// <summary>
		/// 	Sets a position.
		/// </summary>
		/// <param name="Left">
		/// 	The left.
		/// </param>
		/// <param name="Top">
		/// 	The top.
		/// </param>
		public void SetPosition(double Left, double Top)
		{
			this.Left = Left;
			this.Top = Top;

			if (AdornerLayer == null)
			{
				return;
			}

			try
			{
				AdornerLayer.Update(base.AdornedElement);
			}
			catch //(InvalidOperationException ex)
			{
				// Ignore - this hapens when working over a terminal session
			}
		}

		/// <summary>
		/// 	When overridden in a derived class, positions child elements and determines a
		/// 	size for a <see cref="T:System.Windows.FrameworkElement" /> derived class.
		/// </summary>
		/// <seealso cref="M:System.Windows.FrameworkElement.ArrangeOverride(Size)"/>
		protected override Size ArrangeOverride(Size finalSize)
		{
			ContentPresenter.Arrange(new Rect(finalSize));
			return finalSize;
		}

		/// <summary>
		/// 	Overrides
		/// 	<see cref="M:System.Windows.Media.Visual.GetVisualChild(System.Int32)" />, and
		/// 	returns a child at the specified index from a collection of child elements.
		/// </summary>
		/// <seealso cref="M:System.Windows.FrameworkElement.GetVisualChild(int)"/>
		protected override Visual GetVisualChild(int index)
		{
			return ContentPresenter;
		}

		/// <summary>
		/// 	Implements any custom measuring behavior for the adorner.
		/// </summary>
		/// <seealso cref="M:System.Windows.Documents.Adorner.MeasureOverride(Size)"/>
		protected override Size MeasureOverride(Size constraint)
		{
			ContentPresenter.Measure(constraint);
			return ContentPresenter.DesiredSize;
		}


		#endregion
	}
}