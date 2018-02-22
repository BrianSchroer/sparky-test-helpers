namespace SparkyTestHelpers.XmlTransformation
{
    internal class DuplicateCheck
    {
        public string ElementExpression { get; private set; }
        public string KeyAttributeName { get; private set; }
        public string IgnoreKey { get; private set; }

        public DuplicateCheck(string elementExpression, string keyAttributeName, string ignoreKey = null)
        {
            ElementExpression = elementExpression;
            KeyAttributeName = keyAttributeName;
            IgnoreKey = ignoreKey;
        }
    }
}
