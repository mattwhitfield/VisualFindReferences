using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualFindReferences.Core.Graph.Model.Nodes
{
    public static class NodeFactory
    {
        private static readonly Dictionary<Type, Func<NodeGraph, FoundReferences, VFRNode>> _factoryMethods = new Dictionary<Type, Func<NodeGraph, FoundReferences, VFRNode>>
        {
            { typeof(AnonymousFunctionExpressionSyntax), (graph, foundReferences) => new AnonymousFunctionNode(graph, foundReferences) },
            { typeof(AnonymousMethodExpressionSyntax), (graph, foundReferences) => new AnonymousMethodNode(graph, foundReferences) },
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
            { typeof(VariableDeclaratorSyntax), (graph, foundReferences) => new FieldInitializerNode(graph, foundReferences) },
        };

        private static readonly Dictionary<Type, Func<SyntaxNode, bool>> _validators = new Dictionary<Type, Func<SyntaxNode, bool>>
        {
            { typeof(VariableDeclaratorSyntax), node => node.Parent is VariableDeclarationSyntax variableDeclaration && variableDeclaration.Parent is FieldDeclarationSyntax },
            { typeof(AnonymousFunctionExpressionSyntax), node => node.Ancestors().Any(x => x is VariableDeclaratorSyntax) },
            { typeof(AnonymousMethodExpressionSyntax), node => node.Ancestors().Any(x => x is VariableDeclaratorSyntax) },
            { typeof(ParenthesizedLambdaExpressionSyntax), node => node.Ancestors().Any(x => x is VariableDeclaratorSyntax) },
            { typeof(SimpleLambdaExpressionSyntax), node => node.Ancestors().Any(x => x is VariableDeclaratorSyntax) },
        };

        public static bool IsSupportedContainer(SyntaxNode node)
        {
            if (_factoryMethods.ContainsKey(node.GetType()))
            {
                if (_validators.TryGetValue(node.GetType(), out var validator))
                {
                    return validator(node);
                }

                return true;
            }

            return false;
        }

        public static VFRNode? Create(SyntaxNode node, NodeGraph graph, FoundReferences references)
        {
            if (_factoryMethods.TryGetValue(node.GetType(), out var factory))
            {
                if (_validators.TryGetValue(node.GetType(), out var validator))
                {
                    if (!validator(node))
                    {
                        return null;
                    }
                }

                return factory(graph, references);
            }

            return null;
        }
    }
}
