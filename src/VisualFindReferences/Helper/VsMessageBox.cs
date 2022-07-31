using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace VisualFindReferences.Helper
{
    public static class VsMessageBox
    {
        public static void Show(string message, bool isError, IVisualFindReferencesPackage package)
        {
            VsShellUtilities.ShowMessageBox(
                package,
                message,
                Constants.ExtensionName,
                isError ? OLEMSGICON.OLEMSGICON_CRITICAL : OLEMSGICON.OLEMSGICON_WARNING,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}
