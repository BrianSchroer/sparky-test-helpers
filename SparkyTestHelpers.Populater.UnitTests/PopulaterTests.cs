using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SparkyTestHelpers.Populater.UnitTests.TestClasses;
using SparkyTestHelpers.Population;

namespace SparkyTestHelpers.Populater.UnitTests
{
    /// <summary>
    /// <see cref="Populater"/> unit tests.
    /// </summary>
    [TestClass]
    public class PopulaterTests
    {
        private Population.Populater _populater;

        private const string _testThingJson = "{\"String1\":\"String1-1\",\"String2\":\"String2-2\",\"Int1\":3,\"Int2\":4,\"Decimal1\":5.05,\"Decimal2\":6.06,\"DateTime1\":\"2019-01-08T00:00:00\",\"NullableDateTime\":\"2019-01-09T00:00:00\",\"ChildThing\":{\"String1\":\"String1-9\",\"String2\":\"String2-10\",\"Int1\":11,\"Int2\":12,\"Decimal1\":13.13,\"Decimal2\":14.14,\"DateTime1\":\"2019-01-16T00:00:00\",\"NullableDateTime\":\"2019-01-17T00:00:00\"},\"ChildrenThings\":[{\"String1\":\"String1-17\",\"String2\":\"String2-18\",\"Int1\":19,\"Int2\":20,\"Decimal1\":21.21,\"Decimal2\":22.22,\"DateTime1\":\"2019-01-24T00:00:00\",\"NullableDateTime\":\"2019-01-25T00:00:00\"},{\"String1\":\"String1-25\",\"String2\":\"String2-26\",\"Int1\":27,\"Int2\":28,\"Decimal1\":29.29,\"Decimal2\":30.30,\"DateTime1\":\"2019-02-01T00:00:00\",\"NullableDateTime\":\"2019-02-02T00:00:00\"},{\"String1\":\"String1-33\",\"String2\":\"String2-34\",\"Int1\":35,\"Int2\":36,\"Decimal1\":37.37,\"Decimal2\":38.38,\"DateTime1\":\"2019-02-09T00:00:00\",\"NullableDateTime\":\"2019-02-10T00:00:00\"}],\"ChildList\":[{\"String1\":\"String1-41\",\"String2\":\"String2-42\",\"Int1\":43,\"Int2\":44,\"Decimal1\":45.45,\"Decimal2\":46.46,\"DateTime1\":\"2019-02-17T00:00:00\",\"NullableDateTime\":\"2019-02-18T00:00:00\"},{\"String1\":\"String1-49\",\"String2\":\"String2-50\",\"Int1\":51,\"Int2\":52,\"Decimal1\":53.53,\"Decimal2\":54.54,\"DateTime1\":\"2019-02-25T00:00:00\",\"NullableDateTime\":\"2019-02-26T00:00:00\"},{\"String1\":\"String1-57\",\"String2\":\"String2-58\",\"Int1\":59,\"Int2\":60,\"Decimal1\":61.61,\"Decimal2\":62.62,\"DateTime1\":\"2019-03-05T00:00:00\",\"NullableDateTime\":\"2019-03-06T00:00:00\"}],\"ChildEnumerable\":[{\"String1\":\"String1-65\",\"String2\":\"String2-66\",\"Int1\":67,\"Int2\":68,\"Decimal1\":69.69,\"Decimal2\":70.70,\"DateTime1\":\"2019-03-13T00:00:00\",\"NullableDateTime\":\"2019-03-14T00:00:00\"},{\"String1\":\"String1-73\",\"String2\":\"String2-74\",\"Int1\":75,\"Int2\":76,\"Decimal1\":77.77,\"Decimal2\":78.78,\"DateTime1\":\"2019-03-21T00:00:00\",\"NullableDateTime\":\"2019-03-22T00:00:00\"},{\"String1\":\"String1-81\",\"String2\":\"String2-82\",\"Int1\":83,\"Int2\":84,\"Decimal1\":85.85,\"Decimal2\":86.86,\"DateTime1\":\"2019-03-29T00:00:00\",\"NullableDateTime\":\"2019-03-30T00:00:00\"}],\"Bool1\":false,\"Bool2\":true,\"Byte1\":49,\"Byte2\":50,\"ByteArray\":\"MDAwMDAwMDAtMDAwMC0wMDAwLTAwMDAtMDAwMDAwMDAwMDkz\",\"Char1\":\"9\",\"Char2\":\"9\",\"Double1\":96.96,\"Double2\":97.97,\"Float1\":98.98,\"Float2\":99.99,\"Guid1\":\"00000000-0000-0000-0000-000000000100\",\"Guid2\":\"00000000-0000-0000-0000-000000000101\",\"Long1\":102,\"Long2\":103,\"Object1\":\"Object1-104\",\"Sbyte1\":105,\"Sbyte2\":106,\"Short1\":107,\"Short2\":108,\"Uint1\":109,\"Uint2\":110,\"Ulong1\":111,\"Ulong2\":112,\"Ushort1\":113,\"Ushort2\":114}";

