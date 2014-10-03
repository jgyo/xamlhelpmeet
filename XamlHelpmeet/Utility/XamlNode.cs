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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

using NLog;

using XamlHelpmeet.Extensions;
using XamlHelpmeet.Utility.XamlParts;

using YoderZone.Extensions.NLog;

#endregion

/// <summary>
/// Xaml node. Contains information about the opening and closing tags of a
/// control in a Xaml document as well as its contents.
/// </summary>
public class XamlNode
{
    private static readonly Logger logger =
        SettingsHelper.CreateLogger();

    #region Fields

    /// <summary>
    /// The tag matches enumerator.
    /// </summary>
    private static SortedSet<Match>.Enumerator tagMatchesEnumerator;

    /// <summary>
    /// The end tag.
    /// </summary>
    private readonly XamlTag endTag;

    /// <summary>
    /// true if this XamlHelpmeet.Utility.XamlNode is root.
    /// </summary>
    private readonly bool isRoot;

    /// <summary>
    /// The parent.
    /// </summary>
    private readonly XamlNode parent;

    /// <summary>
    /// The start tag.
    /// </summary>
    private readonly XamlTag startTag;

    /// <summary>
    /// The children container.
    /// </summary>
    private XamlNodeCollection childrenContainer;

    /// <summary>
    /// The document text.
    /// </summary>
    private string documentText;

    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the XamlNode class. used to create a
    /// root node.
    /// </summary>
    /// <remarks>
    /// Create the root node of a xaml file with this constructor. All other
    /// child nodes will be created as a result.
    /// </remarks>
    /// <exception cref="ApplicationException">
    /// Thrown when an Application error condition occurs.
    /// </exception>
    /// <param name="documentText">
    /// The document text.
    /// </param>
    public XamlNode(string documentText)
    {
        Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(
                    documentText));
        logger.Debug("Entered member.");

        // The document to be parsed for building a XamlNode tree.
        this.documentText = documentText;
        this.isRoot = true;

        // This object holds a sorted list of Match objects that correspond
        // to each tag in the document.
        var sortedTagMatches = new SortedSet<Match>(new XamlTagMatchComparer());
        MatchCollection matches = Regex.Matches(documentText,
                                                XmlHelpers.STARTTAG_PATTERN,
                                                RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline
                                                | RegexOptions.IgnoreCase);
        foreach (Match item in matches)
        {
            sortedTagMatches.Add(item);
        }
        matches = Regex.Matches(documentText,
                                XmlHelpers.ENDTAG_PATTERN,
                                RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline
                                | RegexOptions.IgnoreCase);
        foreach (Match item in matches)
        {
            sortedTagMatches.Add(item);
        }

        // the _sortedTagMatches collection is complete. That is now used
        // to build out the tree starting with this node, the root node.
        tagMatchesEnumerator = sortedTagMatches.GetEnumerator();
        if (tagMatchesEnumerator.MoveNext())
        {
            this.startTag = new XamlTag(tagMatchesEnumerator.Current);
            if (this.startTag.TagType != XamlTagType.Starting)
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
        while (tagMatchesEnumerator.MoveNext())
        {
            var childTag = new XamlTag(tagMatchesEnumerator.Current);
            if (childTag.TagType == XamlTagType.Unknown)
            {
                throw new ApplicationException("Unknown tag type encountered.");
            }
            if (childTag.TagType == XamlTagType.Ending)
            {
                this.endTag = childTag;

                // This is the end of the root node, so this should be the end
                // of the xaml tags.
                if (tagMatchesEnumerator.MoveNext())
                {
                    throw new ApplicationException("Closing tag encountered before last tag.");
                }
                break;
            }
            var childNode = new XamlNode(childTag, this) { DocumentText = this.documentText };
            this.ChildrenContainer.AddLast(childNode);
        }

        tagMatchesEnumerator.Dispose();
    }

