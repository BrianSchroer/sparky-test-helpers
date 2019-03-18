using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Scenarios;

namespace SparkyTestHelpers.UnitTests.Scenarios
{
    [TestClass]
    public class ScenarioTests
    {
        [TestMethod]
        public void Scenario_Arrange_Assert_should_work_as_expected()
        {
            bool arrange1Called = false;
            bool arrange2Called = false;
            bool assert1Called = false;
            bool assert2Called = false;

            ForTest.Scenarios<TestClass1, TestClass2>()
                .Arrange(t1 => arrange1Called = true).Assert(t2 => assert1Called = true)
                .Arrange(t1 => arrange2Called = true).Assert(t2 => assert2Called = true)
            .TestEach(scenario =>
            {
                var test1 = new TestClass1();
                scenario.Arrange(test1);

                var test2 = new TestClass2();
                scenario.Assert(test2);
            });

            using (new AssertionScope())
            {
                arrange1Called.Should().BeTrue();
                assert1Called.Should().BeTrue();
                arrange2Called.Should().BeTrue();
                assert2Called.Should().BeTrue();
            }
        }

        [TestMethod]
        public void Scenario_Arrange_without_Assert_should_work_as_expected()
        {
            bool arrange1Called = false;
            bool arrange2Called = false;

            ForTest.Scenarios<TestClass1>()
                .Arrange(t1 => arrange1Called = true)
                .Arrange(t1 => arrange2Called = true)
            .TestEach(scenario =>
            {
                var test1 = new TestClass1();
                scenario.Arrange(test1);
            });

            using (new AssertionScope())
            {
                arrange1Called.Should().BeTrue();
                arrange2Called.Should().BeTrue();
            }
        }

        private class TestClass1
        {
        }

        private class TestClass2
        {
        }
    }
}