        [TestInitialize]
        public void TestInitialize()
        {
            _populater = new Population.Populater();
        }

        [TestMethod]
        public void Populater_Populate_with_RandomValueProvider_should_populate_with_random_values()
        {
            var testThing = new TestThing();
            _populater.Populate(testThing, new RandomValueProvider());

            using (new AssertionScope())
            {
                AssertPopulatedTestThingProperties(testThing);
            }
        }

        [TestMethod]
        public void Populater_Populate_with_SequentialValueProvider_should_populate_with_sequential_values()
        {
            var testThing = new TestThing();
            _populater.Populate(testThing, new SequentialValueProvider());

            using (new AssertionScope())
            {
                AssertSequentialTestThingProperties(testThing);
            }
        }

        [TestMethod]
        public void Populater_PopulateWithRandomValues_should_populate_with_random_values()
        {
            var testThing = new TestThing();
            _populater.PopulateWithRandomValues(testThing);

            using (new AssertionScope())
            {
                AssertPopulatedTestThingProperties(testThing);
            }
        }

        [TestMethod]
        public void Populater_Populate_should_populate_with_sequential_values()
        {
            var testThing = new TestThing();
            _populater.Populate(testThing);

            using (new AssertionScope())
            {
                AssertSequentialTestThingProperties(testThing);
            }
        }

        [TestMethod]
        public void Populater_CreateRandom_should_create_instance_with_random_values()
        {
            var testThing = _populater.CreateRandom<TestThing>();

            using (new AssertionScope())
            {
                AssertPopulatedTestThingProperties(testThing);
            }
        }

        [TestMethod]
        public void Populater_CreateRandom_should_not_return_the_same_values_each_time()
        {
            var testThing = _populater.CreateRandom<TestThing>();
            var testThing2 = _populater.CreateRandom<TestThing>();

            testThing2.Int1.Should().NotBe(testThing.Int1);
        }

        [TestMethod]
        public void Populater_CreateAndPopulate_with_RandomValueProvider_should_create_instance_with_random_values()
        {
            var testThing = _populater.CreateAndPopulate<TestThing>(new RandomValueProvider());

            using (new AssertionScope())
            {
                AssertPopulatedTestThingProperties(testThing);
            }
        }

        [TestMethod]
        public void Populater_CreateIEnumerableOf_should_create_IEnumerable()
        {
            TestThing[] testThings = _populater.CreateIEnumerableOf<TestThing>(7).ToArray();

            using (new AssertionScope())
            {
                testThings.Length.Should().Be(7);

                AssertSequentialTestThingProperties(testThings[0]);

                foreach (TestThing testThing in testThings)
                {
                    AssertPopulatedTestThingProperties(testThing);
                }
            }
        }

        [TestMethod]
        public void Populater_CreateIEnumerableOf_with_RandomValuesProvider_should_create_IEnumerable()
        {
            TestThing[] testThings = _populater.CreateIEnumerableOf<TestThing>(7, new RandomValueProvider()).ToArray();

            using (new AssertionScope())
            {
                testThings.Length.Should().Be(7);

                foreach (TestThing testThing in testThings)
                {
                    AssertPopulatedTestThingProperties(testThing);
                }
            }
        }

        [TestMethod]
        public void Populater_CreateRandomArrayOf_should_create_array()
        {
            TestThing[] testThings = _populater.CreateRandomArrayOf<TestThing>(4);

            using (new AssertionScope())
            {
                testThings.Length.Should().Be(4);

                foreach (TestThing testThing in testThings)
                {
                    AssertPopulatedTestThingProperties(testThing);
                }
            }
        }

        [TestMethod]
        public void Populater_CreateRandomArrayOf_with_callback_should_work()
        {
            TestThing[] testThings = _populater.CreateRandomArrayOf<TestThing>(4, x => x.String1 = "abc");

            using (new AssertionScope())
            {
                testThings.Length.Should().Be(4);

                foreach (TestThing testThing in testThings)
                {
                    AssertPopulatedTestThingProperties(testThing);
                    testThing.String1.Should().Be("abc");
                }
            }
        }

        [TestMethod]
        public void Populater_CreateRandomIEnumerableOf_should_create_IEnumerable()
        {
            TestThing[] testThings = _populater.CreateRandomIEnumerableOf<TestThing>(4).ToArray();

            using (new AssertionScope())
            {
                testThings.Length.Should().Be(4);

                foreach (TestThing testThing in testThings)
                {
                    AssertPopulatedTestThingProperties(testThing);
                }
            }
        }

        [TestMethod]
        public void Populater_CreateRandomIEnumerableOf_with_callback_should_work()
        {
            TestThing[] testThings = _populater.CreateRandomIEnumerableOf<TestThing>(4, x => x.String1 = "abc").ToArray();

            using (new AssertionScope())
            {
                testThings.Length.Should().Be(4);

                foreach (TestThing testThing in testThings)
                {
                    AssertPopulatedTestThingProperties(testThing);
                    testThing.String1.Should().Be("abc");
                }
            }
        }