    /// <summary>
    /// Access set to private to prevent usage from outside. Called only by
    /// XamlNode constructors.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown when one or more arguments have unsupported or illegal values.
    /// </exception>
    /// <exception cref="ApplicationException">
    /// Thrown when an Application error condition occurs.
    /// </exception>
    /// <param name="newStartTag">
    /// The new start tag.
    /// </param>
    /// <param name="parent">
    /// The parent.
    /// </param>
    private XamlNode(XamlTag newStartTag, XamlNode parent)
    {
        logger.Debug("Entered member.");

        if (newStartTag.TagType == XamlTagType.Unknown)
        {
            throw new ArgumentException("The startingTag given is invalid.",
                                        "newStartTag");
        }
        if (newStartTag.TagType == XamlTagType.Ending)
        {
            throw new ArgumentException("And ending tag was given instead of a starting tag.",
                                        "newStartTag");
        }

        this.startTag = newStartTag;
        this.parent = parent;
        if (newStartTag.TagType == XamlTagType.SelfClosing)
        {
            // Self closing tags have no children or separate closing tags.
            this.endTag = newStartTag;
            return;
        }

        while (tagMatchesEnumerator.MoveNext())
        {
            var childTag = new XamlTag(tagMatchesEnumerator.Current);
            if (childTag.TagType == XamlTagType.Ending)
            {
                if (childTag.Name != this.Name)
                {
                    throw new ArgumentException(
                        string.Format("Expected \"{0}\" for end tag, but encountered \"{1}.\"",
                                      this.Name,
                                      childTag.Name));
                }
                this.endTag = childTag;
                return;
            }
            var childNode = new XamlNode(childTag, this) { DocumentText = this.documentText };
            this.ChildrenContainer.AddLast(childNode);
        }

// the end of tags represent the end of xaml tags. Since this builds a
// child, it should never reach this point.
        throw new ApplicationException("Premature end of tags encountered.");
    }

    #endregion

    #region Properties and Indexers
/// <summary>
/// Gets the ancestors.
/// </summary>
/// <value>
/// The ancestors.
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
/// Gets the attributes.
/// </summary>
/// <value>
/// The attributes.
/// </value>
    public IReadOnlyList<XamlAttribute> Attributes
    {
        get { return this.startTag.GetAttributes(); }
    }

/// <summary>
/// Gets the bottom point.
/// </summary>
/// <value>
/// The bottom point.
/// </value>
    public int BottomPoint
    {
        get { return this.endTag == null ? int.MaxValue : this.endTag.BottomPoint; }
    }

/// <summary>
/// Gets the children container.
/// </summary>
/// <value>
/// The children container.
/// </value>
    public XamlNodeCollection ChildrenContainer
    {
        get
        {
            return this.childrenContainer
                   ?? (this.childrenContainer = new XamlNodeCollection(this));
        }
    }

/// <summary>
/// Gets the document text.
/// </summary>
/// <value>
/// The document text.
/// </value>
    public string DocumentText
    {
        get { return this.documentText.IsNullOrWhiteSpace() ? string.Empty : this.documentText; }
        private set
        {
            this.documentText = value;
            if (this.childrenContainer == null)
            {
                return;
            }

            foreach (var item in this.ChildrenContainer)
            {
                item.DocumentText = value;
            }
        }
    }

/// <summary>
/// Gets the end tag.
/// </summary>
/// <value>
/// The end tag.
/// </value>
    public XamlTag EndTag { get { return this.endTag; } }

/// <summary>
/// Gets a value indicating whether this XamlNode has children.
/// </summary>
/// <value>
/// true if this XamlNode has children, otherwise false.
/// </value>
    public bool HasChildren
    {
        get { return !this.ChildrenContainer.IsEmpty(); }
    }

/// <summary>
/// Gets a value indicating whether this XamlNode is root.
/// </summary>
/// <value>
/// true if this XamlNode is root, otherwise false.
/// </value>
    public bool IsRoot
    {
        get { return this.isRoot; }
    }

/// <summary>
/// Gets a value indicating whether this XamlHelpmeet.Utility.XamlNode is
/// self closing.
/// </summary>
/// <value>
/// true if this XamlHelpmeet.Utility.XamlNode is self closing, false if
/// not.
/// </value>
    public bool IsSelfClosing { get { return this.startTag == this.endTag; } }

/// <summary>
/// Gets the name.
/// </summary>
/// <value>
/// The name.
/// </value>
    public string Name
    {
        get { return this.startTag.Name; }
    }

/// <summary>
/// Gets the parent.
/// </summary>
/// <value>
/// The parent.
/// </value>
    public XamlNode Parent
    {
        get { return this.parent; }
    }

/// <summary>
/// Gets the siblings of this node.
/// </summary>
/// <value>
/// The siblings.
/// </value>
    public XamlNodeCollection Siblings
    {
        get { return this.Parent == null ? null : this.Parent.ChildrenContainer; }
    }

/// <summary>
/// Gets the start tag.
/// </summary>
/// <value>
/// The start tag.
/// </value>
    public XamlTag StartTag { get { return this.startTag; } }

/// <summary>
/// Gets the top point.
/// </summary>
/// <value>
/// The top point.
/// </value>
    public int TopPoint { get { return this.startTag == null ? 0 : this.startTag.TopPoint; } }

