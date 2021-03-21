using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SparkyTestHelpers.NonPublic.UnitTests
{
    [TestClass]
    public class FieldHelperTests 
    {
        private TestClass _testClass;

        [TestInitialize]
        public void TestInitialize()
        {
            _testClass = new TestClass();
        }

        [TestMethod]
        public void Field_Get_should_throw_MissingFieldException_for_bad_Field_name()
        {
            Action action = () => _testClass.NonPublic().Field("BadFieldName").Get();

            action.Should()
                .Throw<MissingFieldException>()
                .WithMessage("Field 'SparkyTestHelpers.NonPublic.UnitTests.TestClass.BadFieldName' not found.");
        }

        [TestMethod]
        public void Field_Set_should_throw_MissingFieldException_for_bad_Field_name()
        {
            Action action = () => _testClass.NonPublic().Field("BadFieldName").Set("test");

            action.Should()
                .Throw<MissingFieldException>()
                .WithMessage("Field 'SparkyTestHelpers.NonPublic.UnitTests.TestClass.BadFieldName' not found.");
        }
        [TestMethod]
        public void Field_Set_and_Get_should_work_for_InternalStringField()
        {
            FieldHelper<TestClass> fieldHelper = _testClass.NonPublic().Field("InternalStringField");

            fieldHelper.Set("value");
            fieldHelper.Get().Should().Be("value");

            fieldHelper.Set("new value");
            fieldHelper.Get<string>().Should().Be("new value");
        }

        [TestMethod]
        public void Field_Set_and_Get_should_work_for_PrivateStringField()
        {
            FieldHelper<TestClass> fieldHelper = _testClass.NonPublic().Field("PrivateStringField");

            fieldHelper.Set("value");
            fieldHelper.Get().Should().Be("value");

            fieldHelper.Set("new value");
            fieldHelper.Get<string>().Should().Be("new value");
        }

        [TestMethod]
        public void Field_Set_and_Get_should_work_for_ProtectedStringField()
        {
            FieldHelper<TestClass> fieldHelper = _testClass.NonPublic().Field("ProtectedStringField");

            fieldHelper.Set("value");
            fieldHelper.Get().Should().Be("value");

            fieldHelper.Set("new value");
            fieldHelper.Get<string>().Should().Be("new value");
        }

        [TestMethod]
        public void Field_Set_and_Get_should_work_for_PublicStringField()
        {
            FieldHelper<TestClass> fieldHelper = _testClass.NonPublic().Field("PublicStringField");

            fieldHelper.Set("value");
            fieldHelper.Get().Should().Be("value");

            fieldHelper.Set("new value");
            fieldHelper.Get<string>().Should().Be("new value");
        }

        [TestMethod]
        public void StaticField_Get_and_Set_should_work()
        {
            FieldHelper<TestClass> fieldHelper = _testClass.NonPublic().StaticField("PrivateStaticStringField");

            fieldHelper.Set("value");
            fieldHelper.Get().Should().Be("value");

            fieldHelper.Set("new value");
            fieldHelper.Get<string>().Should().Be("new value");
        }
    }
} 