namespace SparkyTestHelpers.UnitTests.Moq
{
    public interface IMockable
    {
        void WithNoParms();

        void WithBoolean(bool input);

        void WithDecimal(decimal input);
    }
}
