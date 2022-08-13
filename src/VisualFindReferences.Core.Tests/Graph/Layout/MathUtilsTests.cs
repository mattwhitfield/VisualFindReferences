namespace VisualFindReferences.Core.Tests.Graph.Layout
{
    using VisualFindReferences.Core.Graph.Layout;
    using Xunit;
    using FluentAssertions;

    public static class MathUtilsTests
    {
        [Fact]
        public static void CanCallIsZero()
        {
            MathUtils.IsZero(702496849.23).Should().BeFalse();
            MathUtils.IsZero(0.00000000000000001).Should().BeTrue();
        }

        [Fact]
        public static void CanCallNearEqualWithDoubleAndDouble()
        {
            MathUtils.NearEqual(702496849.23, 702496849.2300000001).Should().BeTrue();
            MathUtils.NearEqual(702496850.23, 702496849.2300000001).Should().BeFalse();
        }
    }
}