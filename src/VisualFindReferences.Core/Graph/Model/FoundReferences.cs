﻿using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace VisualFindReferences.Core.Graph.Model
{

    public class FoundReferences : SyntaxNodeWithSymbol
    {
        public FoundReferences(ISymbol symbol, SyntaxNode syntaxNode, SemanticModel semanticModel, Solution solution, IList<ReferencingSymbol> referencingSymbols, Document? sourceDocument)
            : base(symbol, syntaxNode, semanticModel)
        {
            ReferencingSymbols = referencingSymbols;
            SourceDocument = sourceDocument;
            ReferencingLocations = new List<ReferencingLocation>();
            Solution = solution;
        }
        public FoundReferences(ISymbol symbol, SyntaxNode syntaxNode, SemanticModel semanticModel, Solution solution, IList<ReferencingLocation> referencingLocations)
            : base(symbol, syntaxNode, semanticModel)
        {
            ReferencingLocations = referencingLocations;
            ReferencingSymbols = new List<ReferencingSymbol>();
            Solution = solution;
        }

        public IList<ReferencingSymbol> ReferencingSymbols { get; }

        public Document? SourceDocument { get; }

        public IList<ReferencingLocation> ReferencingLocations { get; }

        public Solution Solution { get; }
    }
}
