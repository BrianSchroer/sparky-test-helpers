using System.Web;

namespace SparkyTestHelpers.AspNetMvc.Routing.Mocks
{
    internal class MockHttpRequestBase : HttpRequestBase
    {
        public string AreaName { get; set; }

        public string RelativeUrl { get; set; }

        public override string RawUrl => $"~{AreaNameUrlPrefix}/{RelativeUrl}";

        public override string AppRelativeCurrentExecutionFilePath => RawUrl;

        public override string PathInfo => null;

        private string AreaNameUrlPrefix => (AreaName == null) ? null : $"/{AreaName}";
    }
}
