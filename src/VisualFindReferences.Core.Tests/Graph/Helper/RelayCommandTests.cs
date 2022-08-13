namespace VisualFindReferences.Core.Tests.Graph.Helper
{
    using VisualFindReferences.Core.Graph.Helper;
    using System;
    using Xunit;
    using FluentAssertions;

    public class RelayCommandTests
    {
        private RelayCommand _testClass;
        private Action _action;
        private bool _actionCalled;

        public RelayCommandTests()
        {
            _action = () => _actionCalled = true;
            _testClass = new RelayCommand(_action);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new RelayCommand(_action);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanCallCanExecute()
        {
            // Arrange
            var parameter = new object();

            // Act
            var result = _testClass.CanExecute(parameter);

            // Assert
            result.Should().BeTrue();

            _testClass.CanExecuteCommand = false;
            result = _testClass.CanExecute(parameter);
            result.Should().BeFalse();
        }

        [Fact]
        public void CanCallExecute()
        {
            // Arrange
            var parameter = new object();

            // Act
            _testClass.Execute(parameter);

            // Assert
            _actionCalled.Should().BeTrue();
        }

        [Fact]
        public void CanSetAndGetCanExecuteCommand()
        {
            // Arrange
            var testValue = true;

            // Act
            _testClass.CanExecuteCommand = testValue;

            // Assert
            _testClass.CanExecuteCommand.Should().Be(testValue);
        }
    }
}