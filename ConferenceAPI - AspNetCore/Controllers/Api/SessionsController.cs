using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ConferenceAPI.Models;

namespace ConferenceAPI.Controllers.Api
{
    [Route("api/[controller]")]
    public class SessionsController 
    {
        private readonly IDataStore _dataStore;

        public SessionsController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [HttpGet("")]        
        public IEnumerable<Session> GetSessions()
        {
            return _dataStore.GetSessions();
        }
    }
}