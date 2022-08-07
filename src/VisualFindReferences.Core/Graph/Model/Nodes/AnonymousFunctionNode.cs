namespace VisualFindReferences.Core.Graph.Model.Nodes
{
    public class AnonymousFunctionNode : VariableContainedFunctionNode
    {
        public AnonymousFunctionNode(NodeGraph flowChart, FoundReferences foundReferences) : base(flowChart, foundReferences)
        { }

        public override string NodeSymbolType => "Anonymous Function";
    }
}