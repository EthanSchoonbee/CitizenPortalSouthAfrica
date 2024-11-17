//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     11/10/2024
 * Last Modified:    16/11/2024
 * 
 * Description:
 * This class implements a binary search tree (BST) with red-black tree balancing 
 * for storing, searching, and managing reports. It provides methods for inserting 
 * reports, maintaining the tree's balancing properties, and performing searches.
 * 
 * Usage:
 * The class ensures that reports are stored in an ordered structure, and it 
 * guarantees balanced insertion for efficient search operations. It includes 
 * features like in-order traversal to return sorted reports, as well as search 
 * functionality to find reports by matching string queries with report attributes.
 * 
 * The red-black tree balancing ensures optimal performance by adjusting colors 
 * and performing rotations after each insertion.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System;
using System.Collections.Generic;
using static CitizenPortalSouthAfrica.Models.Constants;

namespace CitizenPortalSouthAfrica.Models
{
    /// <summary>
    /// Represents a binary search tree (BST) with red-black tree balancing 
    /// for storing reports. This tree supports insertion, balancing, and 
    /// efficient searching of reports.
    /// </summary>
    public class ReportBST
    {
        /// <summary>
        /// Root node of the report binary search tree.
        /// </summary>
        private ReportNode root;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportBST"/> class.
        /// The tree starts empty.
        /// </summary>
        public ReportBST()
        {
            root = null; // Initialize the root to null (empty tree).
        }

        /// <summary>
        /// Inserts a new report into the binary search tree, ensuring that the tree 
        /// maintains its balance properties.
        /// </summary>
        /// <param name="report">The report to insert into the tree.</param>
        public void Insert(Report report)
        {
            var newNode = new ReportNode(report); // Create a new node with the report.
            root = InsertRec(root, newNode); // Insert the new node recursively.
            FixInsert(newNode); // Ensure the red-black tree properties are maintained.
        }

        /// <summary>
        /// Recursive method to insert a new node into the binary search tree.
        /// </summary>
        /// <param name="root">The current root of the subtree.</param>
        /// <param name="newNode">The new node to insert.</param>
        /// <returns>The updated root of the subtree.</returns>
        private ReportNode InsertRec(ReportNode root, ReportNode newNode)
        {
            if (root == null)
            {
                return newNode; // Insert the node if the root is null (leaf position).
            }

            if (newNode.Report.Id < root.Report.Id) // Compare report IDs for tree ordering.
            {
                root.Left = InsertRec(root.Left, newNode); // Insert in the left subtree.
                root.Left.Parent = root; // Update the parent of the left child.
            }
            else if (newNode.Report.Id > root.Report.Id) // If the report ID is greater.
            {
                root.Right = InsertRec(root.Right, newNode); // Insert in the right subtree.
                root.Right.Parent = root; // Update the parent of the right child.
            }

            return root;
        }

        /// <summary>
        /// Fixes any violations of red-black tree properties after an insert.
        /// Performs rotations and color changes as needed.
        /// </summary>
        /// <param name="node">The newly inserted node.</param>
        private void FixInsert(ReportNode node)
        {
            while (node != root && node.Parent.Color == NodeColor.Red) // While parent is red.
            {
                var parent = node.Parent;
                var grandparent = parent.Parent;

                if (parent == grandparent.Left) // If parent is left child of grandparent.
                {
                    var uncle = grandparent.Right; // Uncle node of the parent.
                    if (uncle != null && uncle.Color == NodeColor.Red) // Case 1: Uncle is red
                    {
                        parent.Color = NodeColor.Black; // Parent and uncle turn black.
                        uncle.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red; // Grandparent turns red.
                        node = grandparent; // Move up the tree to continue fixing.
                    }
                    else
                    {
                        if (node == parent.Right) // Case 2: Node is right child of parent
                        {
                            node = parent; // Move the node up for rotation.
                            LeftRotate(node); // Perform left rotation.
                        }
                        // Case 3: Node is left child, right rotation on grandparent.
                        parent.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red;
                        RightRotate(grandparent);
                    }
                }
                else // If Parent is right child of grandparent
                {
                    var uncle = grandparent.Left;
                    if (uncle != null && uncle.Color == NodeColor.Red) // Case 1: Uncle is red
                    {
                        parent.Color = NodeColor.Black;
                        uncle.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red;
                        node = grandparent; // Move up the tree to continue fixing.
                    }
                    else
                    {
                        if (node == parent.Left) // Case 2: Node is left child
                        {
                            node = parent;
                            RightRotate(node); // Perform right rotation.
                        }
                        // Case 3: Node is right child, perform left rotation on grandparent.
                        parent.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red;
                        LeftRotate(grandparent);
                    }
                }
            }

            root.Color = NodeColor.Black; // Root is always black
        }

