using Microsoft.CodeAnalysis;

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
}
