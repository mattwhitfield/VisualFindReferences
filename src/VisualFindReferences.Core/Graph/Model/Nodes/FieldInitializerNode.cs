using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace VisualFindReferences.Core.Graph.Model.Nodes
{
    public class FieldInitializerNode : VFRNode
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M21.41 11.58L12.41 2.58A2 2 0 0 0 11 2H4A2 2 0 0 0 2 4V11A2 2 0 0 0 2.59 12.42L11.59 21.42A2 2 0 0 0 13 22A2 2 0 0 0 14.41 21.41L21.41 14.41A2 2 0 0 0 22 13A2 2 0 0 0 21.41 11.58M13 20L4 11V4H11L20 13M6.5 5A1.5 1.5 0 1 1 5 6.5A1.5 1.5 0 0 1 6.5 5Z");

        public FieldInitializerNode(NodeGraph flowChart, FoundReferences foundReferences) : base(flowChart, foundReferences, DefaultIcon, Brushes.Purple)
        { }

        public override string NodeSymbolType => "Field Initializer";

        public override IEnumerable<SearchableSymbol> GetSearchableSymbols()
        {
            yield return new SearchableSymbol(NodeFoundReferences, new[] { NodeFoundReferences.Symbol }, NodeFoundReferences.Solution, "field " + ContainerName);

            var type = NodeFoundReferences.Symbol.ContainingType;
            var constructors = type.Constructors.OfType<ISymbol>().ToList();
            yield return new SearchableSymbol(NodeFoundReferences, constructors, NodeFoundReferences.Solution, "all constructors of type " + TypeName);
        }

    }
}