namespace VisualFindReferences
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.LanguageServices;
    using Microsoft.VisualStudio.Threading;
    using VisualFindReferences.Options;
    using VisualFindReferences.Views;

    public interface IVisualFindReferencesPackage : IServiceProvider
    {
        JoinableTaskFactory JoinableTaskFactory { get; }

        Task<object> GetServiceAsync(Type serviceType);

        IVisualFindReferencesToolWindow ShowToolWindow();

        GeneralOptions Options { get; }

        VisualStudioWorkspace Workspace { get; }
    }
}