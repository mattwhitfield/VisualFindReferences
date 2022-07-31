using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.View
{
    public class NodeView : ContentControl
    {
        public NodeViewModel? ViewModel { get; private set; }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(NodeView), new PropertyMetadata(false));

        public NodeView()
        {
            DataContextChanged += NodeView_DataContextChanged;
            Loaded += NodeView_Loaded;
        }

        private void NodeView_Loaded(object sender, RoutedEventArgs e)
        {
            SynchronizeProperties();
        }

        private void NodeView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewModel = DataContext as NodeViewModel;
            if (null == ViewModel)
                throw new Exception("ViewModel must be bound as DataContext in NodeView.");
            ViewModel.View = this;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;

            SynchronizeProperties();
        }

        protected virtual void SynchronizeProperties()
        {
            IsSelected = ViewModel?.IsSelected ?? false;
        }

        protected virtual void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SynchronizeProperties();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (ViewModel == null)
            {
                return;
            }

            FlowChart flowChart = ViewModel.Model.Owner;
            var view = flowChart.ViewModel.View;

            if (view != null)
            {
                if (view.IsSelecting)
                {
                    view.EndDragSelection(false);
                }

                if (!view.AreNodesReallyDragged &&
                    view.MouseLeftDownNode == ViewModel.Model)
                {
                    view.TrySelection(ViewModel.Model,
                        Keyboard.IsKeyDown(Key.LeftCtrl),
                        Keyboard.IsKeyDown(Key.LeftShift),
                        Keyboard.IsKeyDown(Key.LeftAlt));
                }

                view.EndDragNode();

                view.MouseLeftDownNode = null;

                e.Handled = true;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (ViewModel == null)
            {
                return;
            }

            FlowChart flowChart = ViewModel.Model.Owner;
            FlowChartView? flowChartView = flowChart.ViewModel.View;

            if (flowChartView == null)
            {
                return;
            }

            Keyboard.Focus(flowChartView);

            flowChartView.EndDragNode();
            flowChartView.EndDragSelection(false);

            flowChartView.MouseLeftDownNode = ViewModel.Model;

            flowChartView.BeginDragNode();

            e.Handled = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            FlowChartView? flowChartView = ViewModel?.Model.Owner.ViewModel.View;

            if (flowChartView == null || ViewModel == null)
            {
                return;
            }

            if (flowChartView.IsNodeDragging &&
                flowChartView.MouseLeftDownNode == ViewModel.Model &&
                !IsSelected)
            {
                Node node = ViewModel.Model;
                flowChartView.TrySelection(node, false, false, false);
            }
        }
    }
}