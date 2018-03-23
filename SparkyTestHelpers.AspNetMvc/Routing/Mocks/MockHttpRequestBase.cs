using System.Web;

namespace SparkyTestHelpers.AspNetMvc.Routing.Mocks
{
    internal class MockHttpRequestBase : HttpRequestBase
    {
        public string RelativeUrl { get; set; }

        public override string RawUrl => $"~/{RelativeUrl}";

        public override string AppRelativeCurrentExecutionFilePath => $"~/{RelativeUrl}";

        public override string PathInfo => null;
    }
}
