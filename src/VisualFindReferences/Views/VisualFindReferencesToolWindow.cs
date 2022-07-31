using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace VisualFindReferences.Views
{
    [Guid("f27b912a-9ce0-4e68-a8db-cdd2516793cd")]
    internal class VisualFindReferencesToolWindow : ToolWindowPane, IVisualFindReferencesToolWindow
    {
        private readonly VisualFindReferencesHost _host;

        public VisualFindReferencesToolWindow() :
            base(null)
        {
            Caption = "Visual Find References";

            base.Content = _host = new VisualFindReferencesHost();
        }

        public void FindReferences()
        {
        }
    }
}
