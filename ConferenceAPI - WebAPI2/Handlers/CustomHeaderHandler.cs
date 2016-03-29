using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConferenceAPI.Handlers
{
    public class CustomHeaderHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith((task) =>
            {
                // Here you can inspect/manipulate the outgoing response.
                var response = task.Result;
                response.Headers.Add("conference-header", "This is my custom header.");
                return response;
            });
        }
    }
}