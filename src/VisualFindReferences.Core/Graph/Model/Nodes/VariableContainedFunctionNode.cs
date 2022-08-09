using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace VisualFindReferences.Core.Graph.Model.Nodes
{
    public abstract class VariableContainedFunctionNode : VFRNode
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M6,20L10.16,7.91L9.34,6H8V4H10C10.42,4 10.78,4.26 10.93,4.63L16.66,18H18V20H16C15.57,20 15.21,19.73 15.07,19.36L11.33,10.65L8.12,20H6Z");

        protected VariableContainedFunctionNode(NodeGraph flowChart, FoundReferences foundReferences) : base(flowChart, foundReferences, DefaultIcon, Brushes.MediumOrchid)
        { }

        public override IEnumerable<SearchableSymbol> GetSearchableSymbols()
        {
            var variableDeclarator = NodeFoundReferences.SyntaxNode.AncestorsAndSelf().OfType<VariableDeclaratorSyntax>().First();
            var targetSymbol = NodeFoundReferences.SemanticModel.GetDeclaredSymbol(variableDeclarator);
            if (targetSymbol != null)
            {
                yield return new SearchableSymbol(NodeFoundReferences, new[] { targetSymbol }, NodeFoundReferences.Solution, "variable " + variableDeclarator.Identifier.Text);
            }
        }
    }
}