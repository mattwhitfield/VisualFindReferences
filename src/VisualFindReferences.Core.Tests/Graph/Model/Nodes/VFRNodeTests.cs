namespace VisualFindReferences.Core.Tests.Graph.Model.Nodes
{
    using VisualFindReferences.Core.Graph.Model.Nodes;
    using Xunit;
    using NSubstitute;
    using VisualFindReferences.Core.Graph.Model;
    using Microsoft.CodeAnalysis;
    using System.Collections.Generic;
    using System.Windows.Media;

    public class VFRNodeTests
    {
        private class TestVFRNode : VFRNode
        {
            public TestVFRNode(NodeGraph flowChart, FoundReferences foundReferences, Geometry icon, Brush iconColor) : base(flowChart, foundReferences, icon, iconColor)
            {
            }

            public override IEnumerable<SearchableSymbol> GetSearchableSymbols()
            {
                return default(IEnumerable<SearchableSymbol>);
            }

            public override string NodeSymbolType { get; }
        }

        private TestVFRNode _testClass;
        private NodeGraph _flowChart;
        private FoundReferences _foundReferences;
        private Geometry _icon;
        private Brush _iconColor;

        public VFRNodeTests()
        {
            _flowChart = new NodeGraph();
            _foundReferences = new FoundReferences(Substitute.For<ISymbol>(), ClassModelProvider.SyntaxNode, ClassModelProvider.SemanticModel, ClassModelProvider.Solution, new ReferencingLocation[] { });
            _icon = Geometry.Empty;
            _iconColor = Brushes.Green;
            _testClass = new TestVFRNode(_flowChart, _foundReferences, _icon, _iconColor);
        }

        [Fact]
        public void CanSetAndGetNoMoreReferences()
        {
            _testClass.CheckProperty(x => x.NoMoreReferences);
        }

        [Fact]
        public void CanSetAndGetReferenceLocationsAdded()
        {
            _testClass.CheckProperty(x => x.ReferenceLocationsAdded);
        }
    }
}