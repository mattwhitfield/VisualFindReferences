using System.Diagnostics;
using System.Windows.Media;
using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.Model
{
    [DebuggerDisplay("{ContainerName}")]
    public class Node : ModelBase
    {
        public NodeGraph Owner { get; }

        public NodeViewModel ViewModel { get; }

        public string ContainerName { get; }

        public string TypeName { get; }

        public bool IsRoot { get; set; }

        private double _x = 0.0;

        public double X
        {
            get { return _x; }
            set
            {
                if (value != _x)
                {
                    _x = value;
                    RaisePropertyChanged(nameof(X));
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
                    RaisePropertyChanged(nameof(Y));
                }
            }
        }

        public Geometry Icon { get; }
        public Brush IconColor { get; }

        public Node(NodeGraph nodeGraph, string containerName, string typeName, double x, double y, Geometry icon, Brush iconColor)
        {
            Owner = nodeGraph;
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