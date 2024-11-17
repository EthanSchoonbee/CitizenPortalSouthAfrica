//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     11/11/2024
 * Last Modified:    16/11/2024
 * 
 * Description:
 * This class represents a node in a binary search tree (BST) or red-black tree 
 * structure. It is designed to store a report and manage references to its 
 * left child, right child, parent node, and the color of the node for red-black 
 * tree balancing purposes.
 * 
 * Usage:
 * Primarily used for organizing reports in a hierarchical structure, allowing 
 * efficient searching, insertion, and deletion of report records.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

namespace CitizenPortalSouthAfrica.Models
{
    /// <summary>
    /// Represents a node in a binary search tree (BST) or red-black tree.
    /// Holds a <see cref="Report"/> object and references to child nodes, 
    /// parent node, and its color for balancing purposes in a red-black tree.
    /// </summary>
    public class ReportNode
    {
        /// <summary>
        /// The report data stored in this node.
        /// </summary>
        public Report Report { get; set; }

        /// <summary>
        /// Reference to the left child node.
        /// </summary>
        public ReportNode Left { get; set; }

        /// <summary>
        /// Reference to the right child node.
        /// </summary>
        public ReportNode Right { get; set; }

        /// <summary>
        /// Reference to the parent node.
        /// </summary>
        public ReportNode Parent { get; set; }

        /// <summary>
        /// The color of the node, used for red-black tree balancing.
        /// Defaults to red for new nodes.
        /// </summary>
        public Constants.NodeColor Color { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportNode"/> class 
        /// with the given report. The node's color is set to red by default.
        /// </summary>
        /// <param name="report">The report data to store in the node.</param>
        public ReportNode(Report report)
        {
            Report = report; // Store the report data in the node.
            Color = Constants.NodeColor.Red; // New nodes default to red for red-black tree balancing.
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\