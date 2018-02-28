using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Xml.Transformation;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;
using System;

namespace SparkyTestHelpers.Xml.UnitTests
{
    /// <summary>
    /// <see cref="PossiblePaths"/> tests.
    /// </summary>
    [TestClass]
    public class PossiblePathsTests 
    {
        [TestMethod]
        public void Constructor_should_throw_exception_if_no_paths_specified()
        {
            AssertExceptionThrown
                .OfType<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: paths")
                .WhenExecuting(() => new PossiblePaths());
        }

        [TestMethod]
        public void Constructor_should_not_throw_exception_with_one_or_more_paths_specified()
        {
            ForTest.Scenarios
            (
                new[] { "path1" },
                new[] { "path1", "path2" }
            )
            .TestEach(scenario => AssertExceptionNotThrown.WhenExecuting(() =>
            {
                var spec = new PossiblePaths(scenario);
                Assert.IsInstanceOfType(spec, typeof(PossiblePaths));
            }));
        }
    }
}
