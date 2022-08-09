using System.Collections.Generic;
using System.Windows.Media;

namespace VisualFindReferences.Core.Graph.Model.Nodes
{
    public class ClassNode : VFRNode
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M6,9V4H13V9H23V16H18V21H11V16H1V9H6M16,16H13V19H16V16M8,9H11V6H8V9M6,14V11H3V14H6M18,11V14H21V11H18M13,11V14H16V11H13M8,11V14H11V11H8Z");

        public ClassNode(NodeGraph flowChart, FoundReferences foundReferences) : base(flowChart, foundReferences, DefaultIcon, Brushes.DarkGoldenrod)
        { }

        public override string NodeSymbolType => "Class";

        public override IEnumerable<SearchableSymbol> GetSearchableSymbols()
        {
            yield return new SearchableSymbol(NodeFoundReferences, new[] { NodeFoundReferences.Symbol }, NodeFoundReferences.Solution, "class " + TypeName);
        }
    }
}