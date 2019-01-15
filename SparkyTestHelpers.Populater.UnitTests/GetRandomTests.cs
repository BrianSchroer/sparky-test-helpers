using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using FluentAssertions;
using SparkyTestHelpers.Populater.UnitTests.TestClasses;
using SparkyTestHelpers.Population;
using SparkyTestHelpers.Scenarios;

namespace SparkyTestHelpers.Populater.UnitTests
{
    /// <summary>
    /// <see cref="GetRandom"/> tests.
    /// </summary>
    [TestClass]
    public class GetRandomTests : RandomValueProvider
    {
        private readonly Guid _testGuid = Guid.NewGuid();

        public override Guid GetGuid() => _testGuid;

        [TestMethod]
        public void GetRandom_InstanceOf_should_work()
        {
            var testThing = GetRandom.InstanceOf<TestThing>();

            Console.WriteLine(
                JsonConvert.SerializeObject(
                    testThing,
                    Formatting.Indented,
                    new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy hh:mm:ss" },
                    new StringEnumConverter()));
        }

        [TestMethod]
        public void GetRandom_InstanceOf_with_RandomValueProvider_should_work()
        {
            var testThing = GetRandom.InstanceOf<TestThing>(this);

            testThing.Guid1.Should().Be(_testGuid);
            testThing.Guid2.Should().Be(_testGuid);
        }

        [TestMethod]
        public void GetRandom_InstanceOf_with_callback_should_work()
        {
            var testThing = GetRandom.InstanceOf<TestThing>(x => x.String1 = "test string");

            Assert.AreEqual("test string", testThing.String1);

            Console.WriteLine(
                JsonConvert.SerializeObject(
                    testThing,
                    Formatting.Indented,
                    new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy hh:mm:ss" },
                    new StringEnumConverter()));
        }

        [TestMethod]
        public void GetRandom_IEnumerableOf_should_work()
        {
            TestThing[] testThings = GetRandom.IEnumerableOf<TestThing>(7).ToArray();

            testThings.Length.Should().Be(7);
        }

        [TestMethod]
        public void GetRandom_IEnumerableOf_with_RandomValueProvider_should_work()
        {
            TestThing[] testThings = GetRandom.IEnumerableOf<TestThing>(this, 7).ToArray();

            testThings.Length.Should().Be(7);

            foreach (TestThing testThing in testThings)
            {
                testThing.Guid1.Should().Be(_testGuid);
                testThing.Guid2.Should().Be(_testGuid);
            }
        }

        [TestMethod]
        public void GetRandom_IEnumerableOf_with_callback_should_work()
        {
            TestThing[] testThings = GetRandom.IEnumerableOf<TestThing>(7, callback: thing => thing.String1 = "xyz").ToArray();

            testThings.Length.Should().Be(7);

            foreach (TestThing testThing in testThings)
            {
                testThing.String1.Should().Be("xyz");
            }
        }

        [TestMethod]
        public void GetRandom_InstanceOf_with_MaximumDepth_should_work()
        {
            int? maximumDepth = null;
            var result = GetRandom.InstanceOf<TestRecursiveThing>(maximumDepth: maximumDepth);
            Assert.IsNotNull(result.ChildRecursiveThing.ChildRecursiveThing.ChildRecursiveThing.ChildRecursiveThing, "No maximum depth");

            maximumDepth = 3;
            result = GetRandom.InstanceOf<TestRecursiveThing>(maximumDepth: maximumDepth);
            Assert.IsNull(result.ChildRecursiveThing.ChildRecursiveThing.ChildRecursiveThing, $"Maximum depth {maximumDepth}");

            maximumDepth = 2;
            result = GetRandom.InstanceOf<TestRecursiveThing>(maximumDepth: maximumDepth);
            Assert.IsNull(result.ChildRecursiveThing.ChildRecursiveThing, $"Maximum depth {maximumDepth}");

            maximumDepth = 1;
            result = GetRandom.InstanceOf<TestRecursiveThing>(maximumDepth: maximumDepth);
            Assert.IsNull(result.ChildRecursiveThing, $"Maximum depth {maximumDepth}");
        }

