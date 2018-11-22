using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SparkyTestHelpers.Mapping.UnitTests.TestClasses;
using System;

namespace SparkyTestHelpers.Mapping.UnitTests
{
    /// <summary>
    /// <see cref="RandomValuesHelper"/> tests.
    /// </summary>
    [TestClass]
    public class RandomValuesHelperTests
    {
        [TestMethod]
        public void CreateInstanceWithRandomValues_should_work()
        {
            var dummy = new RandomValuesHelper().CreateInstanceWithRandomValues<Dummy>();

            Console.WriteLine(
                JsonConvert.SerializeObject(
                    dummy,
                    Formatting.Indented,
                    new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy hh:mm:ss" },
                    new StringEnumConverter()));
        }

        [TestMethod]
        public void UpdatePropertiesWithRandomValues_should_work()
        {
            var dummy = new Dummy();
            new RandomValuesHelper().UpdatePropertiesWithRandomValues(dummy);

            Console.WriteLine(
                JsonConvert.SerializeObject(
                    dummy,
                    Formatting.Indented,
                    new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy hh:mm:ss" },
                    new StringEnumConverter()));
        }

        [TestMethod]
        public void WithMaximumDepth_should_work()
        {
            var helper = new RandomValuesHelper();

            var result = helper.CreateInstanceWithRandomValues<Depth1>();
            Assert.IsNotNull(result.Depth2.Depth3.Depth4, "No maximum depth");

            result = helper.WithMaximumDepth(3).CreateInstanceWithRandomValues<Depth1>();
            Assert.IsNull(result.Depth2.Depth3.Depth4, $"Maximum depth {helper.MaximumDepth}");

            result = helper.WithMaximumDepth(2).CreateInstanceWithRandomValues<Depth1>();
            Assert.IsNull(result.Depth2.Depth3, $"Maximum depth {helper.MaximumDepth}");

            result = helper.WithMaximumDepth(1).CreateInstanceWithRandomValues<Depth1>();
            Assert.IsNull(result.Depth2, $"Maximum depth {helper.MaximumDepth}");
        }

        [TestMethod]
        public void RandomEnumValue_should_work()
        {
            StringComparison randomValue = new RandomValuesHelper().RandomEnumValue<StringComparison>();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomBool_should_work()
        {
            bool randomValue = new RandomValuesHelper().RandomBool();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomByte_should_work()
        {
            byte randomValue = new RandomValuesHelper().RandomByte();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomChar_should_work()
        {
            char randomValue = new RandomValuesHelper().RandomChar();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomDateTime_should_work()
        {
            DateTime randomValue = new RandomValuesHelper().RandomDateTime();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomDecimal_should_work()
        {
            decimal randomValue = new RandomValuesHelper().RandomDecimal();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomDouble_should_work()
        {
            double randomValue = new RandomValuesHelper().RandomDouble();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomGuid_should_work()
        {
            Guid randomValue = new RandomValuesHelper().RandomGuid();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomInt_should_work()
        {
            int randomValue = new RandomValuesHelper().RandomInt();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomUInt_should_work()
        {
            uint randomValue = new RandomValuesHelper().RandomUInt();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomShort_should_work()
        {
            short randomValue = new RandomValuesHelper().RandomShort();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomUShort_should_work()
        {
            ushort randomValue = new RandomValuesHelper().RandomUShort();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomLong_should_work()
        {
            long randomValue = new RandomValuesHelper().RandomLong();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomULong_should_work()
        {
            ulong randomValue = new RandomValuesHelper().RandomULong();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomFloat_should_work()
        {
            float randomValue = new RandomValuesHelper().RandomFloat();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomSByte_should_work()
        {
            sbyte randomValue = new RandomValuesHelper().RandomSByte();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomString_should_work()
        {
            string randomValue = new RandomValuesHelper().RandomString();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void RandomString_with_prefix_should_work()
        {
            string randomValue = new RandomValuesHelper().RandomString("testPrefix");
            StringAssert.StartsWith(randomValue, "testPrefix");
        }
    }
}
