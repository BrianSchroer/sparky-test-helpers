using System;
using System.Collections.Generic;

namespace SparkyTestHelpers.Mapping.UnitTests.TestClasses
{
    public class Dummy
    {
        public string[] StringArray { get; set; }
        public List<int> IntList { get; set; }
        public Dictionary<string, double> Dictionary { get; set; }
        public Source Source { get; set; }
        public StringComparison Enum { get; set; }
        public Guid Guid { get; set; }
        public Guid? NullableGuid { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime? NullableDateTime { get; set; }

        public bool Boolean { get; set; }
        public bool? NullableBoolean { get; set; }
        public byte Byte { get; set; }
        public byte[] ByteArray { get; set; }
        public byte? NullableByte { get; set; }
        public sbyte SByte { get; set; }
        public sbyte? NullableSByte { get; set; }
        public char Char { get; set; }
        public char? NullableChar { get; set; }
        public decimal Decimal { get; set; }
        public decimal? NullableDecimal { get; set; }
        public double Double { get; set; }
        public double? NullableDouble { get; set; }
        public float Single { get; set; }
        public float? NullableSingle { get; set; }
        public int Int { get; set; }
        public int? NullableInt { get; set; }
        public uint UInt { get; set; }
        public uint? NullableUInt { get; set; }
        public long Long { get; set; }
        public long? NullableLong { get; set; }
        public ulong Ulong { get; set; }
        public ulong? NullableUlong { get; set; }
        public object Object { get; set; }
        public short Short { get; set; }
        public short? NullableShort { get; set; }
        public ushort Ushort { get; set; }
        public ushort? NullableUshort { get; set; }
        public string String { get; set; }
        public Int16 Int16 { get; set; }
        public Int16? NullableInt16 { get; set; }
        public Int32 Int32 { get; set; }
        public Int32? NullableInt32 { get; set; }
        public Int64 Int64 { get; set; }
        public Int64? NullableInt64 { get; set; }
        public UInt16 UInt16 { get; set; }
        public UInt16? NullableUInt16 { get; set; }
        public UInt32 UInt32 { get; set; }
        public UInt32? NullableUInt32 { get; set; }
        public UInt64 UInt64 { get; set; }
        public UInt64? NullableUInt64 { get; set; }
    }
}
