// TODO - second round testing with actual syntax trees
//namespace VisualFindReferences.Core.Tests.Graph.Model.Nodes
//{
//    using VisualFindReferences.Core.Graph.Model.Nodes;
//    using System;
//    using Xunit;
//    using FluentAssertions;
//    using NSubstitute;
//    using Microsoft.CodeAnalysis;
//    using VisualFindReferences.Core.Graph.Model;
//    using Microsoft.CodeAnalysis.FindSymbols;
//    using Microsoft.CodeAnalysis.Text;
//    using System.Text;

//    public static class NodeFactoryTests
//    {
//        [Fact]
//        public static void CanCallIsSupportedContainer()
//        {
//            // Arrange
//            var node = ClassModelProvider.SyntaxNode;

//            // Act
//            var result = NodeFactory.IsSupportedContainer(node, out _);

//            // Assert
//            throw new NotImplementedException("Create or modify test");
//        }

//        [Fact]
//        public static void CanCallCreate()
//        {
//            // Arrange
//            var node = ClassModelProvider.SyntaxNode;
//            var graph = new NodeGraph();
//            var references = new FoundReferences(Substitute.For<ISymbol>(), ClassModelProvider.SyntaxNode, ClassModelProvider.SemanticModel, ClassModelProvider.Solution, new[] { new ReferencingLocation(new ReferenceLocation(), SourceText.From("TestValue1350383938", Encoding.GetEncoding(811424207), SourceHashAlgorithm.Sha256)), new ReferencingLocation(new ReferenceLocation(), SourceText.From("TestValue610328248", Encoding.GetEncoding(1068036), SourceHashAlgorithm.None)), new ReferencingLocation(new ReferenceLocation(), SourceText.From("TestValue2013267726", Encoding.GetEncoding(400203046), SourceHashAlgorithm.Sha1)) });

//            // Act
//            var result = NodeFactory.Create(node, graph, references);

//            // Assert
//            throw new NotImplementedException("Create or modify test");
//        }

//        [Fact]
//        public static void CreatePerformsMapping()
//        {
//            // Arrange
//            var node = ClassModelProvider.SyntaxNode;
//            var graph = new NodeGraph();
//            var references = new FoundReferences(Substitute.For<ISymbol>(), ClassModelProvider.SyntaxNode, ClassModelProvider.SemanticModel, ClassModelProvider.Solution, new[] { new ReferencingLocation(new ReferenceLocation(), SourceText.From("TestValue755366342", Encoding.GetEncoding(1374041428), SourceHashAlgorithm.Sha1)), new ReferencingLocation(new ReferenceLocation(), SourceText.From("TestValue268772622", Encoding.GetEncoding(1322599722), SourceHashAlgorithm.Sha1)), new ReferencingLocation(new ReferenceLocation(), SourceText.From("TestValue609036266", Encoding.GetEncoding(1945143054), SourceHashAlgorithm.Sha256)) });

//            // Act
//            var result = NodeFactory.Create(node, graph, references);

//            // Assert
//            result.SourceDocument.Should().BeSameAs(references.SourceDocument);
//        }
//    }
//}