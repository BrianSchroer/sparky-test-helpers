using System;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Scenarios;
using SparkyTestHelpers.Moq;
using System.Collections.Generic;

namespace SparkyTestHelpers.Moq.UnitTests
{
    /// <summary>
    /// <see cref="MoqExtensions"/> tests.
    /// </summary>
    [TestClass]
    public partial class AnyTests
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
                _mock.ResetCalls();
                _test.WithArray(scenario);
                _mock.VerifyOneCallTo(x => x.WithArray(Any.Array<string>()));
            });
        }

        [TestMethod]
        public void Any_Boolean_should_work()
        {
            ForTest.Scenarios(true, false)
                .TestEach(scenario =>
                {
                    _mock.ResetCalls();
                    _test.WithBoolean(scenario);
                    _mock.VerifyOneCallTo(x => x.WithBoolean(Any.Boolean));
                });
        }

        [TestMethod]
        public void Any_DateTime_should_work()
        {
            ForTest.Scenarios(DateTime.Now, DateTime.Now.AddMonths(1))
                .TestEach(scenario =>
                {
                    _mock.ResetCalls();
                    _test.WithDateTime(scenario);
                    _mock.VerifyOneCallTo(x => x.WithDateTime(Any.DateTime));
                });
        }
        [TestMethod]
        public void Any_Decimal_should_work()
        {
            ForTest.Scenarios(1.0m, 2.0m)
                .TestEach(scenario =>
                {
                    _mock.ResetCalls();
                    _test.WithDecimal(scenario);
                    _mock.VerifyOneCallTo(x => x.WithDecimal(Any.Decimal));
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
                _mock.ResetCalls();
                _test.WithDictionary(scenario);
                _mock.VerifyOneCallTo(x => x.WithDictionary(Any.Dictionary<string, string>()));
            });
        }

        [TestMethod]
        public void Any_Double_should_work()
        {
            ForTest.Scenarios(1.0d, 2.0d)
                .TestEach(scenario =>
                {
                    _mock.ResetCalls();
                    _test.WithDouble(scenario);
                    _mock.VerifyOneCallTo(x => x.WithDouble(Any.Double));
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
                _mock.ResetCalls();
                _test.WithIEnumerable(scenario);
                _mock.VerifyOneCallTo(x => x.WithIEnumerable(Any.IEnumerable<string>()));
            });
        }

        [TestMethod]
        public void Any_Int_should_work()
        {
            ForTest.Scenarios(1, 2)
                .TestEach(scenario =>
                {
                    _mock.ResetCalls();
                    _test.WithInt(scenario);
                    _mock.VerifyOneCallTo(x => x.WithInt(Any.Int));
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
                _mock.ResetCalls();
                _test.WithKeyValuePair(scenario);
                _mock.VerifyOneCallTo(x => x.WithKeyValuePair(Any.KeyValuePair<string, string>()));
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
                _mock.ResetCalls();
                _test.WithList(scenario);
                _mock.VerifyOneCallTo(x => x.WithList(Any.List<string>()));
            });
        }

        [TestMethod]
        public void Any_Object_should_work()
        {
            ForTest.Scenarios(new { A = 1 }, new { A = 2 })
                .TestEach(scenario =>
                {
                    _mock.ResetCalls();
                    _test.WithObject(scenario);
                    _mock.VerifyOneCallTo(x => x.WithObject(Any.Object));
                });
        }

        [TestMethod]
        public void Any_String_should_work()
        {
            ForTest.Scenarios("string1", "string2")
                .TestEach(scenario =>
                {
                    _mock.ResetCalls();
                    _test.WithString(scenario);
                    _mock.VerifyOneCallTo(x => x.WithString(Any.String));
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
                _mock.ResetCalls();
                _test.WithTuple(scenario);
                _mock.VerifyOneCallTo(x => x.WithTuple(Any.Tuple<string, int>()));
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
                _mock.ResetCalls();
                _test.WithScenarioTester(scenario);
                _mock.VerifyOneCallTo(x => x.WithScenarioTester(Any.InstanceOf<ScenarioTester<string>>()));
            });
        }
    }
}
