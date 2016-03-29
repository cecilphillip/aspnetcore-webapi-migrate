using System.Collections.Generic;
using System.Web.Http;
using ConferenceAPI.Models;

namespace ConferenceAPI.Controllers.Api
{
    [RoutePrefix("api/sessions")]
    public class SessionsController : ApiController
    {
        private readonly IDataStore _dataStore;

        public SessionsController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<Session> GetSessions()
        {
            return _dataStore.GetSessions();
        }
    }
}