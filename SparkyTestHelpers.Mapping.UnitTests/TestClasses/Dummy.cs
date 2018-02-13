using System;
using System.Collections.Generic;

namespace SparkyTestHelpers.Mapping.UnitTests.TestClasses
{
    public class Dummy
    {
        public string String { get; set; }
        public bool Bool { get; set; }
        public bool? NullableBool { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime? NullableDateTIme { get; set; }
        public decimal Decimal { get; set; }
        public decimal? NullableDecimal { get; set; }
        public double Double { get; set; }
        public double? NullableDouble { get; set; }
        public int Int { get; set; }
        public int? NullableInt { get; set; }
        public long Long { get; set; }
        public long? NullableLong { get; set; }
        public string[] StringArray { get; set; }
        public List<int> IntList { get; set; }
        public Dictionary<string, double> Dictionary { get; set; }
        public Source Source { get; set; }
        public StringComparison Enum { get; set; }
    }
}
