namespace VisualFindReferences.Core.Tests.Graph.View
{
    using VisualFindReferences.Core.Graph.View;
    using System;
    using Xunit;
    using FluentAssertions;
    using System.ComponentModel;
    using VisualFindReferences.Core.Graph.ViewModel;
    using VisualFindReferences.Core.Graph.Model;

    public class ConnectorViewTests
    {
        private class TestConnectorView : ConnectorView
        {
            public TestConnectorView() : base()
            {
            }
        }

        private TestConnectorView _testClass;

        public ConnectorViewTests()
        {
            _testClass = new TestConnectorView();
        }

        [StaFact]
        public void CanConstruct()
        {
            // Act
            var instance = new TestConnectorView();

            // Assert
            instance.Should().NotBeNull();
        }

        [StaFact]
        public void PropertiesAreSynchronized()
        {
            _testClass.DataContext = new Connector(null, null, null).ViewModel;
            _testClass.ViewModel.IsHighlighted = false;
            _testClass.IsHighlighted.Should().BeFalse();
            _testClass.ViewModel.IsHighlighted = true;
            _testClass.IsHighlighted.Should().BeTrue();
            _testClass.ViewModel.IsBidirectional = false;
            _testClass.IsBidirectional.Should().BeFalse();
            _testClass.ViewModel.IsBidirectional = true;
            _testClass.IsBidirectional.Should().BeTrue();
        }

        [StaFact]
        public void CanSetAndGetCurveData()
        {
            // Arrange
            var testValue = "TestValue1015061489";

            // Act
            _testClass.CurveData = testValue;

            // Assert
            _testClass.CurveData.Should().Be(testValue);
        }

        [StaFact]
        public void CanSetAndGetIsHighlighted()
        {
            // Arrange
            var testValue = false;

            // Act
            _testClass.IsHighlighted = testValue;

            // Assert
            _testClass.IsHighlighted.Should().Be(testValue);
        }

        [StaFact]
        public void CanSetAndGetIsBidirectional()
        {
            // Arrange
            var testValue = true;

            // Act
            _testClass.IsBidirectional = testValue;

            // Assert
            _testClass.IsBidirectional.Should().Be(testValue);
        }
    }
}