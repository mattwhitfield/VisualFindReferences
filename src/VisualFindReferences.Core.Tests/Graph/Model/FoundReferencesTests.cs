namespace VisualFindReferences.Core.Tests.Graph.Model
{
    using VisualFindReferences.Core.Graph.Model;
    using Xunit;
    using FluentAssertions;
    using NSubstitute;
    using Microsoft.CodeAnalysis;
    using System.Collections.Generic;

    public class FoundReferencesTests
    {
        private FoundReferences _testClass;
        private ISymbol _symbol;
        private SyntaxNode _syntaxNode;
        private SemanticModel _semanticModel;
        private Solution _solution;
        private IList<ReferencingSymbol> _referencingSymbols;
        private Document _sourceDocument;
        private IList<ReferencingLocation> _referencingLocations;

        public FoundReferencesTests()
        {
            _symbol = Substitute.For<ISymbol>();
            _syntaxNode = ClassModelProvider.SyntaxNode;
            _semanticModel = ClassModelProvider.SemanticModel;
            _solution = ClassModelProvider.Solution;
            _referencingSymbols = Substitute.For<IList<ReferencingSymbol>>();
            _sourceDocument = ClassModelProvider.Document;
            _referencingLocations = Substitute.For<IList<ReferencingLocation>>();
            _testClass = new FoundReferences(_symbol, _syntaxNode, _semanticModel, _solution, _referencingSymbols, _sourceDocument);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new FoundReferences(_symbol, _syntaxNode, _semanticModel, _solution, _referencingSymbols, _sourceDocument);

            // Assert
            instance.Should().NotBeNull();

            // Act
            instance = new FoundReferences(_symbol, _syntaxNode, _semanticModel, _solution, _referencingLocations);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void ReferencingSymbolsIsInitializedCorrectly()
        {
            _testClass.ReferencingSymbols.Should().BeSameAs(_referencingSymbols);
        }

        [Fact]
        public void SourceDocumentIsInitializedCorrectly()
        {
            _testClass.SourceDocument.Should().BeSameAs(_sourceDocument);
        }

        [Fact]
        public void ReferencingLocationsIsInitializedCorrectly()
        {
            _testClass = new FoundReferences(_symbol, _syntaxNode, _semanticModel, _solution, _referencingLocations);
            _testClass.ReferencingLocations.Should().BeSameAs(_referencingLocations);
        }

        [Fact]
        public void SolutionIsInitializedCorrectly()
        {
            _testClass = new FoundReferences(_symbol, _syntaxNode, _semanticModel, _solution, _referencingSymbols, _sourceDocument);
            _testClass.Solution.Should().BeSameAs(_solution);
            _testClass = new FoundReferences(_symbol, _syntaxNode, _semanticModel, _solution, _referencingLocations);
            _testClass.Solution.Should().BeSameAs(_solution);
        }
    }
}