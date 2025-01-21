using System;
using System.Configuration;
using System.Web.Http;
using DB;
using Newtonsoft.Json;

namespace CRUD.Controllers
{
    public class TreatmentController : ApiController
    {
        private readonly TreatmentRepository _repository;

        // Constructor para inicializar el repositorio
        public TreatmentController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            _repository = new TreatmentRepository(connectionString);

            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        // GET: api/treatment
        [HttpGet]
        [Route("api/treatment")]
        public IHttpActionResult GetTreatments()
        {
            try
            {
                var treatments = _repository.GetAllTreatments();
                return Ok(treatments);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/treatment/{id}
        [HttpGet]
        [Route("api/treatment/{id}")]
        public IHttpActionResult GetTreatment(int id)
        {
            try
            {
                var treatment = _repository.GetTreatmentById(id);
                if (treatment == null)
                {
                    return NotFound();
                }

                return Ok(treatment);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/treatment
        [HttpPost]
        [Route("api/treatment")]
        public IHttpActionResult CreateTreatment([FromBody] treatment treatment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _repository.AddTreatment(treatment);
                return CreatedAtRoute("DefaultApi", new { id = treatment.id }, treatment);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/treatment/{id}
        [HttpPut]
        [Route("api/treatment/{id}")]
        public IHttpActionResult UpdateTreatment(int id, [FromBody] treatment treatment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool updated = _repository.UpdateTreatment(id, treatment);
                if (!updated)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/treatment/{id}
        [HttpDelete]
        [Route("api/treatment/{id}")]
        public IHttpActionResult DeleteTreatment(int id)
        {
            try
            {
                bool deleted = _repository.DeleteTreatment(id);
                if (!deleted)
                {
                    return NotFound();
                }

                return Ok($"Treatment with ID {id} and its related consumptions were deleted successfully.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
