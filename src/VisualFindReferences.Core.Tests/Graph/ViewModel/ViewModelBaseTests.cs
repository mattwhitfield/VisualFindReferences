namespace VisualFindReferences.Core.Tests.Graph.ViewModel
{
    using VisualFindReferences.Core.Graph.ViewModel;
    using Xunit;
    using FluentAssertions;
    using VisualFindReferences.Core.Graph.Model;

    public class ViewModelBaseTests
    {
        private ViewModelBase _testClass;
        private ModelBase _model;

        public ViewModelBaseTests()
        {
            _model = new ModelBase();
            _testClass = new ViewModelBase(_model);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new ViewModelBase(_model);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanCallRaisePropertyChanged()
        {
            // Arrange
            var propertyName = "TestValue734531153";
            var prop = "";
            _testClass.PropertyChanged += (sender, e) => prop = e.PropertyName;

            // Act
            _testClass.RaisePropertyChanged(propertyName);

            // Assert
            prop.Should().Be(propertyName);
        }
    }
}