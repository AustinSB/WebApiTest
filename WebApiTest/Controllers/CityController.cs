using Microsoft.AspNetCore.Mvc;
using System.Collections;
using WebApiTest.Data;
using WebApiTest.Model;

namespace WebApiTest.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CityController : ControllerBase
    {
        private readonly DataStore _dataStore;

        public CityController(DataStore dataStore)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<City>> GetCities()
        {
            return Ok(_dataStore.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<City> GetCity(int id)
        {
            var city = new JsonResult(_dataStore.Cities.FirstOrDefault(c => c.Id == id));

            if (city == null) { return NotFound(); }

            return Ok(city);
        }
    }
}
