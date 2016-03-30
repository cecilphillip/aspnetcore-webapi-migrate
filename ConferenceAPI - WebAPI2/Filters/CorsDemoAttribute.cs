using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace ConferenceAPI.Filters
{
    public class CorsDemoAttribute : Attribute, ICorsPolicyProvider
    {     
        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Go grab your policy settings from  your config files, data store, etc
            // You can inspect the incoming request to determine how it match it to policy

            var policy = new CorsPolicy() {
                AllowAnyHeader = true,
                AllowAnyMethod = true                
            };

            policy.Origins.Add("https://my-cool-site.com");            

            return Task.FromResult(policy);
        }
    }
}