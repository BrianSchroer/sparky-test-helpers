using System.Net;
using System.Web;

namespace SparkyTestHelpers.AspNetMvc.Routing.Mocks
{
    internal class MockHttpResponseBase : HttpResponseBase
    {
        private RedirectResults _redirectResults = new RedirectResults();

        public void ResetRedirectResults()
        {
            _redirectResults = new RedirectResults();
        }

        public void VerifyRedirectResults(string expectedRedirectUrl, HttpStatusCode httpStatusCode)
        {
            string statusCode = ((int)httpStatusCode).ToString();
            string actualRedirectUrl = _redirectResults.RedirectLocation ?? string.Empty;

            if (actualRedirectUrl != expectedRedirectUrl)
            {
                throw new RouteTesterException($"Expected RedirectLocation: <{expectedRedirectUrl}>. Actual: <{actualRedirectUrl}>.");
            }

            if (!_redirectResults.Status.StartsWith(statusCode))
            {
                throw new RouteTesterException($"Expected Status: <{statusCode} {httpStatusCode}>. Actual: <{_redirectResults.Status}>.");
            }

            if (!_redirectResults.EndWasCalled)
            {
                throw new RouteTesterException("HttpResponse.End() was not called.");
            }
        }

        public override string RedirectLocation
        {
            get => _redirectResults.RedirectLocation;
            set => _redirectResults.RedirectLocation = value;
        }

        public override string Status
        {
            get => _redirectResults.Status;
            set => _redirectResults.Status = value;
        }

        public override void End()
        {
            _redirectResults.EndWasCalled = true;
        }

        private class RedirectResults
        {
            public string RedirectLocation { get; set; }
            public string Status { get; set; }
            public bool EndWasCalled { get; set; }
        }
    }
}
