using System;
using System.Collections.Generic;
using System.Linq;
using VisualFindReferences.Core.Graph.Helper;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.Layout
{
    public abstract class BalancedGridLayoutAlgorithm : LayoutAlgorithm
    {
        public BalancedGridLayoutAlgorithm(NodeGraph visitedGraph, IDictionary<Node, GraphPoint> verticesPositions) : base(visitedGraph, verticesPositions)
        {
        }

        private HashSet<Node> _disconnectedNodes = new HashSet<Node>();

        private Dictionary<Node, int> _nodeGroupsByNode = new Dictionary<Node, int>();
        private Dictionary<Node, int> _passesByNode = new Dictionary<Node, int>();
        private Dictionary<int, List<Node>> _nodeGroups = new Dictionary<int, List<Node>>();
        private Dictionary<int, List<Node>> _outputGroups = new Dictionary<int, List<Node>>();

        protected override bool AlgorithmReqiuresOverlapRemoval => false;

        protected override void Initialize()
        {
            var laidOutNodes = new HashSet<Node>();

            Tuple<List<Node>, List<Node>> GetLeftAndRightNodes(Node currentNode)
            {
                List<Node> leftNodes = new List<Node>();
                List<Node> rightNodes = new List<Node>();

                foreach (var connector in VisitedGraph.Connectors)
                {
                    if (connector.StartNode == currentNode)
                    {
                        if (!laidOutNodes.Contains(connector.EndNode))
                        {
                            leftNodes.Add(connector.EndNode);
                            //rightNodes.Add(connector.EndNode);
                        }
                    }
                    if (connector.EndNode == currentNode)
                    {
                        if (!laidOutNodes.Contains(connector.StartNode))
                        {
                            rightNodes.Add(connector.StartNode);
                            //leftNodes.Add(connector.StartNode);
                        }
                    }
                }

                return Tuple.Create(leftNodes, rightNodes);
            }

            foreach (var node in VisitedGraph.Nodes)
            {
                _disconnectedNodes.Add(node);
            }

            foreach (var connection in VisitedGraph.Connectors)
            {
                _disconnectedNodes.Remove(connection.StartNode);
                _disconnectedNodes.Remove(connection.EndNode);
            }

            var rightQueue = new Queue<Tuple<Node, int>>();
            var leftQueue = new Queue<Tuple<Node, int>>();
            rightQueue.Enqueue(Tuple.Create(VisitedGraph.Nodes[0], 0));

            int pass = 1;
            while (leftQueue.Count > 0 || rightQueue.Count > 0)
            {
                while (rightQueue.Count > 0)
                {
                    var (currentNode, currentPosition) = rightQueue.Dequeue();
                    if (!laidOutNodes.Add(currentNode))
                    {
                        continue;
                    }
                    GetNodeGroup(currentPosition).Add(currentNode);
                    _passesByNode[currentNode] = pass;

                    // get nodes to the left and right of the current node
                    var (leftNodes, rightNodes) = GetLeftAndRightNodes(currentNode);

                    var rightPosition = currentPosition + 1;
                    foreach (var rightNode in rightNodes)
                    {
                        rightQueue.Enqueue(Tuple.Create(rightNode, rightPosition));
                    }

                    var leftPosition = currentPosition - 1;
                    foreach (var leftNode in leftNodes)
                    {
                        leftQueue.Enqueue(Tuple.Create(leftNode, leftPosition));
                    }
                }

                while (leftQueue.Count > 0)
                {
                    var (currentNode, currentPosition) = leftQueue.Dequeue();
                    if (!laidOutNodes.Add(currentNode))
                    {
                        continue;
                    }
                    GetNodeGroup(currentPosition).Add(currentNode);
                    _passesByNode[currentNode] = pass;

                    // get nodes to the left and right of the current node
                    var (leftNodes, rightNodes) = GetLeftAndRightNodes(currentNode);

                    var rightPosition = currentPosition + 1;
                    foreach (var rightNode in rightNodes)
                    {
                        rightQueue.Enqueue(Tuple.Create(rightNode, rightPosition));
                    }

                    var leftPosition = currentPosition - 1;
                    foreach (var leftNode in leftNodes)
                    {
                        leftQueue.Enqueue(Tuple.Create(leftNode, leftPosition));
                    }
                }

                pass += 2;
            }

            foreach (var group in _nodeGroups)
            {
                foreach (var node in group.Value)
                {
                    _nodeGroupsByNode[node] = group.Key;
                }
            }
        }

        protected abstract void ProcessOutputGroups(IDictionary<int, List<Node>> outputGroups);

        private int GetCrossings(List<Tuple<float, float>> coordinates, IEnumerable<Tuple<float, float>> coordinates2)
        {
            int crossings = 0;
            foreach (var existing in coordinates)
            {
                foreach (var proposed in coordinates2)
                {
                    var leftDiff = proposed.Item1 - existing.Item1;
                    var rightDiff = proposed.Item2 - existing.Item2;
                    if ((leftDiff > 0.1 && rightDiff < -0.1) ||
                        (leftDiff < -0.1 && rightDiff > 0.1))
                    {
                        crossings++;
                    }
                }
            }
            return crossings;
        }

        

        private List<Node> GetNodeGroup(int position)
        {
            return GetGroup(position, _nodeGroups);
        }

        private List<Node> GetOutputGroup(int position)
        {
            return GetGroup(position, _outputGroups);
        }

        private List<Node> GetGroup(int position, Dictionary<int, List<Node>> dictionary)
        {
            if (!dictionary.TryGetValue(position, out var list))
            {
                dictionary[position] = list = new List<Node>();
            }

            return list;
        }

        private IEnumerable<Connector> GetNodeLinksFrom(Node node, int desiredGroup, HashSet<Node> laidOutNodes)
        {
            foreach (var connector in VisitedGraph.Connectors)
            {
                if (connector.StartNode == node && laidOutNodes.Contains(connector.EndNode) && _nodeGroupsByNode.TryGetValue(connector.EndNode, out var group) && group == desiredGroup)
                {
                    yield return connector;
                }
                if (connector.EndNode == node && laidOutNodes.Contains(connector.StartNode) && _nodeGroupsByNode.TryGetValue(connector.StartNode, out var group2) && group2 == desiredGroup)
                {
                    yield return connector;
                }
            }
        }

        protected override void InternalCompute()
        {
            // we now have nodes in their vertical groups, so now we want to try and assign vertical positions, minimizing crossings
            // start at min, forward
            var minGroup = _nodeGroups.Keys.Min();
            var maxGroup = _nodeGroups.Keys.Max();
            var laidOutNodes = new HashSet<Node>();
            var nodesToLayout = _nodeGroups.Select(x => x.Value.Count).Sum();

            // as we are going left to right, we consider links to the left as we place each node - we build up the table of links to compare
            // to as we place the links

            // when going right to left, we consider links to the right as we place each node - and we seed the list of links to compare to first
            // we make two passes, on the first pass we consider only nodes that are connected to the previous group
            // on the second pass, we only have nodes that are only connected to the next group to consider


            for (int pass = 1; laidOutNodes.Count < nodesToLayout; pass++)
            {
                var direction = pass % 2 == 1 ? 1 : -1;
                foreach (var pair in _nodeGroups.OrderBy(x => x.Key))
                {
                    var groupNum = pair.Key;
                    var group = pair.Value;

                    var outputGroup = GetOutputGroup(groupNum);

                    if (_outputGroups.Count == 1)
                    {
                        group.Each(x =>
                        {
                            outputGroup.Add(x);
                            laidOutNodes.Add(x);
                        });
                        continue;
                    }

                    var prevGroup = GetOutputGroup(groupNum - direction);
                    Func<Node, List<Connector>> getLinks = node => GetNodeLinksFrom(node, groupNum - direction, laidOutNodes).ToList();

                    List<Tuple<float, float>> GetYCoordinates()
                    {
                        var yCoordinates = new List<Tuple<float, float>>();
                        var existingLinks = group.SelectMany(node => getLinks(node)).ToList();
                        foreach (var link in existingLinks)
                        {
                            GetCoordinates(outputGroup, prevGroup, yCoordinates, link);
                        }
                        return yCoordinates;
                    }

                    // for each node in the group
                    foreach (var node in group)
                    {
                        var leftLinks = getLinks(node);
                        var isConnectedToPreviousGroup = leftLinks.Any();

                        // skip this node if it's not connected to the left and we're on a foward pass
                        if (direction == 1 && !isConnectedToPreviousGroup)
                        {
                            continue;
                        }

                        // skip any node we have previously laid out
                        if (laidOutNodes.Contains(node))
                        {
                            continue;
                        }

                        if (_passesByNode[node] > pass)
                        {
                            continue;
                        }

                        // if < 1 currently in the output group for the x position just add the node
                        if (outputGroup.Count == 0)
                        {
                            outputGroup.Add(node);
                        }
                        else
                        {
                            var coordinates = GetYCoordinates();

                            // work out the number of crossings that there would be at each index, considering links from the previous group only
                            // put it in the place with least crossings

                            var minCrossings = int.MaxValue;
                            var targetIndex = 0;
                            for (var i = 0; i <= coordinates.Count + 1; i++)
                            {
                                var proposedCoordinates = new List<Tuple<float, float>>();
                                foreach (var link in leftLinks)
                                {
                                    GetProposedCoordinates(prevGroup, i - 0.5f, proposedCoordinates, link);
                                }
                                var indexCrossings = GetCrossings(coordinates, proposedCoordinates);

                                if (indexCrossings < minCrossings)
                                {
                                    targetIndex = i;
                                    minCrossings = indexCrossings;
                                }
                            }

                            if (targetIndex > outputGroup.Count)
                            {
                                outputGroup.Add(node);
                            }
                            else
                            {
                                outputGroup.Insert(targetIndex, node);
                            }
                        }

                        laidOutNodes.Add(node);
                    }
                }
            }

            // add disconnected nodes to a new group to the right
            if (_disconnectedNodes.Any())
            {
                GetOutputGroup(_outputGroups.Keys.Max() + 1).AddRange(_disconnectedNodes);
            }

            _outputGroups.Where(x => x.Value.Count == 0).Select(x => x.Key).ToList().Each(x => _outputGroups.Remove(x));

            ProcessOutputGroups(_outputGroups);
        }

        private static void GetProposedCoordinates(List<Node> prevGroup, float proposedY, List<Tuple<float, float>> yCoordinates, Connector link)
        {
            var toCoordinate = (float)prevGroup.IndexOf(link.EndNode);

            if (toCoordinate >= 0)
            {
                yCoordinates.Add(Tuple.Create(proposedY, toCoordinate));
            }
            else
            {
                var fromCoordinate = (float)prevGroup.IndexOf(link.StartNode);

                if (fromCoordinate >= 0)
                {
                    yCoordinates.Add(Tuple.Create(fromCoordinate, proposedY));
                }
            }
        }

        private static void GetCoordinates(List<Node> outputGroup, List<Node> prevGroup, List<Tuple<float, float>> yCoordinates, Connector link)
        {
            var fromCoordinate = (float)outputGroup.IndexOf(link.StartNode);
            var toCoordinate = (float)prevGroup.IndexOf(link.EndNode);

            if (fromCoordinate >= 0 && toCoordinate >= 0)
            {
                yCoordinates.Add(Tuple.Create(fromCoordinate, toCoordinate));
            }
            else
            {
                fromCoordinate = (float)prevGroup.IndexOf(link.StartNode);
                toCoordinate = (float)outputGroup.IndexOf(link.EndNode);

                if (fromCoordinate >= 0 && toCoordinate >= 0)
                {
                    yCoordinates.Add(Tuple.Create(fromCoordinate, toCoordinate));
                }

            }
        }
    }
}