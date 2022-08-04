namespace VisualFindReferences.Core.Graph.View
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Shapes;

    public partial class LoadingSpinner
    {
        public LoadingSpinner()
        {
            InitializeComponent();
        }

        private const double Sweep = Math.PI * 1.8;
        private const double EllipseSize = 8;

        private void InitialSetup()
        {
            var centerX = Width / 2;
            var centerY = Height / 2;
            var distance = Math.Min(centerX, centerY) - EllipseSize;

            AddEllipses(LoaderContent, 60, 1f, distance, centerX, centerY);
            AddEllipses(LoaderContent2, 40, 0.6f, distance * 0.7, centerX, centerY);
            AddEllipses(LoaderContent3, 20, 0.4f, distance * 0.4, centerX, centerY);
        }

        private static void AddEllipses(Canvas loaderContent, int numberEllipses, float startingOpacity, double distance, double centerX, double centerY)
        {
            loaderContent.Children.Clear();

            for (var i = 0; i < numberEllipses; i++)
            {
                var ellipse = new Ellipse();
                loaderContent.Children.Add(ellipse);
                ellipse.Width = ellipse.Height = EllipseSize;

                var ellipseAngle = Sweep / numberEllipses * i;
                var ellipseX = Math.Cos(ellipseAngle) * distance;
                var ellipseY = Math.Sin(ellipseAngle) * distance;

                ellipse.SetValue(Canvas.LeftProperty, ellipseX + centerX - EllipseSize / 2);
                ellipse.SetValue(Canvas.TopProperty, ellipseY + centerY - EllipseSize / 2);

                ellipse.Opacity = i * startingOpacity / numberEllipses;
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                InitialSetup();
            }

            base.OnPropertyChanged(e);
        }
    }
}
