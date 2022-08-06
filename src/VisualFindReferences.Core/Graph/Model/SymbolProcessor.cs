using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualFindReferences.Core.Graph.Model.Nodes;

namespace VisualFindReferences.Core.Graph.Model
{
    public class SymbolProcessor
    {
        public static async Task<FoundReferences> FindReferencesAsync(Action<string> updateText, ISymbol targetSymbol, SyntaxNode targetSyntaxNode, SemanticModel targetSemanticModel, Document document)
        {
            var list = await SymbolFinder.FindReferencesAsync(targetSymbol, document.Project.Solution).ConfigureAwait(true);
            var outputDictionary = new Dictionary<ISymbol, ReferencingSymbol>(SymbolEqualityComparer.Default);
            updateText("Loading references...");
            foreach (var item in list)
            {
                foreach (var location in item.Locations)
                {
                    var text = await location.Document.GetTextAsync();
                    var syntaxNode = await location.Document.GetSyntaxRootAsync().ConfigureAwait(true);
                    var semanticModel = await location.Document.GetSemanticModelAsync().ConfigureAwait(true);

                    if (syntaxNode != null && semanticModel != null)
                    {
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

            return new FoundReferences(targetSymbol, targetSyntaxNode, targetSemanticModel, outputDictionary.Values.ToList());
        }
    }
}
