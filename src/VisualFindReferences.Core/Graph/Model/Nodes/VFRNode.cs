using Microsoft.CodeAnalysis;
using System.Windows.Media;

namespace VisualFindReferences.Core.Graph.Model.Nodes
{
    public abstract class VFRNode : Node
    {
        protected VFRNode(NodeGraph flowChart, FoundReferences foundReferences, Geometry icon, Brush iconColor) : base(flowChart, GetContainerName(foundReferences), GetTypeName(foundReferences), 0, 0, icon, iconColor)
        {
            NodeFoundReferences = foundReferences;
            ReferenceSearchAvailable = foundReferences.IsSource;
        }

        public bool ReferenceSearchAvailable { get; set; }

        public FoundReferences NodeFoundReferences { get; }

        private bool _referenceLocationsAdded;

        public bool ReferenceLocationsAdded
        {
            get { return _referenceLocationsAdded; }
            set
            {
                if (value != _referenceLocationsAdded)
                {
                    _referenceLocationsAdded = value;
                    RaisePropertyChanged(nameof(ReferenceLocationsAdded));
                }
            }
        }

        private static string GetContainerName(FoundReferences foundReferences)
        {
            var name = foundReferences.Symbol.Name;
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "Unnamed";
            }
            return name;
        }

        private static string GetTypeName(FoundReferences foundReferences)
        {
            if (foundReferences.Symbol.ContainingType is ITypeSymbol typeSymbol)
            {
                return typeSymbol.Name;
            }

            return string.Empty;
        }
    }
}