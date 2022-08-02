using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private bool RunOnNodeGraphView(Action<NodeGraphView, NodeViewModel> action)
        {
            var viewModel = ViewModel;
            var view = viewModel?.Model.Owner.ViewModel.View;

            if (view != null && viewModel != null)
            {
                action(view, viewModel);
            }

            return view != null;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            e.Handled = RunOnNodeGraphView((view, viewModel) =>
            {
                if (view.IsSelecting)
                {
                    view.EndDragSelection(false);
                }

                if (!view.AreNodesReallyDragged &&
                    view.MouseLeftDownNode == viewModel.Model)
                {
                    view.TrySelection(viewModel.Model,
                        Keyboard.IsKeyDown(Key.LeftCtrl),
                        Keyboard.IsKeyDown(Key.LeftShift),
                        Keyboard.IsKeyDown(Key.LeftAlt));
                }

                view.EndDragNode();

                view.MouseLeftDownNode = null;
            });
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            e.Handled = RunOnNodeGraphView((view, viewModel) =>
            {
                Keyboard.Focus(view);

                view.EndDragNode();
                view.EndDragSelection(false);

                view.MouseLeftDownNode = viewModel.Model;

                view.BeginDragNode();
            });
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            RunOnNodeGraphView((view, viewModel) =>
            {
                if (view.IsNodeDragging && view.MouseLeftDownNode == viewModel.Model && !IsSelected)
                {
                    Node node = viewModel.Model;
                    view.TrySelection(node, false, false, false);
                }
            });
        }
    }
}