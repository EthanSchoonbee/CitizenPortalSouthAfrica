using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenPortalSouthAfrica.Models.Constants;

namespace CitizenPortalSouthAfrica.Models
{
    public class ReportBST
    {
        private ReportNode root;

        public ReportBST()
        {
            root = null;
        }

        public void Insert(Report report)
        {
            var newNode = new ReportNode(report);
            root = InsertRec(root, newNode);
            FixInsert(newNode);
        }

        private ReportNode InsertRec(ReportNode root, ReportNode newNode)
        {
            if (root == null)
            {
                return newNode;
            }

            if (newNode.Report.Id < root.Report.Id)
            {
                root.Left = InsertRec(root.Left, newNode);
                root.Left.Parent = root;
            }
            else if (newNode.Report.Id > root.Report.Id)
            {
                root.Right = InsertRec(root.Right, newNode);
                root.Right.Parent = root;
            }

            return root;
        }

        private void FixInsert(ReportNode node)
        {
            while (node != root && node.Parent.Color == NodeColor.Red)
            {
                var parent = node.Parent;
                var grandparent = parent.Parent;

                if (parent == grandparent.Left) // Parent is left child
                {
                    var uncle = grandparent.Right;
                    if (uncle != null && uncle.Color == NodeColor.Red) // Case 1: Uncle is red
                    {
                        parent.Color = NodeColor.Black;
                        uncle.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red;
                        node = grandparent; // Move up the tree
                    }
                    else
                    {
                        if (node == parent.Right) // Case 2: Node is right child
                        {
                            node = parent;
                            LeftRotate(node);
                        }
                        // Case 3: Node is left child
                        parent.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red;
                        RightRotate(grandparent);
                    }
                }
                else // Parent is right child
                {
                    var uncle = grandparent.Left;
                    if (uncle != null && uncle.Color == NodeColor.Red) // Case 1: Uncle is red
                    {
                        parent.Color = NodeColor.Black;
                        uncle.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red;
                        node = grandparent; // Move up the tree
                    }
                    else
                    {
                        if (node == parent.Left) // Case 2: Node is left child
                        {
                            node = parent;
                            RightRotate(node);
                        }
                        // Case 3: Node is right child
                        parent.Color = NodeColor.Black;
                        grandparent.Color = NodeColor.Red;
                        LeftRotate(grandparent);
                    }
                }
            }

            root.Color = NodeColor.Black; // Root is always black
        }

        private void LeftRotate(ReportNode node)
        {
            var temp = node.Right;
            node.Right = temp.Left;

            if (temp.Left != null)
                temp.Left.Parent = node;

            temp.Parent = node.Parent;

            if (node.Parent == null)
                root = temp;
            else if (node == node.Parent.Left)
                node.Parent.Left = temp;
            else
                node.Parent.Right = temp;

            temp.Left = node;
            node.Parent = temp;
        }

        private void RightRotate(ReportNode node)
        {
            var temp = node.Left;
            node.Left = temp.Right;

            if (temp.Right != null)
                temp.Right.Parent = node;

            temp.Parent = node.Parent;

            if (node.Parent == null)
                root = temp;
            else if (node == node.Parent.Right)
                node.Parent.Right = temp;
            else
                node.Parent.Left = temp;

            temp.Right = node;
            node.Parent = temp;
        }

        // In-order traversal to return reports in sorted order
        public List<Report> InOrderTraversal()
        {
            List<Report> reports = new List<Report>();
            InOrderRec(root, reports);
            return reports;
        }

        private void InOrderRec(ReportNode root, List<Report> reports)
        {
            if (root != null)
            {
                InOrderRec(root.Left, reports);
                reports.Add(root.Report);
                InOrderRec(root.Right, reports);
            }
        }

        public List<Report> Search(string query)
        {
            List<Report> matchingReports = new List<Report>();
            SearchRec(root, query, matchingReports);
            return matchingReports;
        }

        private void SearchRec(ReportNode node, string query, List<Report> matchingReports)
        {
            if (node == null) return;

            // Check if the current node's report matches the query
            if (!string.IsNullOrWhiteSpace(query) &&
                (node.Report.Name?.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 node.Report.Location?.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 node.Report.Description?.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 node.Report.Category?.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                 node.Report.Status?.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                matchingReports.Add(node.Report);
            }

            // Continue searching in the left and right subtrees
            SearchRec(node.Left, query, matchingReports);
            SearchRec(node.Right, query, matchingReports);
        }
    }

}
