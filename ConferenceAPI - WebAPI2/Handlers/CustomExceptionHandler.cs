using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace ConferenceAPI.Handlers
{
    public class CustomExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new CustomErrorResult(context.ExceptionContext.Request, "Sorry, something when wrong. Please try again later");
        }


        private class CustomErrorResult : IHttpActionResult
        {
            private HttpRequestMessage Request { get; set; }
            private string Content { get; set; }

            public CustomErrorResult(HttpRequestMessage request, string content)
            {
                Request = request;
                Content = content;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(Content),
                    RequestMessage = Request
                };

                return Task.FromResult(response);
            }
        }
    }
}