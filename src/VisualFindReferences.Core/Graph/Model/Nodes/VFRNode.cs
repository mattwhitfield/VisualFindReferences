using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace VisualFindReferences.Core.Graph.Model.Nodes
{
    public abstract class VFRNode : Node
    {
        protected VFRNode(NodeGraph flowChart, FoundReferences foundReferences, Geometry icon, Brush iconColor) : base(flowChart, GetContainerName(foundReferences), GetTypeName(foundReferences), 0, 0, icon, iconColor)
        {
            NodeFoundReferences = foundReferences;
            NamespaceName = foundReferences.Symbol.ContainingNamespace.ToDisplayString();
            AssemblyName = foundReferences.Symbol.ContainingAssembly.Name;
        }

        public bool ShouldShowTypeName => !string.IsNullOrWhiteSpace(TypeName);

        public string NamespaceName { get; }

        public string AssemblyName { get; }

        public abstract string NodeSymbolType { get; }

        public FoundReferences NodeFoundReferences { get; }

        public List<ISymbol> SearchedSymbols { get; } = new List<ISymbol>();

        private bool _noMoreReferences;

        public bool NoMoreReferences
        {
            get { return _noMoreReferences; }
            set
            {
                if (value != _noMoreReferences)
                {
                    _noMoreReferences = value;
                    RaisePropertyChanged(nameof(NoMoreReferences));
                }
            }
        }

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

        public abstract IEnumerable<SearchableSymbol> GetSearchableSymbols();
    }
}