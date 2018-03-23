using System.Web;

namespace SparkyTestHelpers.AspNetMvc.Routing.Mocks
{
    internal class MockHttpContextBase : HttpContextBase
    {
        private readonly MockHttpRequestBase _httpRequestBase;
        private readonly MockHttpResponseBase _httpResponseBase;

        public MockHttpContextBase(MockHttpRequestBase httpRequestBase, MockHttpResponseBase httpResponseBase)
        {
            _httpRequestBase = httpRequestBase;
            _httpResponseBase = httpResponseBase;
        }

        public override HttpRequestBase Request => _httpRequestBase;
        public override HttpResponseBase Response => _httpResponseBase;
    }
}
