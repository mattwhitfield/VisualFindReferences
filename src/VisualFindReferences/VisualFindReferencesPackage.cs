namespace VisualFindReferences
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.ComponentModelHost;
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
            var window = FindToolWindow(typeof(VisualFindReferencesToolWindow), 0, true);
            if (window?.Frame == null)
            {
                throw new NotSupportedException("Cannot create Visual Find Reference tool window.");
            }
            var windowFrame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
            var toolWin = window as VisualFindReferencesToolWindow;
            toolWin?.SetPackage(this);
            return toolWin;
        }


        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress).ConfigureAwait(true);

            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

#pragma warning disable VSSDK006 // null check is present
            var componentModel = (IComponentModel)await GetServiceAsync(typeof(SComponentModel)).ConfigureAwait(true);
            if (componentModel == null)
            {
                throw new InvalidOperationException();
            }

            await FindReferencesForSymbolCommand.InitializeAsync(this).ConfigureAwait(true);
        }
    }
}