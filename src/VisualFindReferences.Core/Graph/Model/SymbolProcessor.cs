using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualFindReferences.Core.Graph.Helper;
using VisualFindReferences.Core.Graph.Model.Nodes;

namespace VisualFindReferences.Core.Graph.Model
{
    public class SymbolProcessor
    {
        public static Task<FoundReferences> FindReferencesAsync(Action<string> updateText, SyntaxNodeWithSymbol searchingNode, ISymbol targetSymbol, Solution solution)
        {
            return FindReferencesAsync(updateText, searchingNode, new[] { targetSymbol }, solution);
        }

        public static async Task<FoundReferences> FindReferencesAsync(Action<string> updateText, SyntaxNodeWithSymbol searchingNode, IEnumerable<ISymbol> targetSymbols, Solution solution)
        {
            var outputDictionary = new Dictionary<ISymbol, ReferencingSymbol>(SymbolEqualityComparer.Default);

            foreach (var targetSymbol in targetSymbols)
            {
                updateText("Finding references...");
                var list = await SymbolFinder.FindReferencesAsync(targetSymbol, solution).ConfigureAwait(true);

                updateText("Processing references...");
                foreach (var item in list)
                {
                    // for each referencing location
                    foreach (var location in item.Locations)
                    {
                        // get the text, syntax node and semantic model
                        var text = await location.Document.GetTextAsync();
                        var syntaxNode = await location.Document.GetSyntaxRootAsync().ConfigureAwait(true);
                        var semanticModel = await location.Document.GetSemanticModelAsync().ConfigureAwait(true);

                        if (syntaxNode != null && semanticModel != null)
                        {
                            // now walk up the syntax tree until we find a node that NodeFactory supports
                            var current = syntaxNode.FindToken(location.Location.SourceSpan.Start).Parent;

                            while (current != null)
                            {
                                if (NodeFactory.IsSupportedContainer(current))
                                {
                                    var containerSymbol = semanticModel.GetDeclaredSymbol(current);
                                    if (containerSymbol != null)
                                    {
                                        if (!outputDictionary.TryGetValue(containerSymbol, out var referencingSymbol))
                                        {
                                            outputDictionary[containerSymbol] = referencingSymbol = new ReferencingSymbol(containerSymbol, current, semanticModel, new List<ReferencingLocation>());
                                        }

                                        referencingSymbol.ReferencingLocations.Add(new ReferencingLocation(location, text));
                                    }
                                    break;
                                }

                                current = current.Parent;
                            }
                        }
                    }
                }
            }

            return new FoundReferences(searchingNode.Symbol, searchingNode.SyntaxNode, searchingNode.SemanticModel, solution, outputDictionary.Values.ToList());
        }

        public static void ProcessFoundReferences(FoundReferences references, NodeGraph model)
        {
            var vfrModel = model as VFRNodeGraph;
            if (vfrModel == null)
            {
                return;
            }

            // reset ReferenceLocationsAdded flags
            vfrModel.Nodes.OfType<VFRNode>().Each(x => x.ReferenceLocationsAdded = false);

            // if the target doesn't exist, create it
            if (!vfrModel.GetNodeFor(references.Symbol, out var targetNode))
            {
                targetNode = NodeFactory.Create(references.SyntaxNode, vfrModel, references);
                if (targetNode == null)
                {
                    return;
                }

                if (vfrModel.Nodes.Count == 0)
                {
                    targetNode.IsRoot = true;
                }

                vfrModel.Nodes.Add(targetNode);
            }
            targetNode.ReferenceSearchAvailable = false;

            if (references.ReferencingSymbols != null)
            {
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
                    // either create a new node or add a referencing location to an existing node
                    if (!vfrModel.GetNodeFor(referencingSymbol.Symbol, out var referencingNode))
                    {
                        var referencingNodeReferences = new FoundReferences(referencingSymbol.Symbol, referencingSymbol.SyntaxNode, referencingSymbol.SemanticModel, references.Solution, referencingSymbol.ReferencingLocations);
                        referencingNode = NodeFactory.Create(referencingSymbol.SyntaxNode, vfrModel, referencingNodeReferences);
                        if (referencingNode != null)
                        {
                            vfrModel.Nodes.Add(referencingNode);
                        }
                    }
                    else
                    {
                        referencingSymbol.ReferencingLocations.Each(referencingNode.NodeFoundReferences.ReferencingLocations.Add);
                        referencingNode.ReferenceLocationsAdded = referencingSymbol.ReferencingLocations.Count > 0;
                    }

                    // create the link, avoiding duplicates
                    if (referencingNode != null)
                    {
                        var hasExistingLink = connectorMap.TryGetValue(referencingNode, out var set) && set.Contains(targetNode);
                        if (!hasExistingLink)
                        {
                            vfrModel.Connectors.Add(new Connector(vfrModel, referencingNode, targetNode));
                        }
                    }
                }
            }
        }
    }
}
