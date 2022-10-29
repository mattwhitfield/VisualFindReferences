using Microsoft.VisualStudio.Text.Editor;

namespace VisualFindReferences.Views
{
    public interface IVisualFindReferencesToolWindow
    {
        void FindReferences(IWpfTextView textView, IVisualFindReferencesPackage package);

        void Clear();
    }
}