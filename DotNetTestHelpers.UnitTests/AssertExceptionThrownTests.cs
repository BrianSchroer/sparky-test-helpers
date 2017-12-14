using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DotNetTestHelpers.Core.Exceptions;
using DotNetTestHelpers.Core.Scenarios;

namespace DotNetTestHelpers.UnitTests
{
    /// <summary>
    /// <see cref="AssertExceptionThrownTests" /> unit tests.
    /// </summary>
    [TestClass]
    public class AssertExceptionThrownTests
    {
        private static string[] _testExceptionMessages = new[]
        {
            "Whoops!",
            "Test exception message.",
            "Sentence 1. Sentence 2.",
            "Error .* Error!",
            "Error in line [1].",
            "Error in line (1).",
            "Are you sure? That doesn't look right!"
        };

        [TestMethod]
        public void OfType_factory_method_should_return_AssertExceptionThrown_instance()
        {
            AssertExceptionThrown instance = AssertExceptionThrown.OfType<Exception>();
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void OfTypeOrSubclassOfType_factory_method_should_return_AssertExceptionThrown_instance()
        {
            AssertExceptionThrown instance = AssertExceptionThrown.OfTypeOrSubclassOfType<Exception>();
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void WhenExecuting_should_throw_ExpectedExceptionNotThrownException_when_no_exception_is_thrown()
        {
            try
            {
                AssertExceptionThrown.OfType<InvalidOperationException>().WhenExecuting(() => { });
                Assert.Fail("Expected ExpectedExceptionNotThrownException was not thrown.");
            }
            catch (ExpectedExceptionNotThrownException ex)
            {
                Assert.AreEqual("Expected System.InvalidOperationException was not thrown.", ex.Message);
            }
        }

        [TestMethod]
        public void WhenExecuting_should_rethrow_caught_exception_when_not_expected_type()
        {
            var testException = new InvalidOperationException("fail!");

            try
            {
                AssertExceptionThrown.OfType<ArgumentOutOfRangeException>().WhenExecuting(() =>
                {
                    throw testException;
                });
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreSame(testException, ex);
            }
        }

        [TestMethod]
        public void WhenExecuting_should_rethrow_caught_exception_when_not_expected_type_or_subtype()
        {
            var testException = new InvalidOperationException("fail!");

            try
            {
                AssertExceptionThrown.OfTypeOrSubclassOfType<ArgumentException>().WhenExecuting(() =>
                {
                    throw testException;
                });
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual(testException, ex);
            }
        }

        [TestMethod]
        public void WhenExecuting_should_return_caught_exception_when_expected_exception_type_is_thrown()
        {
            var testException = new InvalidOperationException("fail!");

            Exception caughtException =
                AssertExceptionThrown.OfType<InvalidOperationException>()
                .WhenExecuting(() =>
                {
                    throw testException;
                });

            Assert.AreSame(testException, caughtException);
        }

        [TestMethod]
        public void WhenExecuting_should_return_caught_exception_when_expected_exception_type_or_subtype_is_thrown()
        {
            var testException = new ArgumentOutOfRangeException("fail!");

            Exception caughtException =
                AssertExceptionThrown.OfTypeOrSubclassOfType<ArgumentException>()
                .WhenExecuting(() =>
                {
                    throw testException;
                });

            Assert.AreSame(testException, caughtException);
        }

        [TestMethod]
        public void WithMessage_methods_should_only_be_callable_once()
        {
            try
            {
                AssertExceptionThrown.OfType<InvalidOperationException>()
                    .WithMessage("abc")
                    .WithMessageMatching("xyz");
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual("Only one \"WithMessage...\" call is allowed.", ex.Message);
            }
        }

        [TestMethod]
        public void WithMessage_should_handle_matched_message()
        {
            _testExceptionMessages.TestEach(message =>
            {
                AssertExceptionThrown
                    .OfType<InvalidOperationException>()
                    .WithMessage(message)
                    .WhenExecuting(() => throw new InvalidOperationException(message));
            });
        }

        [TestMethod]
        public void WithMessage_should_handle_unmatched_message()
        {
            const string message = "Crap";
            string expected =
                $"Expected message matching \"^Whoops$\". Actual: \"{message}\"."
                + "\n(message from System.InvalidOperationException.)";

            try
            {
                AssertExceptionThrown
                    .OfType<InvalidOperationException>()
                    .WithMessage("Whoops")
                    .WhenExecuting(() => throw new InvalidOperationException(message));
            }
            catch (ExpectedExceptionNotThrownException ex)
            {
                Assert.AreEqual(expected, ex.Message);
            }
        }

        [TestMethod]
        public void WithMessageStartingWith_should_handle_matched_message()
        {
            _testExceptionMessages.TestEach(message =>
            {
                AssertExceptionThrown
                    .OfType<InvalidOperationException>()
                    .WithMessageStartingWith(message)
                    .WhenExecuting(() => throw new InvalidOperationException($"{message} with suffix text"));
            });
        }

        [TestMethod]
        public void WithMessageStartingWith_should_handle_unmatched_message()
        {
            const string message = "This message doesn't start with Whoops!";

            string expected =
                $"Expected message matching \"^Whoops!.*$\". Actual: \"{message}\"."
                + "\n(message from System.InvalidOperationException.)";

            try
            {
                AssertExceptionThrown
                  .OfType<InvalidOperationException>()
                  .WithMessageStartingWith("Whoops!")
                  .WhenExecuting(() => throw new InvalidOperationException(message));
            }
            catch (ExpectedExceptionNotThrownException ex)
            {
                Assert.AreEqual(expected, ex.Message);
            }
        }

        [TestMethod]
        public void WithMessageContaining_should_handle_matched_message()
        {
            _testExceptionMessages.TestEach(message =>
            {
                AssertExceptionThrown
                    .OfType<InvalidOperationException>()
                    .WithMessageContaining(message)
                    .WhenExecuting(() => throw new InvalidOperationException($"blah blah {message} blah blah"));
            });
        }

        [TestMethod]
        public void WithMessageContaining_should_handle_unmatched_message()
        {
            const string message = "This is not the message you're looking for.";

            string expected =
                $"Expected message matching \"Whoops!\". Actual: \"{message}\"."
                + "\n(message from System.InvalidOperationException.)";

            try
            {
                AssertExceptionThrown
                  .OfType<InvalidOperationException>()
                  .WithMessageContaining("Whoops!")
                  .WhenExecuting(() => throw new InvalidOperationException(message));
            }
            catch (ExpectedExceptionNotThrownException ex)
            {
                Assert.AreEqual(expected, ex.Message);
            }
        }

        [TestMethod]
        public void WithMessageMatching_should_handle_matched_message()
        {
            ForTest.Scenarios
            (
              new { Message = "Something bad happened.", Pattern = @"^Something bad happened\.$" },
              new { Message = "Error 415.", Pattern = @"Error 4\d{2}" },
              new { Message = "Invalid operation. SAD!", Pattern = @".*SAD\!" }
            )
            .TestEach(scenario =>
            {
                AssertExceptionThrown
                    .OfType<InvalidOperationException>()
                    .WithMessageMatching(scenario.Pattern)
                    .WhenExecuting(() => throw new InvalidOperationException(scenario.Message));
            });
        }

        [TestMethod]
        public void WithMessageMatching_should_handle_unmatched_message()
        {
            ForTest.Scenarios
            (
              new { Message = "Something bad happened. ", Pattern = @"^Something bad happened\.$" },
              new { Message = "Error 45.", Pattern = @"Error 4\d{2}" },
              new { Message = "Invalid operation. SAD!", Pattern = @".*BAD\!" }
            )
            .TestEach(scenario =>
            {
                try
                {
                    AssertExceptionThrown
                        .OfType<InvalidOperationException>()
                        .WithMessageMatching(scenario.Pattern)
                        .WhenExecuting(() => throw new InvalidOperationException(scenario.Message));
                }
                catch (ExpectedExceptionNotThrownException ex)
                {
                    string expected = $"Expected message matching \"{scenario.Pattern}\". Actual: \"{scenario.Message}\"."
                        + "\n(message from System.InvalidOperationException.)";

                    Assert.AreEqual(expected, ex.Message);
                }
            });
        }
    }
}
