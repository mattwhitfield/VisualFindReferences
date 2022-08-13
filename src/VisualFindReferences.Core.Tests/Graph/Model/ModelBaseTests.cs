namespace VisualFindReferences.Core.Tests.Graph.Model
{
    using VisualFindReferences.Core.Graph.Model;
    using Xunit;
    using FluentAssertions;

    public class ModelBaseTests
    {
        private ModelBase _testClass;

        public ModelBaseTests()
        {
            _testClass = new ModelBase();
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new ModelBase();

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanCallRaisePropertyChanged()
        {
            // Arrange
            var propertyName = "TestValue727140047";
            var prop = "";
            _testClass.PropertyChanged += (sender, e) => prop = e.PropertyName;

            // Act
            _testClass.RaisePropertyChanged(propertyName);

            // Assert
            prop.Should().Be(propertyName);
        }
    }
}