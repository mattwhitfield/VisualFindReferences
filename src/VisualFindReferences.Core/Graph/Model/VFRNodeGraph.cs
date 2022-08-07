using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using VisualFindReferences.Core.Graph.Model.Nodes;

namespace VisualFindReferences.Core.Graph.Model
{
    public class VFRNodeGraph : NodeGraph
    {
        private Dictionary<ISymbol, VFRNode> _nodesByTargetSymbol = new Dictionary<ISymbol, VFRNode>(SymbolEqualityComparer.Default);

        protected override void NodeAdded(Node node)
        {
            base.NodeAdded(node);

            if (node is VFRNode vfrNode)
            {
                _nodesByTargetSymbol[vfrNode.NodeFoundReferences.Symbol] = vfrNode;
            }
        }

        protected override void NodeRemoved(Node node)
        {
            base.NodeRemoved(node);

            if (node is VFRNode vfrNode)
            {
                _nodesByTargetSymbol.Remove(vfrNode.NodeFoundReferences.Symbol);
            }
        }

        public bool GetNodeFor(ISymbol symbol, out VFRNode targetNode)
        {
            return _nodesByTargetSymbol.TryGetValue(symbol, out targetNode);
        }
    }
}