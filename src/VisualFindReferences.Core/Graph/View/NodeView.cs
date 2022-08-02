using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VisualFindReferences.Core.Graph.Helper;
using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.View
{
    public class NodeView : ContentControl, IHighlightable
    {
        public NodeViewModel? ViewModel { get; private set; }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(NodeView), new PropertyMetadata(false));

        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightedProperty =
            DependencyProperty.Register("IsHighlighted", typeof(bool), typeof(NodeView), new PropertyMetadata(false));

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
            if (ViewModel != null)
            {
                ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            }
            ViewModel = DataContext as NodeViewModel;
            if (ViewModel != null)
            {
                ViewModel.View = this;
                ViewModel.PropertyChanged += ViewModelPropertyChanged;
            }

            SynchronizeProperties();
        }

        protected virtual void SynchronizeProperties()
        {
            IsSelected = ViewModel?.IsSelected ?? false;
            IsHighlighted = ViewModel?.IsHighlighted ?? false;
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

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            RunOnNodeGraphView((view, viewModel) =>
            {
                if (view.IsSelecting || view.IsNodeDragging)
                {
                    return;
                }

                view.ViewModel?.Model.SetHighlightFromRootTo(viewModel.Model);
            });
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            RunOnNodeGraphView((view, viewModel) =>
            {
                var model = view.ViewModel?.Model;
                model?.Nodes.Each(x => x.ViewModel.IsHighlighted = false);
                model?.Connectors.Each(x => x.ViewModel.IsHighlighted = false);
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