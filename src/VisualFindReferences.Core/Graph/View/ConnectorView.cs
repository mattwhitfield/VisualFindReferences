using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.View
{
    public class ConnectorView : ContentControl, IHighlightable
    {
        public ConnectorViewModel? ViewModel { get; private set; }

        public string CurveData
        {
            get { return (string)GetValue(CurveDataProperty); }
            set { SetValue(CurveDataProperty, value); }
        }

        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        public bool IsBidirectional
        {
            get { return (bool)GetValue(IsBidirectionalProperty); }
            set { SetValue(IsBidirectionalProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightedProperty =
            DependencyProperty.Register("IsHighlighted", typeof(bool), typeof(ConnectorView), new PropertyMetadata(false));

        public static readonly DependencyProperty IsBidirectionalProperty =
            DependencyProperty.Register("IsBidirectional", typeof(bool), typeof(ConnectorView), new PropertyMetadata(false));

        public static readonly DependencyProperty CurveDataProperty =
            DependencyProperty.Register("CurveData", typeof(string), typeof(ConnectorView), new PropertyMetadata(string.Empty));

        public ConnectorView()
        {
            LayoutUpdated += ConnectorView_LayoutUpdated;
            DataContextChanged += ConnectorView_DataContextChanged;
            Loaded += ConnectorView_Loaded;
        }

        private void ConnectorView_Loaded(object sender, RoutedEventArgs e)
        {
            SynchronizeProperties();
        }

        private void ConnectorView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            }
            ViewModel = DataContext as ConnectorViewModel;
            if (ViewModel != null)
            {
                ViewModel.View = this;
                ViewModel.PropertyChanged += ViewModelPropertyChanged;
            }

            SynchronizeProperties();
        }

        protected virtual void SynchronizeProperties()
        {
            IsHighlighted = ViewModel?.IsHighlighted ?? false;
            IsBidirectional = ViewModel?.IsBidirectional ?? false;
        }

        protected virtual void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SynchronizeProperties();
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
            NodeGraphView? nodeGraphView = connector.NodeGraph.ViewModel.View;

            if (nodeGraphView == null || !nodeGraphView.IsAncestorOf(this))
            {
                return;
            }

            var start = GetCenter(connector.StartNode);
            var end = GetCenter(connector.EndNode);

            var center = new Point((start.X + end.X) * 0.5, (start.Y + end.Y) * 0.5);

            var rotation = Math.Atan2(end.Y - start.Y, end.X - start.X);

            var unit = new Point(Math.Cos(rotation), Math.Sin(rotation));

            var pointRotation = rotation - Math.PI / 2;
            const double ScaledLength = TriangleLength;
            const double InverseScaledLength = ScaledLength * -1;
            const double ScaledWidth = TriangleWidth;
            const double InverseScaledWidth = ScaledWidth * -1;

            if (IsBidirectional)
            {
                //                              triPoint5    triPoint1
                //  start -------- triPoint6   center-gap -- center+gap    triPoint2 ------------- end
                //                              triPoint4    triPoint3
                const double Gap = 3;
                const double InverseGap = Gap * -1;
                const double ScaledLengthWithGap = (ScaledLength * 1.5) + Gap;
                const double InverseScaledLengthWithGap = (InverseScaledLength * 1.5) + InverseGap;
                var centerMinusGap = new Point(center.X + unit.X * InverseGap, center.Y + unit.Y * InverseGap);
                var centerPlusGap = new Point(center.X + unit.X * Gap, center.Y + unit.Y * Gap);

                var cos = Math.Cos(pointRotation);
                var sin = Math.Sin(pointRotation);
                var triPoint1 = new Point(centerPlusGap.X + cos * InverseScaledWidth, centerPlusGap.Y + sin * InverseScaledWidth);
                var triPoint3 = new Point(centerPlusGap.X + cos * ScaledWidth, centerPlusGap.Y + sin * ScaledWidth);
                var triPoint5 = new Point(centerMinusGap.X + cos * InverseScaledWidth, centerMinusGap.Y + sin * InverseScaledWidth);
                var triPoint4 = new Point(centerMinusGap.X + cos * ScaledWidth, centerMinusGap.Y + sin * ScaledWidth);

                var triPoint6 = new Point(center.X + unit.X * InverseScaledLengthWithGap, center.Y + unit.Y * InverseScaledLengthWithGap);
                var triPoint2 = new Point(center.X + unit.X * ScaledLengthWithGap, center.Y + unit.Y * ScaledLengthWithGap);

                CurveData = string.Format(CultureInfo.InvariantCulture, "M{0},{1}L{2},{3} {4},{5} {6},{7} {2},{3} M{8},{9} L{10},{11} M{14},{15} L{16},{17} {12},{13} {14},{15} {18},{19}",
                    start.X, start.Y, // 0, 1
                    triPoint6.X, triPoint6.Y, // 2, 3
                    triPoint5.X, triPoint5.Y, // 4, 5
                    triPoint4.X, triPoint4.Y, // 6, 7
                    centerMinusGap.X, centerMinusGap.Y, // 8,9
                    centerPlusGap.X, centerPlusGap.Y, // 10, 11
                    triPoint1.X, triPoint1.Y, // 12, 13
                    triPoint2.X, triPoint2.Y, // 14, 15
                    triPoint3.X, triPoint3.Y, // 16, 17
                    end.X, end.Y // 18, 19
                );
            }
            else
            {
                //                      triPoint1
                //  start ------------- mid   center    triPoint2 ------------- end
                //                      triPoint3
                var mid = new Point(center.X + unit.X * InverseScaledLength, center.Y + unit.Y * InverseScaledLength);
                var triPoint2 = new Point(center.X + unit.X * ScaledLength, center.Y + unit.Y * ScaledLength);

                var cos = Math.Cos(pointRotation);
                var sin = Math.Sin(pointRotation);
                var triPoint1 = new Point(mid.X + cos * InverseScaledWidth, mid.Y + sin * InverseScaledWidth);
                var triPoint3 = new Point(mid.X + cos * ScaledWidth, mid.Y + sin * ScaledWidth);

                CurveData = string.Format(CultureInfo.InvariantCulture, "M{0},{1}L{2},{3} {4},{5} {6},{7} {8},{9} {2},{3} M{6},{7} {10},{11}",
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
}