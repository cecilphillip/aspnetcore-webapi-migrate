using System.Linq;
using ConferenceAPI.Filters;
using ConferenceAPI.Models;
using Microsoft.AspNet.Http.Extensions;
using Microsoft.AspNet.Mvc;

namespace ConferenceAPI.Controllers.Api
{
    [Route("api/[controller]")]
    public class SpeakersController : Controller
    {
        private readonly IDataStore _dateStore;

        public SpeakersController(IDataStore dateStore)
        {
            _dateStore = dateStore;
        }
        
        [HttpGet("list")]
        public IActionResult List(int? page = null)
        {
            var results = page.HasValue ? _dateStore.GetSpeakers(page.Value) : _dateStore.GetSpeakers();
            return Ok(results);
        }
       
        [HttpGet("{id:int}")]
        public IActionResult Retrieve(int id)
        {
            var result = _dateStore.GetSpeakers().SingleOrDefault(s => s.Id == id);

            if (result == null) return HttpNotFound();

            return Ok(result);
        }
       
        [HttpGet("{id:int}/sessions")]
        public IActionResult RetrieveSessions(int id)
        {
            var result = _dateStore.GetSpeakers().SingleOrDefault(s => s.Id == id);

            if (result == null) return HttpNotFound();

            return Ok(result.Sessions);
        }

        [ValidateModel]
        [HttpPost("create")]
        public IActionResult Create([FromBody]Speaker speaker)
        {
            if (ModelState.IsValid)
            {
                _dateStore.AddSpeaker(speaker);

                return Created(Request.GetDisplayUrl() + "/" + speaker.Id, speaker);
            }

            return HttpBadRequest(ModelState);
        }

        [HttpDelete("remove/{id:int}")]
        public IActionResult Remove(int id)
        {
            var result = _dateStore.RemoveSpeaker(id);
            if (result)
            {
                return Ok(result.Data);
            }
            return HttpNotFound();
        }
    }
}
