using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApiTest.Data;
using WebApiTest.Model;
using WebApiTest.Services;

namespace WebApiTest.Controllers
{
    [Route("api/cities/{cityId}/buildings")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly ILogger<BuildingController> _logger;
        private readonly IMailService _mailService;
        private readonly DataStore _dataStore;

        public BuildingController(
            ILogger<BuildingController> logger, 
            IMailService mailService, 
            DataStore dataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Building>> GetBuildings(int cityId)
        {
            try
            {
                var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);

                if (city is null)
                {
                    _logger.LogInformation($"City with id {cityId} was not found.");
                    return NotFound();
                }

                return Ok(city.Buildings);
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Error getting buildings for city with id {cityId}", e);
                return StatusCode(500, "Problem occurred while handling the request.");
            }
        }

        [HttpGet("{buildingId}", Name = "GetBuilding")]
        public ActionResult<Building> GetBuilding(int cityId, int buildingId)
        {
            var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city is null) { return NotFound();}

            var building = city.Buildings.FirstOrDefault(b => b.Id == buildingId);
            if (building is null) { return NotFound(); }

            return Ok(building);
        }

        [HttpPost]
        public ActionResult<Building> CreateBuilding(int cityId, BuildingCreate buildingCreate)
        {
            var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city is null) { return NotFound(); };

            var maxId = _dataStore.Cities.SelectMany(c => c.Buildings).Max(b => b.Id);

            var newBuilding = new Building()
            {
                Id = ++maxId,
                Name = buildingCreate.Name,
                Description = buildingCreate.Description
            };

            city.Buildings.Add(newBuilding);

            return CreatedAtRoute("GetBuilding", 
                new
                {
                    cityId,
                    buildingId = newBuilding.Id
                },
                newBuilding);
        }

        [HttpPut("{buildingId}")]
        public ActionResult UpdateBuilding(int cityId, int buildingId, BuildingUpdate buildingUpdate)
        {
            var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) { return NotFound(); }

            var building = city.Buildings.FirstOrDefault(b => b.Id == buildingId);
            if (building == null) { return NotFound();}

            building.Name = buildingUpdate.Name;
            building.Description = buildingUpdate.Description;

            return NoContent();
        }

        [HttpPatch("{buildingId}")]
        public ActionResult PartiallyUpdateBuilding(int cityId, int buildingId, JsonPatchDocument<BuildingUpdate> patchDocument)
        {
            var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) { return NotFound(); }

            var building = city.Buildings.FirstOrDefault(b => b.Id == buildingId);
            if (building == null) { return NotFound(); }

            var buildingPatch = new BuildingUpdate()
            {
                Name = building.Name,
                Description = building.Description
            };
            patchDocument.ApplyTo(buildingPatch, ModelState);

            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (!TryValidateModel(buildingPatch)) { return BadRequest(ModelState); }

            building.Name = buildingPatch.Name;
            building.Description = buildingPatch.Description;

            return NoContent();
        }

        [HttpDelete("{buildingId}")]
        public ActionResult DeleteBuilding(int cityId, int buildingId)
        {
            var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) { return NotFound(); }

            var building = city.Buildings.FirstOrDefault(b => b.Id == buildingId);
            if (building == null) { return NotFound(); }

            city.Buildings.Remove(building);
            _mailService.Send("Building deleted.", $"Building {building.Name} with id {building.Id} deleted.");
            return NoContent();
        }
    }
}
