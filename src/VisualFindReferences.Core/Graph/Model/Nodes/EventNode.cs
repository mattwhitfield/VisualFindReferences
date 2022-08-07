using System.Collections.Generic;
using System.Windows.Media;

namespace VisualFindReferences.Core.Graph.Model.Nodes
{
    public class EventNode : VFRNode
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M7,2H17L13.5,9H17L10,22V14H7V2M9,4V12H12V14.66L14,11H10.24L13.76,4H9Z");

        public EventNode(NodeGraph flowChart, FoundReferences foundReferences) : base(flowChart, foundReferences, DefaultIcon, Brushes.Purple)
        { }

        public override string NodeSymbolType => "Event Accessor";

        public override IEnumerable<SearchableSymbol> GetSearchableSymbols()
        {
            yield return new SearchableSymbol(NodeFoundReferences, new[] { NodeFoundReferences.Symbol }, NodeFoundReferences.Solution, "event " + ContainerName);
        }
    }
}