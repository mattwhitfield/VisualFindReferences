namespace VisualFindReferences.Core.Graph.Model.Nodes
{
    public class AnonymousMethodNode : VariableContainedFunctionNode
    {
        public AnonymousMethodNode(NodeGraph flowChart, FoundReferences foundReferences) : base(flowChart, foundReferences)
        { }

        public override string NodeSymbolType => "Anonymous Method";
    }
}