using System.Windows.Media;

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
    }
}