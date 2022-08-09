using System;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.View
{
    public class NodeEventArgs : EventArgs
    {
        public NodeEventArgs(Node node)
        {
            Node = node;
        }
        public Node Node { get; }
    }
}