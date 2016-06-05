using System.Collections.Generic;
using ConferenceAPI.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<Session> GetSessions() => _dataStore.GetSessions();
    }
}