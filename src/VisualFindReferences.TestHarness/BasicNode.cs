using System.Windows.Media;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.TestHarness
{
    public class MethodNode : Node
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M21,16.5C21,16.88 20.79,17.21 20.47,17.38L12.57,21.82C12.41,21.94 12.21,22 12,22C11.79,22 11.59,21.94 11.43,21.82L3.53,17.38C3.21,17.21 3,16.88 3,16.5V7.5C3,7.12 3.21,6.79 3.53,6.62L11.43,2.18C11.59,2.06 11.79,2 12,2C12.21,2 12.41,2.06 12.57,2.18L20.47,6.62C20.79,6.79 21,7.12 21,7.5V16.5M12,4.15L6.04,7.5L12,10.85L17.96,7.5L12,4.15M5,15.91L11,19.29V12.58L5,9.21V15.91M19,15.91V9.21L13,12.58V19.29L19,15.91Z");

        public MethodNode(NodeGraph flowChart, string containerName, string typeName, double x, double y) : base(flowChart, containerName, typeName, x, y, DefaultIcon, Brushes.Purple)
        { }
    }

    public class LocalMethodNode : Node
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M17,22V20H20V17H22V20.5C22,20.89 21.84,21.24 21.54,21.54C21.24,21.84 20.89,22 20.5,22H17M7,22H3.5C3.11,22 2.76,21.84 2.46,21.54C2.16,21.24 2,20.89 2,20.5V17H4V20H7V22M17,2H20.5C20.89,2 21.24,2.16 21.54,2.46C21.84,2.76 22,3.11 22,3.5V7H20V4H17V2M7,2V4H4V7H2V3.5C2,3.11 2.16,2.76 2.46,2.46C2.76,2.16 3.11,2 3.5,2H7M13,17.25L17,14.95V10.36L13,12.66V17.25M12,10.92L16,8.63L12,6.28L8,8.63L12,10.92M7,14.95L11,17.25V12.66L7,10.36V14.95M18.23,7.59C18.73,7.91 19,8.34 19,8.91V15.23C19,15.8 18.73,16.23 18.23,16.55L12.75,19.73C12.25,20.05 11.75,20.05 11.25,19.73L5.77,16.55C5.27,16.23 5,15.8 5,15.23V8.91C5,8.34 5.27,7.91 5.77,7.59L11.25,4.41C11.5,4.28 11.75,4.22 12,4.22C12.25,4.22 12.5,4.28 12.75,4.41L18.23,7.59Z");

        public LocalMethodNode(NodeGraph flowChart, string containerName, string typeName, double x, double y) : base(flowChart, containerName, typeName, x, y, DefaultIcon, Brushes.Purple)
        { }       
    }

    public class LambdaNode : Node
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M6,20L10.16,7.91L9.34,6H8V4H10C10.42,4 10.78,4.26 10.93,4.63L16.66,18H18V20H16C15.57,20 15.21,19.73 15.07,19.36L11.33,10.65L8.12,20H6Z");

        public LambdaNode(NodeGraph flowChart, string containerName, string typeName, double x, double y) : base(flowChart, containerName, typeName, x, y, DefaultIcon, Brushes.Purple)
        { }
    }

    public class PropertyNode : Node
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M22.7,19L13.6,9.9C14.5,7.6 14,4.9 12.1,3C10.1,1 7.1,0.6 4.7,1.7L9,6L6,9L1.6,4.7C0.4,7.1 0.9,10.1 2.9,12.1C4.8,14 7.5,14.5 9.8,13.6L18.9,22.7C19.3,23.1 19.9,23.1 20.3,22.7L22.6,20.4C23.1,20 23.1,19.3 22.7,19Z");

        public PropertyNode(NodeGraph flowChart, string containerName, string typeName, double x, double y) : base(flowChart, containerName, typeName, x, y, DefaultIcon, Brushes.WhiteSmoke)
        { }
    }

    public class OperatorNode : Node
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M12,15.5A3.5,3.5 0 0,1 8.5,12A3.5,3.5 0 0,1 12,8.5A3.5,3.5 0 0,1 15.5,12A3.5,3.5 0 0,1 12,15.5M19.43,12.97C19.47,12.65 19.5,12.33 19.5,12C19.5,11.67 19.47,11.34 19.43,11L21.54,9.37C21.73,9.22 21.78,8.95 21.66,8.73L19.66,5.27C19.54,5.05 19.27,4.96 19.05,5.05L16.56,6.05C16.04,5.66 15.5,5.32 14.87,5.07L14.5,2.42C14.46,2.18 14.25,2 14,2H10C9.75,2 9.54,2.18 9.5,2.42L9.13,5.07C8.5,5.32 7.96,5.66 7.44,6.05L4.95,5.05C4.73,4.96 4.46,5.05 4.34,5.27L2.34,8.73C2.21,8.95 2.27,9.22 2.46,9.37L4.57,11C4.53,11.34 4.5,11.67 4.5,12C4.5,12.33 4.53,12.65 4.57,12.97L2.46,14.63C2.27,14.78 2.21,15.05 2.34,15.27L4.34,18.73C4.46,18.95 4.73,19.03 4.95,18.95L7.44,17.94C7.96,18.34 8.5,18.68 9.13,18.93L9.5,21.58C9.54,21.82 9.75,22 10,22H14C14.25,22 14.46,21.82 14.5,21.58L14.87,18.93C15.5,18.67 16.04,18.34 16.56,17.94L19.05,18.95C19.27,19.03 19.54,18.95 19.66,18.73L21.66,15.27C21.78,15.05 21.73,14.78 21.54,14.63L19.43,12.97Z");

        public OperatorNode(NodeGraph flowChart, string containerName, string typeName, double x, double y) : base(flowChart, containerName, typeName, x, y, DefaultIcon, Brushes.DarkGreen)
        { }
    }

    public class FieldInitializerNode : Node
    {
        static readonly Geometry DefaultIcon = Geometry.Parse("M21.41 11.58L12.41 2.58A2 2 0 0 0 11 2H4A2 2 0 0 0 2 4V11A2 2 0 0 0 2.59 12.42L11.59 21.42A2 2 0 0 0 13 22A2 2 0 0 0 14.41 21.41L21.41 14.41A2 2 0 0 0 22 13A2 2 0 0 0 21.41 11.58M13 20L4 11V4H11L20 13M6.5 5A1.5 1.5 0 1 1 5 6.5A1.5 1.5 0 0 1 6.5 5Z");

        public FieldInitializerNode(NodeGraph flowChart, string containerName, string typeName, double x, double y) : base(flowChart, containerName, typeName, x, y, DefaultIcon, Brushes.LightBlue)
        { }
    }

    // 
}