namespace VisualFindReferences.Core.Graph.Model.Nodes
{
    public class LambdaNode : VariableContainedFunctionNode
    {
        public LambdaNode(NodeGraph flowChart, FoundReferences foundReferences) : base(flowChart, foundReferences)
        { }

        public override string NodeSymbolType => "Lambda Expression";
    }
}