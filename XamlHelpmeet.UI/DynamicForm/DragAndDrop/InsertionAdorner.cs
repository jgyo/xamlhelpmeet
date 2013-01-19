using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace XamlHelpmeet.UI.DynamicForm.DragAndDrop
{
	public class InsertionAdorner : Adorner
	{
		#region Properties

		public bool IsInFirstHalf
		{
			get;
			set;
		}

		public bool IsSeparatorHorizontal
		{
			get;
			set;
		}

		private static Pen Pen
		{
			get;
			set;
		}

		private static PathGeometry Triangle
		{
			get;
			set;
		}

		private AdornerLayer AdornerLayer
		{
			get;
			set;
		}

		#endregion Properties

		#region Constructors

		static InsertionAdorner()
		{
			Pen = new Pen()
			{
				Brush = Brushes.Gray
			};
			Pen.Freeze();

			var firstLine = new LineSegment(new Point(0, -5), false);
			firstLine.Freeze();

			var secondLine = new LineSegment(new Point(0, 5), false);
			secondLine.Freeze();

			var figure = new PathFigure()
			{
				StartPoint = new Point(5, 0)
			};
			figure.Segments.Add(firstLine);
			figure.Segments.Add(secondLine);
			figure.Freeze();
			Triangle = new PathGeometry();
			Triangle.Figures.Add(figure);
			Triangle.Freeze();
		}

		public InsertionAdorner(bool IsSeparatorHorizontal, bool IsInFirstHalf, UIElement AdornedElement, AdornerLayer AdornerLayer)
			: base(AdornedElement)
		{
			this.IsSeparatorHorizontal = IsSeparatorHorizontal;
			this.IsInFirstHalf = IsInFirstHalf;
			this.AdornerLayer = AdornerLayer;
			IsHitTestVisible = false;
			AdornerLayer.Add(this);
		}

		#endregion Constructors

		#region Methods

		public void Detach()
		{
			AdornerLayer.Remove(this);
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			Point startPoint;
			Point endPoint;

			CalculateStartAndEndPoint(out startPoint, out endPoint);
			drawingContext.DrawLine(InsertionAdorner.Pen, startPoint, endPoint);

			if (IsSeparatorHorizontal)
			{
				DrawTriangle(drawingContext, startPoint, 0);
				DrawTriangle(drawingContext, endPoint, 0);
			}
			else
			{
				DrawTriangle(drawingContext, startPoint, 90);
				DrawTriangle(drawingContext, endPoint, -90);
			}
		}

		private void CalculateStartAndEndPoint(out Point StartPoint, out Point EndPoint)
		{
			StartPoint = new Point();
			EndPoint = new Point();

			var width = base.AdornedElement.RenderSize.Width;
			var height = base.AdornedElement.RenderSize.Height;

			if (IsSeparatorHorizontal)
			{
				EndPoint.X = width;

				if (!IsInFirstHalf)
				{
					StartPoint.Y = Height;
					EndPoint.Y = height;
				}
			}
			else
			{
				EndPoint.Y = height;

				if (IsInFirstHalf)
					return;

				StartPoint.X = width;
				EndPoint.X = width;
			}
		}

		private void DrawTriangle(DrawingContext DrawingContext, Point Origin, double Angle)
		{
			DrawingContext.PushTransform(new TranslateTransform(Origin.X, Origin.Y));
			DrawingContext.PushTransform(new RotateTransform(Angle));
			DrawingContext.DrawGeometry(InsertionAdorner.Pen.Brush, null, Triangle);
			DrawingContext.Pop();
			DrawingContext.Pop();
		}

		#endregion Methods
	}
}