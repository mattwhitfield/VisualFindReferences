using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualFindReferences.Core.Graph.Model
{
    public class SyntaxNodeWithSymbol
    {
        public SyntaxNodeWithSymbol(ISymbol symbol, SyntaxNode syntaxNode, SemanticModel semanticModel)
        {
            Symbol = symbol;
            SyntaxNode = syntaxNode;
            SemanticModel = semanticModel;
        }

        public ISymbol Symbol { get; }

        public SyntaxNode SyntaxNode { get; }

        public SemanticModel SemanticModel { get; }
    }

    public class FoundReferences : SyntaxNodeWithSymbol
    {
        public FoundReferences(ISymbol symbol, SyntaxNode syntaxNode, SemanticModel semanticModel, IList<ReferencingSymbol> referencingSymbols)
            : base(symbol, syntaxNode, semanticModel)
        {
            ReferencingSymbols = referencingSymbols;
            ReferencingLocations = null;
            IsTarget = true;
        }
        public FoundReferences(ISymbol symbol, SyntaxNode syntaxNode, SemanticModel semanticModel, IList<ReferencingLocation> referencingLocations)
            : base(symbol, syntaxNode, semanticModel)
        {
            ReferencingLocations = referencingLocations;
            ReferencingSymbols = null;
            IsSource = true;
        }

        public bool IsSource { get; }

        public bool IsTarget { get; }

        public IList<ReferencingSymbol>? ReferencingSymbols { get; }

        public IList<ReferencingLocation>? ReferencingLocations { get; }
    }

    public class ReferencingSymbol : SyntaxNodeWithSymbol
    {
        public ReferencingSymbol(ISymbol symbol, SyntaxNode syntaxNode, SemanticModel semanticModel, IList<ReferencingLocation> referencingLocations)
            : base(symbol, syntaxNode, semanticModel)
        {
            ReferencingLocations = referencingLocations;
        }

        public IList<ReferencingLocation> ReferencingLocations { get; }
    }

    public class ReferencingLocation
    {
        public ReferencingLocation(ReferenceLocation location, SourceText referencingSourceText)
        {
            Location = location;
            ReferencingSourceText = referencingSourceText;
        }

        public ReferenceLocation Location { get; }

        public SourceText ReferencingSourceText { get; }
    }
}
