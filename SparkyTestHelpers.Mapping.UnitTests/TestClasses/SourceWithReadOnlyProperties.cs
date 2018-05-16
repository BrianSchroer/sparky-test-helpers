namespace SparkyTestHelpers.Mapping.UnitTests.TestClasses
{
    public class SourceWithReadOnlyProperties
    {
        public int Id => 123;
        public string Name => "Test Name";

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
    }
}
