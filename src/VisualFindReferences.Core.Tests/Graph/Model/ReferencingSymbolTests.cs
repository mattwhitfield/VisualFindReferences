namespace VisualFindReferences.Core.Tests.Graph.Model
{
    using VisualFindReferences.Core.Graph.Model;
    using Xunit;
    using FluentAssertions;
    using NSubstitute;
    using Microsoft.CodeAnalysis;
    using System.Collections.Generic;

    public class ReferencingSymbolTests
    {
        private ReferencingSymbol _testClass;
        private ISymbol _symbol;
        private SyntaxNode _syntaxNode;
        private SemanticModel _semanticModel;
        private IList<ReferencingLocation> _referencingLocations;

        public ReferencingSymbolTests()
        {
            _symbol = Substitute.For<ISymbol>();
            _syntaxNode = ClassModelProvider.SyntaxNode;
            _semanticModel = ClassModelProvider.SemanticModel;
            _referencingLocations = Substitute.For<IList<ReferencingLocation>>();
            _testClass = new ReferencingSymbol(_symbol, _syntaxNode, _semanticModel, _referencingLocations);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new ReferencingSymbol(_symbol, _syntaxNode, _semanticModel, _referencingLocations);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void ReferencingLocationsIsInitializedCorrectly()
        {
            _testClass.ReferencingLocations.Should().BeSameAs(_referencingLocations);
        }
    }
}