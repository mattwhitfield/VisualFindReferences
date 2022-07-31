using System.Collections.ObjectModel;
using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.Model
{
    public class FlowChart : ModelBase
    {
        public FlowChartViewModel ViewModel { get; }

        public ObservableCollection<Node> Nodes { get; } = new ObservableCollection<Node>();

        public ObservableCollection<Connector> Connectors { get; } = new ObservableCollection<Connector>();

        /// <summary>
        /// Never call this constructor directly. Use GraphManager.CreateFlowChart() method.
        /// </summary>
        public FlowChart()
        {
            ViewModel = new FlowChartViewModel(this);
        }
    }
}