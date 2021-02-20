using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SparkyTestHelpers.AspNetMvc.UnitTests.Controllers
{
    public class TestApiController : ApiController
    {
        public HttpResponseMessage ActionResponseMessage { get; set; }

        public IHttpActionResult HttpActionResult { get; set; }

        public HttpResponseMessage HttpResponseMessageActionWithoutArguments()
        {
            return ActionResponseMessage;
        }

        public HttpResponseMessage HttpResponseMessageActionWithArgument(string input)
        {
            return ActionResponseMessage;
        }

        public HttpResponseMessage HttpResponseMessageActionThatChecksModelState()
        {
            return new HttpResponseMessage { StatusCode = ModelState.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest };
        }

        public IHttpActionResult HttpActionResultActionWithoutArguments()
        {
            return HttpActionResult;
        }

        public IHttpActionResult HttpActionResultActionWithArgument(string input)
        {
            return HttpActionResult;
        }

        public IHttpActionResult HttpActionResultActionThatChecksModelState()
        {
            if (ModelState.IsValid) return Ok();

            return BadRequest();
        }
    }
}
