namespace VisualFindReferences.Core.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;
    using Microsoft.CodeAnalysis;
    using VisualFindReferences.Core.Graph.Model;

    internal static class ClassModelProvider
    {
        private static readonly object Lock = new object();

        public static SemanticModel SemanticModel
        {
            get
            {
                return null;
            }
        }

        public static SyntaxNode SyntaxNode
        {
            get
            {
                return null;
            }
        }

        public static Solution Solution
        {
            get
            {
                return null;
            }
        }

        public static Document Document
        {
            get
            {
                return null;
            }
        }

        //public static ClassModel Instance
        //{
        //    get
        //    {
        //        lock (Lock)
        //        {
        //            return _instance ?? (_instance = CreateModel());
        //        }
        //    }
        //}

        //public static ClassModel GenericInstance
        //{
        //    get
        //    {
        //        lock (Lock)
        //        {
        //            return _genericInstance ?? (_genericInstance = CreateGenericModel());
        //        }
        //    }
        //}

        //private static ClassModel CreateModel()
        //{
        //    return CreateModel(TestClasses.PocoInitialization);
        //}

        //private static ClassModel CreateGenericModel()
        //{
        //    return CreateModel(TestClasses.TypeGenericDisambiguation);
        //}

        //public static ClassModel CreateModel(string classAsText)
        //{
        //    var tree = CSharpSyntaxTree.ParseText(classAsText, new CSharpParseOptions(LanguageVersion.Latest));

        //    var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
        //    var references = new List<MetadataReference>
        //    {
        //        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
        //        MetadataReference.CreateFromFile(typeof(Stream).Assembly.Location),
        //        MetadataReference.CreateFromFile(typeof(System.Drawing.Brushes).Assembly.Location),
        //        MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")),
        //        MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")),
        //    };

        //    var compilation = CSharpCompilation.Create(
        //        "MyTest",
        //        syntaxTrees: new[] { tree },
        //        references: references);

        //    var semanticModel = compilation.GetSemanticModel(tree);

        //    var model = new TestableItemExtractor(semanticModel.SyntaxTree, semanticModel);
        //    return model.Extract(null, Substitute.For<IUnitTestGeneratorOptions>()).First();
        //}

        //public static T GetNode<T>(this ClassModel model)
        //{
        //    return model.Declaration.SyntaxTree.GetRoot().DescendantNodes().OfType<T>().First();
        //}

        public static NodeGraph TestGraph
        {
            get
            {
                var graph = new NodeGraph();
                var node1 = new Node(graph, "TestValue1621437443", "TestValue1439008545", 423196358.31, 190494652.59, Geometry.Empty, Brushes.White);
                var node2 = new Node(graph, "TestValue1621437443", "TestValue1439008545", 423196358.31, 190494652.59, Geometry.Empty, Brushes.White);
                var node3 = new Node(graph, "TestValue1621437443", "TestValue1439008545", 423196358.31, 190494652.59, Geometry.Empty, Brushes.White);
                var node4 = new Node(graph, "TestValue1621437443", "TestValue1439008545", 423196358.31, 190494652.59, Geometry.Empty, Brushes.White);
                var node5 = new Node(graph, "TestValue1621437443", "TestValue1439008545", 423196358.31, 190494652.59, Geometry.Empty, Brushes.White);

                graph.Nodes.Add(node1);
                graph.Nodes.Add(node2);
                graph.Nodes.Add(node3);
                graph.Nodes.Add(node4);
                graph.Nodes.Add(node5);

                graph.Connectors.Add(new Connector(graph, node5, node3));
                graph.Connectors.Add(new Connector(graph, node4, node3));
                graph.Connectors.Add(new Connector(graph, node3, node1));
                graph.Connectors.Add(new Connector(graph, node2, node1));

                return graph;
            }
        }

        public static void Consume<T>(this IEnumerable<T> enumerable)
        {
            enumerable.ToList().ForEach(_ => { });
        }
    }
}
