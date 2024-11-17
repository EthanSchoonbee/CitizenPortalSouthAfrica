//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/* 
 * Author:           Ethan Schoonbee
 * Date Created:     11/10/2024
 * Last Modified:    16/11/2024
 * 
 * Description:
 * This class represents a generic Graph data structure, where the nodes (vertices) 
 * can be of any type (denoted by the generic type T). The graph is implemented 
 * using an adjacency list, which is a dictionary where the key is a node and 
 * the value is a list of adjacent nodes. The class provides methods to add edges, 
 * check for the existence of an edge, retrieve reports at a given location, and 
 * clear the graph.
 * 
 * Usage:
 * This class is designed for scenarios where a graph needs to be used, such 
 * as representing relationships between reports and their locations. For example, 
 * it can be used to model which reports are associated with a specific location.
 * 
 * Methods:
 * - AddEdge: Adds an edge between two nodes.
 * - ContainsEdge: Checks if there is an edge between two nodes.
 * - GetReportsAtLocation: Retrieves a list of reports (nodes) associated with a given location.
 * - Clear: Clears all edges in the graph.
 */
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System.Collections.Generic;

namespace CitizenPortalSouthAfrica.Models
{
    /// <summary>
    /// Represents a generic graph structure using an adjacency list.
    /// The graph can store and manage edges between nodes of type T.
    /// This class provides methods to add edges, check for existing edges, 
    /// retrieve adjacent nodes, and clear the graph.
    /// </summary>
    public class Graph<T>
    {
        private readonly Dictionary<T, List<T>> _adjacencyList;

        /// <summary>
        /// Initializes a new instance of the Graph class.
        /// </summary>
        public Graph()
        {
            // Initialize the adjacency list as an empty dictionary
            _adjacencyList = new Dictionary<T, List<T>>();
        }

        /// <summary>
        /// Checks if there is an edge between the given 'from' and 'to' nodes.
        /// </summary>
        /// <param name="from">The starting node.</param>
        /// <param name="to">The destination node.</param>
        /// <returns>True if an edge exists, otherwise false.</returns>
        public bool ContainsEdge(T from, T to)
        {
            // Check if the 'from' node exists in the adjacency list and if the 'to' node is in the list of adjacent nodes
            return _adjacencyList.ContainsKey(from) && _adjacencyList[from].Contains(to);
        }

        /// <summary>
        /// Adds an edge from the 'location' node to the 'reportId' node in the graph.
        /// </summary>
        /// <param name="location">The source node (e.g., a location).</param>
        /// <param name="reportId">The destination node (e.g., a report identifier).</param>
        public void AddEdge(T location, T reportId)
        {
            // If the location doesn't already exist in the graph, add it with an empty list of adjacent nodes
            if (!_adjacencyList.ContainsKey(location))
                _adjacencyList[location] = new List<T>();

            // Add the reportId to the list of adjacent nodes for the given location
            _adjacencyList[location].Add(reportId);
        }

        /// <summary>
        /// Clears all the edges from the graph by resetting the adjacency list.
        /// </summary>
        public void Clear()
        {
            // Clears all the entries in the adjacency list
            _adjacencyList.Clear();
        }

        /// <summary>
        /// Retrieves all the reports (nodes) associated with a given location.
        /// </summary>
        /// <param name="location">The node for which adjacent reports are to be retrieved.</param>
        /// <returns>A list of reports (nodes) associated with the given location. 
        /// Returns an empty list if the location does not exist in the graph.</returns>
        public List<T> GetReportsAtLocation(T location)
        {
            // Check if the location exists in the graph and return its adjacent nodes
            return _adjacencyList.ContainsKey(location) ? _adjacencyList[(location)] : new List<T>();
        }
    }
}
//---------------....oooOO0_END_OF_FILE_0OOooo....---------------\\