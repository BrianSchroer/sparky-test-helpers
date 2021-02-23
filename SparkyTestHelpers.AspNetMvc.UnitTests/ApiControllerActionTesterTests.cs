using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetMvc.UnitTests.Controllers;
using SparkyTestHelpers.Scenarios;
using System.Collections.Specialized;

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
        public void WithRequestQueryStringValues_should_build_expected_uri_with_siteUrlPrefix_and_with_QueryStringParameters()
        {
            _controllerTester
                .Action<string>(x => x.StringResultActionWithoutArguments)
                .WithRequestQueryStringValues("http://fake.com", new QueryStringParameter("parm1", "value1"), new QueryStringParameter("parm2", 2));

            _controller.Request.RequestUri.ToString().Should().Be("http://fake.com/?parm1=value1&parm2=2");
        }

        [TestMethod]
        public void WithRequestQueryStringValues_should_build_expected_uri_without_siteUrlPrefix_and_with_QueryStringParameters()
        {
            _controllerTester
                .Action<string>(x => x.StringResultActionWithoutArguments)
                .WithRequestQueryStringValues(new QueryStringParameter("parm1", "value1"), new QueryStringParameter("parm2", 2));

            _controller.Request.RequestUri.ToString().Should().Be("http://localhost/?parm1=value1&parm2=2");
        }

        [TestMethod]
        public void WithRequestQueryStringValues_should_build_expected_uri_with_siteUrlPrefix_and_with_NameValueCollection()
        {
            _controllerTester
                .Action<string>(x => x.StringResultActionWithoutArguments)
                .WithRequestQueryStringValues(
                    "http://fake.com", new NameValueCollection { { "parm1", "value1" }, { "parm2", "2" } });

            _controller.Request.RequestUri.ToString().Should().Be("http://fake.com/?parm1=value1&parm2=2");
        }

        [TestMethod]
        public void WithRequestQueryStringValues_should_build_expected_uri_without_siteUrlPrefix_and_with_NameValueCollection()
        {
            _controllerTester
                .Action<string>(x => x.StringResultActionWithoutArguments)
                .WithRequestQueryStringValues(new NameValueCollection { { "parm1", "value1" }, { "parm2", "2" } });

            _controller.Request.RequestUri.ToString().Should().Be("http://localhost/?parm1=value1&parm2=2");
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