        [TestMethod]
        public void GetRandom_Bool_should_work()
        {
            bool randomValue = GetRandom.Bool();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_Byte_should_work()
        {
            byte randomValue = GetRandom.Byte();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_Char_should_work()
        {
            char randomValue = GetRandom.Char();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_DateTime_should_work()
        {
            DateTime randomValue = GetRandom.DateTime();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_Decimal_should_work()
        {
            decimal randomValue = GetRandom.Decimal();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_Double_should_work()
        {
            double randomValue = GetRandom.Double();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_EnumValue_should_work()
        {
            StringComparison randomValue = GetRandom.EnumValue<StringComparison>();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_Float_should_work()
        {
            float randomValue = GetRandom.Float();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_Guid_should_work()
        {
            Guid randomValue = GetRandom.Guid();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_Int_should_work()
        {
            int randomValue = GetRandom.Int();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_Long_should_work()
        {
            long randomValue = GetRandom.Long();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_Short_should_work()
        {
            short randomValue = GetRandom.Short();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_String_should_work()
        {
            string randomValue = GetRandom.String();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_String_with_prefix_should_work()
        {
            string randomValue = GetRandom.String("testPrefix");
            StringAssert.StartsWith(randomValue, "testPrefix");
        }

        [TestMethod]
        public void GetRandom_SByte_should_work()
        {
            sbyte randomValue = GetRandom.SByte();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_UInt_should_work()
        {
            uint randomValue = GetRandom.UInt();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_ULong_should_work()
        {
            ulong randomValue = GetRandom.ULong();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_UShort_should_work()
        {
            ushort randomValue = GetRandom.UShort();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void GetRandom_ValuesFor_should_work()
        {
            var testThing = GetRandom.ValuesFor(new TestThing());

            Console.WriteLine(
                JsonConvert.SerializeObject(
                    testThing,
                    Formatting.Indented,
                    new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy hh:mm:ss" },
                    new StringEnumConverter()));
        }

        [TestMethod]
        public void GetRandom_ValuesFor_with_RandomValueProvider_should_work()
        {
            var testThing = GetRandom.ValuesFor<TestThing>(new TestThing(), this);

            testThing.Guid1.Should().Be(_testGuid);
            testThing.Guid2.Should().Be(_testGuid);
        }

        [TestMethod]
        public void GetRandom_ValuesFor_with_MaximumDepth_should_work()
        {
            int? maximumDepth = null;
            var result = GetRandom.ValuesFor(new TestRecursiveThing(), maximumDepth: maximumDepth);
            Assert.IsNotNull(result.ChildRecursiveThing.ChildRecursiveThing.ChildRecursiveThing.ChildRecursiveThing, "No maximum depth");

            maximumDepth = 3;
            result = GetRandom.ValuesFor(new TestRecursiveThing(), maximumDepth: maximumDepth);
            Assert.IsNull(result.ChildRecursiveThing.ChildRecursiveThing.ChildRecursiveThing, $"Maximum depth {maximumDepth}");

            maximumDepth = 2;
            result = GetRandom.ValuesFor(new TestRecursiveThing(), maximumDepth: maximumDepth);
            Assert.IsNull(result.ChildRecursiveThing.ChildRecursiveThing, $"Maximum depth {maximumDepth}");

            maximumDepth = 1;
            result = GetRandom.ValuesFor(new TestRecursiveThing(), maximumDepth: maximumDepth);
            Assert.IsNull(result.ChildRecursiveThing, $"Maximum depth {maximumDepth}");
        }

        [TestMethod]
        public void GetRandom_IntegerInRange_should_work()
        {
            Enumerable.Range(1, 100).TestEach(_ =>
            {
                int value = GetRandom.IntegerInRange(1, 10);
                Console.WriteLine(value);
                value.Should().BeInRange(1, 10);
            });
        }
    }
}