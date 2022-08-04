using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using VisualFindReferences.Core.Graph.Helper;
using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.View
{
    public class NodeGraphView : ContentControl
    {
        private DispatcherTimer _dragBoundsTimer = new DispatcherTimer();
        private DispatcherTimer _animationTimer = new DispatcherTimer();
        private Stopwatch? _animationStopwatch;
        private double _animationSeconds;
        private IEasingFunction _animationEasing = new QuadraticEase();
        private GraphAnimation? _animation;

        public NodeGraphViewModel? ViewModel { get; private set; }

        public ZoomAndPan ZoomAndPan { get; } = new ZoomAndPan();

        private FrameworkElement? _nodeCanvas;

        private FrameworkElement? _connectorCanvas;

        private FrameworkElement? _partConnectorViewsContainer;

        private FrameworkElement? _partNodeViewsContainer;

        private int _lastWheelDirection;
        private MouseArea _wheelConstrainedMouseArea;

        public bool IsNodeDragging { get; private set; }
        public bool AreNodesReallyDragged { get; private set; }

        private bool IsDragging;
        public bool IsSelecting { get; private set; }
        public Node? MouseLeftDownNode { get; set; }

        private Point SelectingStartPoint;

        private IList<Node>? _originalSelections;

        static NodeGraphView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeGraphView), new FrameworkPropertyMetadata(typeof(NodeGraphView)));
        }

        public NodeGraphView()
        {
            Focusable = true;
            DataContextChanged += NodeGraphViewDataContextChanged;
            SizeChanged += NodeGraphViewSizeChanged;

            _dragBoundsTimer.Interval = new TimeSpan(0, 0, 0, 0, 33);
            _dragBoundsTimer.Tick += DragBoundsTimerTick;

            _animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 16);
            _animationTimer.Tick += _animationTimer_Tick; ;

            ZoomAndPan.UpdateTransform += _ZoomAndPan_UpdateTransform;
        }

        public event EventHandler<ContextMenuEventArgs>? NodeContextMenuRequested;

        public double SelectionWidth
        {
            get { return (double)GetValue(SelectionWidthProperty); }
            set { SetValue(SelectionWidthProperty, value); }
        }

        public double SelectionHeight
        {
            get { return (double)GetValue(SelectionHeightProperty); }
            set { SetValue(SelectionHeightProperty, value); }
        }

        public double SelectionStartY
        {
            get { return (double)GetValue(SelectionStartYProperty); }
            set { SetValue(SelectionStartYProperty, value); }
        }

        public double SelectionStartX
        {
            get { return (double)GetValue(SelectionStartXProperty); }
            set { SetValue(SelectionStartXProperty, value); }
        }

        public Visibility SelectionVisibility
        {
            get { return (Visibility)GetValue(SelectionVisibilityProperty); }
            set { SetValue(SelectionVisibilityProperty, value); }
        }

        public static readonly DependencyProperty SelectionWidthProperty = DependencyProperty.Register("SelectionWidth", typeof(double), typeof(NodeGraphView), new PropertyMetadata(0.0));

        public static readonly DependencyProperty SelectionHeightProperty = DependencyProperty.Register("SelectionHeight", typeof(double), typeof(NodeGraphView), new PropertyMetadata(0.0));

        public static readonly DependencyProperty SelectionStartYProperty = DependencyProperty.Register("SelectionStartY", typeof(double), typeof(NodeGraphView), new PropertyMetadata(0.0));

        public static readonly DependencyProperty SelectionStartXProperty = DependencyProperty.Register("SelectionStartX", typeof(double), typeof(NodeGraphView), new PropertyMetadata(0.0));

        public static readonly DependencyProperty SelectionVisibilityProperty = DependencyProperty.Register("SelectionVisibility", typeof(Visibility), typeof(NodeGraphView), new PropertyMetadata(Visibility.Collapsed));

        public void StartAnimation(IEnumerable<Tuple<Node, double, double>> nodes, double scale, double startX, double startY)
        {
            _animation = new GraphAnimation(ZoomAndPan, scale, startX, startY);
            foreach (var pair in nodes)
            {
                _animation.NodeAnimations.Add(new NodeAnimation(pair.Item1, pair.Item2, pair.Item3));
            }

            _animationStopwatch = Stopwatch.StartNew();
            _animationSeconds = 0.66;
            _animationTimer.Start();
        }

        private void _animationTimer_Tick(object sender, EventArgs e)
        {
            if (_animationStopwatch == null || _animation == null)
            {
                _animationTimer.Stop();
                return;
            }

            var factor = _animationStopwatch.ElapsedMilliseconds / 1000.0 / _animationSeconds;
            if (factor >= 1)
            {
                _animation.Apply(1);
                _animation = null;
                _animationStopwatch.Stop();
                _animationStopwatch = null;
                _animationTimer.Stop();
            }
            else
            {
                _animation.Apply(_animationEasing.Ease(factor));
            }
        }

        public void TrySelection(Node node, bool bCtrl, bool bShift, bool bAlt)
        {
            bool bAdd;
            if (bCtrl)
            {
                bAdd = !node.ViewModel.IsSelected;
            }
            else if (bShift)
            {
                bAdd = true;
            }
            else if (bAlt)
            {
                bAdd = false;
            }
            else
            {
                DeselectAllNodes();
                bAdd = true;
            }

            if (bAdd)
            {
                if (!node.ViewModel.IsSelected)
                {
                    AddSelection(node);
                }
            }
            else
            {
                if (node.ViewModel.IsSelected)
                {
                    RemoveSelection(node);
                }
            }
        }



        private void AddSelection(Node node)
        {
            node.ViewModel.IsSelected = true;
        }

        private void RemoveSelection(Node node)
        {
            node.ViewModel.IsSelected = false;
        }

        public void DeselectAllNodes()
        {
            ViewModel?.NodeViewModels.Each(x => x.IsSelected = false);
        }

        public void SelectAllNodes()
        {
            ViewModel?.NodeViewModels.Each(x => x.IsSelected = true);
        }


        public Node? FindNodeUnderMouse(Point mousePos, out Point viewSpacePos, out Point modelSpacePos)
        {
            viewSpacePos = mousePos;
            modelSpacePos = ZoomAndPan.MatrixInv.Transform(mousePos);

            HitTestResult hitResult = VisualTreeHelper.HitTest(this, mousePos);
            if (hitResult != null && hitResult.VisualHit != null)
            {
                DependencyObject hit = hitResult.VisualHit;

                NodeView? nodeView = ViewUtil.FindFirstParent<NodeView>(hit);
                if (nodeView != null)
                {
                    return nodeView.ViewModel?.Model;
                }
            }

            return null;
        }

        private IEnumerable<Node> SelectedNodes => ViewModel?.NodeViewModels.Where(x => x.IsSelected).Select(x => x.Model) ?? Enumerable.Empty<Node>();


        private void BeginDragSelection(Point start)
        {
            BeginDragging();

            SelectingStartPoint = start;

            IsSelecting = true;
            SelectionVisibility = Visibility.Visible;

            _originalSelections = SelectedNodes.ToList();
        }

        private void UpdateDragSelection(Point end, bool bCtrl, bool bShift, bool bAlt)
        {
            if (ViewModel == null)
            {
                return;
            }

            double startX = SelectingStartPoint.X;
            double startY = SelectingStartPoint.Y;

            Point selectionStart = new Point(Math.Min(startX, end.X), Math.Min(startY, end.Y));
            Point selectionEnd = new Point(Math.Max(startX, end.X), Math.Max(startY, end.Y));

            bool bAdd;
            if (bCtrl)
            {
                bAdd = true;
            }
            else if (bShift)
            {
                bAdd = true;
            }
            else if (bAlt)
            {
                bAdd = false;
            }
            else
            {
                bAdd = true;
            }

            foreach (var node in ViewModel.Model.Nodes)
            {
                var nodeView = node.ViewModel.View;
                Point nodeStart = new Point(node.X, node.Y);
                Point nodeEnd = nodeView != null ? new Point(node.X + nodeView.ActualWidth, node.Y + nodeView.ActualHeight) : nodeStart;

                bool isInOriginalSelection = _originalSelections != null && _originalSelections.Contains(node);

                var isSelected = nodeStart.X < selectionEnd.X && nodeEnd.X > selectionStart.X &&
                                 nodeStart.Y < selectionEnd.Y && nodeEnd.Y > selectionStart.Y;

                if (!isSelected)
                {
                    if (isInOriginalSelection)
                    {
                        if (bCtrl || !bAdd)
                        {
                            AddSelection(node);
                        }
                    }
                    else
                    {
                        if (bCtrl || bAdd)
                        {
                            RemoveSelection(node);
                        }
                    }

                    continue;
                }

                bool bThisAdd = bAdd;
                if (isInOriginalSelection && bCtrl)
                {
                    bThisAdd = false;
                }

                if (bThisAdd)
                {
                    AddSelection(node);
                }
                else
                {
                    RemoveSelection(node);
                }
            }
        }

        public void EndDragSelection(bool bCancel)
        {
            EndDragging();

            if (IsSelecting)
            {
                if (bCancel)
                {
                    if (IsSelecting && _originalSelections != null)
                    {
                        DeselectAllNodes();

                        foreach (var node in _originalSelections)
                        {
                            AddSelection(node);
                        }
                    }
                }

                if (ViewModel?.View != null)
                {
                    ViewModel.View.SelectionVisibility = Visibility.Collapsed;
                }
                IsSelecting = false;
                _originalSelections = null;
            }
        }

        private void DestroySelectedNodes()
        {
            if (ViewModel == null)
            {
                return;
            }

            foreach (var node in SelectedNodes.ToList())
            {
                ViewModel.Model.Nodes.Remove(node.ViewModel.Model);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _partConnectorViewsContainer = GetTemplateChild("PART_ConnectorViewsContainer") as FrameworkElement;

            _partNodeViewsContainer = GetTemplateChild("PART_NodeViewsContainer") as FrameworkElement;
        }

        private void NodeGraphViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ZoomAndPan.ViewWidth = ActualWidth;
            ZoomAndPan.ViewHeight = ActualHeight;
        }

        private void NodeGraphViewDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewModel = DataContext as NodeGraphViewModel;
            if (null == ViewModel)
                return;

            ViewModel.View = this;

            if (_connectorCanvas == null)
            {
                _connectorCanvas = ViewUtil.FindChild<Canvas>(_partConnectorViewsContainer);
            }

            if (_nodeCanvas == null)
            {
                _nodeCanvas = ViewUtil.FindChild<Canvas>(_partNodeViewsContainer);
            }
        }

        private void _ZoomAndPan_UpdateTransform()
        {
            if (_nodeCanvas != null && _connectorCanvas != null)
            {
                _nodeCanvas.RenderTransform = new MatrixTransform(ZoomAndPan.Matrix);
                _connectorCanvas.RenderTransform = new MatrixTransform(ZoomAndPan.Matrix);
            }
        }

        protected Point _RightButtonDownPos;
        protected Point _LeftButtonDownPos;
        protected Point _PrevMousePos;
        protected bool _IsDraggingCanvas;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (null == ViewModel)
            {
                return;
            }

            Keyboard.Focus(this);

            _LeftButtonDownPos = e.GetPosition(this);
            _PrevMousePos = _LeftButtonDownPos;

            if (!IsNodeDragging &&
                !IsSelecting)
            {
                Point mousePos = e.GetPosition(this);

                BeginDragSelection(ZoomAndPan.MatrixInv.Transform(mousePos));

                SelectionStartX = mousePos.X;
                SelectionWidth = 0;
                SelectionStartY = mousePos.Y;
                SelectionHeight = 0;

                bool bCtrl = Keyboard.IsKeyDown(Key.LeftCtrl);
                bool bShift = Keyboard.IsKeyDown(Key.LeftShift);
                bool bAlt = Keyboard.IsKeyDown(Key.LeftAlt);

                if (!bCtrl && !bShift && !bAlt)
                {
                    DeselectAllNodes();
                }
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (null == ViewModel)
            {
                return;
            }

            EndDragNode();

            if (IsSelecting)
            {
                EndDragSelection(false);
            }
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);

            if (null == ViewModel)
            {
                return;
            }

            Keyboard.Focus(this);

            _RightButtonDownPos = e.GetPosition(this);

            if (!IsDragging)
            {
                _IsDraggingCanvas = true;

                Mouse.Capture(this, CaptureMode.SubTree);
            }
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);

            if (null == ViewModel)
            {
                return;
            }

            EndDragNode();
            EndDragSelection(true);

            Point mousePos = e.GetPosition(this);
            Point diff = new Point(
                Math.Abs(_RightButtonDownPos.X - mousePos.X),
                Math.Abs(_RightButtonDownPos.Y - mousePos.Y));

            bool wasDraggingCanvas = 5.0 < diff.X || 5.0 < diff.Y;

            if (_IsDraggingCanvas)
            {
                _IsDraggingCanvas = false;
                Mouse.Capture(null);
            }

            if (!wasDraggingCanvas)
            {
                var node = FindNodeUnderMouse(mousePos, out var viewSpacePos, out var modelSpacePos);

                if (node != null)
                {
                    ContextMenuEventArgs args = new ContextMenuEventArgs(node, viewSpacePos, modelSpacePos);

                    NodeContextMenuRequested?.Invoke(this, args);

                    if (args.ContextMenu != null)
                    {
                        args.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                        args.ContextMenu.PlacementTarget = node.ViewModel.View;
                        args.ContextMenu.IsOpen = true;
                    }
                }
            }
        }

        [DllImport("user32.dll")]
        public static extern void ClipCursor(ref System.Drawing.Rectangle rect);

        [DllImport("user32.dll")]
        public static extern void ClipCursor(IntPtr rect);

        private void BeginDragging()
        {
            IsDragging = true;

            _dragBoundsTimer.Start();

            Point startLocation = PointToScreen(new Point(0, 0));

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                (int)startLocation.X + 2, (int)startLocation.Y + 2,
                (int)(startLocation.X + ActualWidth) - 2,
                (int)(startLocation.Y + ActualHeight) - 2);
            ClipCursor(ref rect);
        }

        private void EndDragging()
        {
            IsDragging = false;
            ClipCursor(IntPtr.Zero);
        }

        public void BeginDragNode()
        {
            BeginDragging();

            IsNodeDragging = true;
        }

        public void EndDragNode()
        {
            EndDragging();

            IsNodeDragging = false;
            AreNodesReallyDragged = false;
        }

        private void DragNode(Point delta)
        {
            if (!IsNodeDragging)
                return;

            AreNodesReallyDragged = true;

            foreach (var nodeView in SelectedNodes)
            {
                Node node = nodeView.ViewModel.Model;
                node.X += delta.X;
                node.Y += delta.Y;
            }
        }

        private void UpdateDragging(Point mousePos, Point delta)
        {
            if (IsNodeDragging)
            {
                double invScale = 1.0f / ZoomAndPan.Scale;
                DragNode(new Point(delta.X * invScale, delta.Y * invScale));
            }
            else if (IsSelecting)
            {
                // gather nodes in area.

                bool bCtrl = Keyboard.IsKeyDown(Key.LeftCtrl);
                bool bShift = Keyboard.IsKeyDown(Key.LeftShift);
                bool bAlt = Keyboard.IsKeyDown(Key.LeftAlt);

                UpdateDragSelection(ZoomAndPan.MatrixInv.Transform(mousePos), bCtrl, bShift, bAlt);

                Point startPos = ZoomAndPan.Matrix.Transform(SelectingStartPoint);

                Point selectionStart = new Point(Math.Min(startPos.X, mousePos.X), Math.Min(startPos.Y, mousePos.Y));
                Point selectionEnd = new Point(Math.Max(startPos.X, mousePos.X), Math.Max(startPos.Y, mousePos.Y));

                SelectionStartX = selectionStart.X;
                SelectionStartY = selectionStart.Y;
                SelectionWidth = selectionEnd.X - selectionStart.X;
                SelectionHeight = selectionEnd.Y - selectionStart.Y;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (null == ViewModel)
            {
                return;
            }

            Point mousePos = e.GetPosition(this);

            Point delta = new Point(mousePos.X - _PrevMousePos.X, mousePos.Y - _PrevMousePos.Y);

            if (IsDragging)
            {
                UpdateDragging(mousePos, delta);
            }
            else
            {
                if (_IsDraggingCanvas)
                {
                    ZoomAndPan.StartX -= delta.X;
                    ZoomAndPan.StartY -= delta.Y;
                }
            }

            _PrevMousePos = mousePos;

            e.Handled = true;
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (null == ViewModel)
            {
                return;
            }

            EndDragNode();
            EndDragSelection(true);

            if (_IsDraggingCanvas)
            {
                _IsDraggingCanvas = false;
                Mouse.Capture(null);
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (null == ViewModel)
            {
                return;
            }
            
            const int ConstraintBorder = 20;

            double newScale = ZoomAndPan.Scale;
            var direction = Math.Sign(e.Delta);

            if (direction != _lastWheelDirection)
            {
                _wheelConstrainedMouseArea = MouseArea.None;
                _lastWheelDirection = direction;
            }

            // get viewport in model co-ordinates before applying transform
            var oldTopLeftInModelSpace = ZoomAndPan.MatrixInv.Transform(new Point(0, 0));
            var oldBottomRigtInModelSpace = ZoomAndPan.MatrixInv.Transform(new Point(ActualWidth, ActualHeight));

            newScale += direction * 0.05;

            var oldConstraintBorderInModelSpace = ConstraintBorder / ZoomAndPan.Scale;

            Point vsZoomCenter = e.GetPosition(this);
            Point zoomCenter = ZoomAndPan.MatrixInv.Transform(vsZoomCenter);

            ZoomAndPan.Scale = ConstrainScale(newScale);

            Point vsNextZoomCenter = ZoomAndPan.Matrix.Transform(zoomCenter);
            Point vsDelta = new Point(vsZoomCenter.X - vsNextZoomCenter.X, vsZoomCenter.Y - vsNextZoomCenter.Y);

            ZoomAndPan.StartX -= vsDelta.X;
            ZoomAndPan.StartY -= vsDelta.Y;

            // now work out wheel constraints if zooming in

            if (direction < 0)
            {
                return;
            }

            // get viewport in model co-ordinates
            var topLeftInModelSpace = ZoomAndPan.MatrixInv.Transform(new Point(0, 0));
            var bottomRigtInModelSpace = ZoomAndPan.MatrixInv.Transform(new Point(ActualWidth, ActualHeight));

            CalculateContentSize(ViewModel.Model, false, out var minX, out var maxX, out var minY, out var maxY);

            var constraintBorderInModelSpace = ConstraintBorder / ZoomAndPan.Scale;

            Console.WriteLine(constraintBorderInModelSpace);

            var oldMinX = minX - oldConstraintBorderInModelSpace;
            var oldMinY = minY - oldConstraintBorderInModelSpace;
            var oldMaxX = maxX + oldConstraintBorderInModelSpace;
            var oldMaxY = maxY + oldConstraintBorderInModelSpace;
            minX -= constraintBorderInModelSpace;
            minY -= constraintBorderInModelSpace;
            maxX += constraintBorderInModelSpace;
            maxY += constraintBorderInModelSpace;

            // flag up to 1 x constraints && 1 y constraint as the displayed model crossed the edge of the viewport
            var (constraintsSetX, constraintsSetY) = CountSetWheelConstraints();
            if (constraintsSetX < 1 && topLeftInModelSpace.X > minX && oldTopLeftInModelSpace.X <= oldMinX)
            {
                _wheelConstrainedMouseArea |= MouseArea.Left;
                constraintsSetX++;
            }
            if (constraintsSetY < 1 && topLeftInModelSpace.Y > minY && oldTopLeftInModelSpace.Y <= oldMinY)
            {
                _wheelConstrainedMouseArea |= MouseArea.Top;
                constraintsSetY++;
            }
            if (constraintsSetX < 1 && bottomRigtInModelSpace.X < maxX && oldBottomRigtInModelSpace.X >= oldMaxX)
            {
                _wheelConstrainedMouseArea |= MouseArea.Right;
            }
            if (constraintsSetY < 1 && bottomRigtInModelSpace.Y < maxY && oldBottomRigtInModelSpace.Y >= oldMaxY)
            {
                _wheelConstrainedMouseArea |= MouseArea.Bottom;
            }

            // apply constraints
            if ((_wheelConstrainedMouseArea & MouseArea.Left) > 0 && topLeftInModelSpace.X > minX)
            {
                ZoomAndPan.StartX -= topLeftInModelSpace.X - minX;
            }
            if ((_wheelConstrainedMouseArea & MouseArea.Top) > 0 && topLeftInModelSpace.Y > minY)
            {
                ZoomAndPan.StartY -= topLeftInModelSpace.Y - minY;
            }
            if ((_wheelConstrainedMouseArea & MouseArea.Right) > 0 && bottomRigtInModelSpace.X < maxX)
            {
                ZoomAndPan.StartX -= bottomRigtInModelSpace.X - maxX;
            }
            if ((_wheelConstrainedMouseArea & MouseArea.Bottom) > 0 && bottomRigtInModelSpace.Y < maxY)
            {
                ZoomAndPan.StartY -= bottomRigtInModelSpace.Y - maxY;
            }

        }

        private (int, int) CountSetWheelConstraints()
        {
            int IsSet(MouseArea flag)
            {
                return (_wheelConstrainedMouseArea & flag) > 0 ? 1 : 0;
            }

            return (IsSet(MouseArea.Left) +
                    IsSet(MouseArea.Right),
                    IsSet(MouseArea.Top) +
                    IsSet(MouseArea.Bottom));
        }

        private void DragBoundsTimerTick(object sender, EventArgs e)
        {
            if (null == ViewModel)
            {
                return;
            }

            if (IsDragging)
            {
                MouseArea area = CheckMouseArea();

                if (MouseArea.None != area)
                {
                    Point delta = new Point(0.0, 0.0);
                    if (MouseArea.Left == (area & MouseArea.Left))
                        delta.X = -10.0;
                    if (MouseArea.Right == (area & MouseArea.Right))
                        delta.X = 10.0;
                    if (MouseArea.Top == (area & MouseArea.Top))
                        delta.Y = -10.0;
                    if (MouseArea.Bottom == (area & MouseArea.Bottom))
                        delta.Y = 10.0;

                    Point mousePos = Mouse.GetPosition(this);
                    UpdateDragging(
                        new Point(mousePos.X + delta.X, mousePos.Y + delta.Y), // virtual mouse-position.
                        delta); // virtual delta.

                    ZoomAndPan.StartX += delta.X;
                    ZoomAndPan.StartY += delta.Y;
                }
            }
            else
            {
                _dragBoundsTimer.Stop();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (null == ViewModel)
            {
                return;
            }

            if (IsFocused)
            {
                if (Key.Delete == e.Key)
                {
                    DestroySelectedNodes();
                }
                else if (Key.Escape == e.Key)
                {
                    DeselectAllNodes();
                }
                else if (Key.A == e.Key)
                {
                    if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        SelectAllNodes();
                    }
                    else
                    {
                        FitNodesToView(false);
                    }
                }
                else if (Key.F == e.Key)
                {
                    FitNodesToView(true);
                }
            }
        }

        public enum MouseArea : uint
        {
            None = 0x00000000,
            Left = 0x00000001,
            Right = 0x00000002,
            Top = 0x00000004,
            Bottom = 0x00000008,
        }

        public MouseArea CheckMouseArea()
        {
            Point absPosition = Mouse.GetPosition(this);
            Point absTopLeft = new Point(0.0, 0.0);
            Point absBottomRight = new Point(ActualWidth, ActualHeight);

            MouseArea area = MouseArea.None;

            if (absPosition.X < absTopLeft.X + 4.0)
                area |= MouseArea.Left;
            if (absPosition.X > absBottomRight.X - 4.0)
                area |= MouseArea.Right;
            if (absPosition.Y < absTopLeft.Y + 4.0)
                area |= MouseArea.Top;
            if (absPosition.Y > absBottomRight.Y - 4.0)
                area |= MouseArea.Bottom;

            return area;
        }

        private void CalculateContentSize(NodeGraph nodeGraph, bool bOnlySelected, out double minX, out double maxX, out double minY, out double maxY)
        {
            minX = double.MaxValue;
            maxX = double.MinValue;
            minY = double.MaxValue;
            maxY = double.MinValue;

            bool hasNodes = false;
            foreach (var node in nodeGraph.Nodes)
            {
                var nodeView = node.ViewModel.View;
                if (node.Owner == nodeGraph)
                {
                    if (bOnlySelected && !node.ViewModel.IsSelected)
                        continue;

                    minX = Math.Min(node.X, minX);
                    maxX = Math.Max(node.X + nodeView?.ActualWidth ?? 0, maxX);
                    minY = Math.Min(node.Y, minY);
                    maxY = Math.Max(node.Y + nodeView?.ActualHeight ?? 0, maxY);
                    hasNodes = true;
                }
            }

            if (!hasNodes)
            {
                minX = maxX = minY = maxY = 0.0;
            }
        }

        public void FitNodesToView(bool bOnlySelected)
        {
            if (ViewModel == null)
            {
                return;
            }

            CalculateContentSize(ViewModel.Model, bOnlySelected, out var minX, out var maxX, out var minY, out var maxY);
            if (minX == maxX || minY == maxY)
            {
                return;
            }

            double vsWidth = ZoomAndPan.ViewWidth;
            double vsHeight = ZoomAndPan.ViewHeight;

            Point margin = new Point(vsWidth * 0.05, vsHeight * 0.05);
            minX -= margin.X;
            minY -= margin.Y;
            maxX += margin.X;
            maxY += margin.Y;

            double contentWidth = maxX - minX;
            double contentHeight = maxY - minY;

            ZoomAndPan.StartX = (minX + maxX - vsWidth) * 0.5;
            ZoomAndPan.StartY = (minY + maxY - vsHeight) * 0.5;
            ZoomAndPan.Scale = 1.0;

            Point vsZoomCenter = new Point(vsWidth * 0.5, vsHeight * 0.5);
            Point zoomCenter = ZoomAndPan.MatrixInv.Transform(vsZoomCenter);

            double newScale = Math.Min(vsWidth / contentWidth, vsHeight / contentHeight);
            ZoomAndPan.Scale = ConstrainScale(newScale);

            Point vsNextZoomCenter = ZoomAndPan.Matrix.Transform(zoomCenter);
            Point vsDelta = new Point(vsZoomCenter.X - vsNextZoomCenter.X, vsZoomCenter.Y - vsNextZoomCenter.Y);

            ZoomAndPan.StartX -= vsDelta.X;
            ZoomAndPan.StartY -= vsDelta.Y;
        }

        private double ConstrainScale(double scale)
        {
            return Math.Max(0.05, Math.Min(3.0, scale));
        }
    }
}