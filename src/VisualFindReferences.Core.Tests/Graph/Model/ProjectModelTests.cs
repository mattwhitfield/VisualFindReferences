namespace VisualFindReferences.Core.Tests.Graph.Model
{
    using VisualFindReferences.Core.Graph.Model;
    using System;
    using Xunit;
    using FluentAssertions;

    public class ProjectModelTests
    {
        private ProjectModel _testClass;
        private string _projectName;
        private bool _isIncludedInSearches;
        private Guid _projectId;

        public ProjectModelTests()
        {
            _projectName = "TestValue691148629";
            _isIncludedInSearches = true;
            _projectId = new Guid("ef1a418b-9c83-43d2-87b2-5615015d1c3f");
            _testClass = new ProjectModel(_projectName, _isIncludedInSearches, _projectId);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new ProjectModel(_projectName, _isIncludedInSearches, _projectId);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void ProjectNameIsInitializedCorrectly()
        {
            _testClass.ProjectName.Should().Be(_projectName);
        }

        [Fact]
        public void ProjectIdIsInitializedCorrectly()
        {
            _testClass.ProjectId.Should().Be(_projectId);
        }

        [Fact]
        public void IsIncludedInSearchesIsInitializedCorrectly()
        {
            _testClass.IsIncludedInSearches.Should().Be(_isIncludedInSearches);
        }

        [Fact]
        public void CanSetAndGetIsIncludedInSearches()
        {
            _testClass.CheckProperty(x => x.IsIncludedInSearches);
        }
    }
}