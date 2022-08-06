namespace VisualFindReferences.Views
{
    using System.Windows.Controls;
    using VisualFindReferences.Core.Graph.Model;
    using VisualFindReferences.Core.Graph.ViewModel;

    /// <summary>
    /// Interaction logic for VisualFindReferencesHost.xaml
    /// </summary>
    public partial class VisualFindReferencesHost : UserControl
    {
        public VisualFindReferencesHost()
        {
            InitializeComponent();
            Model = new VFRNodeGraph();
            GraphView.DataContext = ViewModel = Model.ViewModel;
        }

        public NodeGraphViewModel ViewModel { get; }

        public VFRNodeGraph Model { get; }

        private void NodeGraphViewNodeContextMenuRequested(object sender, Core.Graph.View.ContextMenuEventArgs e)
        {
            //_contextMenuNode = e.Node;
            //e.ContextMenu = this.FindResource("NodeContextMenu") as ContextMenu;
        }
    }
}
