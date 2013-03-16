// <copyright file="PositionAndNode.cs" company="Gil Yoder">
// Copyright (c) 2013 Gil Yoder. All rights reserved.
// </copyright>
// <author>Gil Yoder</author>
// <date>3/11/2013</date>
// <summary>Implements the position and node class</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlHelpmeet.Utility
{
	/// <summary>
	/// 	Position and node.
	/// </summary>
	public class PositionAndNode
	{
		/// <summary>
		/// 	Initializes a new instance of the PositionAndNode class.
		/// </summary>
		public PositionAndNode()
		{
		}

		/// <summary>
		/// 	Initializes a new instance of the PositionAndNode class.
		/// </summary>
		/// <param name="node">
		/// 	The node.
		/// </param>
		public PositionAndNode(XamlNode node, int positionIndex)
		{
			Node = node;
			PositionIndex = positionIndex;
		}

		/// <summary>
		/// 	Gets the bottom of content. (Top of the bottom tag.)
		/// </summary>
		/// <value>
		/// 	The bottom of content.
		/// </value>
		public int BottomOfContent
		{
			get { return Node.EndTag.TopPoint; }
		}

		/// <summary>
		/// 	Gets the bottom of node. (Bottom of the bottom tag.)
		/// </summary>
		/// <value>
		/// 	The bottom of node.
		/// </value>
		public int BottomOfNode
		{
			get { return Node.EndTag.BottomPoint; }
		}

		/// <summary>
		/// 	Gets or sets the node.
		/// </summary>
		/// <value>
		/// 	The node.
		/// </value>
		public XamlNode Node { get; set; }

		/// <summary>
		/// 	Gets or sets the point of concern.
		/// </summary>
		/// <value>
		/// 	The point of concern.
		/// </value>
		public int PositionIndex { get; set; }

		/// <summary>
		/// 	Gets the position of the PointOfConcern.
		/// 	
		/// 	Returns one of the following:
		/// 	Before
		/// 	TopTag
		/// 	Content
		/// 	BottomTag
		/// 	After
		/// </summary>
		/// <value>
		/// 	The position.
		/// </value>
		public NodePosition Position
		{
			get
			{
				if (PositionIndex <= TopOfNode)
					return NodePosition.Before;
				if (PositionIndex < TopOfContent)
					return NodePosition.TopTag;
				if (PositionIndex <= BottomOfContent)
					return NodePosition.Content;
				if (PositionIndex < BottomOfNode)
					return NodePosition.BottomTag;
				return NodePosition.After;
			}
		}

		/// <summary>
		/// 	Gets the top of content. (Bottom of start tag.) 
		/// </summary>
		/// <value>
		/// 	The top of content.
		/// </value>
		public int TopOfContent
		{
			get { return Node.StartTag.BottomPoint; }
		}

		/// <summary>
		/// 	Gets the top of node. (Top of start tag.)
		/// </summary>
		/// <value>
		/// 	The top of node.
		/// </value>
		public int TopOfNode
		{
			get { return Node.StartTag.TopPoint; }
		}
	}
}
