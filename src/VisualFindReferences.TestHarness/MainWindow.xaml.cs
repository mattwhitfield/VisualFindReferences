using System;
using System.Collections.Generic;
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
        public FlowChartViewModel FlowChartViewModel
        {
            get { return (FlowChartViewModel)GetValue(FlowChartViewModelProperty); }
            set { SetValue(FlowChartViewModelProperty, value); }
        }

        public static readonly DependencyProperty FlowChartViewModelProperty =
            DependencyProperty.Register("FlowChartViewModel", typeof(FlowChartViewModel), typeof(MainWindow), new PropertyMetadata(null));

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
                var existing = FlowChartViewModel.NodeViewModels[r.Next(FlowChartViewModel.NodeViewModels.Count)].Model;
                var flowChart = FlowChartViewModel.Model;

                var n1 = CreateNode(flowChart, existing.X, existing.Y);
                var n2 = CreateNode(flowChart, existing.X, existing.Y);

                flowChart.Nodes.Add(n1);
                flowChart.Nodes.Add(n2);

                var c1 = new Connector(flowChart, existing, n1);
                var c2 = new Connector(flowChart, existing, n2);

                flowChart.Connectors.Add(c1);
                flowChart.Connectors.Add(c2);

                var view = FlowChartViewModel.View;
                var nodes = new List<Tuple<Node, double, double>>();
                nodes.Add(Tuple.Create(n1, n1.X + 200, n1.Y - 50));
                nodes.Add(Tuple.Create(n2, n2.X + 200, n2.Y + 50));
                FlowChartViewModel.View.StartAnimation(nodes, view.ZoomAndPan.Scale + (r.NextDouble() - 0.5), view.ZoomAndPan.StartX + ((r.NextDouble() - 0.5) * 20), view.ZoomAndPan.StartY + ((r.NextDouble() - 0.5) * 20));
            }

            if (e.Key == System.Windows.Input.Key.L)
            {
                var dictionary = new Dictionary<Node, Core.Graph.Layout.Point>();
                foreach(var node in FlowChartViewModel.Model.Nodes)
                {
                    dictionary[node] = new Core.Graph.Layout.Point(node.X, node.Y);
                }
                var algo = new FRLayoutAlgorithm(FlowChartViewModel.Model, dictionary);
                algo.Initialize();
                algo.InternalCompute();

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

                var view = FlowChartViewModel.View;
                var nodes = new List<Tuple<Node, double, double>>();

                foreach (var pair in fsaDictionary)
                {
                    nodes.Add(Tuple.Create(pair.Key, pair.Value.X, pair.Value.Y));
                }
                FlowChartViewModel.View.StartAnimation(nodes, view.ZoomAndPan.Scale, view.ZoomAndPan.StartX, view.ZoomAndPan.StartY);
            }
        }

        private Node CreateNode(FlowChart flowChart, double x, double y)
        {
            switch (r.Next(6))
            {
                case 0: return new MethodNode(flowChart, "Method", "MyProgram.Stuff.MethodType", x, y);
                case 1: return new LocalMethodNode(flowChart, "LocalMethod", "MyProgram.Stuff.LocalMethodType", x, y);
                case 2: return new PropertyNode(flowChart, "Property", "MyProgram.Stuff.PropertyType", x, y);
                case 3: return new LambdaNode(flowChart, "Lambda", "MyProgram.Stuff.LambdaNode", x, y);
                case 4: return new OperatorNode(flowChart, "Operator", "MyProgram.Stuff.OperatorType", x, y);
                default: return new FieldInitializerNode(flowChart, "FieldInitializer", "MyProgram.Stuff.FieldInitializerType", x, y);
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FlowChart flowChart = new FlowChart();
            FlowChartViewModel = flowChart.ViewModel;

            var n1 = CreateNode(flowChart, 100, 100);
            var n2 = CreateNode(flowChart, 300, 100);
            var n3 = CreateNode(flowChart, 100, 160);

            flowChart.Nodes.Add(n1);
            flowChart.Nodes.Add(n2);
            flowChart.Nodes.Add(n3);

            var c1 = new Connector(flowChart, n1, n2);
            var c2 = new Connector(flowChart, n1, n3);

            flowChart.Connectors.Add(c1);
            flowChart.Connectors.Add(c2);
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            FlowChartViewModel.Model.Nodes.Remove(_contextMenuNode);
        }

        private void FlowChartView_NodeContextMenuRequested(object sender, Core.Graph.View.ContextMenuEventArgs e)
        {
            _contextMenuNode = e.Node;
            e.ContextMenu = this.FindResource("NodeContextMenu") as ContextMenu;
        }
    }
}