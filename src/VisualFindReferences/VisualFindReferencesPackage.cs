namespace VisualFindReferences
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using VisualFindReferences.Commands;
    using VisualFindReferences.Core.Graph.View;
    using VisualFindReferences.Options;
    using VisualFindReferences.Views;
    using Task = System.Threading.Tasks.Task;

    [ProvideAutoLoad(UIContextGuids.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(Constants.ExtensionGuid)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(GeneralOptions), "Visual Find References", "General Options", 0, 0, true)]
    [ProvideToolWindow(typeof(VisualFindReferencesToolWindow))]
    public sealed class VisualFindReferencesPackage : AsyncPackage, IVisualFindReferencesPackage
    {
        public GeneralOptions Options => (GeneralOptions)GetDialogPage(typeof(GeneralOptions));

        public IVisualFindReferencesToolWindow ShowToolWindow()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
#if VS2022
            StyleFinder.ResourceUri = "pack://application:,,,/VisualFindReferencesVS2022;component/views/generic.xaml";
#else
            StyleFinder.ResourceUri = "pack://application:,,,/VisualFindReferencesVS2019;component/views/generic.xaml";

#endif
            VisualFindReferencesToolWindow window = null;

            JoinableTaskFactory.RunAsync(async () =>
            {
                window = await ShowToolWindowAsync(typeof(VisualFindReferencesToolWindow), 0, true, DisposalToken) as VisualFindReferencesToolWindow;
                if (window?.Frame == null)
                {
                    throw new NotSupportedException("Cannot create Visual Find Reference tool window.");
                }
                var windowFrame = (IVsWindowFrame)window.Frame;
                ErrorHandler.ThrowOnFailure(windowFrame.Show());
            }).Join();

            return window;
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress).ConfigureAwait(true);

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            await FindReferencesForSymbolCommand.InitializeAsync(this).ConfigureAwait(true);
        }

        public override IVsAsyncToolWindowFactory GetAsyncToolWindowFactory(Guid toolWindowType)
        {
            return toolWindowType.Equals(Guid.Parse(VisualFindReferencesToolWindow.ToolWindowGuid)) ? this : null;
        }

        protected override string GetToolWindowTitle(Type toolWindowType, int id)
        {
            return toolWindowType == typeof(VisualFindReferencesToolWindow) ? VisualFindReferencesToolWindow.Title : base.GetToolWindowTitle(toolWindowType, id);
        }

        protected override Task<object> InitializeToolWindowAsync(Type toolWindowType, int id, CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(Options);
        }
    }
}