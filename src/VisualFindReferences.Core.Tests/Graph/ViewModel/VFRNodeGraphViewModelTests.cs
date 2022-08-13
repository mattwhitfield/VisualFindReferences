namespace VisualFindReferences.Core.Tests.Graph.ViewModel
{
    using VisualFindReferences.Core.Graph.ViewModel;
    using T = System.String;
    using System;
    using Xunit;
    using FluentAssertions;
    using System.Collections.ObjectModel;
    using VisualFindReferences.Core.Graph.Model;

    public class VFRNodeGraphViewModelTests
    {
        private class TestVFRNodeGraphViewModel : VFRNodeGraphViewModel
        {
            public TestVFRNodeGraphViewModel(VFRNodeGraph nodeGraph) : base(nodeGraph)
            {
            }

            public void PublicHandleContinuation<T>(T result)
            {
                base.HandleContinuation<T>(result);
            }
        }

        private TestVFRNodeGraphViewModel _testClass;
        private VFRNodeGraph _nodeGraph;

        public VFRNodeGraphViewModelTests()
        {
            _nodeGraph = new VFRNodeGraph();
            _testClass = new TestVFRNodeGraphViewModel(_nodeGraph);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new TestVFRNodeGraphViewModel(_nodeGraph);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanSetAndGetProjectFilterMatchPattern()
        {
            _testClass.CheckProperty(x => x.ProjectFilterMatchPattern);
        }

        [Fact]
        public void CanSetAndGetDoubleClickAction()
        {
            _testClass.CheckProperty(x => x.DoubleClickAction, DoubleClickAction.FindReferences, DoubleClickAction.GoToCode);
        }

        [Fact]
        public void CanGetProjects()
        {
            // Assert
            _testClass.Projects.Should().BeAssignableTo<ObservableCollection<ProjectModel>>();
        }

        [Fact]
        public void CanSetAndGetFilteredReferencesMessage()
        {
            _testClass.CheckProperty(x => x.FilteredReferencesMessage);

            _testClass.FilteredReferencesMessage = string.Empty;
            _testClass.ShowFilteredReferencesPrompt.Should().BeFalse();
            _testClass.FilteredReferencesMessage = "ref";
            _testClass.ShowFilteredReferencesPrompt.Should().BeTrue();
        }
    }
}