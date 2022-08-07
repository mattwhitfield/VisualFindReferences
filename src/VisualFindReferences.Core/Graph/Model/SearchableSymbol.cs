using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace VisualFindReferences.Core.Graph.Model
{
    public class SearchableSymbol
    {
        public SearchableSymbol(SyntaxNodeWithSymbol searchingSymbol, IList<ISymbol> targets, Solution solution, string name)
        {
            SearchingSymbol = searchingSymbol;
            Targets = targets;
            Solution = solution;
            Name = name;
        }

        public SyntaxNodeWithSymbol SearchingSymbol { get; }

        public IList<ISymbol> Targets { get; }

        public Solution Solution { get; }

        public string Name { get; }
    }
}
