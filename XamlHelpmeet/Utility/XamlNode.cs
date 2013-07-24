// <copyright file="XamlNode.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/7/2013</date>
// <summary>Implements the xaml node class</summary>
namespace XamlHelpmeet.Utility
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using XamlHelpmeet.Extensions;
    using XamlHelpmeet.Utility.XamlParts;

    #endregion

    /// <summary>
    ///     Xaml node. Contains information about the opening and closing
    ///     tags of a control in a Xaml document as well as its contents.
    /// </summary>
    public class XamlNode
    {
        #region Fields

        private static SortedSet<Match> _sortedTagMatches;

        private static SortedSet<Match>.Enumerator _tagMatchesEnumerator;

        private readonly XamlTag _endTag;

        private readonly bool _isRoot;

        private readonly XamlNode _parent;

        private readonly XamlTag _startTag;

        private XamlNodeCollection _childrenContainer;

        private string _documentText;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the XamlNode class. used to create a
        ///     root node.
        /// </summary>
        /// <exception cref="ApplicationException">
        ///     Thrown when an Application error condition occurs.
        /// </exception>
        /// <param name="documentText">
        ///     The document text.
        /// </param>
        /// <remarks>
        ///     Create the root node of a xaml file with this constructor. All other
        ///     child nodes will be created as a result.
        /// </remarks>
        public XamlNode(string documentText)
        {
            // The document to be parsed for building a XamlNode tree.
            this._documentText = documentText;
            this._isRoot = true;

            // This object holds a sorted list of Match objects that correspond
            // to each tag in the document.
            _sortedTagMatches = new SortedSet<Match>(new XamlTagMatchComparer());
            MatchCollection matches = Regex.Matches(documentText,
                                                    XmlHelpers.STARTTAG_PATTERN,
                                                    RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline
                                                    | RegexOptions.IgnoreCase);
            foreach (Match item in matches)
            {
                _sortedTagMatches.Add(item);
            }
            matches = Regex.Matches(documentText,
                                    XmlHelpers.ENDTAG_PATTERN,
                                    RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline
                                    | RegexOptions.IgnoreCase);
            foreach (Match item in matches)
            {
                _sortedTagMatches.Add(item);
            }

            // the _sortedTagMatches collection is complete. That is now used
            // to build out the tree starting with this node, the root node.
            _tagMatchesEnumerator = _sortedTagMatches.GetEnumerator();
            if (_tagMatchesEnumerator.MoveNext())
            {
                this._startTag = new XamlTag(_tagMatchesEnumerator.Current);
                if (this._startTag.TagType != XamlTagType.Starting)
                {
                    throw new ApplicationException("Starting tag expected.");
                }
            }
            else
            {
                throw new ApplicationException("Unable to enumerate Xaml tags.");
            }

            // Except for this node's end tag, the only thing it will see in
            // this loop are its children node. The child nodes work similarly,
            // so by the time they return, the nodes that belong to the children
            // will be consumed.
            while (_tagMatchesEnumerator.MoveNext())
            {
                var childTag = new XamlTag(_tagMatchesEnumerator.Current);
                if (childTag.TagType == XamlTagType.Unknown)
                {
                    throw new ApplicationException("Unknown tag type encountered.");
                }
                if (childTag.TagType == XamlTagType.Ending)
                {
                    this._endTag = childTag;

                    // This is the end of the root node, so this should be the end
                    // of the xaml tags.
                    if (_tagMatchesEnumerator.MoveNext())
                    {
                        throw new ApplicationException("Closing tag encountered before last tag.");
                    }
                    break;
                }
                var childNode = new XamlNode(childTag, this) { DocumentText = this._documentText };
                this.ChildrenContainer.AddLast(childNode);
            }

            _tagMatchesEnumerator.Dispose();
            _sortedTagMatches = null;
        }

        // Access set to private to prevent usage from outside. Called only by
        // XamlNode constructors.
        private XamlNode(XamlTag newStartTag, XamlNode parent)
        {
            if (newStartTag.TagType == XamlTagType.Unknown)
            {
                throw new ArgumentException("The startingTag given is invalid.", "newStartTag");
            }
            if (newStartTag.TagType == XamlTagType.Ending)
            {
                throw new ArgumentException("And ending tag was given instead of a starting tag.", "newStartTag");
            }

            this._startTag = newStartTag;
            this._parent = parent;
            if (newStartTag.TagType == XamlTagType.SelfClosing)
            {
                // Self closing tags have no children or separate closing tags.
                this._endTag = newStartTag;
                return;
            }

            while (_tagMatchesEnumerator.MoveNext())
            {
                var childTag = new XamlTag(_tagMatchesEnumerator.Current);
                if (childTag.TagType == XamlTagType.Ending)
                {
                    if (childTag.Name != this.Name)
                    {
                        throw new ArgumentException(
                            string.Format("Expected \"{0}\" for end tag, but encountered \"{1}.\"",
                                          this.Name,
                                          childTag.Name));
                    }
                    this._endTag = childTag;
                    return;
                }
                var childNode = new XamlNode(childTag, this) { DocumentText = this._documentText };
                this.ChildrenContainer.AddLast(childNode);
            }

            // the end of tags represent the end of xaml tags. Since this builds a
            // child, it should never reach this point.
            throw new ApplicationException("Premature end of tags encountered.");
        }

        #endregion

        #region Properties and Indexers

        /// <summary>
        ///     Gets the ancestors.
        /// </summary>
        /// <value>
        ///     The ancestors.
        /// </value>
        public IEnumerable<XamlNode> Ancestors
        {
            get
            {
                XamlNode node = this;

                while (node.IsRoot == false)
                {
                    yield return node;
                    node = node.Parent;
                }
                yield return node;
            }
        }

        /// <summary>
        ///     Gets the attributes.
        /// </summary>
        /// <value>
        ///     The attributes.
        /// </value>
        public IReadOnlyList<XamlAttribute> Attributes
        {
            get { return this._startTag.GetAttributes(); }
        }

        /// <summary>
        ///     Gets the bottom point.
        /// </summary>
        /// <value>
        ///     The bottom point.
        /// </value>
        public int BottomPoint
        {
            get { return this._endTag == null ? int.MaxValue : this._endTag.BottomPoint; }
        }

        /// <summary>
        ///     Gets the children container.
        /// </summary>
        /// <value>
        ///     The children container.
        /// </value>
        public XamlNodeCollection ChildrenContainer
        {
            get
            {
                if (this._childrenContainer == null)
                {
                    this._childrenContainer = new XamlNodeCollection(this);
                }
                return this._childrenContainer;
            }
        }

        /// <summary>
        ///     Gets the document text.
        /// </summary>
        /// <value>
        ///     The document text.
        /// </value>
        public string DocumentText
        {
            get { return this._documentText.IsNullOrWhiteSpace() ? string.Empty : this._documentText; }
            private set
            {
                this._documentText = value;
                if (this._childrenContainer != null)
                {
                    foreach (var item in this.ChildrenContainer)
                    {
                        item.DocumentText = value;
                    }
                }
            }
        }

        public XamlTag EndTag { get { return this._endTag; } }

        /// <summary>
        ///     Gets a value indicating whether this XamlNode has children.
        /// </summary>
        /// <value>
        ///     true if this XamlNode has children, otherwise false.
        /// </value>
        public bool HasChildren
        {
            get { return !this.ChildrenContainer.IsEmpty(); }
        }

        /// <summary>
        ///     Gets a value indicating whether this XamlNode is root.
        /// </summary>
        /// <value>
        ///     true if this XamlNode is root, otherwise false.
        /// </value>
        public bool IsRoot
        {
            get { return this._isRoot; }
        }

        public bool IsSelfClosing { get { return this._startTag == this._endTag; } }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name
        {
            get { return this._startTag.Name; }
        }

        /// <summary>
        ///     Gets the parent.
        /// </summary>
        /// <value>
        ///     The parent.
        /// </value>
        public XamlNode Parent
        {
            get { return this._parent; }
        }

        /// <summary>
        ///     Gets the siblings of this node.
        /// </summary>
        /// <value>
        ///     The siblings.
        /// </value>
        public XamlNodeCollection Siblings
        {
            get { return this.Parent == null ? null : this.Parent.ChildrenContainer; }
        }

        public XamlTag StartTag { get { return this._startTag; } }

        public int TopPoint { get { return this._startTag == null ? 0 : this._startTag.TopPoint; } }

        #endregion

        #region Methods (public)

        public XamlNode GetChildNodeWithOffset(int offset)
        {
            foreach (var child in this.ChildrenContainer)
            {
                if (offset > child.TopPoint && offset <= child.BottomPoint)
                {
                    return child;
                }
            }
            return null;
        }

        /// <summary>
        ///     Gets the common ancestor of this item and one other.
        /// </summary>
        /// <param name="otherNode">
        ///     The other node.
        /// </param>
        /// <returns>
        ///     The common ancestor.
        /// </returns>
        public XamlNode GetCommonAncestor(XamlNode otherNode)
        {
            foreach (var ancestor in this.Ancestors)
            {
                foreach (var othersAncestor in otherNode.Ancestors)
                {
                    if (ancestor == othersAncestor)
                    {
                        return ancestor;
                    }
                }
            }
            return null;
        }

        public EditorPoints GetContentEndPoints()
        {
            int top = this.StartTag.BottomPoint + 1;
            int bottom = this.EndTag.TopPoint;
            string content = this.DocumentText.Substring(top, bottom - top);
            Match match = Regex.Match(content, @"\A\s*", RegexOptions.Singleline);
            if (match.Success)
            {
                top = top + match.Length;
            }
            content = content.Trim();
            bottom = top + content.Length;
            return EditorPoints.GetEditorPoints(top + 1, bottom + 1, this.DocumentText);
        }

        /// <summary>
        ///     This method should return a tupple with two nodes without fail (if
        ///     it is called through the root node). The
        ///     first node should be the node in which the topPoint falls, if it
        ///     falls within one of it's tags, after the last tag of its last child,
        ///     if it has no children. The second node should be the node in
        ///     which the bottom point falls, if it falls within one of its tags,
        ///     before the first tag of its first child, or if it has no children.
        /// </summary>
        /// <param name="topPoint">
        ///     The top point.
        /// </param>
        /// <param name="bottomPoint">
        ///     The bottom point.
        /// </param>
        /// <returns>
        ///     The first last nodes between points.
        /// </returns>
        public Tuple<XamlNode, XamlNode> GetFirstLastNodesBetweenPoints(int topPoint, int bottomPoint)
        {
            if (topPoint == bottomPoint)
            {
                XamlNode node = this.GetNodeWithOffset(topPoint);
                return new Tuple<XamlNode, XamlNode>(node, node);
            }

            PositionAndNode pan1 = this.GetTopPointPositionAndNode(topPoint);
            PositionAndNode pan2 = this.GetBottomPointPositionAndNode(bottomPoint);

            if (pan1.Node.Parent != pan2.Node.Parent)
            {
                XamlNode parent = pan1.Node.GetCommonAncestor(pan2.Node);
                XamlNode node1 = parent.GetChildNodeWithOffset(topPoint);
                XamlNode node2 = parent.GetChildNodeWithOffset(bottomPoint);
                return new Tuple<XamlNode, XamlNode>(node1, node2);
            }
            return new Tuple<XamlNode, XamlNode>(pan1.Node, pan2.Node);
        }

        /// <summary>
        ///     Gets node with offset.
        /// </summary>
        /// <param name="offset">
        ///     The document point.
        /// </param>
        /// <returns>
        ///     The node with offset.
        /// </returns>
        public XamlNode GetNodeWithOffset(int offset)
        {
            if (offset <= this.TopPoint || offset >= this.BottomPoint)
            {
                // documentPoint is not in this node or its children.
                return null;
            }

            foreach (var child in this.ChildrenContainer)
            {
                XamlNode descendant = child.GetNodeWithOffset(offset);
                if (descendant == null)
                {
                    continue;
                }

                // documentPoint is within this child.
                return descendant;
            }

            // documentPoint is within this node, but not in any of its
            // children.
            return this;
        }

        public XamlNode[] GetSelectedNodes(EditorPoints ep)
        {
            var nodes = new List<XamlNode>();
            if (this.IsSelected(ep))
            {
                nodes.Add(this);
                return nodes.ToArray();
            }
            foreach (var child in this.ChildrenContainer)
            {
                if (child.IsSelected(ep))
                {
                    nodes.Add(child);
                }
            }
            if (nodes.Count > 0)
            {
                return nodes.ToArray();
            }
            foreach (var child in this.ChildrenContainer)
            {
                XamlNode[] nodeArry = child.GetSelectedNodes(ep);
                if (nodeArry != null)
                {
                    return nodeArry;
                }
            }
            return null;
        }

        public SpanType[] GetSpanTypes(int point1, int point2)
        {
            int min = Math.Min(point1, point2);
            int max = Math.Max(point1, point2);
            var spanTypes = new List<SpanType>();
            var spanType = SpanType.Unknown;

            for (int i = min; i <= max; i++)
            {
                SpanType temp = this.GetSpanType(i);
                if (temp == spanType)
                {
                    continue;
                }
                spanType = temp;
                spanTypes.Add(spanType);
            }
            return spanTypes.ToArray();
        }

        public bool IsNodeWithin(int topPoint, int bottomPoint)
        {
            return (this.TopPoint >= topPoint && this.BottomPoint <= bottomPoint);
        }

        #endregion

        #region Methods (private)

        private PositionAndNode GetBottomPointPositionAndNode(int bottomPoint)
        {
            var pan = new PositionAndNode(this, bottomPoint);

            if (pan.Position != NodePosition.Content || !this.HasChildren)
            {
                return pan;
            }

            LinkedListNode<XamlNode> childNode = this.ChildrenContainer.Last;
            do
            {
                PositionAndNode childPan = childNode.Value.GetBottomPointPositionAndNode(bottomPoint);
                if (childPan.Position != NodePosition.Before)
                {
                    return childPan;
                }
            }
            while ((childNode = childNode.Previous) != null);
            return pan;
        }

        private SpanType GetSpanType(int point)
        {
            if (point < this.TopPoint || point >= this.BottomPoint)
            {
                // Outside the root node
                return SpanType.OutSide;
            }
            if (point < this._startTag.BottomPoint)
            {
                return this.IsSelfClosing ? SpanType.SelfClosingTag : SpanType.StartingTag;
            }
            if (point >= this._endTag.TopPoint)
            {
                return SpanType.EndingTag;
            }

            //! The point is somewhere between this node's two tags.

            if (!this.HasChildren)
            {
                return SpanType.BetweenNodeTags;
            }

            foreach (var node in this.ChildrenContainer)
            {
                SpanType temp = node.GetSpanType(point);
                if (temp != SpanType.OutSide)
                {
                    return temp;
                }
            }
            return SpanType.BetweenNodes;
        }

        private PositionAndNode GetTopPointPositionAndNode(int topPoint)
        {
            var pan = new PositionAndNode(this, topPoint);

            if (pan.Position != NodePosition.Content || !this.HasChildren)
            {
                return pan;
            }

            LinkedListNode<XamlNode> childNode = this.ChildrenContainer.First;
            do
            {
                PositionAndNode childPan = childNode.Value.GetTopPointPositionAndNode(topPoint);
                if (childPan.Position != NodePosition.After)
                {
                    return childPan;
                }
            }
            while ((childNode = childNode.Next) != null);
            return pan;
        }

        private bool IsSelected(EditorPoints ep)
        {
            return this.TopPoint >= ep.TopPoint - 1 && this.BottomPoint <= ep.BottomPoint - 1;
        }

        #endregion

        #region Nested type: XamlTagMatchComparer

        /// <summary>
        ///     Xaml tag match comparer.
        /// </summary>
        /// <seealso
        ///     cref="T:System.Collections.Generic.IComparer{System.Text.RegularExpressions.Match}" />
        public class XamlTagMatchComparer : IComparer<Match>
        {
            #region IComparer<Match> Members

            /// <summary>
            ///     Compares two Match objects to determine their relative ordering.
            /// </summary>
            /// <param name="match1">
            ///     Match to be compared.
            /// </param>
            /// <param name="match2">
            ///     Match to be compared.
            /// </param>
            /// <returns>
            ///     Negative if 'match1' is less than 'match2', 0 if they are equal, or
            ///     positive if it is greater.
            /// </returns>
            public Int32 Compare(Match match1, Match match2)
            {
                bool preferMatch1 = match1.Groups["Attributes"].Success;
                int result = match1.Index.CompareTo(match2.Index);
                return result != 0 ? result : preferMatch1 ? 1 : -1;
            }

            #endregion
        }

        #endregion
    }
}
