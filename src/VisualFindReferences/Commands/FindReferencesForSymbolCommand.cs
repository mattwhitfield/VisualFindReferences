namespace VisualFindReferences.Commands
{
    using System;
    using System.ComponentModel.Design;
    using Microsoft.VisualStudio.Shell;
    using VisualFindReferences;
    using VisualFindReferences.Helper;
    using Task = System.Threading.Tasks.Task;

    /// <summary>
    /// Command handler.
    /// </summary>
    internal sealed class FindReferencesForSymbolCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        private const int CommandId = 0x0102;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        private static readonly Guid CommandSet = new Guid("7f92c847-7ac9-4fab-8302-381e222bce9f");

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        private static FindReferencesForSymbolCommand _instance;

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly IVisualFindReferencesPackage _package;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoToUnitTestsForSymbolCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file).
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private FindReferencesForSymbolCommand(IVisualFindReferencesPackage package, OleMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandId = new CommandID(CommandSet, CommandId);

            var menuItem = new OleMenuCommand(Execute, menuCommandId);

            commandService.AddCommand(menuItem);
        }

        private IServiceProvider ServiceProvider => _package;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task InitializeAsync(IVisualFindReferencesPackage package)
        {
            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)).ConfigureAwait(true) as OleMenuCommandService;
            _instance = new FindReferencesForSymbolCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            Attempt.Action(() =>
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                var textView = TextViewHelper.GetTextView(ServiceProvider);
                if (textView == null)
                {
                    throw new InvalidOperationException("Could not find the text view");
                }
                var window = _package.ShowToolWindow();
                window.Clear();
                window.FindReferences(textView, _package);
            }, _package);
        }
    }
}