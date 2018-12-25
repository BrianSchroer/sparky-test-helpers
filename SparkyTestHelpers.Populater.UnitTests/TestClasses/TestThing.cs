using System;
using System.Collections.Generic;

namespace SparkyTestHelpers.Populater.UnitTests.TestClasses
{
    public class TestThing
    {
        public string String1 { get; set; }
        public string String2 { get; set; }
        public int Int1 { get; set; }
        public int Int2 { get; set; }
        public decimal Decimal1 { get; set; }
        public decimal Decimal2 { get; set; }
        public DateTime DateTime1 { get; set; }
        public DateTime? NullableDateTime { get; set; }
        public TestChildThing ChildThing { get; set; }
        public TestChildThing[] ChildrenThings { get; set; }

        public List<TestChildThing> ChildList { get; set; }
        public IEnumerable<TestChildThing> ChildEnumerable { get; set; }

        public bool Bool1 { get; set; }
        public bool? Bool2 { get; set; }
        public byte Byte1 { get; set; }
        public byte? Byte2 { get; set; }
        public byte[] ByteArray { get; set; }
        public char Char1 { get; set; }
        public char? Char2 { get; set; }
        public double Double1 { get; set; }
        public double? Double2 { get; set; }
        public float Float1 { get; set; }
        public float? Float2 { get; set; }
        public Guid Guid1 { get; set; }
        public Guid? Guid2 { get; set; }
        public long Long1 { get; set; }
        public long? Long2 { get; set; }
        public object Object1 { get; set; }
        public sbyte Sbyte1 { get; set; }
        public sbyte? Sbyte2 { get; set; }
        public short Short1 { get; set; }
        public short? Short2 { get; set; }
        public uint Uint1 { get; set; }
        public uint? Uint2 { get; set; }
        public ulong Ulong1 { get; set; }
        public ulong? Ulong2 { get; set; }
        public ushort Ushort1 { get; set; }
        public ushort? Ushort2 { get; set; }
    }
}