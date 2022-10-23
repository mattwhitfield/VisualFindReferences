using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisualFindReferences.Core.Graph.Layout;
using VisualFindReferences.Core.Graph.Model.Nodes;
using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.Model
{
    public class SymbolProcessor
    {
        public static Task<FoundReferences> FindReferencesAsync(Action<string> updateText, SyntaxNodeWithSymbol searchingNode, ISymbol targetSymbol, Document document, CancellationToken cancellation)
        {
            return FindReferencesAsync(updateText, searchingNode, new[] { targetSymbol }, document.Project.Solution, document, cancellation);
        }

        public static async Task<FoundReferences> FindReferencesAsync(Action<string> updateText, SyntaxNodeWithSymbol searchingNode, IEnumerable<ISymbol> targetSymbols, Solution solution, Document document, CancellationToken cancellation)
        {
            var outputDictionary = new Dictionary<ISymbol, ReferencingSymbol>(SymbolEqualityComparer.Default);

            foreach (var targetSymbol in targetSymbols)
            {
                updateText("Finding references...");
                var list = await SymbolFinder.FindReferencesAsync(targetSymbol, solution, cancellation).ConfigureAwait(true);

                cancellation.ThrowIfCancellationRequested();

                updateText("Processing references...");
                foreach (var item in list)
                {
                    // for each referencing location
                    foreach (var location in item.Locations)
                    {
                        await ProcessLocation(outputDictionary, location, cancellation);
                    }
                }
            }

            return new FoundReferences(searchingNode.Symbol, searchingNode.SyntaxNode, searchingNode.SemanticModel, solution, outputDictionary.Values.ToList(), document);
        }

        private static async Task ProcessLocation(Dictionary<ISymbol, ReferencingSymbol> outputDictionary, ReferenceLocation location, CancellationToken cancellation)
        {
            // get the text, syntax node and semantic model
            var text = await location.Document.GetTextAsync(cancellation);
            var syntaxNode = await location.Document.GetSyntaxRootAsync(cancellation).ConfigureAwait(true);
            var semanticModel = await location.Document.GetSemanticModelAsync(cancellation).ConfigureAwait(true);

            cancellation.ThrowIfCancellationRequested();

            if (syntaxNode != null && semanticModel != null)
            {
                WalkTree(outputDictionary, location, text, syntaxNode, semanticModel);
            }
        }

        private static void WalkTree(Dictionary<ISymbol, ReferencingSymbol> outputDictionary, ReferenceLocation location, Microsoft.CodeAnalysis.Text.SourceText text, SyntaxNode syntaxNode, SemanticModel semanticModel)
        {
            // now walk up the syntax tree until we find a node that NodeFactory supports
            var current = syntaxNode.FindToken(location.Location.SourceSpan.Start).Parent;

            while (current != null)
            {
                if (NodeFactory.IsSupportedContainer(current, out var actualNode))
                {
                    AddNode(outputDictionary, location, text, semanticModel, current, actualNode);
                    break;
                }

                current = current.Parent;
            }
        }

        private static void AddNode(Dictionary<ISymbol, ReferencingSymbol> outputDictionary, ReferenceLocation location, Microsoft.CodeAnalysis.Text.SourceText text, SemanticModel semanticModel, SyntaxNode current, SyntaxNode actualNode)
        {
            var containerSymbol = semanticModel.GetDeclaredSymbol(actualNode);
            if (containerSymbol != null)
            {
                if (!outputDictionary.TryGetValue(containerSymbol, out var referencingSymbol))
                {
                    outputDictionary[containerSymbol] = referencingSymbol = new ReferencingSymbol(containerSymbol, current, semanticModel, new List<ReferencingLocation>());
                }

                referencingSymbol.ReferencingLocations.Add(new ReferencingLocation(location, text));
            }
        }

        public static void ProcessFoundReferences(FoundReferences references, NodeGraph model)
        {
            var vfrModel = model as VFRNodeGraph;
            var viewModel = vfrModel?.ViewModel as VFRNodeGraphViewModel;
            if (vfrModel == null || viewModel == null)
            {
                return;
            }

            // reset ReferenceLocationsAdded flags
            viewModel.FilteredReferencesMessage = string.Empty;

            List<Node> nodesToLayOut = new List<Node>();

            var isInitialLayout = model.Nodes.Count == 0;

            // if the target doesn't exist, create it
            if (!vfrModel.GetNodeFor(references.Symbol, out var targetNode))
            {
                targetNode = NodeFactory.Create(references.SyntaxNode, vfrModel, references);
                if (targetNode == null)
                {
                    return;
                }

                nodesToLayOut.Add(targetNode);

                if (vfrModel.Nodes.Count == 0)
                {
                    targetNode.IsRoot = true;
                }

                targetNode.SourceDocument = references.SourceDocument;
                vfrModel.Nodes.Add(targetNode);
            }
            targetNode.SearchedSymbols.Add(references.Symbol);
            targetNode.NoMoreReferences = false;

            var filteredReferenceCount = 0;

            bool anyAdded = false;

            HandleReferencingSymbols(references, vfrModel, viewModel, nodesToLayOut, targetNode, ref filteredReferenceCount, ref anyAdded);

            if (!anyAdded)
            {
                targetNode.NoMoreReferences = true;
            }

            if (filteredReferenceCount > 0)
            {
                viewModel.FilteredReferencesMessage = anyAdded ? "References filtered: " + filteredReferenceCount : "All references (" + filteredReferenceCount + ") were filtered";
            }

            AnimateAdditions(vfrModel, viewModel, nodesToLayOut, isInitialLayout);
        }

        private static void HandleReferencingSymbols(FoundReferences references, VFRNodeGraph vfrModel, VFRNodeGraphViewModel viewModel, List<Node> nodesToLayOut, VFRNode targetNode, ref int filteredReferenceCount, ref bool anyAdded)
        {
            if (references.ReferencingSymbols != null)
            {
                Func<Project, bool> projectIsIncluded = viewModel.GetProjectFilter();

                // build map of connectors to avoid duplicating connectors
                Dictionary<Node, HashSet<Node>> connectorMap = new Dictionary<Node, HashSet<Node>>();
                foreach (var connector in vfrModel.Connectors)
                {
                    if (!connectorMap.TryGetValue(connector.StartNode, out var set))
                    {
                        connectorMap[connector.StartNode] = set = new HashSet<Node>();
                    }
                    set.Add(connector.EndNode);
                }

                foreach (var referencingSymbol in references.ReferencingSymbols)
                {
                    var referencingLocationsInAllowedProjects = referencingSymbol.ReferencingLocations.Where(x => projectIsIncluded(x.Location.Document.Project)).ToList();
                    filteredReferenceCount += referencingSymbol.ReferencingLocations.Count - referencingLocationsInAllowedProjects.Count;
                    if (referencingLocationsInAllowedProjects.Count == 0)
                    {
                        continue;
                    }

                    VFRNode? referencingNode = CreateReferencingNode(references, vfrModel, nodesToLayOut, targetNode, referencingSymbol, referencingLocationsInAllowedProjects);

                    // create the link, avoiding duplicates
                    if (referencingNode != null)
                    {
                        var hasExistingLink = connectorMap.TryGetValue(referencingNode, out var set) && set.Contains(targetNode);
                        if (!hasExistingLink)
                        {
                            vfrModel.Connectors.Add(new Connector(vfrModel, referencingNode, targetNode));
                            anyAdded = true;
                        }
                    }
                }
            }
        }

        private static VFRNode? CreateReferencingNode(FoundReferences references, VFRNodeGraph vfrModel, List<Node> nodesToLayOut, VFRNode targetNode, ReferencingSymbol referencingSymbol, List<ReferencingLocation> referencingLocationsInAllowedProjects)
        {
            // either create a new node or add a referencing location to an existing node
            if (!vfrModel.GetNodeFor(referencingSymbol.Symbol, out var referencingNode))
            {
                var referencingNodeReferences = new FoundReferences(referencingSymbol.Symbol, referencingSymbol.SyntaxNode, referencingSymbol.SemanticModel, references.Solution, referencingLocationsInAllowedProjects);
                referencingNode = NodeFactory.Create(referencingSymbol.SyntaxNode, vfrModel, referencingNodeReferences);
                if (referencingNode != null)
                {
                    referencingNode.X = targetNode.X;
                    referencingNode.Y = targetNode.Y;
                    vfrModel.Nodes.Add(referencingNode);

                    nodesToLayOut.Add(referencingNode);
                }
            }
            else
            {
                referencingNode.ReferenceLocationsAdded = false;
                foreach (var reference in referencingLocationsInAllowedProjects)
                {
                    if (referencingNode.NodeFoundReferences.ReferencingLocations.Any(existing => existing.Location == reference.Location && existing.LinePrompt == reference.LinePrompt))
                    {
                        continue;
                    }
                    referencingNode.NodeFoundReferences.ReferencingLocations.Add(reference);
                    referencingNode.ReferenceLocationsAdded = true;
                }
            }

            return referencingNode;
        }

        private static void AnimateAdditions(VFRNodeGraph vfrModel, VFRNodeGraphViewModel viewModel, List<Node> nodesToLayOut, bool isInitialLayout)
        {
            if (viewModel.View != null)
            {
                viewModel.View.Dispatcher.BeginInvoke(new Action(() =>
                {
                    viewModel.View.UpdateLayout();

                    var nodesForAlgorithm = new Dictionary<Node, GraphPoint>();
                    foreach (var node in nodesToLayOut)
                    {
                        nodesForAlgorithm[node] = new GraphPoint(node.X, node.Y);
                    }

                    var positions = vfrModel.GetLayoutPositions(viewModel.LayoutType, nodesForAlgorithm);

                    var proposedZoomAndPan = viewModel.View.ZoomAndPan;

                    if (isInitialLayout || viewModel.AutoFitToDisplay)
                    {
                        vfrModel.CalculateContentSize(positions, false, out var rect);
                        proposedZoomAndPan = viewModel.View.ZoomAndPan.GetTarget(rect);
                    }

                    viewModel.View.StartAnimation(positions, proposedZoomAndPan.Scale, proposedZoomAndPan.StartX, proposedZoomAndPan.StartY);
                }));
            }
        }
    }
}
