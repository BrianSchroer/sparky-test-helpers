using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SparkyTestHelpers.Exceptions;

namespace SparkyTestHelpers.UnitTests
{
    /// <summary>
    /// <see cref="AssertExceptionNotThrown" /> unit tests.
    /// </summary>
    [TestClass]
    public class AssertExceptionNotThrownTests
    {
        [TestMethod]
        public void AssertExceptionNotThrown_WhenExecuting_should_be_successful_when_no_exception_is_thrown()
        {
            AssertExceptionNotThrown.WhenExecuting(() => { });
        }

        [TestMethod]
        public void AssertExceptionNotThrown_WhenExecuting_should_rethrow_caught_exception()
        {
            var exception = new InvalidOperationException("Testing AssertExceptionNotThrown");

            try
            {
                AssertExceptionNotThrown.WhenExecuting(() => throw exception);
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreSame(exception, ex);
            }
        }
    }
}
