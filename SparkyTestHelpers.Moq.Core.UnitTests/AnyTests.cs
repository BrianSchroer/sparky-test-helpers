using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;

namespace SparkyTestHelpers.Moq.Core.UnitTests
{
    /// <summary>
    /// <see cref="Any"/> tests.
    /// </summary>
    [TestClass]
    public class AnyTests
    {
        private Mock<IMockable> _mock;
        private IMockable _test;

        [TestInitialize]
        public void TestInitialize()
        {
            _mock = new Mock<IMockable>();
            _test = _mock.Object;
        }

        [TestMethod]
        public void Any_Array_should_work()
        {
            ForTest.Scenarios
            (
                new[] { "a", "b" },
                new[] { "c", "d" }
            )
            .TestEach(scenario =>
            {
                _mock.Invocations.Clear();
                _test.WithArray(scenario);
                _mock.Verify(x => x.WithArray(Any.Array<string>()), Times.Once);
            });
        }

        [TestMethod]
        public void Any_Boolean_should_work()
        {
            ForTest.Scenarios(true, false)
                .TestEach(scenario =>
                {
                    _mock.Invocations.Clear();
                    _test.WithBoolean(scenario);
                    _mock.Verify(x => x.WithBoolean(Any.Boolean), Times.Once);
                });
        }

        [TestMethod]
        public void Any_DateTime_should_work()
        {
            ForTest.Scenarios(DateTime.Now, DateTime.Now.AddMonths(1))
                .TestEach(scenario =>
                {
                    _mock.Invocations.Clear();
                    _test.WithDateTime(scenario);
                    _mock.Verify(x => x.WithDateTime(Any.DateTime), Times.Once);
                });
        }

        [TestMethod]
        public void Any_Decimal_should_work()
        {
            ForTest.Scenarios(1.0m, 2.0m)
                .TestEach(scenario =>
                {
                    _mock.Invocations.Clear();
                    _test.WithDecimal(scenario);
                    _mock.Verify(x => x.WithDecimal(Any.Decimal), Times.Once);
                });
        }

        [TestMethod]
        public void Any_Dictionary_should_work()
        {
            ForTest.Scenarios
            (
                new Dictionary<string, string>()
            )
            .TestEach(scenario =>
            {
                _mock.Invocations.Clear();
                _test.WithDictionary(scenario);
                _mock.Verify(x => x.WithDictionary(Any.Dictionary<string, string>()), Times.Once);
            });
        }

        [TestMethod]
        public void Any_Double_should_work()
        {
            ForTest.Scenarios(1.0d, 2.0d)
                .TestEach(scenario =>
                {
                    _mock.Invocations.Clear();
                    _test.WithDouble(scenario);
                    _mock.Verify(x => x.WithDouble(Any.Double), Times.Once);
                });
        }

        [TestMethod]
        public void Any_IEnumerable_should_work()
        {
            ForTest.Scenarios
            (
                new[] { "a", "b" },
                new[] { "c", "d" }
            )
            .TestEach(scenario =>
            {
                _mock.Invocations.Clear();
                _test.WithIEnumerable(scenario);
                _mock.Verify(x => x.WithIEnumerable(Any.IEnumerable<string>()), Times.Once);
            });
        }

        [TestMethod]
        public void Any_Int_should_work()
        {
            ForTest.Scenarios(1, 2)
                .TestEach(scenario =>
                {
                    _mock.Invocations.Clear();
                    _test.WithInt(scenario);
                    _mock.Verify(x => x.WithInt(Any.Int), Times.Once);
                });
        }

        [TestMethod]
        public void Any_KeyValuePair_should_work()
        {
            ForTest.Scenarios
            (
                new KeyValuePair<string, string>("key", "value")
            )
            .TestEach(scenario =>
            {
                _mock.Invocations.Clear();
                _test.WithKeyValuePair(scenario);
                _mock.Verify(x => x.WithKeyValuePair(Any.KeyValuePair<string, string>()), Times.Once);
            });
        }

        [TestMethod]
        public void Any_List_should_work()
        {
            ForTest.Scenarios
            (
                new List<string> { "a", "b" },
                new List<string> { "c", "d" }
            )
            .TestEach(scenario =>
            {
                _mock.Invocations.Clear();
                _test.WithList(scenario);
                _mock.Verify(x => x.WithList(Any.List<string>()), Times.Once);
            });
        }

        [TestMethod]
        public void Any_Object_should_work()
        {
            ForTest.Scenarios(new { A = 1 }, new { A = 2 })
                .TestEach(scenario =>
                {
                    _mock.Invocations.Clear();
                    _test.WithObject(scenario);
                    _mock.Verify(x => x.WithObject(Any.Object), Times.Once);
                });
        }

        [TestMethod]
        public void Any_Out_should_work()
        {
            _mock.Setup(x => x.WithOut(Any.Int, out Any.Out.Int)).Returns(true);

            bool response = _mock.Object.WithOut(3, out int outValue);

            Assert.AreEqual(true, response);
        }

        [TestMethod]
        public void Any_Ref_should_work()
        {
            _mock.Setup(x => x.WithRef(Any.String, ref Any.Ref.Int));

            int refValue = 7;
            AssertExceptionNotThrown.WhenExecuting(() => _mock.Object.WithRef("yo", ref refValue));
        }

        [TestMethod]
        public void Any_String_should_work()
        {
            ForTest.Scenarios("string1", "string2")
                .TestEach(scenario =>
                {
                    _mock.Invocations.Clear();
                    _test.WithString(scenario);
                    _mock.Verify(x => x.WithString(Any.String), Times.Once);
                });
        }

        [TestMethod]
        public void Any_Tuple_should_work()
        {
            ForTest.Scenarios
            (
                Tuple.Create("A", 1),
                Tuple.Create("B", 2)
            )
            .TestEach(scenario =>
            {
                _mock.Invocations.Clear();
                _test.WithTuple(scenario);
                _mock.Verify(x => x.WithTuple(Any.Tuple<string, int>()), Times.Once);
            });
        }

        [TestMethod]
        public void Any_InstanceOf_should_work()
        {
            ForTest.Scenarios
            (
                new ScenarioTester<string>(new[] { "a", "b" }),
                new ScenarioTester<string>(new[] { "c", "d" })
            )
            .TestEach(scenario =>
            {
                _mock.Invocations.Clear();
                _test.WithScenarioTester(scenario);
                _mock.Verify(x => x.WithScenarioTester(Any.InstanceOf<ScenarioTester<string>>()));
            });
        }
    }
}
