namespace VisualFindReferences.Core.Tests.Graph.Model
{
    using VisualFindReferences.Core.Graph.Model;
    using Xunit;
    using FluentAssertions;
    using NSubstitute;
    using Microsoft.CodeAnalysis;
    using System.Collections.Generic;

    public class SearchableSymbolTests
    {
        private SearchableSymbol _testClass;
        private SyntaxNodeWithSymbol _searchingSymbol;
        private IList<ISymbol> _targets;
        private Solution _solution;
        private string _name;

        public SearchableSymbolTests()
        {
            _searchingSymbol = new SyntaxNodeWithSymbol(Substitute.For<ISymbol>(), ClassModelProvider.SyntaxNode, ClassModelProvider.SemanticModel);
            _targets = Substitute.For<IList<ISymbol>>();
            _solution = ClassModelProvider.Solution;
            _name = "TestValue398959655";
            _testClass = new SearchableSymbol(_searchingSymbol, _targets, _solution, _name);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new SearchableSymbol(_searchingSymbol, _targets, _solution, _name);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void SearchingSymbolIsInitializedCorrectly()
        {
            _testClass.SearchingSymbol.Should().BeSameAs(_searchingSymbol);
        }

        [Fact]
        public void TargetsIsInitializedCorrectly()
        {
            _testClass.Targets.Should().BeSameAs(_targets);
        }

        [Fact]
        public void SolutionIsInitializedCorrectly()
        {
            _testClass.Solution.Should().BeSameAs(_solution);
        }

        [Fact]
        public void NameIsInitializedCorrectly()
        {
            _testClass.Name.Should().Be(_name);
        }
    }
}