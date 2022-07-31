using System;
using System.Windows;
using System.Windows.Controls;
using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.View
{
    public class ConnectorView : ContentControl
    {
        public ConnectorViewModel? ViewModel { get; private set; }

        public string CurveData
        {
            get { return (string)GetValue(CurveDataProperty); }
            set { SetValue(CurveDataProperty, value); }
        }

        public static readonly DependencyProperty CurveDataProperty =
            DependencyProperty.Register("CurveData", typeof(string), typeof(ConnectorView), new PropertyMetadata(string.Empty));

        public ConnectorView()
        {
            LayoutUpdated += ConnectorView_LayoutUpdated;
            DataContextChanged += ConnectorView_DataContextChanged;
        }

        private void ConnectorView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewModel = DataContext as ConnectorViewModel;
            if (ViewModel != null)
            {
                ViewModel.View = this;
            }
        }

        private void ConnectorView_LayoutUpdated(object sender, EventArgs e)
        {
            BuildCurveData();
        }

        private const double TriangleLength = 8;
        private const double TriangleWidth = 5;

        private Point GetCenter(Node node)
        {
            var view = node.ViewModel.View;
            if (view != null)
            {
                return new Point(node.X + view.ActualWidth / 2, node.Y + view.ActualHeight / 2);
            }

            return new Point(node.X, node.Y);
        }

        public void BuildCurveData()
        {
            if (ViewModel == null)
            {
                return;
            }

            Connector connector = ViewModel.Model;
            FlowChartView? flowChartView = connector.FlowChart.ViewModel.View;

            if (flowChartView == null || !flowChartView.IsAncestorOf(this))
            {
                return;
            }

            var start = GetCenter(connector.StartNode);
            var end = GetCenter(connector.EndNode);

            var center = new Point((start.X + end.X) * 0.5, (start.Y + end.Y) * 0.5);

            var rotation = Math.Atan2(end.Y - start.Y, end.X - start.X);

            var unit = new Point(Math.Cos(rotation), Math.Sin(rotation));

            //                      triPoint1
            //  start ------------- mid   center    triPoint2 ------------- end
            //                      triPoint3

            var scaledLength = TriangleLength;
            var invScaledLength = scaledLength * -1;
            var mid = new Point(center.X + unit.X * invScaledLength, center.Y + unit.Y * invScaledLength);
            var triPoint2 = new Point(center.X + unit.X * scaledLength, center.Y + unit.Y * scaledLength);

            var pointRotation = rotation - Math.PI / 2;

            var cos = Math.Cos(pointRotation);
            var sin = Math.Sin(pointRotation);
            var scaledWidth = TriangleWidth;
            var invScaledWidth = scaledWidth * -1;
            var triPoint1 = new Point(mid.X + cos * invScaledWidth, mid.Y + sin * invScaledWidth);
            var triPoint3 = new Point(mid.X + cos * scaledWidth, mid.Y + sin * scaledWidth);

            CurveData = string.Format("M{0},{1}L{2},{3} {4},{5} {6},{7} {8},{9} {2},{3} M{6},{7} {10},{11}",
                start.X, start.Y, // 0, 1
                mid.X, mid.Y, // 2, 3
                triPoint1.X, triPoint1.Y, // 4, 5
                triPoint2.X, triPoint2.Y, // 6, 7
                triPoint3.X, triPoint3.Y, // 8, 9
                end.X, end.Y // 10, 11
            );
        }
    }
}