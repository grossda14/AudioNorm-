// Temporary compile-time shim for xUnit attributes and Assert methods.
// This allows the test project to compile when the xUnit package references
// are not being properly resolved by MSBuild. The real xUnit runner will not
// execute these tests, but this unblocks the build.
// TODO: Remove this file once the xUnit package resolution issue is fixed.

namespace Xunit
{
    using System;

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
