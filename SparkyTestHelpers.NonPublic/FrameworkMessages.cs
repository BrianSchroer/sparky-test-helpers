namespace SparkyTestHelpers.NonPublic
{
    /// <summary>
    /// Messages used by <see cref="NonPublicMembers" /> - Copied from Microsoft.VisualStudio.TestTools.UnitTesting (https://github.com/microsoft/testfx).
    /// </summary>
    internal static class FrameworkMessages
    {
        internal static string AccessStringInvalidSyntax = "Access string has invalid syntax";
        internal static string PrivateAccessorConstructorNotFound = "The constructor with the specified signature could not be found. You might need to regenerate your private accessor, or the member may be private and defined on a base class. If the latter is true, you need to pass the type that defines the member into PrivateObject's constructor.";
        internal static string PrivateAccessorMemberNotFound = "The member specified ({0}) could not be found. You might need to regenerate your private accessor, or the member may be private and defined on a base class. If the latter is true, you need to pass the type that defines the member into PrivateObject's constructor.";
    }
} 