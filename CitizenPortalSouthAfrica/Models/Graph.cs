using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenPortalSouthAfrica.Models
{
    public class Graph<T>
    {
        private readonly Dictionary<T, List<T>> _adjacencyList;

        public Graph()
        { 
            _adjacencyList = new Dictionary<T, List<T>>();
        }

        public void AddEdge(T location, T reportId)
        {
            if (!_adjacencyList.ContainsKey(location))
                _adjacencyList[location] = new List<T>();

            _adjacencyList[location].Add(reportId);
        }

        public List<T> GetReportsAtLocation(T location)
        {
            return _adjacencyList.ContainsKey(location) ? _adjacencyList[(location)] : new List<T>();
        }
    }
}
