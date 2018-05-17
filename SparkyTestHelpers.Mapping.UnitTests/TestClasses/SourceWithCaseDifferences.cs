using System;

namespace SparkyTestHelpers.Mapping.UnitTests.TestClasses
{
    public class SourceWithCaseDifferences
    {
        public int ID { get; set; }
        public string NaMe { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string[] Children { get; set; }
    }
}
