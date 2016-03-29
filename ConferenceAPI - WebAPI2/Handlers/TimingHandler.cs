using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConferenceAPI.Handlers
{
    public class TimingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            return base.SendAsync(request, cancellationToken).ContinueWith((task) =>
            {
                stopWatch.Stop();
                var response = task.Result;
                response.Headers.Add("ExecutionTime", stopWatch.ElapsedMilliseconds.ToString());
                return response;
            });
        }
    }
}