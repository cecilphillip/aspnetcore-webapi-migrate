using System.Linq;
using System.Web.Http;
using ConferenceAPI.Filters;
using ConferenceAPI.Models;

namespace ConferenceAPI.Controllers.Api
{
    [RoutePrefix("api/speakers")]
    public class SpeakersController : ApiController
    {
        private readonly IDataStore _dateStore;

        public SpeakersController(IDataStore dateStore)
        {
            _dateStore = dateStore;
        }

        [HttpGet]
        [Route("list")]
        public IHttpActionResult List(int? page = null)
        {
            var results = page.HasValue ?  _dateStore.GetSpeakers(page.Value): _dateStore.GetSpeakers() ;
            return Ok(results);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Retrieve(int id)
        {
            var result = _dateStore.GetSpeakers().SingleOrDefault(s => s.Id == id);

            if (result == null) return NotFound();

            return Ok(result);
        }

        [ValidateModel]
        [HttpPost]
        [Route("create")]
        public IHttpActionResult Create(Speaker speaker)
        {           
            _dateStore.AddSpeaker(speaker);
            return Created(Request.RequestUri + "/" + speaker.Id, speaker);
        }

        [HttpDelete]
        [Route("remove/{id:int}")]
        public IHttpActionResult Remove(int id)
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
