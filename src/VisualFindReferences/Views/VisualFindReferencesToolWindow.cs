using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using VisualFindReferences.Core.Graph.Helper;
using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.Model.Nodes;
using VisualFindReferences.Core.Graph.ViewModel;
using VisualFindReferences.Helper;

namespace VisualFindReferences.Views
{
    [Guid("f27b912a-9ce0-4e68-a8db-cdd2516793cd")]
    internal class VisualFindReferencesToolWindow : ToolWindowPane, IVisualFindReferencesToolWindow
    {
        private readonly VisualFindReferencesHost _host;

        public VisualFindReferencesToolWindow() :
            base(null)
        {
            Caption = "Visual Find References";

            base.Content = _host = new VisualFindReferencesHost();
        }

        public void FindReferences(IWpfTextView textView, IVisualFindReferencesPackage package)
        {
            async Task<FoundReferences> FindReferencesAsync(Action<string> updateText, NodeGraphViewModel viewModel)
            {
                var (syntaxNode, semanticModel) = await TextViewHelper.GetTargetSymbolAsync(textView).ConfigureAwait(true);
                var caretPosition = textView.Caret.Position.BufferPosition;
                var document = caretPosition.Snapshot.GetOpenDocumentInCurrentContextWithChanges();

                if (syntaxNode != null)
                {
                    updateText("Locating references...");
                    var declaredSymbol = semanticModel.GetDeclaredSymbol(syntaxNode);
                    if (declaredSymbol != null)
                    {
                        return await SymbolProcessor.FindReferencesAsync(updateText, declaredSymbol, syntaxNode, semanticModel, document);
                    }
                    else
                    {
                        throw new InvalidOperationException("Could not find a declared symbol at the caret position.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Could not find a syntax node at the caret position.");
                }
            }

            void ProcessFoundReferences(FoundReferences references)
            {
                _host.Model.Nodes.OfType<VFRNode>().Each(x => x.ReferenceLocationsAdded = false);

                if (!_host.Model.GetNodeFor(references.Symbol, out var targetNode))
                {
                    targetNode = NodeFactory.Create(references.SyntaxNode, _host.Model, references);
                    _host.Model.Nodes.Add(targetNode);
                }
                targetNode.ReferenceSearchAvailable = false;

                if (references.ReferencingSymbols != null)
                {
                    foreach (var referencingSymbol in references.ReferencingSymbols)
                    {
                        if (!_host.Model.GetNodeFor(referencingSymbol.Symbol, out var referencingNode))
                        {
                            var referencingNodeReferences = new FoundReferences(referencingSymbol.Symbol, referencingSymbol.SyntaxNode, referencingSymbol.SemanticModel, referencingSymbol.ReferencingLocations);
                            referencingNode = NodeFactory.Create(referencingSymbol.SyntaxNode, _host.Model, referencingNodeReferences);
                            _host.Model.Nodes.Add(referencingNode);
                        }
                        else
                        {
                            referencingSymbol.ReferencingLocations.Each(referencingNode.NodeFoundReferences.ReferencingLocations.Add);
                            referencingNode.ReferenceLocationsAdded = referencingSymbol.ReferencingLocations.Count > 0;
                        }

                        _host.Model.Connectors.Add(new Connector(_host.Model, referencingNode, targetNode));
                    }
                }
            }

            _host.ViewModel.RunAction(FindReferencesAsync, ProcessFoundReferences);
        }
    }
}
