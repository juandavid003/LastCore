
using CRUD.Repositories;
using System.Web.Http;

namespace CRUD.Controllers
{
    public class ConsumptionController : ApiController
    {
        private readonly ConsumptionRepository _repository;

        public ConsumptionController()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            _repository = new ConsumptionRepository(connectionString);
        }

        [HttpGet]
        [Route("api/consumption")]
        public IHttpActionResult Get()
        {
            var consumptions = _repository.GetAllConsumptions();
            return Ok(consumptions);
        }

        [HttpGet]
        [Route("api/consumption/{id}")]
        public IHttpActionResult Get(int id)
        {
            var consumption = _repository.GetConsumptionById(id);
            if (consumption == null)
                return NotFound();

            return Ok(consumption);
        }

        [HttpPost]
        [Route("api/consumption")]
        public IHttpActionResult Post([FromBody] ConsumptionWithProduct consumptionDTO)
        {
            if (!ModelState.IsValid || !_repository.AddOrUpdateConsumption(consumptionDTO))
                return BadRequest("Invalid data or operation failed.");

            return Ok();
        }

        [HttpPut]
        [Route("api/consumption/{id}")]
        public IHttpActionResult Put(int id, [FromBody] ConsumptionWithProduct consumptionDTO)
        {
            if (!ModelState.IsValid || !_repository.UpdateConsumption(id, consumptionDTO))
                return BadRequest("Invalid data or operation failed.");

            return Ok();
        }

        [HttpDelete]
        [Route("api/consumption/{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (!_repository.DeleteConsumption(id))
                return NotFound();

            return Ok();
        }
    }
}