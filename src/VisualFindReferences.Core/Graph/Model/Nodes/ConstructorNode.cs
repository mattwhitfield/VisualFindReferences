using System.Collections.Generic;
using System.Windows.Media;

namespace VisualFindReferences.Core.Graph.Model.Nodes
{
    public class ConstructorNode : VFRNode
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M21,16.5C21,16.88 20.79,17.21 20.47,17.38L12.57,21.82C12.41,21.94 12.21,22 12,22C11.79,22 11.59,21.94 11.43,21.82L3.53,17.38C3.21,17.21 3,16.88 3,16.5V7.5C3,7.12 3.21,6.79 3.53,6.62L11.43,2.18C11.59,2.06 11.79,2 12,2C12.21,2 12.41,2.06 12.57,2.18L20.47,6.62C20.79,6.79 21,7.12 21,7.5V16.5M12,4.15L6.04,7.5L12,10.85L17.96,7.5L12,4.15M5,15.91L11,19.29V12.58L5,9.21V15.91M19,15.91V9.21L13,12.58V19.29L19,15.91Z");

        public ConstructorNode(NodeGraph flowChart, FoundReferences foundReferences) : base(flowChart, foundReferences, DefaultIcon, Brushes.Purple)
        { }

        public override string NodeSymbolType => "Constructor";

        public override IEnumerable<SearchableSymbol> GetSearchableSymbols()
        {
            yield return new SearchableSymbol(NodeFoundReferences, new[] { NodeFoundReferences.Symbol }, NodeFoundReferences.Solution, "constructor for type " + TypeName);
        }
    }

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

    public class StructNode : VFRNode
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M18,15H16V17H18M18,11H16V13H18M20,19H12V17H14V15H12V13H14V11H12V9H20M10,7H8V5H10M10,11H8V9H10M10,15H8V13H10M10,19H8V17H10M6,7H4V5H6M6,11H4V9H6M6,15H4V13H6M6,19H4V17H6M12,7V3H2V21H22V7H12Z");

        public StructNode(NodeGraph flowChart, FoundReferences foundReferences) : base(flowChart, foundReferences, DefaultIcon, Brushes.DarkGoldenrod)
        { }

        public override string NodeSymbolType => "Struct";

        public override IEnumerable<SearchableSymbol> GetSearchableSymbols()
        {
            yield return new SearchableSymbol(NodeFoundReferences, new[] { NodeFoundReferences.Symbol }, NodeFoundReferences.Solution, "struct " + TypeName);
        }
    }

    public class RecordNode : VFRNode
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M12,11A1,1 0 0,0 11,12A1,1 0 0,0 12,13A1,1 0 0,0 13,12A1,1 0 0,0 12,11M12,16.5C9.5,16.5 7.5,14.5 7.5,12C7.5,9.5 9.5,7.5 12,7.5C14.5,7.5 16.5,9.5 16.5,12C16.5,14.5 14.5,16.5 12,16.5M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z");

        public RecordNode(NodeGraph flowChart, FoundReferences foundReferences) : base(flowChart, foundReferences, DefaultIcon, Brushes.DarkGoldenrod)
        { }

        public override string NodeSymbolType => "Record";

        public override IEnumerable<SearchableSymbol> GetSearchableSymbols()
        {
            yield return new SearchableSymbol(NodeFoundReferences, new[] { NodeFoundReferences.Symbol }, NodeFoundReferences.Solution, "record " + TypeName);
        }
    }

    public class InterfaceNode : VFRNode
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M9,5A7,7 0 0,0 2,12A7,7 0 0,0 9,19C10.04,19 11.06,18.76 12,18.32C12.94,18.76 13.96,19 15,19A7,7 0 0,0 22,12A7,7 0 0,0 15,5C13.96,5 12.94,5.24 12,5.68C11.06,5.24 10.04,5 9,5M9,7C9.34,7 9.67,7.03 10,7.1C8.72,8.41 8,10.17 8,12C8,13.83 8.72,15.59 10,16.89C9.67,16.96 9.34,17 9,17A5,5 0 0,1 4,12A5,5 0 0,1 9,7M15,7A5,5 0 0,1 20,12A5,5 0 0,1 15,17C14.66,17 14.33,16.97 14,16.9C15.28,15.59 16,13.83 16,12C16,10.17 15.28,8.41 14,7.11C14.33,7.04 14.66,7 15,7M12,8C13.26,8.95 14,10.43 14,12C14,13.57 13.26,15.05 12,16C10.74,15.05 10,13.57 10,12C10,10.43 10.74,8.95 12,8Z");

        public InterfaceNode(NodeGraph flowChart, FoundReferences foundReferences) : base(flowChart, foundReferences, DefaultIcon, Brushes.LightBlue)
        { }

        public override string NodeSymbolType => "Interface";

        public override IEnumerable<SearchableSymbol> GetSearchableSymbols()
        {
            yield return new SearchableSymbol(NodeFoundReferences, new[] { NodeFoundReferences.Symbol }, NodeFoundReferences.Solution, "interface " + TypeName);
        }
    }
}