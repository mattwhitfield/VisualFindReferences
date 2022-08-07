using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace VisualFindReferences.Core.Graph.Model
{
    public class ReferencingSymbol : SyntaxNodeWithSymbol
    {
        public ReferencingSymbol(ISymbol symbol, SyntaxNode syntaxNode, SemanticModel semanticModel, IList<ReferencingLocation> referencingLocations)
            : base(symbol, syntaxNode, semanticModel)
        {
            ReferencingLocations = referencingLocations;
        }

        public IList<ReferencingLocation> ReferencingLocations { get; }
    }
}
