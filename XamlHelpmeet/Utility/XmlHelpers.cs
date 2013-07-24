// <copyright file="XmlHelpers.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/7/2013</date>
// <summary>Implements the xml helpers class</summary>
namespace XamlHelpmeet.Utility
{
    #region Imports

    using System;
    using System.Text.RegularExpressions;
    using EnvDTE;
    using XamlHelpmeet.Extensions;

    #endregion

    /// <summary>
    ///     Xml helpers.
    /// </summary>
    public static class XmlHelpers
    {
        #region Constants

        /// <summary>
        ///     A pattern specifying an attribute.
        /// </summary>
        public const string ATTRIBUTE_PATTERN =
            @"(?:\s+(?<Attribute>(?<AttribName>\w+?(?::\w+?)?)=""(?<AttribValue>.*?)""))";

        /// <summary>
        ///     A pattern specifying a comment.
        /// </summary>
        public const string COMMENT_PATTERN = @"<!--
\s*?
(?<Comment>\b[\w\s[-\]\\.!?""',:/|[}{()+=*&^%$#@!~`]*?)
\s*?
-->";

        /// <summary>
        ///     A pattern specifying an end tag.
        /// </summary>
        public const string ENDTAG_PATTERN = @"</(?<EndTagName>\w+?(?::\w+?)?(?:\.\w+?)*?)
(?:\s+(?:\w+?(?::\w+?)?=""[^><]*?""))*
\s*?>";

        /// <summary>
        ///     A pattern specifying an start tag.
        /// </summary>
        public const string STARTTAG_PATTERN = @"<(?<StartTagName>[\w:.]+?)(?=[\s/>])
(?:\s+(?<AttribName>[\w\.]+?(?::\w+?)?="".*?""))*
\s*?/?>";

        /// <summary>
        ///     A pattern specifying tagnamechars.
        /// </summary>
        public const string TAGNAMECHARS_PATTERN = @"[\w:\.]+?";

        /// <summary>
        ///     A pattern specifying xml comments.
        /// </summary>
        public const string XMLCOMMENTS_PATTERN = @"<!--
\s*?
(?<Comment>\b[\w\s[-\]\\.!?""',:/|[}{()+=*&^%$#@!~`]*?)
\s*?
-->";

        #endregion

        #region Fields

        private static string _documentText;

        private static XamlNode _rootNode;

        #endregion

        #region Methods (public)

        /// <summary>
        ///     A TextSelection extension method that determine if we are siblings
        ///     selected.
        /// </summary>
        /// <param name="selection">
        ///     The target to act on.
        /// </param>
        /// <returns>
        ///     true if siblings selected, otherwise false.
        /// </returns>
        public static bool AreSiblingsSelected(this TextSelection selection)
        {
            EditorPoints ep = GetEditorPoints(selection);
            if (ep.IsInvalid)
            {
                return false;
            }
            //ep.RestoreSelectedText(selection);
            Tuple<XamlNode, XamlNode> firstAndLast = _rootNode.GetFirstLastNodesBetweenPoints(ep.TopPoint - 1,
                                                                                              ep.BottomPoint - 1);
            return firstAndLast.Item1.Parent == firstAndLast.Item2.Parent;
        }

        /// <summary>
        ///     A TextSelection extension method that contract selection.
        /// </summary>
        /// <param name="selection">
        ///     The target to act on.
        /// </param>
        /// <returns>
        ///     .
        /// </returns>
        public static NarrowSelectionResult ContractSelection(this TextSelection selection)
        {
            if (selection.IsEmpty)
            {
                return NarrowSelectionResult.SelectionIsEmpty;
            }

            //! If the selection begins or ends other than at the beginning and
            //! ending of a node, narrow the selection to include only the node.
            //
            //! If the selection begins and ends at the ends of a node, narrow
            //! the selection by removing the node tags from the selection.
            //
            //! If neither of the above, report a failure.

            // Get the original end points of the selection
            EditorPoints origEndPoints = GetEditorPoints(selection);

            // Get the first and last selected nodes.
            Tuple<XamlNode, XamlNode> firstAndLast = _rootNode.GetFirstLastNodesBetweenPoints(
                                                                                              origEndPoints.TopPoint - 1,
                                                                                              origEndPoints.BottomPoint
                                                                                              - 1);

            //+ Case: No nodes are returned
            //  Means that the selection nodes were not within one parent
            if (firstAndLast == null)
            {
                return NarrowSelectionResult.InconsistentSelectionEnds;
            }

            //+ Case: one node is returned.
            if (firstAndLast.Item1 == firstAndLast.Item2)
            {
                if (firstAndLast.Item1.IsNodeWithin(origEndPoints.TopPoint - 1, origEndPoints.BottomPoint - 1)
                    && !firstAndLast.Item1.IsSelfClosing)
                {
                    EditorPoints contentEndPoints = firstAndLast.Item1.GetContentEndPoints();
                    contentEndPoints.RestoreSelectedText(selection);
                }
                else
                {
                    selection.MoveToAbsoluteOffset(selection.ActivePoint.AbsoluteCharOffset);
                }
                return NarrowSelectionResult.Success;
            }

            //+ Case: multiple nodes are selected within one parent.

            // if we have more than one node, we narrow the selection by removing
            // the node farthest away from the anchor point from the selection.

            int top;
            int bottom;
            if (selection.IsActiveEndGreater)
            {
                // remove the top node from the selection.
                // +1 to point beyond the last character.
                // +1 to change from 0- to 1-based offset.
                top = firstAndLast.Item1.BottomPoint + 2;
                bottom = firstAndLast.Item2.BottomPoint + 2;
            }
            else
            {
                // remove the bottom node from the selection.
                // +1 to change from 0- to 1-based offset.
                top = firstAndLast.Item1.TopPoint + 1;
                bottom = firstAndLast.Item2.TopPoint + 1;
            }
            if (selection.IsActiveEndGreater)
            {
                selection.MoveToAbsoluteOffset(top);
                selection.MoveToAbsoluteOffset(bottom, true);
            }
            else
            {
                selection.MoveToAbsoluteOffset(bottom);
                selection.MoveToAbsoluteOffset(top, true);
            }
            selection.Trim();
            return NarrowSelectionResult.Success;
        }

        /// <summary>
        ///     A TextSelection extension method that expand selection.
        /// </summary>
        /// <param name="selection">
        ///     The target to act on.
        /// </param>
        /// <returns>
        ///     .
        /// </returns>
        public static WiddenSelectionResult ExpandSelection(this TextSelection selection)
        {
            EditorPoints ep = selection.GetEditorPoints();

            // If nothing is selected, select the closest node.
            if (ep.IsEmpty)
            {
                selection.ResetSelection(ep);
                return selection.SelectNode() ? WiddenSelectionResult.Success : WiddenSelectionResult.NodeSelectError;
            }
            // If a node or nodes are selected, select the parent.
            if (selection.IsNodeSelected() || selection.AreSiblingsSelected())
            {
                return selection.SelectParent()
                           ? WiddenSelectionResult.Success
                           : WiddenSelectionResult.ParentSelectError;
            }
            return WiddenSelectionResult.LogicError;
        }

        /// <summary>
        ///     A TextSelection extension method that queries if 'selection' is node
        ///     selected.
        /// </summary>
        /// <param name="selection">
        ///     The target to act on.
        /// </param>
        /// <param name="name">
        ///     (optional) the name.
        /// </param>
        /// <returns>
        ///     true if node selected, otherwise false.
        /// </returns>
        public static bool IsNodeSelected(this TextSelection selection, string name = "")
        {
            EditorPoints ep = GetEditorPoints(selection);
            if (ep.IsInvalid)
            {
                return false;
            }
            ep.RestoreSelectedText(selection);
            Tuple<XamlNode, XamlNode> firstAndLast = _rootNode.GetFirstLastNodesBetweenPoints(ep.TopPoint - 1,
                                                                                              ep.BottomPoint - 1);
            if (firstAndLast.Item1 != firstAndLast.Item2)
            {
                return false;
            }
            return name == string.Empty
                   || Regex.IsMatch(selection.Text, string.Format(@"<(?<StartTagName>{0})(?=[\s/>])", name));
        }

        /// <summary>
        ///     A TextSelection extension method that selects a single node.
        /// </summary>
        /// <param name="selection">
        ///     The target to act on.
        /// </param>
        /// <returns>
        ///     true if it succeeds, otherwise false.
        /// </returns>
        public static bool SelectNode(this TextSelection selection)
        {
            // Works only when the selected text is empty.
            if (!selection.IsEmpty)
            {
                return false;
            }

            // This has to be done here primarily to prime the data
            // needed by called methods. Othwerwise we could just
            // reference the offsets directly.
            EditorPoints ep = GetEditorPoints(selection);
            XamlNode node = _rootNode.GetNodeWithOffset(ep.ActivePoint - 1);
            if (node == null)
            {
                return false;
            }
            selection.SetSelection(node.BottomPoint + 1, node.TopPoint + 1);
            return true;
        }

        /// <summary>
        ///     A TextSelection extension method that selects one or more nodes
        ///     contained by one parent node.
        /// </summary>
        /// <param name="selection">
        ///     The target to act on.
        /// </param>
        /// <returns>
        ///     true if it succeeds, otherwise false.
        /// </returns>
        public static bool SelectNodes(this TextSelection selection)
        {
            EditorPoints ep = GetEditorPoints(selection);

            // Find the nodes that are within or that contain the ends of the selection.
            Tuple<XamlNode, XamlNode> firstAndLast = _rootNode.GetFirstLastNodesBetweenPoints(ep.TopPoint - 1,
                                                                                              ep.BottomPoint - 1);
            // Null is returned if one end of the selection is outside the root node.
            if (firstAndLast.Item1 == null || firstAndLast.Item2 == null)
            {
                // Select the root node.
                selection.SelectAll();
                selection.Trim();
                return true;
            }

            // Select the the text for the nodes found above.
            selection.SetSelection(firstAndLast.Item2.BottomPoint + 1, firstAndLast.Item1.TopPoint + 1);

            return true;
        }

        /// <summary>
        ///     A TextSelection extension method that select parent.
        /// </summary>
        /// <param name="selection">
        ///     The target to act on.
        /// </param>
        /// <returns>
        ///     true if it succeeds, otherwise false.
        /// </returns>
        public static bool SelectParent(this TextSelection selection)
        {
            EditorPoints ep = selection.GetEditorPoints();
            XamlNode[] nodes = _rootNode.GetSelectedNodes(ep);
            if (nodes == null || nodes == null)
            {
                return false;
            }
            XamlNode parent = nodes[0].Parent;
            selection.SetSelection(parent.BottomPoint + 1, parent.TopPoint + 1);
            return true;
        }

        #endregion

        #region Methods (private)

        private static EditorPoints GetEditorPoints(Tuple<XamlNode, XamlNode> firstAndLast)
        {
            EditorPoints ep = EditorPoints.GetEditorPoints(firstAndLast.Item1.TopPoint + 1,
                                                           firstAndLast.Item2.BottomPoint + 1,
                                                           _documentText);
            return ep;
        }

        private static EditorPoints GetEditorPoints(TextSelection sel, bool handleExceptions = true)
        {
            EditorPoints ep = sel.GetEditorPoints();
            try
            {
                if (_documentText == ep.DocumentText)
                {
                    return ep;
                }

                _documentText = ep.DocumentText;
                _rootNode = new XamlNode(_documentText);
                return ep;
            }
            catch
            {
                ep.RestoreSelectedText(sel);
                return EditorPoints.GetInvalidEditorPoints();
            }
        }

        #endregion
    }
}
