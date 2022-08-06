using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VisualFindReferences.Core.Graph.Layout;
using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.TestHarness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NodeGraphViewModel NodeGraphViewModel
        {
            get { return (NodeGraphViewModel)GetValue(NodeGraphViewModelProperty); }
            set { SetValue(NodeGraphViewModelProperty, value); }
        }

        public static readonly DependencyProperty NodeGraphViewModelProperty =
            DependencyProperty.Register("NodeGraphViewModel", typeof(NodeGraphViewModel), typeof(MainWindow), new PropertyMetadata(null));

        private Node _contextMenuNode;

        private Random r = new Random();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            PreviewKeyUp += MainWindow_PreviewKeyUp;
        }

        private void MainWindow_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.P)
            {
                NodeGraphViewModel.RunAction<Node>(async (setBusyText, viewModel) =>
                {
                    await Task.Delay(1500);
                    setBusyText("Still going...");
                    await Task.Delay(500);
                    var existing = viewModel.NodeViewModels.FirstOrDefault(x => x.IsSelected)?.Model;
                    if (existing == null)
                    {
                        existing = viewModel.NodeViewModels[r.Next(viewModel.NodeViewModels.Count)].Model;
                    }
                    return existing;
                },
                node =>
                {
                    var nodeGraph = NodeGraphViewModel.Model;

                    var n1 = CreateNode(nodeGraph, node.X, node.Y);
                    var n2 = CreateNode(nodeGraph, node.X, node.Y);

                    nodeGraph.Nodes.Add(n1);
                    nodeGraph.Nodes.Add(n2);

                    var c1 = new Connector(nodeGraph, node, n1);
                    var c2 = new Connector(nodeGraph, node, n2);

                    nodeGraph.Connectors.Add(c1);
                    nodeGraph.Connectors.Add(c2);

                    var view = NodeGraphViewModel.View;
                    var nodes = new List<Tuple<Node, double, double>>();
                    nodes.Add(Tuple.Create(n1, n1.X + 200, n1.Y - 50));
                    nodes.Add(Tuple.Create(n2, n2.X + 200, n2.Y + 50));
                    NodeGraphViewModel.View.StartAnimation(nodes, view.ZoomAndPan.Scale + (r.NextDouble() - 0.5), view.ZoomAndPan.StartX + ((r.NextDouble() - 0.5) * 20), view.ZoomAndPan.StartY + ((r.NextDouble() - 0.5) * 20));
                });
            }

            if (e.Key == System.Windows.Input.Key.L)
            {
                var dictionary = new Dictionary<Node, Core.Graph.Layout.Point>();
                foreach(var node in NodeGraphViewModel.Model.Nodes)
                {
                    dictionary[node] = new Core.Graph.Layout.Point(node.X, node.Y);
                }
                //var algo = new FRLayoutAlgorithm(NodeGraphViewModel.Model, dictionary);
                //var algo = new HorizontalBalancedGridLayoutAlgorithm(NodeGraphViewModel.Model, dictionary);
                var algo = new VerticalBalancedGridLayoutAlgorithm(NodeGraphViewModel.Model, dictionary);
                algo.Layout();

                var fsaDictionary = new Dictionary<Node, Core.Graph.Layout.Rect>();
                foreach (var pair in dictionary)
                {
                    fsaDictionary[pair.Key] = new Core.Graph.Layout.Rect(pair.Value.X, pair.Value.Y, pair.Key.ViewModel.View.ActualWidth, pair.Key.ViewModel.View.ActualHeight);
                }

                var overlapRemoval = new FSAAlgorithm(fsaDictionary);
                overlapRemoval.InternalCompute();

                //foreach (var pair in fsaDictionary)
                //{
                //    pair.Key.X = pair.Value.X;
                //    pair.Key.Y = pair.Value.Y;
                //}

                var view = NodeGraphViewModel.View;
                var nodes = new List<Tuple<Node, double, double>>();

                foreach (var pair in fsaDictionary)
                {
                    nodes.Add(Tuple.Create(pair.Key, pair.Value.X, pair.Value.Y));
                }
                NodeGraphViewModel.View.StartAnimation(nodes, view.ZoomAndPan.Scale, view.ZoomAndPan.StartX, view.ZoomAndPan.StartY);
            }
        }

        private Node CreateNode(NodeGraph nodeGraph, double x, double y, string text = "")
        {
            Node GetNode()
            {
                switch (r.Next(10))
                {
                    case 0: return new MethodNode(nodeGraph, text + "Method", "MyProgram.Stuff.MethodType", x, y);
                    case 1: return new LocalMethodNode(nodeGraph, text + "LocalMethod", "MyProgram.Stuff.LocalMethodType", x, y);
                    case 2: return new PropertyNode(nodeGraph, text + "Property", "MyProgram.Stuff.PropertyType", x, y);
                    case 3: return new LambdaNode(nodeGraph, text + "Lambda", "MyProgram.Stuff.LambdaNode", x, y);
                    case 4: return new OperatorNode(nodeGraph, text + "Operator", "MyProgram.Stuff.OperatorType", x, y);
                    case 5: return new EventNode(nodeGraph, text + "Event", "MyProgram.Stuff.EventType", x, y);
                    case 6: return new IndexerNode(nodeGraph, text + "Indexer", "MyProgram.Stuff.IndexerType", x, y);
                    case 7: return new ConstructorNode(nodeGraph, text + "Constructor", "MyProgram.Stuff.ConstructorType", x, y);
                    case 8: return new DestructorNode(nodeGraph, text + "Destructor", "MyProgram.Stuff.DestructorType", x, y);
                    default: return new FieldInitializerNode(nodeGraph, text + "FieldInitializer", "MyProgram.Stuff.FieldInitializerType", x, y);
                }
            }
            var node = GetNode();
            nodeGraph.Nodes.Add(node);
            return node;
        }

        private Node CreateNode(NodeGraph nodeGraph, int num)
        {
            var x = r.NextDouble() * 500;
            var y = r.NextDouble() * 500;

            return CreateNode(nodeGraph, x, y, num + ": ");
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            NodeGraph nodeGraph = new NodeGraph();
            NodeGraphViewModel = nodeGraph.ViewModel;

            bool includeBack = true;

            var node1 = CreateNode(nodeGraph, 1);
            node1.IsRoot = true;

            var node2 = CreateNode(nodeGraph, 2);
            var node3 = CreateNode(nodeGraph, 3);
            var node4 = CreateNode(nodeGraph, 4);
            var node5 = CreateNode(nodeGraph, 5);
            var node6 = CreateNode(nodeGraph, 6);
            var node7 = CreateNode(nodeGraph, 7);
            var node8 = CreateNode(nodeGraph, 8);
            var node9 = CreateNode(nodeGraph, 9);
            var node10 = CreateNode(nodeGraph, 10);

            node6.ViewModel.IsHighlighted = true;

            nodeGraph.Connectors.Add(new Connector(nodeGraph, node1, node5));
            nodeGraph.Connectors.Add(new Connector(nodeGraph, node1, node7));
            nodeGraph.Connectors.Add(new Connector(nodeGraph, node7, node4));
            nodeGraph.Connectors.Add(new Connector(nodeGraph, node7, node6));
            nodeGraph.Connectors.Add(new Connector(nodeGraph, node6, node3));
            nodeGraph.Connectors.Add(new Connector(nodeGraph, node5, node2));
            //nodeGraph.Connectors.Add(new Connector(nodeGraph, node4, node5));
            nodeGraph.Connectors.Add(new Connector(nodeGraph, node2, node3));
            nodeGraph.Connectors.Add(new Connector(nodeGraph, node5, node8));
            nodeGraph.Connectors.Add(new Connector(nodeGraph, node2, node9));
            nodeGraph.Connectors.Add(new Connector(nodeGraph, node2, node10));

            if (includeBack)
            {
                var node11 = CreateNode(nodeGraph, 11);
                var node12 = CreateNode(nodeGraph, 12);
                nodeGraph.Connectors.Add(new Connector(nodeGraph, node11, node2));
                nodeGraph.Connectors.Add(new Connector(nodeGraph, node12, node2));
            }

        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            NodeGraphViewModel.Model.Nodes.Remove(_contextMenuNode);
        }

        private void NodeGraphViewNodeContextMenuRequested(object sender, Core.Graph.View.ContextMenuEventArgs e)
        {
            _contextMenuNode = e.Node;
            e.ContextMenu = this.FindResource("NodeContextMenu") as ContextMenu;
        }
    }
}