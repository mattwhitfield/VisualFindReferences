namespace VisualFindReferences.Options
{
    using System.ComponentModel;
    using Microsoft.VisualStudio.Shell;
    using VisualFindReferences.Core.Graph.Layout;
    using VisualFindReferences.Core.Graph.ViewModel;

    public class GeneralOptions : DialogPage
    {
        [Category("Defaults")]
        [DisplayName("Automatic fit to display")]
        [Description("A default value for the automatic 'fit to display' option that governs whether the graph is resized after a search")]
        public bool DefaultFitToDisplay { get; set; } = true;

        [Category("Defaults")]
        [DisplayName("Project filter")]
        [Description("A default value for the project filter that specifies what projects to ignore while finding references")]
        public string DefaultProjectFilter { get; set; } = "*.Tests;*.Specs;*.UnitTests";

        [Category("Defaults")]
        [DisplayName("Double click action")]
        [Description("The default action to take when double clicking a node")]
        public DoubleClickAction DefaultDoubleClickAction { get; set; } = DoubleClickAction.GoToCode;

        [Category("Defaults")]
        [DisplayName("Layout type")]
        [Description("The default layout type to use when arranging nodes")]
        public LayoutAlgorithmType DefaultLayoutAlgorithmType { get; set; } = LayoutAlgorithmType.VerticalBalancedGrid;
    }
}