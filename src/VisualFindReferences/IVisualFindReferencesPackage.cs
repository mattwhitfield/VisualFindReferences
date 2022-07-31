namespace VisualFindReferences
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.LanguageServices;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Threading;
    using VisualFindReferences.Views;

    public interface IVisualFindReferencesPackage : IServiceProvider
    {
        JoinableTaskFactory JoinableTaskFactory { get; }

        VisualStudioWorkspace Workspace { get; }

        Task<object> GetServiceAsync(Type serviceType);

        IVisualFindReferencesToolWindow ShowToolWindow();
    }
}