        [TestMethod]
        public void Populater_CreateRandomIListOf_should_create_IEnumerable()
        {
            IList<TestThing> testThings = _populater.CreateRandomIListOf<TestThing>(4);

            using (new AssertionScope())
            {
                testThings.Count.Should().Be(4);

                foreach (TestThing testThing in testThings)
                {
                    AssertPopulatedTestThingProperties(testThing);
                }
            }
        }

        [TestMethod]
        public void Populater_CreateRandomIListOf_with_callback_should_work()
        {
            IList<TestThing> testThings = _populater.CreateRandomIListOf<TestThing>(4, x => x.String1 = "abc");

            using (new AssertionScope())
            {
                testThings.Count.Should().Be(4);

                foreach (TestThing testThing in testThings)
                {
                    AssertPopulatedTestThingProperties(testThing);
                    testThing.String1.Should().Be("abc");
                }
            }
        }

        [TestMethod]
        public void Populater_CreateRandomListOf_should_create_IEnumerable()
        {
            List<TestThing> testThings = _populater.CreateRandomListOf<TestThing>(4);

            using (new AssertionScope())
            {
                testThings.Count.Should().Be(4);

                foreach (TestThing testThing in testThings)
                {
                    AssertPopulatedTestThingProperties(testThing);
                }
            }
        }

        [TestMethod]
        public void Populater_CreateRandomListOf_with_callback_should_work()
        {
            List<TestThing> testThings = _populater.CreateRandomListOf<TestThing>(4, x => x.String1 = "abc");

            using (new AssertionScope())
            {
                testThings.Count.Should().Be(4);

                foreach (TestThing testThing in testThings)
                {
                    AssertPopulatedTestThingProperties(testThing);
                    testThing.String1.Should().Be("abc");
                }
            }
        }

        [TestMethod]
        public void Populater_CreateAndPopulate_should_create_instance_with_sequential_values()
        {
            var testThing = _populater.CreateAndPopulate<TestThing>();

            using (new AssertionScope())
            {
                AssertSequentialTestThingProperties(testThing);
            }
        }

        [TestMethod]
        public void Populater_CreateAndPopulate_with_SequentialValueProvider_should_create_instance_with_sequential_values()
        {
            var testThing = _populater.CreateAndPopulate<TestThing>(new SequentialValueProvider());

            using (new AssertionScope())
            {
                AssertSequentialTestThingProperties(testThing);
            }
        }

        [TestMethod]
        public void Populater_WithMaximumDepth_should_return_hierarchy_with_specified_maximum_depth()
        {
            int maximumDepth = 3;

            TestRecursiveThing recursiveThing = new Population.Populater().WithMaximumDepth(maximumDepth).CreateAndPopulate<TestRecursiveThing>();

            AssertExpectedMaximumDepth(recursiveThing, maximumDepth);
        }

        [TestMethod]
        public void Populater_MaximumDepth_should_default_to_5()
        {
            _populater.MaximumDepth.Should().Be(5);

            TestRecursiveThing recursiveThing = _populater.CreateAndPopulate<TestRecursiveThing>();

            AssertExpectedMaximumDepth(recursiveThing, 5);
        }

        private static void AssertExpectedMaximumDepth(TestRecursiveThing recursiveThing, int maximumDepth)
        {
            int count = 1;

            while (recursiveThing.ChildRecursiveThing != null)
            {
                count++;
                recursiveThing = recursiveThing.ChildRecursiveThing;
            }

            count.Should().Be(maximumDepth);
        }

        private void AssertSequentialTestThingProperties(TestThing testThing)
        {
            AssertPopulatedTestThingProperties(testThing);

            string json = JsonConvert.SerializeObject(testThing);

            json.Should().Be(_testThingJson);
        }

        private void AssertPopulatedTestThingProperties(TestThing testThing)
        {
            testThing.DateTime1.Should().NotBe(DateTime.MinValue);
            testThing.Decimal1.Should().NotBe(0);
            testThing.Decimal2.Should().NotBe(0);
            testThing.Int1.Should().NotBe(0);
            testThing.Int2.Should().NotBe(0);
            testThing.NullableDateTime.Should().NotBeNull().And.NotBe(DateTime.MinValue);
            testThing.String1.Should().NotBeNullOrWhiteSpace();
            testThing.String2.Should().NotBeNullOrWhiteSpace();

            AssertPopulatedTestChildThingProperties(testThing.ChildThing);

            testThing.ChildrenThings.Should().NotBeNull().And.HaveCountGreaterOrEqualTo(1);

            foreach (TestChildThing child in testThing.ChildrenThings)
            {
                AssertPopulatedTestChildThingProperties(child);
            }
        }

        private void AssertPopulatedTestChildThingProperties(TestChildThing testChildThing)
        {
            testChildThing.DateTime1.Should().NotBe(DateTime.MinValue);
            testChildThing.Decimal1.Should().NotBe(0);
            testChildThing.Decimal2.Should().NotBe(0);
            testChildThing.Int1.Should().NotBe(0);
            testChildThing.Int2.Should().NotBe(0);
            testChildThing.NullableDateTime.Should().NotBeNull().And.NotBe(DateTime.MinValue);
        }
    }
}