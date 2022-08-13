namespace VisualFindReferences.Core.Tests.Graph.Model
{
    using VisualFindReferences.Core.Graph.Model;
    using Xunit;
    using FluentAssertions;
    using NSubstitute;
    using Microsoft.CodeAnalysis;

    public class SyntaxNodeWithSymbolTests
    {
        private SyntaxNodeWithSymbol _testClass;
        private ISymbol _symbol;
        private SyntaxNode _syntaxNode;
        private SemanticModel _semanticModel;

        public SyntaxNodeWithSymbolTests()
        {
            _symbol = Substitute.For<ISymbol>();
            _syntaxNode = ClassModelProvider.SyntaxNode;
            _semanticModel = ClassModelProvider.SemanticModel;
            _testClass = new SyntaxNodeWithSymbol(_symbol, _syntaxNode, _semanticModel);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new SyntaxNodeWithSymbol(_symbol, _syntaxNode, _semanticModel);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void SymbolIsInitializedCorrectly()
        {
            _testClass.Symbol.Should().BeSameAs(_symbol);
        }

        [Fact]
        public void SyntaxNodeIsInitializedCorrectly()
        {
            _testClass.SyntaxNode.Should().BeSameAs(_syntaxNode);
        }

        [Fact]
        public void SemanticModelIsInitializedCorrectly()
        {
            _testClass.SemanticModel.Should().BeSameAs(_semanticModel);
        }
    }
}