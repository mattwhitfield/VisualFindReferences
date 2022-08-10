namespace VisualFindReferences.Views
{
    using Microsoft.CodeAnalysis;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.LanguageServices;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using VisualFindReferences.Core.Graph.Helper;
    using VisualFindReferences.Core.Graph.Layout;
    using VisualFindReferences.Core.Graph.Model;
    using VisualFindReferences.Core.Graph.Model.Nodes;
    using VisualFindReferences.Core.Graph.View;
    using VisualFindReferences.Core.Graph.ViewModel;

    /// <summary>
    /// Interaction logic for VisualFindReferencesHost.xaml
    /// </summary>
    public partial class VisualFindReferencesHost : UserControl
    {
        private VisualFindReferencesPackage _package;

        public VisualFindReferencesHost()
        {
            InitializeComponent();
            Model = new VFRNodeGraph();
            MainDisplay.DataContext = FilteringDisplay.DataContext = ViewModel = Model.ViewModel as VFRNodeGraphViewModel;
        }

        public VFRNodeGraphViewModel ViewModel { get; }

        public VFRNodeGraph Model { get; }

        private void NodeGraphViewNodeContextMenuRequested(object sender, Core.Graph.View.ContextMenuEventArgs e)
        {
            var contextMenu = (ContextMenu)FindResource("NodeContextMenu");
            contextMenu.Items.Clear();

            contextMenu.Items.Add(new MenuItem { Header = "Delete node", Command = GetDeleteCommand(e.Node) });

            if (e.Node is VFRNode vfrNode)
            {
                var searchable = GetSearchableLocations(vfrNode);
                if (searchable.Any())
                {
                    contextMenu.Items.Add(new Separator());
                    foreach (var searchableSymbol in searchable)
                    {
                        contextMenu.Items.Add(new MenuItem { Header = "Find references to " + searchableSymbol.Name, Command = GetSearchCommand(searchableSymbol) });
                    }
                }

                if (vfrNode.NodeFoundReferences.ReferencingLocations.Count > 0)
                {
                    contextMenu.Items.Add(new Separator());
                    foreach (var referencingLocation in vfrNode.NodeFoundReferences.ReferencingLocations)
                    {
                        contextMenu.Items.Add(new MenuItem { Header = "Go to " + referencingLocation.LinePrompt, Command = GetGoToLocation(referencingLocation) });
                    }
                }
            }

            e.ContextMenu = contextMenu;
        }

        private static List<SearchableSymbol> GetSearchableLocations(VFRNode vfrNode)
        {
            return vfrNode.GetSearchableSymbols().Where(x => x.Targets.Any(t => !vfrNode.SearchedSymbols.Contains(t))).ToList();
        }

        private ICommand GetGoToLocation(ReferencingLocation referencingLocation)
        {
            Action goToLocation = () =>
            {
                var cm = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
                var tm = (IVsTextManager)Package.GetGlobalService(typeof(SVsTextManager));
                var ws = (Workspace)cm.GetService<VisualStudioWorkspace>();
                ws.OpenDocument(referencingLocation.Location.Document.Id);
                tm.GetActiveView(1, null, out var av);
                var pos = referencingLocation.Location.Location.GetMappedLineSpan();
                av.SetCaretPos(pos.StartLinePosition.Line, pos.StartLinePosition.Character);
            };
            return new RelayCommand(goToLocation);
        }

        private ICommand GetSearchCommand(SearchableSymbol searchableSymbol)
        {
            Task<FoundReferences> FindReferencesForSearchableSymbolAsync(Action<string> updateText, NodeGraphViewModel viewModel)
            {
                return SymbolProcessor.FindReferencesAsync(updateText, searchableSymbol.SearchingSymbol, searchableSymbol.Targets, searchableSymbol.Solution);
            }

            Action search = () =>
            {
                ViewModel.RunAction(FindReferencesForSearchableSymbolAsync, SymbolProcessor.ProcessFoundReferences);
            };

            return new RelayCommand(search);
        }

        private ICommand GetDeleteCommand(Node node)
        {
            return new RelayCommand(() => Model.Nodes.Remove(node));
        }

        internal void SetPackage(VisualFindReferencesPackage visualFindReferencesPackage)
        {
            var packageWasNull = _package == null;
            _package = visualFindReferencesPackage;
            if (packageWasNull)
            {
                var options = _package.Options;
                ViewModel.DoubleClickAction = options.DefaultDoubleClickAction;
                ViewModel.ProjectFilterMatchPattern = options.DefaultProjectFilter;
                ViewModel.LayoutType = options.DefaultLayoutAlgorithmType;
                SetMenuChecks();
            }
        }

        private void SetMenuChecks()
        {
            var doubleClickMenu = (ContextMenu)FindResource("DoubleClickActionContextMenu");
            SetMenuCheck(doubleClickMenu, "GoToCodeMenuItem", ViewModel.DoubleClickAction == DoubleClickAction.GoToCode);
            SetMenuCheck(doubleClickMenu, "FindReferencesMenuItem", ViewModel.DoubleClickAction == DoubleClickAction.FindReferences);

            var layoutMenu = (ContextMenu)FindResource("LayoutContextMenu");
            SetMenuCheck(layoutMenu, "HorizontalGridMenuItem", ViewModel.LayoutType == LayoutAlgorithmType.HorizontalBalancedGrid);
            SetMenuCheck(layoutMenu, "VerticalGridMenuItem", ViewModel.LayoutType == LayoutAlgorithmType.VerticalBalancedGrid);
            SetMenuCheck(layoutMenu, "ForceDirectedMenuItem", ViewModel.LayoutType == LayoutAlgorithmType.ForceDirected);
        }

        private void SetMenuCheck(ContextMenu menu, string name, bool value)
        {
            var item = menu.Items.OfType<MenuItem>().FirstOrDefault(x => x.Name == name);
            if (item != null)
            {
                item.IsChecked = value;
            }
        }

        private void ChooseDoubleClickAction(object sender, RoutedEventArgs e)
        {
            var contextMenu = (ContextMenu)FindResource("DoubleClickActionContextMenu");
            contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            contextMenu.PlacementTarget = ChooseDoubleClickActionButton;
            contextMenu.IsOpen = true;
        }

        private void GoToReferencesOnDoubleClick(object sender, RoutedEventArgs e)
        {
            ViewModel.DoubleClickAction = DoubleClickAction.GoToCode;
            SetMenuChecks();
        }

        private void FindReferencesOnDoubleClick(object sender, RoutedEventArgs e)
        {
            ViewModel.DoubleClickAction = DoubleClickAction.FindReferences;
            SetMenuChecks();
        }

        private void ChooseLayoutClick(object sender, RoutedEventArgs e)
        {
            var contextMenu = (ContextMenu)FindResource("LayoutContextMenu");
            contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            contextMenu.PlacementTarget = ChooseLayoutButton;
            contextMenu.IsOpen = true;
        }

        private void OpenFilteringDisplay(object sender, RoutedEventArgs e)
        {
            MainDisplay.Visibility = Visibility.Collapsed;
            FilteringDisplay.Visibility = Visibility.Visible;
        }

        private void CloseFilteringDisplay(object sender, RoutedEventArgs e)
        {
            MainDisplay.Visibility = Visibility.Visible;
            FilteringDisplay.Visibility = Visibility.Collapsed;
        }

        private void FitToDisplayClick(object sender, RoutedEventArgs e)
        {
            ViewModel.FitNodesToView(false);
        }

        private void ApplyLayoutClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ApplyLayout(true);
        }

        private void HorizontalGridClick(object sender, RoutedEventArgs e)
        {
            ViewModel.LayoutType = LayoutAlgorithmType.HorizontalBalancedGrid;
            ViewModel.ApplyLayout(true);
            SetMenuChecks();
        }

        private void VerticalGridClick(object sender, RoutedEventArgs e)
        {
            ViewModel.LayoutType = LayoutAlgorithmType.VerticalBalancedGrid;
            ViewModel.ApplyLayout(true);
            SetMenuChecks();
        }

        private void ForceDirectedClick(object sender, RoutedEventArgs e)
        {
            ViewModel.LayoutType = LayoutAlgorithmType.ForceDirected;
            ViewModel.ApplyLayout(true);
            SetMenuChecks();
        }

        private void NodeDoubleClicked(object sender, NodeEventArgs e)
        {
            if (e.Node is VFRNode vfrNode)
            {
                switch (ViewModel.DoubleClickAction)
                {
                    case DoubleClickAction.FindReferences:
                        var searchable = GetSearchableLocations(vfrNode).FirstOrDefault();
                        if (searchable != null)
                        {
                            GetSearchCommand(searchable).Execute(vfrNode);
                        }
                        break;
                    case DoubleClickAction.GoToCode:
                        var location = vfrNode.NodeFoundReferences.ReferencingLocations.FirstOrDefault();
                        if (location != null)
                        {
                            GetGoToLocation(location).Execute(vfrNode);
                        }
                        break;
                }
            }
        }
    }
}
