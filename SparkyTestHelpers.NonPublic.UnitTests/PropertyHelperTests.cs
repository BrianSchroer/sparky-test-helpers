using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SparkyTestHelpers.NonPublic.UnitTests
{
    [TestClass]
    public class NonPublicPropertyTests 
    {
        private TestClass _testClass;

        [TestInitialize]
        public void TestInitialize()
        {
            _testClass = new TestClass();
        }

        [TestMethod]
        public void Property_Get_should_throw_MissingMethodException_for_bad_property_name()
        {
            Action action = () => _testClass.NonPublic().Property("BadPropertyName").Get();

            action.Should()
                .Throw<MissingMethodException>()
                .WithMessage("Method 'SparkyTestHelpers.NonPublic.UnitTests.TestClass.BadPropertyName()' not found.");
        }

        [TestMethod]
        public void Property_Set_should_throw_MissingMethodException_for_bad_property_name()
        {
            Action action = () => _testClass.NonPublic().Property("BadPropertyName").Set("test");

            action.Should()
                .Throw<MissingMethodException>()
                .WithMessage("Method 'SparkyTestHelpers.NonPublic.UnitTests.TestClass.BadPropertyName()' not found.");
        }

        [TestMethod]
        public void Property_Set_and_Get_should_work_for_InternalStringProperty()
        {
            PropertyHelper<TestClass> propertyHelper = _testClass.NonPublic().Property("InternalStringProperty");

            propertyHelper.Set("new value");
            _testClass.CalledMembers.Should().Contain("Set InternalStringProperty");

            propertyHelper.Get().Should().Be("new value");
            _testClass.CalledMembers.Should().Contain("Get InternalStringProperty");

            _testClass.CalledMembers.Clear();
            propertyHelper.Get<string>().Should().Be("new value");
            _testClass.CalledMembers.Should().Contain("Get InternalStringProperty");
        }

        [TestMethod]
        public void Property_Set_and_Get_should_work_for_PrivateStringProperty()
        {
            PropertyHelper<TestClass> propertyHelper = _testClass.NonPublic().Property("PrivateStringProperty");

            propertyHelper.Set("new value");
            _testClass.CalledMembers.Should().Contain("Set PrivateStringProperty");

            propertyHelper.Get().Should().Be("new value");
            _testClass.CalledMembers.Should().Contain("Get PrivateStringProperty");

            _testClass.CalledMembers.Clear();
            propertyHelper.Get<string>().Should().Be("new value");
            _testClass.CalledMembers.Should().Contain("Get PrivateStringProperty");
        }

        [TestMethod]
        public void Property_Set_and_Get_should_work_for_ProtectedStringProperty()
        {
            PropertyHelper<TestClass> propertyHelper = _testClass.NonPublic().Property("ProtectedStringProperty");

            propertyHelper.Set("new value");
            _testClass.CalledMembers.Should().Contain("Set ProtectedStringProperty");

            propertyHelper.Get().Should().Be("new value");
            _testClass.CalledMembers.Should().Contain("Get ProtectedStringProperty");

            _testClass.CalledMembers.Clear();
            propertyHelper.Get<string>().Should().Be("new value");
            _testClass.CalledMembers.Should().Contain("Get ProtectedStringProperty");
        }

        [TestMethod]
        public void Property_Set_and_Get_should_work_for_PublicStringProperty()
        {
            PropertyHelper<TestClass> propertyHelper = _testClass.NonPublic().Property("PublicStringProperty");

            propertyHelper.Set("new value");
            _testClass.CalledMembers.Should().Contain("Set PublicStringProperty");

            propertyHelper.Get().Should().Be("new value");
            _testClass.CalledMembers.Should().Contain("Get PublicStringProperty");

            _testClass.CalledMembers.Clear();
            propertyHelper.Get<string>().Should().Be("new value");
            _testClass.CalledMembers.Should().Contain("Get PublicStringProperty");
        }

        [TestMethod]
        public void StaticProperty_Get_and_Set_should_work()
        {
            PropertyHelper<TestClass> propertyHelper = _testClass.NonPublic().StaticProperty("PrivateStaticStringProperty");

            propertyHelper.Set("value");
            propertyHelper.Get().Should().Be("value");

            propertyHelper.Set("new value");
            propertyHelper.Get<string>().Should().Be("new value");
        }
    }
} 