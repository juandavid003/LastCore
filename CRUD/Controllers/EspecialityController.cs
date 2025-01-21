using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace CRUD.Controllers
{
    public class EspecialityController : ApiController
    {
        private readonly EspecialityRepository _repository;

        public EspecialityController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            _repository = new EspecialityRepository(connectionString);
        }

        // GET: api/especiality
        [HttpGet]
        [Route("api/especiality")]
        public IHttpActionResult GetEspecialities()
        {
            try
            {
                var especialities = _repository.GetAllEspecialities();
                return Ok(especialities);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
