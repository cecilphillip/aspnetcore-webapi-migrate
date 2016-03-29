using System.Linq;
using System.Web.Http;
using ConferenceAPI.Models;
using Microsoft.AspNet.Mvc;

namespace ConferenceAPI.Controllers.Api
{
    [Route("api/[controller]")]
    public class SpeakersController : ApiController
    {
        private readonly IDataStore _dateStore;

        public SpeakersController(IDataStore dateStore)
        {
            _dateStore = dateStore;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult List(int? page = null)
        {
            var results = page.HasValue ? _dateStore.GetSpeakers(page.Value) : _dateStore.GetSpeakers();
            return Ok(results);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult Retrieve(int id)
        {
            var result = _dateStore.GetSpeakers().SingleOrDefault(s => s.Id == id);

            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPost("create")]
        public IActionResult Create(Speaker speaker)
        {
            //TODO: Validate
            _dateStore.AddSpeaker(speaker);
            return Created(Request.RequestUri + "/" + speaker.Id, speaker);
        }

        [HttpDelete("remove/{id:int}")]
        public IActionResult Remove(int id)
        {
            var result = _dateStore.RemoveSpeaker(id);
            if (result)
            {
                return Ok(result.Data);
            }
            return NotFound();
        }
    }
}
