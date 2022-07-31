using System.Windows.Media;
using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.Model
{
    public class Node : ModelBase
    {
        public FlowChart Owner { get; }

        public NodeViewModel ViewModel { get; }

        public string ContainerName { get; }

        public string TypeName { get; }

        private double _x = 0.0;

        public double X
        {
            get { return _x; }
            set
            {
                if (value != _x)
                {
                    _x = value;
                    RaisePropertyChanged("X");
                }
            }
        }

        private double _y = 0.0;

        public double Y
        {
            get { return _y; }
            set
            {
                if (value != _y)
                {
                    _y = value;
                    RaisePropertyChanged("Y");
                }
            }
        }

        public Geometry Icon { get; }
        public Brush IconColor { get; }

        public Node(FlowChart flowChart, string containerName, string typeName, double x, double y, Geometry icon, Brush iconColor)
        {
            Owner = flowChart;
            ViewModel = new NodeViewModel(this);
            ContainerName = containerName;
            TypeName = typeName;
            X = x;
            Y = y;
            Icon = icon;
            IconColor = iconColor;
        }
    }
}