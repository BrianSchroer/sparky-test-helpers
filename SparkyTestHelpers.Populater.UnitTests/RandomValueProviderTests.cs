using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Population;

namespace SparkyTestHelpers.Populater.UnitTests
{
    /// <summary>
    /// <see cref="RandomValueProvider"/> unit tests.
    /// </summary>
    [TestClass]
    public class RandomValueProviderTests
    {
        private RandomValueProvider _provider;

        [TestInitialize]
        public void TestInitialize()
        {
            _provider = new RandomValueProvider();
        }

        [TestMethod]
        public void RandomValueProvider_GetBool_should_return_bool()
        {
            _provider.Invoking(p => p.GetBool()).Should().NotThrow();
        }

        [TestMethod]
        public void RandomValueProvider_GetByte_should_return_byte()
        {
            _provider.GetByte().Should().BeOfType(typeof(byte));
        }

        [TestMethod]
        public void RandomValueProvider_GetChar_should_return_char()
        {
            _provider.GetChar().Should().BeOfType(typeof(char));
        }

        [TestMethod]
        public void RandomValueProvider_GetDateTime_should_return_DateTime()
        {
            Assert.IsInstanceOfType(_provider.GetDateTime(), typeof(DateTime));
        }

        [TestMethod]
        public void RandomValueProvider_GetDecimal_should_return_decimal()
        {
            Assert.IsInstanceOfType(_provider.GetDecimal(), typeof(decimal));
        }

        [TestMethod]
        public void RandomValueProvider_GetDouble_should_return_double()
        {
            Assert.IsInstanceOfType(_provider.GetDouble(), typeof(double));
        }

        [TestMethod]
        public void RandomValueProvider_GetFloat_should_return_float()
        {
            Assert.IsInstanceOfType(_provider.GetFloat(), typeof(float));
        }

        [TestMethod]
        public void RandomValueProvider_GetGuid_should_return_Guid()
        {
            Assert.IsInstanceOfType(_provider.GetGuid(), typeof(Guid));
        }

        [TestMethod]
        public void RandomValueProvider_GetEnumerableSize_should_return_number_within_expected_range()
        {
            _provider.GetEnumerableSize().Should().BeInRange(1, _provider.MaximumIEnumerableSize);
        }

        [TestMethod]
        public void RandomValueProvider_GetInt_should_return_int()
        {
            Assert.IsInstanceOfType(_provider.GetInt(), typeof(int));
        }

        [TestMethod]
        public void RandomValueProvider_GetLong_should_return_long()
        {
            Assert.IsInstanceOfType(_provider.GetLong(), typeof(long));
        }

        [TestMethod]
        public void RandomValueProvider_GetSByte_should_return_sbyte()
        {
            Assert.IsInstanceOfType(_provider.GetSByte(), typeof(sbyte));
        }

        [TestMethod]
        public void RandomValueProvider_GetShort_should_return_short()
        {
            Assert.IsInstanceOfType(_provider.GetShort(), typeof(short));
        }

        [TestMethod]
        public void RandomValueProvider_GetString_should_return_string()
        {
            Assert.IsInstanceOfType(_provider.GetString(), typeof(string));
        }

        [TestMethod]
        public void RandomValueProvider_GetString_with_prefix_should_return_string_with_prefix()
        {
            _provider.GetString("prefix").Should().StartWith("prefix");
        }

        [TestMethod]
        public void RandomValueProvider_GetEnum_should_return_TEnum()
        {
            Assert.IsInstanceOfType(_provider.GetEnum<StringComparison>(), typeof(StringComparison));
        }

        [TestMethod]
        public void RandomValueProvider_GetUInt_should_return_uint()
        {
            Assert.IsInstanceOfType(_provider.GetUInt(), typeof(uint));
        }

        [TestMethod]
        public void RandomValueProvider_GetULong_should_return_ulong()
        {
            Assert.IsInstanceOfType(_provider.GetULong(), typeof(ulong));
        }

        [TestMethod]
        public void RandomValueProvider_GetUShort_should_return_ushort()
        {
            Assert.IsInstanceOfType(_provider.GetUShort(), typeof(ushort));
        }
    }
}
