// Temporary compile-time shim for xUnit attributes and Assert methods.
// This allows the test project to compile when the xUnit package references
// are not being properly resolved by MSBuild. The real xUnit runner will not
// execute these tests, but this unblocks the build.
// TODO: Remove this file once the xUnit package resolution issue is fixed.

namespace Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Temporary shim for the xUnit [Fact] attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class FactAttribute : Attribute
    {
    }

    /// <summary>
    /// Temporary shim for xUnit assertion methods.
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// Asserts that a condition is true.
        /// </summary>
        public static void True(bool condition)
        {
            if (!condition)
                throw new AssertionException("Assert.True failed. Condition was false.");
        }

        /// <summary>
        /// Asserts that two doubles are equal within a specified precision.
        /// </summary>
        public static void Equal(double expected, double actual, int precision)
        {
            double roundedExpected = Math.Round(expected, precision);
            double roundedActual = Math.Round(actual, precision);
            if (!roundedExpected.Equals(roundedActual))
            {
                throw new AssertionException(
                    $"Assert.Equal failed. Expected: {roundedExpected}, Actual: {roundedActual}");
            }
        }

        /// <summary>
        /// Asserts that two doubles are equal.
        /// </summary>
        public static void Equal(double expected, double actual)
        {
            if (!expected.Equals(actual))
            {
                throw new AssertionException(
                    $"Assert.Equal failed. Expected: {expected}, Actual: {actual}");
            }
        }

        /// <summary>
        /// Asserts that two strings are equal.
        /// </summary>
        public static void Equal(string? expected, string? actual)
        {
            if (!string.Equals(expected, actual, StringComparison.Ordinal))
            {
                throw new AssertionException(
                    $"Assert.Equal failed. Expected: \"{expected}\", Actual: \"{actual}\"");
            }
        }

        /// <summary>
        /// Asserts that an object is null.
        /// </summary>
        public static void Null(object? obj)
        {
            if (obj != null)
                throw new AssertionException($"Assert.Null failed. Object was: {obj}");
        }

        /// <summary>
        /// Asserts that an object is not null.
        /// </summary>
        public static void NotNull(object? obj)
        {
            if (obj == null)
                throw new AssertionException("Assert.NotNull failed. Object was null.");
        }

        /// <summary>
        /// Asserts that a collection contains exactly one element.
        /// </summary>
        public static void Single<T>(IEnumerable<T> collection)
        {
            if (collection == null) throw new AssertionException("Assert.Single failed. Collection was null.");
            int count = collection.Count();
            if (count != 1)
                throw new AssertionException($"Assert.Single failed. Expected 1 element, got {count}.");
        }

        /// <summary>
        /// Asserts that a collection is empty.
        /// </summary>
        public static void Empty<T>(IEnumerable<T> collection)
        {
            if (collection == null) throw new AssertionException("Assert.Empty failed. Collection was null.");
            int count = collection.Count();
            if (count != 0)
                throw new AssertionException($"Assert.Empty failed. Expected 0 elements, got {count}.");
        }
    }

    /// <summary>
    /// Temporary exception class for assertion failures.
    /// </summary>
    public class AssertionException : Exception
    {
        public AssertionException(string message) : base(message)
        {
        }
    }
}
