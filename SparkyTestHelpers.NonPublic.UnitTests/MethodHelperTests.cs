using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SparkyTestHelpers.NonPublic.UnitTests
{
    [TestClass]
    public class NonPublicMethodTests 
    {
        private TestClass _testClass;

        [TestInitialize]
        public void TestInitialize()
        {
            _testClass = new TestClass();
        }

        [TestMethod]
        public void Method_Invoke_should_throw_MissingMethodException_for_bad_method_name()
        {
            Action action = () => _testClass.NonPublic().Method("BadName").Invoke();

            action.Should()
                .Throw<MissingMethodException>()
                .WithMessage("Method 'SparkyTestHelpers.NonPublic.UnitTests.TestClass.BadName()' not found.");
        }

        [TestMethod]
        public void Method_Invoke_should_work_for_InternalVoidMethod()
        {
            _testClass.NonPublic().Method("InternalVoidMethod").Invoke();
            _testClass.CalledMembers.Should().Contain("InternalVoidMethod");
        }

        [TestMethod]
        public void Method_Invoke_should_work_for_PrivateVoidMethod()
        {
            _testClass.NonPublic().Method("PrivateVoidMethod").Invoke();
            _testClass.CalledMembers.Should().Contain("PrivateVoidMethod");
        }

        [TestMethod]
        public void Method_Invoke_should_work_for_ProtectedVoidMethod()
        {
            _testClass.NonPublic().Method("ProtectedVoidMethod").Invoke();
            _testClass.CalledMembers.Should().Contain("ProtectedVoidMethod");
        }

        [TestMethod]
        public void Method_Invoke_should_work_for_PublicVoidMethod()
        {
            _testClass.NonPublic().Method("PublicVoidMethod").Invoke();
            _testClass.CalledMembers.Should().Contain("PublicVoidMethod");
        }

        [TestMethod]
        public void Method_Invoke_should_work_for_InternalStringMethod()
        {
            object response = _testClass.NonPublic().Method("InternalStringMethod").Invoke();
            response.Should().Be("success");
            _testClass.CalledMembers.Should().Contain("InternalStringMethod");
        }

        [TestMethod]
        public void Method_Invoke_should_work_for_PrivateStringMethod()
        {
            object response = _testClass.NonPublic().Method("PrivateStringMethod").Invoke();
            response.Should().Be("success");
            _testClass.CalledMembers.Should().Contain("PrivateStringMethod");
        }

        [TestMethod]
        public void Method_Invoke_should_work_for_ProtectedStringMethod()
        {
            object response = _testClass.NonPublic().Method("ProtectedStringMethod").Invoke();
            response.Should().Be("success");
            _testClass.CalledMembers.Should().Contain("ProtectedStringMethod");
        }

        [TestMethod]
        public void Method_Invoke_should_work_for_PublicStringMethod()
        {
            object response = _testClass.NonPublic().Method("PublicStringMethod").Invoke();
            response.Should().Be("success");
            _testClass.CalledMembers.Should().Contain("PublicStringMethod");
        }

        [TestMethod]
        public void Method_Invoke_TResult_should_InvalidCastException_for_incorrect_type()
        {
            Action action = () => _testClass.NonPublic().Method("PrivateStringMethod").Invoke<TestClass>();

            action.Should()
                .Throw<InvalidCastException>()
                .WithMessage("Invalid cast from 'System.String' to 'SparkyTestHelpers.NonPublic.UnitTests.TestClass'.");
        }

        [TestMethod]
        public void Method_Invoke_TResult_should_work_for_InternalStringMethod()
        {
            string response = _testClass.NonPublic().Method("InternalStringMethod").Invoke<string>();
            response.Should().Be("success");
            _testClass.CalledMembers.Should().Contain("InternalStringMethod");
        }

        [TestMethod]
        public void Method_Invoke_TResult_should_work_for_PrivateStringMethod()
        {
            string response = _testClass.NonPublic().Method("PrivateStringMethod").Invoke<string>();
            response.Should().Be("success");
            _testClass.CalledMembers.Should().Contain("PrivateStringMethod");
        }

        [TestMethod]
        public void Method_Invoke_TResult_should_work_for_ProtectedStringMethod()
        {
            string response = _testClass.NonPublic().Method("ProtectedStringMethod").Invoke<string>();
            response.Should().Be("success");
            _testClass.CalledMembers.Should().Contain("ProtectedStringMethod");
        }

        [TestMethod]
        public void Method_Invoke_TResult_should_work_for_PublicStringMethod()
        {
            string response = _testClass.NonPublic().Method("PublicStringMethod").Invoke<string>();
            response.Should().Be("success");
            _testClass.CalledMembers.Should().Contain("PublicStringMethod");
        }

        [TestMethod]
        public void Method_Invoke_with_args_should_work()
        {
            string response = _testClass.NonPublic().Method("PrivateMethodWithArgs").Invoke<string>(3, "test", DateTime.Now);
            response.Should().Be("success");
            _testClass.CalledMembers.Should().Contain("PrivateMethodWithArgs");
        }

        [TestMethod]
        public void Method_Invoke_with_wrong_type_args_should_throw_MissingMethodException()
        {
            Action action = () => _testClass.NonPublic().Method("PrivateMethodWithArgs").Invoke<string>(DateTime.Now, "test", 3);

            action.Should()
                .Throw<MissingMethodException>()
                .WithMessage("Method 'SparkyTestHelpers.NonPublic.UnitTests.TestClass.PrivateMethodWithArgs(System.DateTime, System.String, System.Int32)' not found.");
        }

        [TestMethod]
        public void Method_Invoke_with_overrides_should_work()
        {
            string response = _testClass.NonPublic().Method("PrivateMethodWithOverrides").Invoke<string>(3, "test");
            response.Should().Be("success");
            _testClass.CalledMembers.Should().Contain("int, string PrivateMethodWithOverrides");

            response = _testClass.NonPublic().Method("PrivateMethodWithOverrides").Invoke<string>("test", 3);
            response.Should().Be("success");
            _testClass.CalledMembers.Should().Contain("string, int PrivateMethodWithOverrides");
        }

        [TestMethod]
        public void Static_Method_Invoke_should_work()
        {
            _testClass.NonPublic().StaticMethod("PrivateStaticMethodWithOverrides").Invoke(3, "test");

            _testClass.NonPublic().StaticMethod("PrivateStaticMethodWithOverrides").Invoke("test", 3);
        }
    }
} 