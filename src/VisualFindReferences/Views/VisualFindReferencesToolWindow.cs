using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using VisualFindReferences.Core.Graph.Helper;
using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.Model.Nodes;
using VisualFindReferences.Core.Graph.ViewModel;
using VisualFindReferences.Helper;
using VisualFindReferences.Options;

namespace VisualFindReferences.Views
{
    [Guid(ToolWindowGuid)]
    internal class VisualFindReferencesToolWindow : ToolWindowPane, IVisualFindReferencesToolWindow
    {
        public const string ToolWindowGuid = "f27b912a-9ce0-4e68-a8db-cdd2516793cd";
        public const string Title = "Visual Find References";

        private readonly VisualFindReferencesHost _host;

        public VisualFindReferencesToolWindow(GeneralOptions options) :
            base(null)
        {
            Caption = Title;

            base.Content = _host = new VisualFindReferencesHost(options);
        }

        public void Clear()
        {
            _host.Model.Nodes.ToList().Each(x => _host.Model.Nodes.Remove(x));
        }

        private static HashSet<Type> ParentedTypes = new HashSet<Type> { typeof(ParameterListSyntax), typeof(BlockSyntax) };

        public void FindReferences(IWpfTextView textView, IVisualFindReferencesPackage package)
        {
            async Task<FoundReferences> FindReferencesAsync(Action<string> updateText, NodeGraphViewModel viewModel, CancellationToken cancellation)
            {
                var (syntaxNode, semanticModel) = await TextViewHelper.GetTargetSymbolAsync(textView, cancellation).ConfigureAwait(true);
                var caretPosition = textView.Caret.Position.BufferPosition;
                var document = caretPosition.Snapshot.GetOpenDocumentInCurrentContextWithChanges();

                cancellation.ThrowIfCancellationRequested();

                if (syntaxNode != null)
                {
                    if (!NodeFactory.IsSupportedContainer(syntaxNode, out _))
                    {
                        var symbol = await SymbolFinder.FindSymbolAtPositionAsync(document, caretPosition);
                        if (symbol != null)
                        {
                            var node = symbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax();
                            if (node != null)
                            {
                                syntaxNode = node;
                            }
                        }
                    }

                    if (!NodeFactory.IsSupportedContainer(syntaxNode, out _))
                    {
                        var parentableType = syntaxNode.AncestorsAndSelf().FirstOrDefault(x => ParentedTypes.Contains(x.GetType()));
                        if (parentableType != null && NodeFactory.IsSupportedContainer(parentableType.Parent, out _))
                        {
                            syntaxNode = parentableType.Parent;
                        }
                    }

                    if (NodeFactory.IsSupportedContainer(syntaxNode, out var actualNode))
                    {
                        ISymbol declaredSymbol = null;
                        if (semanticModel.SyntaxTree == actualNode.SyntaxTree)
                        {
                            declaredSymbol = semanticModel.GetDeclaredSymbol(actualNode);
                        }
                        else
                        {
                            semanticModel = await package.Workspace.CurrentSolution.GetDocument(actualNode.SyntaxTree).GetSemanticModelAsync();
                            declaredSymbol = semanticModel.GetDeclaredSymbol(actualNode);
                        }

                        if (declaredSymbol != null)
                        {
                            var searchingSymbol = new SyntaxNodeWithSymbol(declaredSymbol, actualNode, semanticModel);
                            return await SymbolProcessor.FindReferencesAsync(updateText, searchingSymbol, declaredSymbol, document, cancellation);
                        }
                        else
                        {
                            throw new InvalidOperationException("Could not find a declared symbol at the caret position.");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("SyntaxNode of type " + syntaxNode.GetType().Name + " are not supported.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Could not find a syntax node at the caret position.");
                }
            }

            _host.ViewModel.RunAction(FindReferencesAsync, SymbolProcessor.ProcessFoundReferences);
        }
    }
}
