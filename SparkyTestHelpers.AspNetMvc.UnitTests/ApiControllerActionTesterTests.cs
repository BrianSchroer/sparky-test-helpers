using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetMvc.UnitTests.Controllers;
using SparkyTestHelpers.Scenarios;

namespace SparkyTestHelpers.AspNetMvc.UnitTests
{
    [TestClass]
    public class ApiConrollerActionTesterTests 
    {
        private TestApiController _controller;
        private ApiControllerTester<TestApiController> _controllerTester;

        [TestInitialize]
        public void TestInitialize()
        {
            _controller = new TestApiController();
            _controllerTester = _controller.CreateTester();
        }

        [TestMethod]
        public void WhenModelStateIsValidEquals_method_should_work_as_expected()
        {
            ForTest.Scenarios(true, false)
            .TestEach(isValid => 
            {
                bool result = _controllerTester.Action<bool>(x => x.BoolResultActionThatChecksModelState)
                    .WhenModelStateIsValidEquals(isValid)
                    .Test(response => response.Should().Be(isValid));

                result.Should().Be(isValid);
            });
        }

        [TestMethod]
        public void Test_action_without_parameters_should_work_as_expected()
        {
            var result = _controllerTester.Action<string>(x => x.StringResultActionWithoutArguments)
                .Test(response => response.Should().Be("test"));

            result.Should().Be("test");
        }

        [TestMethod]
        public void Test_action_with_parameters_should_work_as_expected()
        {
            var result = _controllerTester.Action<string>(x => () => x.StringResultActionWithArgument("yo"))
                .Test(response => response.Should().Be("yo"));

            result.Should().Be("yo");
        }
    }
} 