using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SparkyTestHelpers.Mapping.UnitTests.TestClasses;
using System;

namespace SparkyTestHelpers.Mapping.UnitTests
{
    /// <summary>
    /// <see cref="CreateRandom"/> tests.
    /// </summary>
    [TestClass]
    public class CreateRandomTests
    {
        [TestMethod]
        public void CreateRandom_InstanceOf_should_work()
        {
            var dummy = CreateRandom.InstanceOf<Dummy>();

            Console.WriteLine(
                JsonConvert.SerializeObject(
                    dummy,
                    Formatting.Indented,
                    new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy hh:mm:ss" },
                    new StringEnumConverter()));
        }

        [TestMethod]
        public void CreateRandom_InstanceOf_with_callback_should_work()
        {
            var dummy = CreateRandom.InstanceOf<Dummy>(x => x.String = "test string");

            Assert.AreEqual("test string", dummy.String);

            Console.WriteLine(
                JsonConvert.SerializeObject(
                    dummy,
                    Formatting.Indented,
                    new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy hh:mm:ss" },
                    new StringEnumConverter()));
        }

        [TestMethod]
        public void CreateRandom_InstanceOf_with_MaximumDepth_should_work()
        {
            int? maximumDepth = null;
            var result = CreateRandom.InstanceOf<Depth1>(maximumDepth: maximumDepth);
            Assert.IsNotNull(result.Depth2.Depth3.Depth4, "No maximum depth");

            maximumDepth = 3;
            result = CreateRandom.InstanceOf<Depth1>(maximumDepth: maximumDepth);
            Assert.IsNull(result.Depth2.Depth3.Depth4, $"Maximum depth {maximumDepth}");

            maximumDepth = 2;
            result = CreateRandom.InstanceOf<Depth1>(maximumDepth: maximumDepth);
            Assert.IsNull(result.Depth2.Depth3, $"Maximum depth {maximumDepth}");

            maximumDepth = 1;
            result = CreateRandom.InstanceOf<Depth1>(maximumDepth: maximumDepth);
            Assert.IsNull(result.Depth2, $"Maximum depth {maximumDepth}");
        }

        [TestMethod]
        public void CreateRandom_Bool_should_work()
        {
            bool randomValue = CreateRandom.Bool();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_Byte_should_work()
        {
            byte randomValue = CreateRandom.Byte();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_Char_should_work()
        {
            char randomValue = CreateRandom.Char();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_DateTime_should_work()
        {
            DateTime randomValue = CreateRandom.DateTime();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_Decimal_should_work()
        {
            decimal randomValue = CreateRandom.Decimal();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_Double_should_work()
        {
            double randomValue = CreateRandom.Double();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_EnumValue_should_work()
        {
            StringComparison randomValue = CreateRandom.EnumValue<StringComparison>();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_Float_should_work()
        {
            float randomValue = CreateRandom.Float();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_Guid_should_work()
        {
            Guid randomValue = CreateRandom.Guid();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_Int_should_work()
        {
            int randomValue = CreateRandom.Int();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_Long_should_work()
        {
            long randomValue = CreateRandom.Long();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_Short_should_work()
        {
            short randomValue = CreateRandom.Short();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_String_should_work()
        {
            string randomValue = CreateRandom.String();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_String_with_prefix_should_work()
        {
            string randomValue = CreateRandom.String("testPrefix");
            StringAssert.StartsWith(randomValue, "testPrefix");
        }

        [TestMethod]
        public void CreateRandom_SByte_should_work()
        {
            sbyte randomValue = CreateRandom.SByte();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_UInt_should_work()
        {
            uint randomValue = CreateRandom.UInt();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_ULong_should_work()
        {
            ulong randomValue = CreateRandom.ULong();
            Assert.IsNotNull(randomValue);
        }

        [TestMethod]
        public void CreateRandom_UShort_should_work()
        {
            ushort randomValue = CreateRandom.UShort();
            Assert.IsNotNull(randomValue);
        }
    }
}
