using System;
using System.Windows;
using System.Windows.Media;
using VisualFindReferences.Core.Graph.Layout;

namespace VisualFindReferences.Core.Graph.View
{
    public class ZoomAndPan
    {
        private double _viewWidth;

        public double ViewWidth
        {
            get { return _viewWidth; }
            set
            {
                if (value != _viewWidth)
                {
                    _viewWidth = value;
                }
            }
        }

        private double _viewHeight;

        public double ViewHeight
        {
            get { return _viewHeight; }
            set
            {
                if (value != _viewHeight)
                {
                    _viewHeight = value;
                }
            }
        }

        private double _startX = 0.0;

        public double StartX
        {
            get { return _startX; }
            set
            {
                if (value != _startX)
                {
                    _startX = value;
                    _UpdateTransform();
                }
            }
        }

        private double _startY = 0.0;

        public double StartY
        {
            get { return _startY; }
            set
            {
                if (value != _startY)
                {
                    _startY = value;
                    _UpdateTransform();
                }
            }
        }

        private double _scale = 1.0;

        public double Scale
        {
            get { return _scale; }
            set
            {
                if (value != _scale)
                {
                    _scale = value;
                    _UpdateTransform();
                }
            }
        }

        private Matrix _Matrix = Matrix.Identity;

        public Matrix Matrix
        {
            get { return _Matrix; }
            set
            {
                if (value != _Matrix)
                {
                    _Matrix = value;
                    _MatrixInv = value;
                    _MatrixInv.Invert();
                }
            }
        }

        private Matrix _MatrixInv = Matrix.Identity;

        public Matrix MatrixInv => _MatrixInv;

        private void _UpdateTransform()
        {
            Matrix newMatrix = Matrix.Identity;
            newMatrix.Scale(_scale, _scale);
            newMatrix.Translate(-_startX, -_startY);

            Matrix = newMatrix;

            UpdateTransform?.Invoke();
        }

        public delegate void UpdateTransformDelegate();

        public event UpdateTransformDelegate? UpdateTransform;

        public ZoomAndPan GetTarget(GraphRect area)
        {
            var vsWidth = ViewWidth;
            var vsHeight = ViewHeight;

            var minX = area.Left;
            var minY = area.Top;
            var maxX = area.Right;
            var maxY = area.Bottom;

            var zoomAndPan = new ZoomAndPan();

            Point margin = new Point(vsWidth * 0.05, vsHeight * 0.05);
            minX -= margin.X;
            minY -= margin.Y;
            maxX += margin.X;
            maxY += margin.Y;

            double contentWidth = maxX - minX;
            double contentHeight = maxY - minY;

            zoomAndPan.StartX = (minX + maxX - vsWidth) * 0.5;
            zoomAndPan.StartY = (minY + maxY - vsHeight) * 0.5;
            zoomAndPan.Scale = 1.0;

            Point vsZoomCenter = new Point(vsWidth * 0.5, vsHeight * 0.5);
            Point zoomCenter = zoomAndPan.MatrixInv.Transform(vsZoomCenter);

            double newScale = Math.Min(vsWidth / contentWidth, vsHeight / contentHeight);
            zoomAndPan.Scale = ConstrainScale(newScale);

            Point vsNextZoomCenter = zoomAndPan.Matrix.Transform(zoomCenter);
            Point vsDelta = new Point(vsZoomCenter.X - vsNextZoomCenter.X, vsZoomCenter.Y - vsNextZoomCenter.Y);

            zoomAndPan.StartX -= vsDelta.X;
            zoomAndPan.StartY -= vsDelta.Y;

            return zoomAndPan;
        }

        public static double ConstrainScale(double scale)
        {
            return Math.Max(0.05, Math.Min(3.0, scale));
        }
    }
}