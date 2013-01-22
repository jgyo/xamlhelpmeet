using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace XamlHelpmeet.UI.GridColumnAndRowEditor
{
	/// <summary>
	/// Interaction logic for GridRowColunnEditForm.xaml
	/// </summary>
	public partial class GridRowColumnEditWindow : Window
	{
		#region Declarations

		private enum GridAction
		{
			DeleteRow,
			DeleteColumn,
			InsertRowBefore,
			InsertRowAfter,
			InsertColumnBefore,
			InsertColumnAfter
		}

		#endregion Declarations

		#region Constructors

		public GridRowColumnEditWindow(XmlDocument Document)
		{
			InitializeComponent();
			this.UsersXamlDocument = Document;
		}

		public GridRowColumnEditWindow()
		{
			InitializeComponent();
		}

		#endregion Constructors

		#region Properties

		public XmlDocument UsersXamlDocument
		{
			get;
			set;
		}

		#endregion Properties

		#region Enums
		

		private enum InsertLocation
		{
			Before,
			After
		}

		private enum SpanType
		{
			Column,
			Row
		}
		#endregion

		#region Methods

		private static XmlAttribute GetFirstNamedAttribute(XmlAttributeCollection nodeAttributes, string attributeName)
		{
			return (from XmlAttribute a in ((XmlAttributeCollection)nodeAttributes)
					where a.Name == attributeName
					select a).FirstOrDefault();
		}

		private static bool GetIsRowRequested<TDefinition>()
			where TDefinition : DefinitionBase
		{
			// Determine the type for the request.
			bool IsRowRequested = typeof(TDefinition) == typeof(RowDefinition);
			if (!IsRowRequested && typeof(TDefinition) != typeof(ColumnDefinition))
				throw new InvalidOperationException("Generic method requires either RowDefinition type or ColumnDefinion");
			return IsRowRequested;
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void btnCreate_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void ContextMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var rectangle = (sender as ContextMenu).PlacementTarget as Rectangle;

			// Original code checked rectangle for null as if it might be possible
			// we might not be able to determine the location of the context
			// menu's placment, but it only skipped the initialization of
			// rectangleRow, and rectangleColumn if rectangle was null. But
			// if if rectangleRow and rectangleColumn are not initialized,
			// we would not know which row and column to work on in the
			// switch block below. So I added to the logic flow an exit if we
			// don't really get a rectangle. I'll flatten the code and do a
			// little refactoring at the same time.

			if (rectangle == null)
				return;

			int rectangleRow = (int)rectangle.GetValue(Grid.RowProperty);
			int rectangleColumn = (int)rectangle.GetValue(Grid.ColumnProperty);

			switch ((GridAction)((e.OriginalSource as MenuItem).Tag))
			{
				case GridAction.DeleteRow:
					DeleteSpan<RowDefinition>(rectangleRow);
					break;

				case GridAction.DeleteColumn:
					DeleteSpan<ColumnDefinition>(rectangleColumn);
					break;

				case GridAction.InsertRowBefore:
					InsertSpan(rectangleRow, InsertLocation.Before, SpanType.Row);
					break;

				case GridAction.InsertRowAfter:
					InsertSpan(rectangleRow, InsertLocation.After, SpanType.Row);
					break;

				case GridAction.InsertColumnBefore:
					InsertSpan(rectangleColumn, InsertLocation.Before, SpanType.Column);
					break;

				case GridAction.InsertColumnAfter:
					InsertSpan(rectangleColumn, InsertLocation.After, SpanType.Column);
					break;

				default:
					break;
			}
		}

		private void DeleteSpan<TDefinition>(int spanToDeleteIndex)
			where TDefinition : DefinitionBase
		{
			bool IsRowDeleteRequested = GetIsRowRequested<TDefinition>();

			// Dummy just helps to specify the generic method.
			// No real value is returned through it, but
			// something must be assigned to it to prevent
			// a compiler error.
			// dummy = null;

			List<TDefinition> spanDefinitions;

			GetSpanDefinitions(out spanDefinitions);
			var columnDefinitions = spanDefinitions as List<ColumnDefinition>;
			var rowDefinitions = spanDefinitions as List<RowDefinition>;

			if (spanDefinitions == null || IsRowDeleteRequested ? rowDefinitions.Count == 0 : columnDefinitions.Count == 0)
				return;

			XmlNode removeNode = null;
			XmlNode removeWhiteSpace = null;
			int nodeIndex = 0;

			var spanName = IsRowDeleteRequested ? "Grid.RowDefinitions" : "Grid.ColumnDefinitions";

			var gridDotDefinitions = (from XmlNode x in UsersXamlDocument.ChildNodes[0].ChildNodes
									  where x.Name == spanName
									  select x).FirstOrDefault();

			if (gridDotDefinitions == null)
			{
				foreach (XmlNode span in gridDotDefinitions.ChildNodes)
				{
					if (span.NodeType != XmlNodeType.Whitespace && span.NodeType != XmlNodeType.Comment)
					{
						if (nodeIndex == spanToDeleteIndex)
						{
							removeNode = span;
							break;
						}
						nodeIndex++;
					}
					else if (span.NodeType == XmlNodeType.Whitespace)
					{
						removeWhiteSpace = span;
					}
				}

				if (removeNode == null)
					return;

				if (removeWhiteSpace != null)
				{
					gridDotDefinitions.RemoveChild(removeWhiteSpace);
				}

				gridDotDefinitions.RemoveChild(removeNode);
				IncrementRowOrColumnOnOrAfter(spanToDeleteIndex, -1, IsRowDeleteRequested ? SpanType.Row : SpanType.Column);
			}
		}

		private void IncrementRowOrColumnOnOrAfter(int startIndex, int incrementValue, SpanType spanType)
		{
			var spanName = spanType == SpanType.Column ? "Grid.Column" : "Grid.Row";

			foreach (var xmlNode in (from XmlNode x in UsersXamlDocument.ChildNodes[0].ChildNodes
									 where x.Name.StartsWith("Grid.") == false && x.Name != "#whitespace"
									 select x))
			{
				var attribute = (from XmlAttribute a in xmlNode.Attributes
								 where a.Name == spanName
								 select a).FirstOrDefault();

				var gridSpanIndex = int.Parse(attribute.Value);

				if (gridSpanIndex >= startIndex)
					attribute.Value = (gridSpanIndex + incrementValue > 0 ? gridSpanIndex + incrementValue : 0).ToString();
			}
		}

		private void InsertSpan(int spanIndex, GridRowColumnEditWindow.InsertLocation loc, SpanType spanType)
		{
			string orientationSizeName;
			string orientationSpanName;

			if (spanType == SpanType.Row)
			{
				orientationSizeName = "Height";
				orientationSpanName = "Row";
			}
			else
			{
				orientationSizeName = "Width";
				orientationSpanName = "Column";
			}

			var orientationDefinition = String.Format("{0}Definition", orientationSpanName);
			var orientationDefinitions = String.Format("Grid.{0}s", orientationDefinition);

			var definitionNodeCollection = (from XmlNode x in UsersXamlDocument.ChildNodes[0].ChildNodes
											where x.Name == orientationDefinitions
											select x).First();

			XmlNode spanDefinitionWhiteSpaceNode = null;
			var spanDefinifitionNodesWithoutWhiteSpace = new List<XmlNode>();

			foreach (XmlNode node in definitionNodeCollection.ChildNodes)
			{
				if (node.NodeType != XmlNodeType.Whitespace && node.NodeType != XmlNodeType.Comment)
					spanDefinifitionNodesWithoutWhiteSpace.Add(node);
				else
					spanDefinitionWhiteSpaceNode = node.CloneNode(true);
			}

			List<RowDefinition> rowDefinitions = null;
			List<ColumnDefinition> columnDefintions = null;
			XmlElement newSpanElement = null;

			if (spanType == SpanType.Row)
			{
				GetSpanDefinitions(out rowDefinitions);
				newSpanElement = UsersXamlDocument.CreateNode(XmlNodeType.Element, "", orientationDefinition, string.Empty) as XmlElement;
				newSpanElement.SetAttribute(orientationSizeName, ParseGridDefinitionLength(rowDefinitions[spanIndex].Height));
			}
			else
			{
				GetSpanDefinitions(out columnDefintions);
				newSpanElement = UsersXamlDocument.CreateNode(XmlNodeType.Element, "", orientationDefinition, string.Empty) as XmlElement;
				newSpanElement.SetAttribute(orientationSizeName, ParseGridDefinitionLength(columnDefintions[spanIndex].Width));
			}
			newSpanElement.SetAttribute("Tag", "New");

			if (loc == InsertLocation.Before)
			{
				definitionNodeCollection.InsertBefore(newSpanElement, spanDefinifitionNodesWithoutWhiteSpace[spanIndex]);

				if (spanDefinitionWhiteSpaceNode != null)
				{
					definitionNodeCollection.InsertBefore(spanDefinitionWhiteSpaceNode, spanDefinifitionNodesWithoutWhiteSpace[spanIndex]);
				}

				IncrementRowOrColumnOnOrAfter(spanIndex, 1, spanType);
			}
			else if (loc == InsertLocation.After)
			{
				definitionNodeCollection.InsertAfter(newSpanElement, spanDefinifitionNodesWithoutWhiteSpace[spanIndex]);

				if (spanDefinitionWhiteSpaceNode != null)
				{
					definitionNodeCollection.InsertAfter(spanDefinitionWhiteSpaceNode, spanDefinifitionNodesWithoutWhiteSpace[spanIndex]);
				}

				IncrementRowOrColumnOnOrAfter(spanIndex + 1, 1, spanType);
			}
			else
			{
				throw new ArgumentOutOfRangeException("loc", loc, "The value passed in was not programmed.");
			}

			BuildGrid();
		}


		// Parses a GridLength object and returns a string for defining'
		// a Width or Hight atributes in xaml.
		private string ParseGridDefinitionLength(GridLength obj)
		{
			if (obj.IsAuto)
				return "Auto";

			if (!obj.IsStar)
				return obj.Value.ToString();

			if (obj.Value != 1 && obj.Value != 0)
				return string.Format("{0}*", obj.Value);

			return "*";
		}

		private static void PromoteAttribute(XmlNode xmlNode, XmlAttribute attribute)
		{
			xmlNode.Attributes.Prepend(xmlNode.Attributes.Remove(attribute));
		}
		#endregion Methods

		#region Loaded Methods

		// Insures that a Grid control has at least one Grid.Row element and one
		// Grid.Column element.
		private void AddMissingGridElementAttributes()
		{
			foreach (var xmlNode in (from XmlNode x in UsersXamlDocument.ChildNodes[0].ChildNodes
									 where x.Name.StartsWith("Grid.") == false && x.NodeType != XmlNodeType.Whitespace && x.NodeType != XmlNodeType.Comment
									 select x))
			{
				CheckAndFixMissingRowOrColumnAttribute(xmlNode, "Grid.Row");
				CheckAndFixMissingRowOrColumnAttribute(xmlNode, "Grid.Column");
			}

			CheckAndFixMissingDefinitionsElement();
		}

		// Adds a new column or row to a list of column or row definitions.
		// This overload does not take a GridUnitType so it defaults to
		// pixel sizes.
		private void AddNewSpanDefinition<TDefinition>(List<TDefinition> SpanDefinitions, int size, bool AddTagAttribute)
			where TDefinition : DefinitionBase
		{
			// Determine the type for the request.
			bool IsRowRequested = GetIsRowRequested<TDefinition>();

			if (IsRowRequested)
			{
				var newDefinition = new RowDefinition()
							{
								Height = new GridLength(size),
								Tag = AddTagAttribute ? "New" : null
							};
				(SpanDefinitions as List<RowDefinition>).Add(newDefinition);
			}
			else
			{
				var newDefinition = new ColumnDefinition()
							{
								Width = new GridLength(size),
								Tag = AddTagAttribute ? "New" : null
							};
				(SpanDefinitions as List<ColumnDefinition>).Add(newDefinition);
			}
		}

		// Adds a new column or row to a list of column or row definitions.
		// This overload also specifies a GridUnitType
		private void AddNewSpanDefinition<TDefinition>(List<TDefinition> SpanDefinitions, int size, GridUnitType GridUnitType, bool AddTagAttribute)
			where TDefinition : DefinitionBase
		{
			// Determine the type for the request.
			bool IsRowRequested = GetIsRowRequested<TDefinition>();

			if (IsRowRequested)
			{
				var newDefinition = new RowDefinition()
							{
								Height = new GridLength(size, GridUnitType),
								Tag = AddTagAttribute ? "New" : null
							};
				(SpanDefinitions as List<RowDefinition>).Add(newDefinition);
			}
			else
			{
				var newDefinition = new ColumnDefinition()
							{
								Width = new GridLength(size, GridUnitType),
								Tag = AddTagAttribute ? "New" : null
							};
				(SpanDefinitions as List<ColumnDefinition>).Add(newDefinition);
			}
		}

		// Adds a list of rows and a list of Columns to the window.
		private void AddRowsAndColumnsToWindowsGrid(List<RowDefinition> rowDefinitions, List<ColumnDefinition> columnDefinitions)
		{
			// Add Rows and Columns to Window's Grid
			int rowIndex = 0;
			int columnIndex;

			foreach (var row in rowDefinitions)
			{
				columnIndex = 0;
				foreach (var column in columnDefinitions)
				{
					var rectangle = new Rectangle()
					{
						Margin = new Thickness(5),
						Stroke = new SolidColorBrush(Colors.Gray),
						StrokeThickness = 1,
						Fill = new SolidColorBrush(Colors.WhiteSmoke),
						VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
						HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
					};

					if (rowIndex % 2 == 1)
						rectangle.Fill = new SolidColorBrush(Colors.AntiqueWhite);

					if (row.Tag != null && column.Tag != null)
					{
						rectangle.Stroke = new SolidColorBrush(Colors.Blue);
						rectangle.StrokeThickness = 2;
					}

					rectangle.SetValue(Grid.RowProperty, rowIndex);
					rectangle.SetValue(Grid.ColumnProperty, columnIndex);
					rectangle.ContextMenu = CreateCellContextMenu();
					gridLayout.Children.Add(rectangle);
					columnIndex++;
				}
				rowIndex++;
			}
		}

		// Builds this windows Grid based on the selected Grid of the user.
		private void BuildGrid()
		{
			List<RowDefinition> rowDefs;
			List<ColumnDefinition> colDefs;

			GetUsersRowAndColumnDefinitions(out rowDefs, out colDefs);
			AddRowsAndColumnsToWindowsGrid(rowDefs, colDefs);
		}

		// Checks the document that represents the user's grid, and insures
		// that it contains row and column definitions (at least one of each.
		// If such are not found, they are added.
		private void CheckAndFixMissingDefinitionsElement()
		{
			string mainElementname = "Grid.RowDefinitions";
			string orientation = "Height";
			string subElementName = "RowDefinition";

			for (var x=0; x < 2; x++)
			{
				var definitionElement = from XmlNode e in UsersXamlDocument.ChildNodes[0].ChildNodes
										where e.Name == mainElementname
										select e;

				if (definitionElement == null || definitionElement.Count() == 0)
				{
					var newDefinitionsElement = UsersXamlDocument.CreateElement(mainElementname);
					var newDefinitionAttribute = UsersXamlDocument.CreateAttribute(orientation);
					newDefinitionAttribute.Value = "*";

					var newDefinitionElement = UsersXamlDocument.CreateElement(subElementName);
					newDefinitionElement.Attributes.Prepend(newDefinitionAttribute);
					newDefinitionsElement.PrependChild(newDefinitionElement);
					UsersXamlDocument.ChildNodes[0].PrependChild(newDefinitionsElement);
				}

				mainElementname = "Grid.ColumnDefinitions";
				orientation = "Width";
				subElementName = "ColumnDefinition";
			}
		}

		// Checks to make sure that a node has a specified attribute, and adds
		// it if it does not.
		private void CheckAndFixMissingRowOrColumnAttribute(XmlNode xmlNode, string AttributeName)
		{
			var attribute = GetFirstNamedAttribute(xmlNode.Attributes, AttributeName);

			if (attribute == null)
			{
				var newAttribute = UsersXamlDocument.CreateAttribute(AttributeName);
				newAttribute.Value = "0";
				xmlNode.Attributes.Prepend(newAttribute);
			}
			else
			{
				PromoteAttribute(xmlNode, attribute);
			}
		}

		// Creates the context menu for the cells of the window's grid.
		private ContextMenu CreateCellContextMenu()
		{
			var cm = new ContextMenu();
			var mi = new MenuItem()
			{
				Header = "Column Commands"
			};
			mi.Items.Add(new MenuItem()
			{
				Header = "Insert Column Before",
				Tag = GridAction.InsertColumnBefore
			});
			mi.Items.Add(new MenuItem()
			{
				Header = "Insert Column After",
				Tag = GridAction.InsertColumnAfter
			});
			mi.Items.Add(new MenuItem()
			{
				Header = "Delete Column",
				Tag = GridAction.DeleteColumn
			});
			cm.Items.Add(mi);
			mi = new MenuItem()
			{
				Header = "Row Commands"
			};
			mi.Items.Add(new MenuItem()
			{
				Header = "Insert Row Before",
				Tag = GridAction.InsertRowBefore
			});
			mi.Items.Add(new MenuItem()
			{
				Header = "Inser Row After",
				Tag = GridAction.InsertRowAfter
			});
			mi.Items.Add(new MenuItem()
			{
				Header = "Delete Row",
				Tag = GridAction.DeleteRow
			});
			cm.Items.Add(mi);

			return cm;
		}

		// Builds a list of row or column definitions from the user's grid.
		private void GetSpanDefinitions<TDefinition>(out List<TDefinition> definitions)
			where TDefinition : DefinitionBase
		{
			// Determine the type for the request.
			bool IsRowRequested = GetIsRowRequested<TDefinition>();

			definitions = null;
			var spanName = IsRowRequested ? "Grid.RowDefinitions" : "Grid.ColumnDefinitions";

			definitions = new List<TDefinition>();
			var definitionNode = (from XmlNode x in UsersXamlDocument.ChildNodes[0].ChildNodes
								  where x.Name == spanName
								  select x).First();

			// If there are no children, create a default span, and return it.
			if (definitionNode == null)
			{
				AddNewSpanDefinition(definitions, 1, GridUnitType.Star, false);
				return;
			}

			foreach (XmlNode node in definitionNode.ChildNodes)
			{
				// if this node is all whitespace or a comment, ignore it.
				if (node.NodeType == XmlNodeType.Whitespace || node.NodeType == XmlNodeType.Comment)
					continue;

				var addTag = node.Attributes.GetNamedItem("Tag") != null;

				var heightAttribute = node.Attributes.GetNamedItem("Height") as XmlAttribute;

				// If there is no height attribute
				if (heightAttribute == null)
				{
					AddNewSpanDefinition(definitions, 1, GridUnitType.Star, addTag);
					continue;
				}

				// if the height attribute is "Auto" (case insignificant comparison)
				if (string.Compare(heightAttribute.Value, "Auto", true) == 0)
				{
					AddNewSpanDefinition(definitions, 0, GridUnitType.Auto, addTag);
					continue;
				}

				// if the height attribute does not have a star, it's just a number.
				if (heightAttribute.Value.Contains("*") == false)
				{
					// Assume that the Value string is numeric
					AddNewSpanDefinition(definitions, int.Parse(heightAttribute.Value), addTag);
					continue;
				}

				// if the height attribute has a star, it's just a star, or a
				// number and a star.
				int starHeight;

				if (!int.TryParse(heightAttribute.Value.Replace("*", string.Empty), out starHeight))
					starHeight = 1;
				AddNewSpanDefinition(definitions, starHeight, GridUnitType.Star, addTag);
			}
			return;
		}

		// Builds a list of row definitions and a list of column definitions 
		// from the user's grid.
		private void GetUsersRowAndColumnDefinitions(out List<RowDefinition> rowDefinitions, out List<ColumnDefinition> columnDefinitions)
		{
			// Get User's Row and Column Definitions
			GetSpanDefinitions(out rowDefinitions);
			GetSpanDefinitions(out columnDefinitions);

			gridLayout.Children.Clear();
			gridLayout.RowDefinitions.Clear();
			gridLayout.ColumnDefinitions.Clear();
			gridLayout.ShowGridLines = true;

			foreach (var row in rowDefinitions)
			{
				gridLayout.RowDefinitions.Add(new RowDefinition()
				{
					Height = new GridLength(40)
				});
			}

			foreach (var column in columnDefinitions)
			{
				gridLayout.ColumnDefinitions.Add(new ColumnDefinition()
				{
					Width = new GridLength(80)
				});
			}
		}

		// Handles the Loaded event for the window. Used to create the initial
		// grid seen in the window when first opened.
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			AddMissingGridElementAttributes();
			BuildGrid();
			EventManager.RegisterClassHandler(typeof(ContextMenu), MenuItem.ClickEvent, new RoutedEventHandler(ContextMenuItem_Click));
		}

		#endregion Loaded Methods
	}
}