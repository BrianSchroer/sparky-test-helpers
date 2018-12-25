using System;
using System.Text;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SparkyTestHelpers.Populater.UnitTests
{
    /// <summary>
    /// <see cref="SequentialValueProvider"/> unit tests.
    /// </summary>
    [TestClass]
    public class SequentialValueProviderTests
    {
        private SequentialValueProvider _provider;

        [TestInitialize]
        public void TestInitialize()
        {
            _provider = new SequentialValueProvider();
        }

        [TestMethod]
        public void SequentialValueProvider_GetBool_should_return_expected_values()
        {
            _provider.GetBool().Should().BeFalse();
            _provider.GetBool().Should().BeTrue();
            _provider.GetBool().Should().BeFalse();
            _provider.GetBool().Should().BeTrue();
        }

        [TestMethod]
        public void SequentialValueProvider_GetByte_should_return_byte()
        {
            _provider.GetByte().Should().Be(Encoding.ASCII.GetBytes("1")[0]);
            _provider.GetByte().Should().Be(Encoding.ASCII.GetBytes("2")[0]);
        }

        [TestMethod]
        public void SequentialValueProvider_GetChar_should_return_char()
        {
            _provider.GetChar().Should().Be('1');
            _provider.GetChar().Should().Be('2');
        }

        [TestMethod]
        public void SequentialValueProvider_GetDateTime_should_return_DateTime_using_BaseDate()
        {
            DateTime baseDate = 8.August(2018);
            _provider.BaseDate = baseDate;

            _provider.GetDateTime().Should().Be(baseDate.AddDays(1));
            _provider.GetDateTime().Should().Be(baseDate.AddDays(2));
        }

        [TestMethod]
        public void SequentialValueProvid_BaseDate_should_default_to_20190101()
        {
            DateTime baseDate = _provider.BaseDate;

            baseDate.Should().Be(1.January(2019));

            _provider.GetDateTime().Should().Be(baseDate.AddDays(1));
            _provider.GetDateTime().Should().Be(baseDate.AddDays(2));
        }

        [TestMethod]
        public void SequentialValueProvider_GetDecimal_should_return_decimal()
        {
            _provider.GetDecimal().Should().Be(1.01m);
            _provider.GetDecimal().Should().Be(2.02m);

            _provider.SetValue(98);
            _provider.GetDecimal().Should().Be(99.99m);
            _provider.GetDecimal().Should().Be(100.00m);
            _provider.GetDecimal().Should().Be(101.01m);

            _provider.SetValue(123456);
            _provider.GetDecimal().Should().Be(123457.57m);
        }

        [TestMethod]
        public void SequentialValueProvider_GetDouble_should_return_double()
        {
            _provider.GetDouble().Should().Be(1.01d);
            _provider.GetDouble().Should().Be(2.02d);

            _provider.SetValue(98);
            _provider.GetDouble().Should().Be(99.99d);
            _provider.GetDouble().Should().Be(100.00d);
            _provider.GetDouble().Should().Be(101.01d);
        }

        [TestMethod]
        public void SequentialValueProvider_GetFloat_should_return_float()
        {
            _provider.GetFloat().Should().Be(1.01f);
            _provider.GetFloat().Should().Be(2.02f);

            _provider.SetValue(98);
            _provider.GetFloat().Should().Be(99.99f);
            _provider.GetFloat().Should().Be(100.00f);
            _provider.GetFloat().Should().Be(101.01f);
        }

        [TestMethod]
        public void SequentialValueProvider_GetGuid_should_return_Guid()
        {
            _provider.GetGuid().ToString().Should().Be("00000000-0000-0000-0000-000000000001");
            _provider.GetGuid().ToString().Should().Be("00000000-0000-0000-0000-000000000002");
        }

        [TestMethod]
        public void SequentialValueProvider_GetEnumerableSize_should_return_EnumerableSize()
        {
            _provider.GetEnumerableSize().Should().Be(_provider.EnumerableSize);
        }

        [TestMethod]
        public void SequentialValueProvider_GetInt_should_return_int()
        {
            _provider.GetInt().Should().Be(1);
            _provider.GetInt().Should().Be(2);
            _provider.GetInt().Should().Be(3);
            _provider.GetInt().Should().Be(4);

            _provider.SetValue(int.MaxValue);
            _provider.GetInt().Should().Be(1);
        }

        [TestMethod]
        public void SequentialValueProvider_GetLong_should_return_long()
        {
            _provider.GetLong().Should().Be(1);
            _provider.GetLong().Should().Be(2);
            _provider.GetLong().Should().Be(3);
            _provider.GetLong().Should().Be(4);

            _provider.SetValue(int.MaxValue);
            _provider.GetLong().Should().Be(1);
        }

        [TestMethod]
        public void SequentialValueProvider_GetSByte_should_return_sbyte()
        {
            _provider.GetSByte().Should().Be(Convert.ToSByte(1));
            _provider.GetSByte().Should().Be(Convert.ToSByte(2));

            _provider.SetValue(SByte.MaxValue);
            _provider.GetSByte().Should().Be(Convert.ToSByte(1));
        }

        [TestMethod]
        public void SequentialValueProvider_GetShort_should_return_short()
        {
            _provider.GetShort().Should().Be(Convert.ToInt16(1));
            _provider.GetShort().Should().Be(Convert.ToInt16(2));

            _provider.SetValue(Int16.MaxValue);
            _provider.GetShort().Should().Be(Convert.ToInt16(1));
        }

        [TestMethod]
        public void SequentialValueProvider_GetString_should_return_string()
        {
            _provider.GetString().Should().Be("00000000-0000-0000-0000-000000000001");
            _provider.GetString().Should().Be("00000000-0000-0000-0000-000000000002");
        }

        [TestMethod]
        public void SequentialValueProvider_GetString_with_prefix_should_return_string_with_prefix()
        {
            _provider.GetString("prefix").Should().Be("prefix-1");
            _provider.GetString("prefix").Should().Be("prefix-2");
        }

        [TestMethod]
        public void SequentialValueProvider_GetEnum_should_return_TEnum()
        {
            _provider.GetEnum<StringComparison>().Should().Be(StringComparison.CurrentCulture);
            _provider.GetEnum<StringComparison>().Should().Be(StringComparison.CurrentCultureIgnoreCase);
            _provider.GetEnum<StringComparison>().Should().Be(StringComparison.InvariantCulture);
            _provider.GetEnum<StringComparison>().Should().Be(StringComparison.InvariantCultureIgnoreCase);
            _provider.GetEnum<StringComparison>().Should().Be(StringComparison.Ordinal);
            _provider.GetEnum<StringComparison>().Should().Be(StringComparison.OrdinalIgnoreCase);

            _provider.GetEnum<StringComparison>().Should().Be(StringComparison.CurrentCulture);
        }

        [TestMethod]
        public void SequentialValueProvider_GetUInt_should_return_uint()
        {
            _provider.GetUInt().Should().Be(Convert.ToUInt32(1));
            _provider.GetUInt().Should().Be(Convert.ToUInt32(2));

            _provider.SetValue(Int32.MaxValue);
            _provider.GetUInt().Should().Be(Convert.ToUInt32(1));
        }

        [TestMethod]
        public void SequentialValueProvider_GetULong_should_return_ulong()
        {
            _provider.GetULong().Should().Be(Convert.ToUInt64(1));
            _provider.GetULong().Should().Be(Convert.ToUInt64(2));

            _provider.SetValue(Int32.MaxValue);
            _provider.GetULong().Should().Be(Convert.ToUInt64(1));
        }

        [TestMethod]
        public void SequentialValueProvider_GetUShort_should_return_ushort()
        {
            _provider.GetUShort().Should().Be(Convert.ToUInt16(1));
            _provider.GetUShort().Should().Be(Convert.ToUInt16(2));

            _provider.SetValue(UInt16.MaxValue);
            _provider.GetUShort().Should().Be(Convert.ToUInt16(1));
        }
    }
}
