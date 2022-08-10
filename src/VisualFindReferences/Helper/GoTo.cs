using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;

namespace VisualFindReferences.Helper
{
    public static class GoTo
    {
        public static void Location(Location location, Document document)
        {
            var cm = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
            var tm = (IVsTextManager)Package.GetGlobalService(typeof(SVsTextManager));
            var ws = (Workspace)cm.GetService<VisualStudioWorkspace>();
            ws.OpenDocument(document.Id);
            tm.GetActiveView(1, null, out var av);
            var pos = location.GetMappedLineSpan();
            av.SetCaretPos(pos.StartLinePosition.Line, pos.StartLinePosition.Character);
        }
    }
}
