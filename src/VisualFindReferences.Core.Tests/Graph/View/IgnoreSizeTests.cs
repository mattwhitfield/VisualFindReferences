namespace VisualFindReferences.Core.Tests.Graph.View
{
    using VisualFindReferences.Core.Graph.View;
    using Xunit;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Controls;
    using FluentAssertions;

    public class IgnoreSizeTests
    {
        private class TestIgnoreSize : IgnoreSize
        {
            public Size PublicMeasureOverride(Size constraint)
            {
                return base.MeasureOverride(constraint);
            }

            public Size PublicArrangeOverride(Size arrangeSize)
            {
                return base.ArrangeOverride(arrangeSize);
            }

            public Geometry PublicGetLayoutClip(Size layoutSlotSize)
            {
                return base.GetLayoutClip(layoutSlotSize);
            }
        }

        private TestIgnoreSize _testClass;

        public IgnoreSizeTests()
        {
            _testClass = new TestIgnoreSize();
            _testClass.Child = new TextBlock { Text = "hello" };
        }

        [StaFact]
        public void CanCallMeasureOverride()
        {
            // Arrange
            var constraint = new Size();

            // Act
            var result = _testClass.PublicMeasureOverride(constraint);

            // Assert
            result.Should().BeEquivalentTo(new Size(0, 0));
        }

        [StaFact]
        public void CanCallArrangeOverride()
        {
            // Arrange
            var arrangeSize = new Size();

            // Act
            var result = _testClass.PublicArrangeOverride(arrangeSize);

            // Assert
            result.Should().BeEquivalentTo(_testClass.Child.DesiredSize);
        }

        [StaFact]
        public void CanCallGetLayoutClip()
        {
            // Arrange
            var layoutSlotSize = new Size();

            // Act
            var result = _testClass.PublicGetLayoutClip(layoutSlotSize);

            // Assert
            result.Should().BeNull();
        }
    }
}