        /// <summary>
        /// Performs a left rotation on a given node to maintain red-black tree balance.
        /// </summary>
        /// <param name="node">The node to rotate left.</param>
        private void LeftRotate(ReportNode node)
        {
            var temp = node.Right;
            node.Right = temp.Left;

            if (temp.Left != null)
                temp.Left.Parent = node;

            temp.Parent = node.Parent;

            if (node.Parent == null)
                root = temp; // If the node is the root, update the root.
            else if (node == node.Parent.Left)
                node.Parent.Left = temp; // Update parent's left child.
            else
                node.Parent.Right = temp; // Update parent's right child.

            temp.Left = node; // Move node to the left.
            node.Parent = temp; // Update node's parent.
        }

        /// <summary>
        /// Performs a right rotation on a given node to maintain red-black tree balance.
        /// </summary>
        /// <param name="node">The node to rotate right.</param>
        private void RightRotate(ReportNode node)
        {
            var temp = node.Left;
            node.Left = temp.Right;

            if (temp.Right != null)
                temp.Right.Parent = node;

            temp.Parent = node.Parent;

            if (node.Parent == null) 
                root = temp; // If the node is the root, update the root.
            else if (node == node.Parent.Right)
                node.Parent.Right = temp; // Update parent's right child.
            else
                node.Parent.Left = temp; // Update parent's left child.

            temp.Right = node; // Move node to the right.
            node.Parent = temp; // Update node's parent.
        }

        /// <summary>
        /// Performs an in-order traversal of the tree to return a sorted list of reports.
        /// </summary>
        /// <returns>A list of reports in sorted order.</returns>
        public List<Report> InOrderTraversal()
        {
            List<Report> reports = new List<Report>();
            InOrderRec(root, reports); // Start in-order traversal from root.
            return reports;
        }

        /// <summary>
        /// Recursive method for in-order traversal of the tree.
        /// </summary>
        /// <param name="root">The current root of the subtree.</param>
        /// <param name="reports">The list of reports to populate.</param>
        private void InOrderRec(ReportNode root, List<Report> reports)
        {
            if (root != null)
            {
                InOrderRec(root.Left, reports); // Traverse left subtree.
                reports.Add(root.Report); // Add the current node's report to the list.
                InOrderRec(root.Right, reports); // Traverse right subtree.
            }
        }

        /// <summary>
        /// Searches the tree for reports matching the given query string.
        /// </summary>
        /// <param name="query">The query string to search for.</param>
        /// <returns>A list of matching reports.</returns>
        public List<Report> Search(string query)
        {
            List<Report> matchingReports = new List<Report>();
            SearchRec(root, query, matchingReports); // Start the search from the root.
            return matchingReports;
        }

        /// <summary>
        /// Recursive method to search for reports matching the query.
        /// </summary>
        /// <param name="node">The current node being searched.</param>
        /// <param name="query">The query string to search for.</param>
        /// <param name="matchingReports">The list to store matching reports.</param>
        private void SearchRec(ReportNode node, string query, List<Report> matchingReports)
        {
            if (node == null) return; // Return if the node is null.

            // Check if the current node's report matches the query
            if (!string.IsNullOrWhiteSpace(query) &&
                (node.Report.Name?.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 node.Report.Location?.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 node.Report.Description?.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 node.Report.Category?.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 node.Report.Status?.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                matchingReports.Add(node.Report); // Add the matching report.
            }

            // Continue searching in the left and right subtrees
            SearchRec(node.Left, query, matchingReports);
            SearchRec(node.Right, query, matchingReports);
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\