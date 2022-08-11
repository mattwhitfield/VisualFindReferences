using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualFindReferences.Core.Graph.Model.Nodes
{
    public static class NodeFactory
    {
        private static readonly Dictionary<Type, Func<NodeGraph, FoundReferences, SyntaxNode, VFRNode>> _factoryMethods = new Dictionary<Type, Func<NodeGraph, FoundReferences, SyntaxNode, VFRNode>>
        {
            { typeof(AnonymousFunctionExpressionSyntax), (graph, foundReferences, node) => new AnonymousFunctionNode(graph, foundReferences) },
            { typeof(AnonymousMethodExpressionSyntax), (graph, foundReferences, node) => new AnonymousMethodNode(graph, foundReferences) },
            { typeof(ParenthesizedLambdaExpressionSyntax), (graph, foundReferences, node) => new LambdaNode(graph, foundReferences) },
            { typeof(SimpleLambdaExpressionSyntax), (graph, foundReferences, node) => new LambdaNode(graph, foundReferences) },
            { typeof(ConstructorDeclarationSyntax), (graph, foundReferences, node) => new ConstructorNode(graph, foundReferences) },
            { typeof(DestructorDeclarationSyntax), (graph, foundReferences, node) => new DestructorNode(graph, foundReferences) },
            { typeof(OperatorDeclarationSyntax), (graph, foundReferences, node) => new OperatorNode(graph, foundReferences) },
            { typeof(ConversionOperatorDeclarationSyntax), (graph, foundReferences, node) => new OperatorNode(graph, foundReferences) },
            { typeof(EventDeclarationSyntax), (graph, foundReferences, node) => new EventNode(graph, foundReferences) },
            { typeof(EventFieldDeclarationSyntax), (graph, foundReferences, node) => new EventNode(graph, foundReferences) },
            { typeof(PropertyDeclarationSyntax), (graph, foundReferences, node) => new PropertyNode(graph, foundReferences) },
            { typeof(AccessorDeclarationSyntax), (graph, foundReferences, node) => HandleAccessor(node)?.Invoke(graph, foundReferences) ?? new UnknownNode(graph, foundReferences) },
            { typeof(IndexerDeclarationSyntax), (graph, foundReferences, node) => new IndexerNode(graph, foundReferences) },
            { typeof(LocalFunctionStatementSyntax), (graph, foundReferences, node) => new LocalMethodNode(graph, foundReferences) },
            { typeof(MethodDeclarationSyntax), (graph, foundReferences, node) => new MethodNode(graph, foundReferences) },
            { typeof(VariableDeclaratorSyntax), (graph, foundReferences, node) => new FieldInitializerNode(graph, foundReferences) },
            { typeof(ClassDeclarationSyntax), (graph, foundReferences, node) => new ClassNode(graph, foundReferences) },
            { typeof(StructDeclarationSyntax), (graph, foundReferences, node) => new StructNode(graph, foundReferences) },
            { typeof(RecordDeclarationSyntax), (graph, foundReferences, node) => new RecordNode(graph, foundReferences) },
            { typeof(InterfaceDeclarationSyntax), (graph, foundReferences, node) => new InterfaceNode(graph, foundReferences) },
        };

        private static Func<NodeGraph, FoundReferences, VFRNode>? HandleAccessor(SyntaxNode node)
        {
            var accessor = (AccessorDeclarationSyntax)node;
            if (accessor.IsKind(SyntaxKind.GetAccessorDeclaration))
            {
                foreach (var ancestor in node.Ancestors())
                {
                    if (ancestor is PropertyDeclarationSyntax)
                    {
                        return (graph, foundReferences) => new PropertyAccessorNode(graph, foundReferences);
                    }
                    if (ancestor is IndexerDeclarationSyntax)
                    {
                        return (graph, foundReferences) => new IndexerAccessorNode(graph, foundReferences);
                    }
                }
            }
            else if (accessor.IsKind(SyntaxKind.SetAccessorDeclaration))
            {
                foreach (var ancestor in node.Ancestors())
                {
                    if (ancestor is PropertyDeclarationSyntax)
                    {
                        return (graph, foundReferences) => new PropertyMutatorNode(graph, foundReferences);
                    }
                    if (ancestor is IndexerDeclarationSyntax)
                    {
                        return (graph, foundReferences) => new IndexerMutatorNode(graph, foundReferences);
                    }
                }
            }
            else if (accessor.IsKind(SyntaxKind.AddAccessorDeclaration) || accessor.IsKind(SyntaxKind.RemoveAccessorDeclaration))
            {
                return (graph, foundReferences) => new EventNode(graph, foundReferences);
            }

            return null;
        }

        private static readonly Dictionary<Type, Func<SyntaxNode, bool>> _validators = new Dictionary<Type, Func<SyntaxNode, bool>>
        {
            { typeof(VariableDeclaratorSyntax), node => node.Parent is VariableDeclarationSyntax variableDeclaration && variableDeclaration.Parent is FieldDeclarationSyntax },
            { typeof(AnonymousFunctionExpressionSyntax), node => node.Ancestors().Any(x => x is VariableDeclaratorSyntax) },
            { typeof(AnonymousMethodExpressionSyntax), node => node.Ancestors().Any(x => x is VariableDeclaratorSyntax) },
            { typeof(ParenthesizedLambdaExpressionSyntax), node => node.Ancestors().Any(x => x is VariableDeclaratorSyntax) },
            { typeof(SimpleLambdaExpressionSyntax), node => node.Ancestors().Any(x => x is VariableDeclaratorSyntax) },
            { typeof(AccessorDeclarationSyntax), node => HandleAccessor(node) != null },
        };

        private static readonly Dictionary<Type, Func<SyntaxNode, SyntaxNode>> _targetTransforms = new Dictionary<Type, Func<SyntaxNode, SyntaxNode>>
        {
            { typeof(AnonymousFunctionExpressionSyntax), node => node.Ancestors().FirstOrDefault(x => x is VariableDeclaratorSyntax) ?? node },
            { typeof(AnonymousMethodExpressionSyntax), node => node.Ancestors().FirstOrDefault(x => x is VariableDeclaratorSyntax) ?? node },
            { typeof(ParenthesizedLambdaExpressionSyntax), node => node.Ancestors().FirstOrDefault(x => x is VariableDeclaratorSyntax) ?? node },
            { typeof(SimpleLambdaExpressionSyntax), node => node.Ancestors().FirstOrDefault(x => x is VariableDeclaratorSyntax) ?? node },
        };

        public static bool IsSupportedContainer(SyntaxNode node, out SyntaxNode actualTarget)
        {
            actualTarget = node;
            if (_factoryMethods.ContainsKey(node.GetType()))
            {
                if (_targetTransforms.TryGetValue(node.GetType(), out var nodeTransform))
                {
                    actualTarget = nodeTransform(node);
                }

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

                return factory(graph, references, node);
            }

            return null;
        }
    }
}
