// TODO - second round testing with actual syntax trees
//namespace VisualFindReferences.Core.Tests.Graph.Model
//{
//    using VisualFindReferences.Core.Graph.Model;
//    using System;
//    using Xunit;
//    using FluentAssertions;
//    using NSubstitute;
//    using Microsoft.CodeAnalysis;
//    using Microsoft.CodeAnalysis.FindSymbols;
//    using Microsoft.CodeAnalysis.Text;
//    using System.Text;
//    using System.Threading.Tasks;

//    public class SymbolProcessorTests
//    {
//        private SymbolProcessor _testClass;

//        public SymbolProcessorTests()
//        {
//            _testClass = new SymbolProcessor();
//        }

//        [Fact]
//        public async Task CanCallFindReferencesAsyncWithUpdateTextAndSearchingNodeAndTargetSymbolAndDocument()
//        {
//            // Arrange
//            Action<string> updateText = x => { };
//            var searchingNode = new SyntaxNodeWithSymbol(Substitute.For<ISymbol>(), ClassModelProvider.SyntaxNode, ClassModelProvider.SemanticModel);
//            var targetSymbol = Substitute.For<ISymbol>();
//            var document = ClassModelProvider.Document;

//            // Act
//            var result = await SymbolProcessor.FindReferencesAsync(updateText, searchingNode, targetSymbol, document);

//            // Assert
//            throw new NotImplementedException("Create or modify test");
//        }

//        [Fact]
//        public async Task CanCallFindReferencesAsyncWithUpdateTextAndSearchingNodeAndTargetSymbolsAndSolutionAndDocument()
//        {
//            // Arrange
//            Action<string> updateText = x => { };
//            var searchingNode = new SyntaxNodeWithSymbol(Substitute.For<ISymbol>(), ClassModelProvider.SyntaxNode, ClassModelProvider.SemanticModel);
//            var targetSymbols = new[] { Substitute.For<ISymbol>(), Substitute.For<ISymbol>(), Substitute.For<ISymbol>() };
//            var solution = ClassModelProvider.Solution;
//            var document = ClassModelProvider.Document;

//            // Act
//            var result = await SymbolProcessor.FindReferencesAsync(updateText, searchingNode, targetSymbols, solution, document);

//            // Assert
//            throw new NotImplementedException("Create or modify test");
//        }

//        [Fact]
//        public async Task FindReferencesAsyncWithUpdateTextAndSearchingNodeAndTargetSymbolsAndSolutionAndDocumentPerformsMapping()
//        {
//            // Arrange
//            Action<string> updateText = x => { };
//            var searchingNode = new SyntaxNodeWithSymbol(Substitute.For<ISymbol>(), ClassModelProvider.SyntaxNode, ClassModelProvider.SemanticModel);
//            var targetSymbols = new[] { Substitute.For<ISymbol>(), Substitute.For<ISymbol>(), Substitute.For<ISymbol>() };
//            var solution = ClassModelProvider.Solution;
//            var document = ClassModelProvider.Document;

//            // Act
//            var result = await SymbolProcessor.FindReferencesAsync(updateText, searchingNode, targetSymbols, solution, document);

//            // Assert
//            result.Solution.Should().BeSameAs(solution);
//        }

//        [Fact]
//        public void CanCallProcessFoundReferences()
//        {
//            // Arrange
//            var references = new FoundReferences(Substitute.For<ISymbol>(), ClassModelProvider.SyntaxNode, ClassModelProvider.SemanticModel, ClassModelProvider.Solution, new[] { new ReferencingLocation(new ReferenceLocation(), SourceText.From("TestValue617868769", Encoding.GetEncoding(552732859), SourceHashAlgorithm.Sha256)), new ReferencingLocation(new ReferenceLocation(), SourceText.From("TestValue155567672", Encoding.GetEncoding(993656277), SourceHashAlgorithm.Sha1)), new ReferencingLocation(new ReferenceLocation(), SourceText.From("TestValue1813980573", Encoding.GetEncoding(1988935997), SourceHashAlgorithm.Sha256)) });
//            var model = new NodeGraph();

//            // Act
//            SymbolProcessor.ProcessFoundReferences(references, model);

//            // Assert
//            throw new NotImplementedException("Create or modify test");
//        }
//    }
//}