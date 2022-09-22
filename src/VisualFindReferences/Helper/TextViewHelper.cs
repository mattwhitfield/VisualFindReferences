using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Threading.Tasks;

namespace VisualFindReferences.Helper
{
    internal static class TextViewHelper
    {
        internal static IWpfTextView GetTextView(IServiceProvider serviceProvider)
        {
            var textManager = (IVsTextManager)serviceProvider.GetService(typeof(SVsTextManager));
            if (textManager != null)
            {
                textManager.GetActiveView(1, null, out var textView);

                var componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));
                var adapterService = componentModel?.GetService<Microsoft.VisualStudio.Editor.IVsEditorAdaptersFactoryService>();

                return adapterService?.GetWpfTextView(textView);
            }

            return null;
        }

        internal static async Task<(SyntaxNode, SemanticModel)> GetTargetSymbolAsync(ITextView textView, System.Threading.CancellationToken cancellation)
        {
            var caretPosition = textView.Caret.Position.BufferPosition;

            var document = caretPosition.Snapshot.GetOpenDocumentInCurrentContextWithChanges();
            if (document != null)
            {
                var syntaxNode = await document.GetSyntaxRootAsync(cancellation).ConfigureAwait(true);

                cancellation.ThrowIfCancellationRequested();

                var semanticModel = await document.GetSemanticModelAsync(cancellation).ConfigureAwait(true);

                cancellation.ThrowIfCancellationRequested();

                return (syntaxNode.FindToken(caretPosition).Parent, semanticModel);
            }

            return (null, null);
        }
    }
}
