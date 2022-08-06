using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace VisualFindReferences.Core.Graph.Model.Nodes
{
    public static class NodeFactory
    {
        private static readonly Dictionary<Type, Func<NodeGraph, FoundReferences, VFRNode>> _factoryMethods = new Dictionary<Type, Func<NodeGraph, FoundReferences, VFRNode>>
        {
            { typeof(AnonymousFunctionExpressionSyntax), (graph, foundReferences) => new LambdaNode(graph, foundReferences) },
            { typeof(AnonymousMethodExpressionSyntax), (graph, foundReferences) => new LambdaNode(graph, foundReferences) },
            { typeof(ParenthesizedLambdaExpressionSyntax), (graph, foundReferences) => new LambdaNode(graph, foundReferences) },
            { typeof(SimpleLambdaExpressionSyntax), (graph, foundReferences) => new LambdaNode(graph, foundReferences) },
            { typeof(ConstructorDeclarationSyntax), (graph, foundReferences) => new ConstructorNode(graph, foundReferences) },
            { typeof(DestructorDeclarationSyntax), (graph, foundReferences) => new DestructorNode(graph, foundReferences) },
            { typeof(OperatorDeclarationSyntax), (graph, foundReferences) => new OperatorNode(graph, foundReferences) },
            { typeof(ConversionOperatorDeclarationSyntax), (graph, foundReferences) => new OperatorNode(graph, foundReferences) },
            { typeof(EventDeclarationSyntax), (graph, foundReferences) => new EventNode(graph, foundReferences) },
            { typeof(EventFieldDeclarationSyntax), (graph, foundReferences) => new EventNode(graph, foundReferences) },
            { typeof(PropertyDeclarationSyntax), (graph, foundReferences) => new PropertyNode(graph, foundReferences) },
            { typeof(IndexerDeclarationSyntax), (graph, foundReferences) => new IndexerNode(graph, foundReferences) },
            { typeof(LocalFunctionStatementSyntax), (graph, foundReferences) => new LocalMethodNode(graph, foundReferences) },
            { typeof(MethodDeclarationSyntax), (graph, foundReferences) => new MethodNode(graph, foundReferences) },
            { typeof(FieldDeclarationSyntax), (graph, foundReferences) => new FieldInitializerNode(graph, foundReferences) },
        };

        public static bool IsSupportedContainer(SyntaxNode node)
        {
            return _factoryMethods.ContainsKey(node.GetType());
        }

        public static VFRNode? Create(SyntaxNode node, NodeGraph graph, FoundReferences references)
        {
            if (_factoryMethods.TryGetValue(node.GetType(), out var factory))
            {
                return factory(graph, references);
            }

            return null;
        }
    }
}
