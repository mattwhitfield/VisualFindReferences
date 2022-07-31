using System;
using System.Windows;
using System.Windows.Controls;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.View
{
    public class ContextMenuEventArgs : EventArgs
    {
        public ContextMenuEventArgs(Node node, Point viewSpaceMouseLocation, Point modelSpaceMouseLocation)
        {
            Node = node;
            ViewSpaceMouseLocation = viewSpaceMouseLocation;
            ModelSpaceMouseLocation = modelSpaceMouseLocation;
        }
        public Node Node { get; }
        public Point ViewSpaceMouseLocation { get; }
        public Point ModelSpaceMouseLocation { get; }
        public ContextMenu? ContextMenu { get; set; }
    }
}