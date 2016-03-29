using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConferenceAPI.Responses
{
    public class ApiResult<T> : IHttpActionResult where T :class
    {
        private readonly HttpRequestMessage _request;
        private readonly T[] _data;


        public ApiResult(HttpRequestMessage request, params T[] data)
        {
            _request = request;
            _data = data;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            if (_data != null && _data.Any() && !_data.Contains(null))
            {
                var apiData = new ApiResultData<T>
                {
                    Count = _data.Count(),
                    RetrivalDate = DateTime.UtcNow,
                    Version = "v1",
                    Results = _data
                };
                response = _request.CreateResponse(HttpStatusCode.OK, apiData);
            }
            else
            {
                var httpErr = new HttpError();
                httpErr["message"] = "No Sessions were found that meet your criteria";
                response = _request.CreateErrorResponse(HttpStatusCode.NotFound, httpErr);
            }

            return Task.FromResult(response);
        }
    }

    public class ApiResultData<T>
    {
        public DateTime RetrivalDate { get; set; }
        public int Count { get; set; }
        public string Version { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}