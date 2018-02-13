namespace SparkyTestHelpers.Mapping.UnitTests
{
    public class Source
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] Children { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string SourceOnly { get; set; }
    }
}
