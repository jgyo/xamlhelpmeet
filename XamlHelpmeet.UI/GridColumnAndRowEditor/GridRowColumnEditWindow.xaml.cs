namespace XamlHelpmeet.UI.GridColumnAndRowEditor
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Xml;

    #endregion

    /// <summary>
    ///     Interaction logic for GridRowColunnEditForm.xaml.
    /// </summary>
    public partial class GridRowColumnEditWindow : Window
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the GridRowColumnEditWindow class.
        /// </summary>
        /// <param name="Document">
        ///     The document.
        /// </param>
        public GridRowColumnEditWindow(XmlDocument Document)
        {
            this.InitializeComponent();
            this.UsersXamlDocument = Document;
        }

        /// <summary>
        ///     Initializes a new instance of the GridRowColumnEditWindow class.
        /// </summary>
        public GridRowColumnEditWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties and Indexers

        /// <summary>
        ///     Gets or sets the users XAML document.
        /// </summary>
        /// <value>
        ///     The users XAML document.
        /// </value>
        public XmlDocument UsersXamlDocument { get; set; }

        #endregion

        #region Methods (private)

        /// <summary>
        ///     Insures that a Grid control has at least one Grid.Row element and one
        ///     Grid.Column element.
        /// </summary>
        private void AddMissingGridElementAttributes()
        {
            foreach (var xmlNode in (from XmlNode x in this.UsersXamlDocument.ChildNodes[0].ChildNodes
                                     where
                                         x.Name.StartsWith("Grid.") == false && x.NodeType != XmlNodeType.Whitespace
                                         && x.NodeType != XmlNodeType.Comment
                                     select x))
            {
                this.CheckAndFixMissingRowOrColumnAttribute(xmlNode, "Grid.Row");
                this.CheckAndFixMissingRowOrColumnAttribute(xmlNode, "Grid.Column");
            }

            this.CheckAndFixMissingDefinitionsElement();
        }

        /// <summary>
        ///     Adds a new column or row to a list of column or row definitions. This
        ///     overload does not take a GridUnitType so it defaults to pixel sizes.
        /// </summary>
        /// <typeparam name="TDefinition">
        ///     Type of the definition.
        /// </typeparam>
        /// <param name="SpanDefinitions">
        ///     The span definitions.
        /// </param>
        /// <param name="size">
        ///     The size.
        /// </param>
        /// <param name="AddTagAttribute">
        ///     true to add tag attribute.
        /// </param>
        private void AddNewSpanDefinition<TDefinition>(List<TDefinition> SpanDefinitions, int size, bool AddTagAttribute)
            where TDefinition : DefinitionBase
        {
            List<RowDefinition> rowDefinitions;
            List<ColumnDefinition> columnDefinitions;

            // Determine the type for the request.
            bool IsRowRequested = GetIsRowRequested<TDefinition>();

            if (IsRowRequested)
            {
                var newDefinition = new RowDefinition
                                    {
                                        Height = new GridLength(size),
                                        Tag = AddTagAttribute ? "New" : null
                                    };
                rowDefinitions = SpanDefinitions as List<RowDefinition>;
                Debug.Assert(rowDefinitions != null, "rowDefinitions != null");
                rowDefinitions.Add(newDefinition);
            }
            else
            {
                var newDefinition = new ColumnDefinition
                                    {
                                        Width = new GridLength(size),
                                        Tag = AddTagAttribute ? "New" : null
                                    };
                columnDefinitions = SpanDefinitions as List<ColumnDefinition>;
                Debug.Assert(columnDefinitions != null, "columnDefinitions != null");
                columnDefinitions.Add(newDefinition);
            }
        }

        /// <summary>
        ///     Adds a new column or row to a list of column or row definitions. This
        ///     overload also specifies a GridUnitType.
        /// </summary>
        /// <typeparam name="TDefinition">
        ///     Type of the definition.
        /// </typeparam>
        /// <param name="SpanDefinitions">
        ///     The span definitions.
        /// </param>
        /// <param name="size">
        ///     The size.
        /// </param>
        /// <param name="GridUnitType">
        ///     Type of the grid unit.
        /// </param>
        /// <param name="AddTagAttribute">
        ///     true to add tag attribute.
        /// </param>
        private void AddNewSpanDefinition<TDefinition>(List<TDefinition> SpanDefinitions,
                                                       int size,
                                                       GridUnitType GridUnitType,
                                                       bool AddTagAttribute) where TDefinition : DefinitionBase
        {
            List<RowDefinition> rowDefinitions;
            List<ColumnDefinition> columnDefinitions;

            // Determine the type for the request.
            bool IsRowRequested = GetIsRowRequested<TDefinition>();

            if (IsRowRequested)
            {
                var newDefinition = new RowDefinition
                                    {
                                        Height = new GridLength(size, GridUnitType),
                                        Tag = AddTagAttribute ? "New" : null
                                    };
                rowDefinitions = SpanDefinitions as List<RowDefinition>;
                Debug.Assert(rowDefinitions != null, "rowDefinitions != null");
                rowDefinitions.Add(newDefinition);
            }
            else
            {
                var newDefinition = new ColumnDefinition
                                    {
                                        Width = new GridLength(size, GridUnitType),
                                        Tag = AddTagAttribute ? "New" : null
                                    };
                columnDefinitions = SpanDefinitions as List<ColumnDefinition>;
                Debug.Assert(columnDefinitions != null, "columnDefinitions != null");
                columnDefinitions.Add(newDefinition);
            }
        }

        /// <summary>
        ///     Adds a list of rows and a list of Columns to the window.
        /// </summary>
        /// <param name="rowDefinitions">
        ///     The row definitions.
        /// </param>
        /// <param name="columnDefinitions">
        ///     The column definitions.
        /// </param>
        private void AddRowsAndColumnsToWindowsGrid(List<RowDefinition> rowDefinitions,
                                                    List<ColumnDefinition> columnDefinitions)
        {
            // Add Rows and Columns to Window's Grid
            int rowIndex = 0;
            int columnIndex;

            foreach (var row in rowDefinitions)
            {
                columnIndex = 0;
                foreach (var column in columnDefinitions)
                {
                    var rectangle = new Rectangle
                                    {
                                        Margin = new Thickness(5),
                                        Stroke = new SolidColorBrush(Colors.Gray),
                                        StrokeThickness = 1,
                                        Fill = new SolidColorBrush(Colors.WhiteSmoke),
                                        VerticalAlignment = VerticalAlignment.Stretch,
                                        HorizontalAlignment = HorizontalAlignment.Stretch
                                    };

                    if (rowIndex % 2 == 1)
                    {
                        rectangle.Fill = new SolidColorBrush(Colors.AntiqueWhite);
                    }

                    if (row.Tag != null && column.Tag != null)
                    {
                        rectangle.Stroke = new SolidColorBrush(Colors.Blue);
                        rectangle.StrokeThickness = 2;
                    }

                    rectangle.SetValue(Grid.RowProperty, rowIndex);
                    rectangle.SetValue(Grid.ColumnProperty, columnIndex);
                    rectangle.ContextMenu = this.CreateCellContextMenu();
                    this.gridLayout.Children.Add(rectangle);
                    columnIndex++;
                }
                rowIndex++;
            }
        }

        /// <summary>
        ///     Builds this windows Grid based on the selected Grid of the user.
        /// </summary>
        private void BuildGrid()
        {
            List<RowDefinition> rowDefs;
            List<ColumnDefinition> colDefs;

            this.GetUsersRowAndColumnDefinitions(out rowDefs, out colDefs);
            this.AddRowsAndColumnsToWindowsGrid(rowDefs, colDefs);
        }

        /// <summary>
        ///     Checks the document that represents the user's grid, and insures that it
        ///     contains row and column definitions (at least one of each. If such are
        ///     not found, they are added.
        /// </summary>
        private void CheckAndFixMissingDefinitionsElement()
        {
            string mainElementname = "Grid.RowDefinitions";
            string orientation = "Height";
            string subElementName = "RowDefinition";

            for (int x = 0; x < 2; x++)
            {
                IEnumerable<XmlNode> definitionElement =
                    from XmlNode e in this.UsersXamlDocument.ChildNodes[0].ChildNodes
                    where e.Name == mainElementname
                    select e;

                if (definitionElement.Count() == 0)
                {
                    XmlElement newDefinitionsElement = this.UsersXamlDocument.CreateElement(mainElementname);
                    XmlAttribute newDefinitionAttribute = this.UsersXamlDocument.CreateAttribute(orientation);
                    newDefinitionAttribute.Value = "*";

                    XmlElement newDefinitionElement = this.UsersXamlDocument.CreateElement(subElementName);
                    newDefinitionElement.Attributes.Prepend(newDefinitionAttribute);
                    newDefinitionsElement.PrependChild(newDefinitionElement);
                    this.UsersXamlDocument.ChildNodes[0].PrependChild(newDefinitionsElement);
                }

                mainElementname = "Grid.ColumnDefinitions";
                orientation = "Width";
                subElementName = "ColumnDefinition";
            }
        }

        /// <summary>
        ///     Checks to make sure that a node has a specified attribute, and adds it if
        ///     it does not.
        /// </summary>
        /// <param name="xmlNode">
        ///     The XML node.
        /// </param>
        /// <param name="AttributeName">
        ///     Name of the attribute.
        /// </param>
        private void CheckAndFixMissingRowOrColumnAttribute(XmlNode xmlNode, string AttributeName)
        {
            XmlAttribute attribute = GetFirstNamedAttribute(xmlNode.Attributes, AttributeName);

            if (attribute == null)
            {
                XmlAttribute newAttribute = this.UsersXamlDocument.CreateAttribute(AttributeName);
                newAttribute.Value = "0";
                xmlNode.Attributes.Prepend(newAttribute);
            }
            else
            {
                PromoteAttribute(xmlNode, attribute);
            }
        }

        /// <summary>
        ///     Event handler. Called by ContextMenuItem for click events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
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
            {
                return;
            }

            var rectangleRow = (int)rectangle.GetValue(Grid.RowProperty);
            var rectangleColumn = (int)rectangle.GetValue(Grid.ColumnProperty);

            switch ((GridAction)((e.OriginalSource as MenuItem).Tag))
            {
                case GridAction.DeleteRow:
                    this.DeleteSpan<RowDefinition>(rectangleRow);
                    break;

                case GridAction.DeleteColumn:
                    this.DeleteSpan<ColumnDefinition>(rectangleColumn);
                    break;

                case GridAction.InsertRowBefore:
                    this.InsertSpan(rectangleRow, InsertLocation.Before, SpanType.Row);
                    break;

                case GridAction.InsertRowAfter:
                    this.InsertSpan(rectangleRow, InsertLocation.After, SpanType.Row);
                    break;

                case GridAction.InsertColumnBefore:
                    this.InsertSpan(rectangleColumn, InsertLocation.Before, SpanType.Column);
                    break;

                case GridAction.InsertColumnAfter:
                    this.InsertSpan(rectangleColumn, InsertLocation.After, SpanType.Column);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        ///     Creates the context menu for the cells of the window's grid.
        /// </summary>
        /// <returns>
        ///     The new cell context menu.
        /// </returns>
        private ContextMenu CreateCellContextMenu()
        {
            var cm = new ContextMenu();
            var mi = new MenuItem { Header = "Column Commands" };
            mi.Items.Add(new MenuItem { Header = "Insert Column Before", Tag = GridAction.InsertColumnBefore });
            mi.Items.Add(new MenuItem { Header = "Insert Column After", Tag = GridAction.InsertColumnAfter });
            mi.Items.Add(new MenuItem { Header = "Delete Column", Tag = GridAction.DeleteColumn });
            cm.Items.Add(mi);
            mi = new MenuItem { Header = "Row Commands" };
            mi.Items.Add(new MenuItem { Header = "Insert Row Before", Tag = GridAction.InsertRowBefore });
            mi.Items.Add(new MenuItem { Header = "Insert Row After", Tag = GridAction.InsertRowAfter });
            mi.Items.Add(new MenuItem { Header = "Delete Row", Tag = GridAction.DeleteRow });
            cm.Items.Add(mi);

            return cm;
        }

        /// <summary>
        ///     Deletes the span described by spanToDeleteIndex.
        /// </summary>
        /// <typeparam name="TDefinition">
        ///     Type of the definition.
        /// </typeparam>
        /// <param name="spanToDeleteIndex">
        ///     Zero-based index of the span to delete.
        /// </param>
        private void DeleteSpan<TDefinition>(int spanToDeleteIndex) where TDefinition : DefinitionBase
        {
            bool IsRowDeleteRequested = GetIsRowRequested<TDefinition>();

            // Dummy just helps to specify the generic method.
            // No real value is returned through it, but
            // something must be assigned to it to prevent
            // a compiler error.
            // dummy = null;

            List<TDefinition> spanDefinitions;

            this.GetSpanDefinitions(out spanDefinitions);
            var columnDefinitions = spanDefinitions as List<ColumnDefinition>;
            var rowDefinitions = spanDefinitions as List<RowDefinition>;

            // Don't allow deleting spans that don't exist.
            if (spanDefinitions == null || IsRowDeleteRequested
                    ? rowDefinitions.Count == 0
                    : columnDefinitions.Count == 0)
            {
                return;
            }

            XmlNode removeNode = null;
            XmlNode removeWhiteSpace = null;
            int nodeIndex = 0;

            string spanName = IsRowDeleteRequested ? "Grid.RowDefinitions" : "Grid.ColumnDefinitions";

            XmlNode gridDotDefinitions =
                (from XmlNode x in this.UsersXamlDocument.ChildNodes[0].ChildNodes where x.Name == spanName select x)
                    .FirstOrDefault();

            if (gridDotDefinitions != null)
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

                // Make sure we have something to delete. Otherwise just leave.
                if (removeNode == null)
                {
                    return;
                }

                // Delete leading whitespace, if any.
                if (removeWhiteSpace != null)
                {
                    gridDotDefinitions.RemoveChild(removeWhiteSpace);
                }

                gridDotDefinitions.RemoveChild(removeNode);
                this.IncrementRowOrColumnOnOrAfter(spanToDeleteIndex,
                                                   -1,
                                                   IsRowDeleteRequested ? SpanType.Row : SpanType.Column);
                // Update the UI.
                this.BuildGrid();
            }
        }

        /// <summary>
        ///     Gets the first named attribute.
        /// </summary>
        /// <param name="nodeAttributes">
        ///     The node attributes.
        /// </param>
        /// <param name="attributeName">
        ///     Name of the attribute.
        /// </param>
        /// <returns>
        ///     The first named attribute.
        /// </returns>
        private static XmlAttribute GetFirstNamedAttribute(XmlAttributeCollection nodeAttributes, string attributeName)
        {
            return (from XmlAttribute a in nodeAttributes where a.Name == attributeName select a).FirstOrDefault();
        }

        /// <summary>
        ///     Gets is row requested.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        /// <typeparam name="TDefinition">
        ///     Type of the definition.
        /// </typeparam>
        /// <returns>
        ///     true if it succeeds, otherwise false.
        /// </returns>
        private static bool GetIsRowRequested<TDefinition>() where TDefinition : DefinitionBase
        {
            // Determine the type for the request.
            bool IsRowRequested = typeof(TDefinition) == typeof(RowDefinition);
            if (!IsRowRequested && typeof(TDefinition) != typeof(ColumnDefinition))
            {
                throw new InvalidOperationException(
                    "Generic method requires either RowDefinition type or ColumnDefinion");
            }
            return IsRowRequested;
        }

        /// <summary>
        ///     Builds a list of row or column definitions from the user's grid.
        /// </summary>
        /// <typeparam name="TDefinition">
        ///     Type of the definition.
        /// </typeparam>
        /// <param name="definitions">
        ///     [out] The definitions.
        /// </param>
        private void GetSpanDefinitions<TDefinition>(out List<TDefinition> definitions)
            where TDefinition : DefinitionBase
        {
            // Determine the type for the request.
            bool IsRowRequested = GetIsRowRequested<TDefinition>();

            definitions = null;
            string spanName = IsRowRequested ? "Grid.RowDefinitions" : "Grid.ColumnDefinitions";

            definitions = new List<TDefinition>();
            XmlNode definitionNode =
                (from XmlNode x in this.UsersXamlDocument.ChildNodes[0].ChildNodes where x.Name == spanName select x)
                    .First();

            // If there are no children, create a default span, and return it.
            if (definitionNode == null)
            {
                this.AddNewSpanDefinition(definitions, 1, GridUnitType.Star, false);
                return;
            }

            foreach (XmlNode node in definitionNode.ChildNodes)
            {
                // if this node is all whitespace or a comment, ignore it.
                if (node.NodeType == XmlNodeType.Whitespace || node.NodeType == XmlNodeType.Comment)
                {
                    continue;
                }

                bool addTag = node.Attributes.GetNamedItem("Tag") != null;

                var heightAttribute = node.Attributes.GetNamedItem("Height") as XmlAttribute;

                // If there is no height attribute
                if (heightAttribute == null)
                {
                    this.AddNewSpanDefinition(definitions, 1, GridUnitType.Star, addTag);
                    continue;
                }

                // if the height attribute is "Auto" (case insignificant comparison)
                if (string.Compare(heightAttribute.Value, "Auto", true) == 0)
                {
                    this.AddNewSpanDefinition(definitions, 0, GridUnitType.Auto, addTag);
                    continue;
                }

                // if the height attribute does not have a star, it's just a number.
                if (heightAttribute.Value.Contains("*") == false)
                {
                    // Assume that the Value string is numeric
                    this.AddNewSpanDefinition(definitions, int.Parse(heightAttribute.Value), addTag);
                    continue;
                }

                // if the height attribute has a star, it's just a star, or a
                // number and a star.
                int starHeight;

                if (!int.TryParse(heightAttribute.Value.Replace("*", string.Empty), out starHeight))
                {
                    starHeight = 1;
                }
                this.AddNewSpanDefinition(definitions, starHeight, GridUnitType.Star, addTag);
            }
        }

        /// <summary>
        ///     Builds a list of row definitions and a list of column definitions from
        ///     the user's grid.
        /// </summary>
        /// <param name="rowDefinitions">
        ///     The row definitions.
        /// </param>
        /// <param name="columnDefinitions">
        ///     The column definitions.
        /// </param>
        private void GetUsersRowAndColumnDefinitions(out List<RowDefinition> rowDefinitions,
                                                     out List<ColumnDefinition> columnDefinitions)
        {
            // Get User's Row and Column Definitions
            this.GetSpanDefinitions(out rowDefinitions);
            this.GetSpanDefinitions(out columnDefinitions);

            this.gridLayout.Children.Clear();
            this.gridLayout.RowDefinitions.Clear();
            this.gridLayout.ColumnDefinitions.Clear();
            this.gridLayout.ShowGridLines = true;

            foreach (var row in rowDefinitions)
            {
                this.gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            }

            foreach (var column in columnDefinitions)
            {
                this.gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
            }
        }

        /// <summary>
        ///     Increment row or column on or after.
        /// </summary>
        /// <param name="startIndex">
        ///     The start index.
        /// </param>
        /// <param name="incrementValue">
        ///     The increment value.
        /// </param>
        /// <param name="spanType">
        ///     Type of the span.
        /// </param>
        private void IncrementRowOrColumnOnOrAfter(int startIndex, int incrementValue, SpanType spanType)
        {
            string spanName = spanType == SpanType.Column ? "Grid.Column" : "Grid.Row";

            foreach (var xmlNode in (from XmlNode x in this.UsersXamlDocument.ChildNodes[0].ChildNodes
                                     where x.Name.StartsWith("Grid.") == false && x.Name != "#whitespace"
                                     select x))
            {
                XmlAttribute attribute =
                    (from XmlAttribute a in xmlNode.Attributes where a.Name == spanName select a).FirstOrDefault();

                int gridSpanIndex = int.Parse(attribute.Value);

                if (gridSpanIndex >= startIndex)
                {
                    attribute.Value =
                        (gridSpanIndex + incrementValue > 0 ? gridSpanIndex + incrementValue : 0).ToString();
                }
            }
        }

        /// <summary>
        ///     Inserts a span.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <param name="spanIndex">
        ///     Zero-based index of the span.
        /// </param>
        /// <param name="loc">
        ///     The location.
        /// </param>
        /// <param name="spanType">
        ///     Type of the span.
        /// </param>
        private void InsertSpan(int spanIndex, InsertLocation loc, SpanType spanType)
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

            string orientationDefinition = String.Format("{0}Definition", orientationSpanName);
            string orientationDefinitions = String.Format("Grid.{0}s", orientationDefinition);

            XmlNode definitionNodeCollection =
                (from XmlNode x in this.UsersXamlDocument.ChildNodes[0].ChildNodes
                 where x.Name == orientationDefinitions
                 select x).First();

            XmlNode spanDefinitionWhiteSpaceNode = null;
            var spanDefinifitionNodesWithoutWhiteSpace = new List<XmlNode>();

            foreach (XmlNode node in definitionNodeCollection.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Whitespace && node.NodeType != XmlNodeType.Comment)
                {
                    spanDefinifitionNodesWithoutWhiteSpace.Add(node);
                }
                else
                {
                    spanDefinitionWhiteSpaceNode = node.CloneNode(true);
                }
            }

            List<RowDefinition> rowDefinitions = null;
            List<ColumnDefinition> columnDefintions = null;
            XmlElement newSpanElement = null;

            if (spanType == SpanType.Row)
            {
                this.GetSpanDefinitions(out rowDefinitions);
                newSpanElement =
                    this.UsersXamlDocument.CreateNode(XmlNodeType.Element, "", orientationDefinition, string.Empty) as
                    XmlElement;
                newSpanElement.SetAttribute(orientationSizeName,
                                            this.ParseGridDefinitionLength(rowDefinitions[spanIndex].Height));
            }
            else
            {
                this.GetSpanDefinitions(out columnDefintions);
                newSpanElement =
                    this.UsersXamlDocument.CreateNode(XmlNodeType.Element, "", orientationDefinition, string.Empty) as
                    XmlElement;
                newSpanElement.SetAttribute(orientationSizeName,
                                            this.ParseGridDefinitionLength(columnDefintions[spanIndex].Width));
            }
            newSpanElement.SetAttribute("Tag", "New");

            if (loc == InsertLocation.Before)
            {
                definitionNodeCollection.InsertBefore(newSpanElement, spanDefinifitionNodesWithoutWhiteSpace[spanIndex]);

                if (spanDefinitionWhiteSpaceNode != null)
                {
                    definitionNodeCollection.InsertBefore(spanDefinitionWhiteSpaceNode,
                                                          spanDefinifitionNodesWithoutWhiteSpace[spanIndex]);
                }

                this.IncrementRowOrColumnOnOrAfter(spanIndex, 1, spanType);
            }
            else if (loc == InsertLocation.After)
            {
                definitionNodeCollection.InsertAfter(newSpanElement, spanDefinifitionNodesWithoutWhiteSpace[spanIndex]);

                if (spanDefinitionWhiteSpaceNode != null)
                {
                    definitionNodeCollection.InsertAfter(spanDefinitionWhiteSpaceNode,
                                                         spanDefinifitionNodesWithoutWhiteSpace[spanIndex]);
                }

                this.IncrementRowOrColumnOnOrAfter(spanIndex + 1, 1, spanType);
            }
            else
            {
                throw new ArgumentOutOfRangeException("loc", loc, "The value passed in was not programmed.");
            }

            this.BuildGrid();
        }

        /// <summary>
        ///     Parses a GridLength object and returns a string for defining' a Width or
        ///     Hight atributes in xaml.
        /// </summary>
        /// <param name="obj">
        ///     The object.
        /// </param>
        /// <returns>
        ///     .
        /// </returns>
        private string ParseGridDefinitionLength(GridLength obj)
        {
            if (obj.IsAuto)
            {
                return "Auto";
            }

            if (!obj.IsStar)
            {
                return obj.Value.ToString();
            }

            if (obj.Value != 1 && obj.Value != 0)
            {
                return string.Format("{0}*", obj.Value);
            }

            return "*";
        }

        /// <summary>
        ///     Promotes the attribute.
        /// </summary>
        /// <param name="xmlNode">
        ///     The XML node.
        /// </param>
        /// <param name="attribute">
        ///     The attribute.
        /// </param>
        private static void PromoteAttribute(XmlNode xmlNode, XmlAttribute attribute)
        {
            xmlNode.Attributes.Prepend(xmlNode.Attributes.Remove(attribute));
        }

        /// <summary>
        ///     Handles the Loaded event for the window. Used to create the initial grid
        ///     seen in the window when first opened.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.AddMissingGridElementAttributes();
            this.BuildGrid();
            EventManager.RegisterClassHandler(typeof(ContextMenu),
                                              MenuItem.ClickEvent,
                                              new RoutedEventHandler(this.ContextMenuItem_Click));
        }

        /// <summary>
        ///     Event handler. Called by btnCancel for click events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        ///     Event handler. Called by btnCreate for click events.
        /// </summary>
        /// <param name="sender">
        ///     Source of the event.
        /// </param>
        /// <param name="e">
        ///     Routed event information.
        /// </param>
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        #endregion

        #region Nested type: GridAction

        /// <summary>
        ///     Values that represent GridAction.
        /// </summary>
        private enum GridAction
        {
            /// <summary>
            ///     .
            /// </summary>
            DeleteRow,

            /// <summary>
            ///     .
            /// </summary>
            DeleteColumn,

            /// <summary>
            ///     .
            /// </summary>
            InsertRowBefore,

            /// <summary>
            ///     .
            /// </summary>
            InsertRowAfter,

            /// <summary>
            ///     .
            /// </summary>
            InsertColumnBefore,

            /// <summary>
            ///     .
            /// </summary>
            InsertColumnAfter
        }

        #endregion

        #region Nested type: InsertLocation

        /// <summary>
        ///     Values that represent InsertLocation.
        /// </summary>
        private enum InsertLocation
        {
            /// <summary>
            ///     .
            /// </summary>
            Before,

            /// <summary>
            ///     .
            /// </summary>
            After
        }

        #endregion

        #region Nested type: SpanType

        /// <summary>
        ///     Values that represent SpanType.
        /// </summary>
        private enum SpanType
        {
            /// <summary>
            ///     .
            /// </summary>
            Column,

            /// <summary>
            ///     .
            /// </summary>
            Row
        }

        #endregion
    }
}