    #endregion

    #region Methods (public)
/// <summary>
/// Gets a child node with offset.
/// </summary>
/// <param name="offset">
/// The offset.
/// </param>
/// <returns>
/// The child node with offset.
/// </returns>
    public XamlNode GetChildNodeWithOffset(int offset)
    {
        logger.Debug("Entered member.");

        return this.ChildrenContainer.FirstOrDefault(child => offset >
                child.TopPoint && offset <= child.BottomPoint);
    }

/// <summary>
/// Gets the common ancestor of this item and one other.
/// </summary>
/// <param name="otherNode">
/// The other node.
/// </param>
/// <returns>
/// The common ancestor.
/// </returns>
    public XamlNode GetCommonAncestor(XamlNode otherNode)
    {
        logger.Debug("Entered member.");

        return (from ancestor in this.Ancestors
                from othersAncestor in otherNode.Ancestors
                where ancestor == othersAncestor
                select ancestor).FirstOrDefault();
    }

/// <summary>
/// Gets content end points.
/// </summary>
/// <returns>
/// The content end points.
/// </returns>
    public EditorPoints GetContentEndPoints()
    {
        Contract.Requires<InvalidOperationException>(!string.IsNullOrEmpty(
                    this.DocumentText));
        Contract.Requires<InvalidOperationException>((this.StartTag.BottomPoint +
                1) <= this.DocumentText.Length);
        Contract.Requires<InvalidOperationException>((this.StartTag.BottomPoint +
                1) <= ((this.DocumentText.Length - (this.EndTag.TopPoint -
                        this.StartTag.BottomPoint + 1))));

        logger.Debug("Entered member.");

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
        return EditorPoints.GetEditorPoints(top + 1, bottom + 1,
                                            this.DocumentText);
    }

/// <summary>
/// This method should return a tupple with two nodes without fail (if it
/// is called through the root node). The first node should be the node
/// in which the topPoint falls, if it falls within one of it's tags,
/// after the last tag of its last child, if it has no children. The
/// second node should be the node in which the bottom point falls, if it
/// falls within one of its tags, before the first tag of its first child,
/// or if it has no children.
/// </summary>
/// <param name="topPoint">
/// The top point.
/// </param>
/// <param name="bottomPoint">
/// The bottom point.
/// </param>
/// <returns>
/// The first last nodes between points.
/// </returns>
    public Tuple<XamlNode, XamlNode> GetFirstLastNodesBetweenPoints(
        int topPoint, int bottomPoint)
    {
        logger.Debug("Entered member.");

        if (topPoint == bottomPoint)
        {
            XamlNode node = this.GetNodeWithOffset(topPoint);
            return new Tuple<XamlNode, XamlNode>(node, node);
        }

        PositionAndNode pan1 = this.GetTopPointPositionAndNode(topPoint);
        PositionAndNode pan2 = this.GetBottomPointPositionAndNode(bottomPoint);

        if (pan1.Node.Parent == pan2.Node.Parent)
        {
            return new Tuple<XamlNode, XamlNode>(pan1.Node, pan2.Node);
        }

        XamlNode commonAncestor = pan1.Node.GetCommonAncestor(pan2.Node);
        XamlNode node1 = commonAncestor.GetChildNodeWithOffset(topPoint);
        XamlNode node2 = commonAncestor.GetChildNodeWithOffset(bottomPoint);
        return new Tuple<XamlNode, XamlNode>(node1, node2);
    }

/// <summary>
/// Gets node with offset.
/// </summary>
/// <param name="offset">
/// The document point.
/// </param>
/// <returns>
/// The node with offset.
/// </returns>
    public XamlNode GetNodeWithOffset(int offset)
    {
        logger.Debug("Entered member.");

        if (offset <= this.TopPoint || offset >= this.BottomPoint)
        {
            // documentPoint is not in this node or its children.
            return null;
        }

        foreach (XamlNode descendant in this.ChildrenContainer.Select(
                     child => child.GetNodeWithOffset(offset)).Where(descendant => descendant
                             != null))
        {
            // documentPoint is within this child.
            return descendant;
        }

        // documentPoint is within this node, but not in any of its
        // children.
        return this;
    }

/// <summary>
/// Gets selected nodes.
/// </summary>
/// <param name="ep">
/// The ep.
/// </param>
/// <returns>
/// An array of XAML node.
/// </returns>
    public XamlNode[] GetSelectedNodes(EditorPoints ep)
    {
        logger.Debug("Entered member.");

        var nodes = new List<XamlNode>();
        if (this.IsSelected(ep))
        {
            nodes.Add(this);
            return nodes.ToArray();
        }

        nodes.AddRange(this.ChildrenContainer.Where(child => child.IsSelected(
                           ep)));
        if (nodes.Count > 0)
        {
            return nodes.ToArray();
        }

        return this.ChildrenContainer.Select(child => child.GetSelectedNodes(ep))
               .FirstOrDefault(nodeArry => nodeArry != null);
    }

/// <summary>
/// Gets span types.
/// </summary>
/// <param name="point1">
/// The first point.
/// </param>
/// <param name="point2">
/// The second point.
/// </param>
/// <returns>
/// An array of span type.
/// </returns>
    public SpanType[] GetSpanTypes(int point1, int point2)
    {
        logger.Debug("Entered member.");

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

/// <summary>
/// Query if 'topPoint' is node within.
/// </summary>
/// <param name="topPoint">
/// The top point.
/// </param>
/// <param name="bottomPoint">
/// The bottom point.
/// </param>
/// <returns>
/// true if node within, false if not.
/// </returns>
    public bool IsNodeWithin(int topPoint, int bottomPoint)
    {
        logger.Debug("Entered member.");

        return (this.TopPoint >= topPoint && this.BottomPoint <= bottomPoint);
    }

    #endregion

    #region Methods (private)
/// <summary>
/// Gets bottom point position and node.
/// </summary>
/// <param name="bottomPoint">
/// The bottom point.
/// </param>
/// <returns>
/// The bottom point position and node.
/// </returns>
    private PositionAndNode GetBottomPointPositionAndNode(int bottomPoint)
    {
        logger.Debug("Entered member.");

        var pan = new PositionAndNode(this, bottomPoint);

        if (pan.Position != NodePosition.Content || !this.HasChildren)
        {
            return pan;
        }

        LinkedListNode<XamlNode> childNode = this.ChildrenContainer.Last;
        do
        {
            PositionAndNode childPan = childNode.Value.GetBottomPointPositionAndNode(
                                           bottomPoint);
            if (childPan.Position != NodePosition.Before)
            {
                return childPan;
            }
        }
        while ((childNode = childNode.Previous) != null);
        return pan;
    }

/// <summary>
/// Gets span type.
/// </summary>
/// <param name="point">
/// The point.
/// </param>
/// <returns>
/// The span type.
/// </returns>
    private SpanType GetSpanType(int point)
    {
        logger.Debug("Entered member.");

        if (point < this.TopPoint || point >= this.BottomPoint)
        {
            // Outside the root node
            return SpanType.OutSide;
        }
        if (point < this.startTag.BottomPoint)
        {
            return this.IsSelfClosing ? SpanType.SelfClosingTag :
                   SpanType.StartingTag;
        }
        if (point >= this.endTag.TopPoint)
        {
            return SpanType.EndingTag;
        }

        //! The point is somewhere between this node's two tags.

        if (!this.HasChildren)
        {
            return SpanType.BetweenNodeTags;
        }

        foreach (SpanType temp in this.ChildrenContainer.Select(
                     node => node.GetSpanType(point)).Where(temp => temp != SpanType.OutSide))
        {
            return temp;
        }

        return SpanType.BetweenNodes;
    }

/// <summary>
/// Gets top point position and node.
/// </summary>
/// <param name="topPoint">
/// The top point.
/// </param>
/// <returns>
/// The top point position and node.
/// </returns>
    private PositionAndNode GetTopPointPositionAndNode(int topPoint)
    {
        logger.Debug("Entered member.");

        var pan = new PositionAndNode(this, topPoint);

        if (pan.Position != NodePosition.Content || !this.HasChildren)
        {
            return pan;
        }

        LinkedListNode<XamlNode> childNode = this.ChildrenContainer.First;
        do
        {
            PositionAndNode childPan = childNode.Value.GetTopPointPositionAndNode(
                                           topPoint);
            if (childPan.Position != NodePosition.After)
            {
                return childPan;
            }
        }
        while ((childNode = childNode.Next) != null);
        return pan;
    }

/// <summary>
/// Query if 'ep' is selected.
/// </summary>
/// <param name="ep">
/// The ep.
/// </param>
/// <returns>
/// true if selected, false if not.
/// </returns>
    private bool IsSelected(EditorPoints ep)
    {
        logger.Debug("Entered member.");

        return this.TopPoint >= ep.TopPoint - 1 &&
               this.BottomPoint <= ep.BottomPoint - 1;
    }

    #endregion

    #region Nested type: XamlTagMatchComparer
/// <summary>
/// Xaml tag match comparer.
/// </summary>
/// <seealso cref="T:System.Collections.Generic.IComparer{System.Text.RegularExpressions.Match}"/>
    public class XamlTagMatchComparer : IComparer<Match>
    {
        // ReSharper disable once MemberHidesStaticFromOuterClass
        private static readonly Logger logger =
            SettingsHelper.CreateLogger();

        #region IComparer<Match> Members
        /// <summary>
        /// Compares two Match objects to determine their relative ordering.
        /// </summary>
        /// <param name="match1">
        /// Match to be compared.
        /// </param>
        /// <param name="match2">
        /// Match to be compared.
        /// </param>
        /// <returns>
        /// Negative if 'match1' is less than 'match2', 0 if they are equal,
        /// or positive if it is greater.
        /// </returns>
        public Int32 Compare(Match match1, Match match2)
        {
            logger.Debug("Entered member.");

            bool preferMatch1 = match1.Groups["Attributes"].Success;
            int result = match1.Index.CompareTo(match2.Index);
            return result != 0 ? result : preferMatch1 ? 1 : -1;
        }

        #endregion
    }

    #endregion
}
}
