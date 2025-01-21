using System;
using System.Configuration;
using System.Web.Http;

namespace CRUD.Controllers
{
    public class CoreObjectiveController : ApiController
    {
        private readonly CoreObjectiveRepository _repository;

        public CoreObjectiveController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            _repository = new CoreObjectiveRepository(connectionString);
        }

        [HttpGet]
        [Route("api/efficiency")]
        public IHttpActionResult GetEfficiencyBySpecialty()
        {
            try
            {
                var results = _repository.GetEfficiencyBySpecialty();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/specialties/consumptions")]
        public IHttpActionResult GetSpecialistsConsumptionsBySpecialty(DateTime startDate, DateTime endDate)
        {
            try
            {
                var results = _repository.GetSpecialistsConsumptionsBySpecialty(startDate, endDate);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
