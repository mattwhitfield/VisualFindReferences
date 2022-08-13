namespace VisualFindReferences.Core.Tests.Graph.View
{
    using VisualFindReferences.Core.Graph.View;
    using System;
    using Xunit;
    using FluentAssertions;

    public class LoadingContainerTests
    {
        private LoadingContainer _testClass;

        public LoadingContainerTests()
        {
            _testClass = new LoadingContainer();
        }

        [StaFact]
        public void CanSetAndGetText()
        {
            // Arrange
            var testValue = "TestValue391779692";

            // Act
            _testClass.Text = testValue;

            // Assert
            _testClass.Text.Should().Be(testValue);
        }

        [StaFact]
        public void CanSetAndGetIsBusy()
        {
            // Arrange
            var testValue = true;

            // Act
            _testClass.IsBusy = testValue;

            // Assert
            _testClass.IsBusy.Should().Be(testValue);
        }
    }
}