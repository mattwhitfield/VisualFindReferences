namespace VisualFindReferences.Core.Tests.Graph.Helper
{
    using VisualFindReferences.Core.Graph.Helper;
    using T = System.String;
    using System;
    using Xunit;
    using FluentAssertions;
    using System.Collections.Specialized;
    using System.Collections.Generic;

    public static class CollectionChangedHandlerTests
    {
        [Fact]
        public static void CanCallHandleCollectionChanged()
        {
            // Arrange
            var addedList = new List<string>();
            var removedList = new List<string>();

            var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new[] { "a", "b", "c" }, new[] { "b", "c", "d" });

            Action<T> added = x => addedList.Add(x);
            Action<T> removed = x => removedList.Add(x);

            // Act
            var result = CollectionChangedHandler.HandleCollectionChanged<T>(e, added, removed);

            // Assert
            addedList.Should().ContainSingle("a");
            removedList.Should().ContainSingle("d");
        }
    }
}