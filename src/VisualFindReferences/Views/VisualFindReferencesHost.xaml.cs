namespace VisualFindReferences.Views
{
    using Microsoft.CodeAnalysis;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.LanguageServices;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Input;
    using VisualFindReferences.Core.Graph.Helper;
    using VisualFindReferences.Core.Graph.Model;
    using VisualFindReferences.Core.Graph.Model.Nodes;
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
            GraphView.DataContext = ViewModel = Model.ViewModel;
        }

        public NodeGraphViewModel ViewModel { get; }

        public VFRNodeGraph Model { get; }

        private void NodeGraphViewNodeContextMenuRequested(object sender, Core.Graph.View.ContextMenuEventArgs e)
        {
            var contextMenu = new ContextMenu();

            contextMenu.Items.Add(new MenuItem { Header = "Delete node", Command = GetDeleteCommand(e.Node) });
            if (e.Node is VFRNode vfrNode)
            {
                if (vfrNode.ReferenceSearchAvailable)
                {
                    contextMenu.Items.Add(new Separator());
                    foreach (var searchableSymbol in vfrNode.GetSearchableSymbols())
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

        internal void SetPackage(VisualFindReferencesPackage visualFindReferencesPackage)
        {
            _package = visualFindReferencesPackage;
        }
    }
}
