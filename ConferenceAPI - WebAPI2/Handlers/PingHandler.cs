using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConferenceAPI.Handlers
{
    public class PingHandler : DelegatingHandler
    {
        private const string PingMe = "X-PingMe";
        private const string PingBack = "X-PingBack";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            if (request.Headers.Contains(PingMe))
            {
                var value = request.Headers.FirstOrDefault(v => v.Key.Equals(PingMe));
                var response = request.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add(PingBack, $"Hi {value}");

                return Task.FromResult(response);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}