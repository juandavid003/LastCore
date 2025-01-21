using CRUD.Repositories;
using DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace CRUD.Controllers
{
    public class SpecialistController : ApiController
    {
        private readonly SpecialistRepository _specialistRepository;
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

        public SpecialistController()
        {
            _specialistRepository = new SpecialistRepository(connectionString);
        }

        // GET: api/specialist
        public IEnumerable<SpecialistWithSpeciality> Get()
        {
            return _specialistRepository.GetAllSpecialists();
        }

        // GET: api/specialist/{id}
        public IHttpActionResult Get(int id)
        {
            if (id <= 0)
                return BadRequest("ID de especialista inválido.");

            var specialistWithSpeciality = _specialistRepository.GetSpecialistById(id);

            if (specialistWithSpeciality == null)
                return NotFound();

            return Ok(specialistWithSpeciality);
        }

        // Método para validar la contraseña
        private bool IsValidPassword(string password)
        {
            var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$");
            return passwordRegex.IsMatch(password);
        }

        // Método para validar que el especialista sea mayor de 18 años
        private bool IsOfLegalAge(DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate > DateTime.Today.AddYears(-age)) age--;
            return age >= 18;
        }

        // POST: api/specialist
        public IHttpActionResult Post([FromBody] specialist newSpecialist)
        {
            if (!ModelState.IsValid || newSpecialist == null)
                return BadRequest("Datos de especialista inválidos.");

            if (!IsValidPassword(newSpecialist.password))
                return BadRequest("La contraseña debe tener al menos 8 caracteres, una letra mayúscula y un número.");

            if (!IsOfLegalAge(newSpecialist.birthDate))
                return BadRequest("El especialista debe ser mayor de 18 años.");

            var existingSpecialist = _specialistRepository.GetSpecialistById(newSpecialist.id);
            if (existingSpecialist != null)
                return BadRequest("Ya existe un especialista con el mismo código.");

            _specialistRepository.AddSpecialist(newSpecialist);

            return CreatedAtRoute("DefaultApi", new { id = newSpecialist.id }, newSpecialist);
        }

        // PUT: api/specialist/{id}
        public IHttpActionResult Put(int id, [FromBody] SpecialistWithSpeciality specialist)
        {
            if (!ModelState.IsValid || specialist == null)
                return BadRequest("Datos de especialista inválidos.");

            if (!IsValidPassword(specialist.Password))
                return BadRequest("La contraseña debe tener al menos 8 caracteres, una letra mayúscula y un número.");

            if (!IsOfLegalAge(specialist.BirthDate))
                return BadRequest("El especialista debe ser mayor de 18 años.");

            var existingSpecialist = _specialistRepository.GetSpecialistById(id);
            if (existingSpecialist == null)
                return NotFound();

            _specialistRepository.UpdateSpecialist(id, specialist);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/specialist/{id}
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("ID de especialista inválido.");

            var existingSpecialist = _specialistRepository.GetSpecialistById(id);
            if (existingSpecialist == null)
                return NotFound();

            _specialistRepository.RemoveSpecialist(id);

            return Ok(existingSpecialist);
        }

        // Métodos adicionales como GetSpecialistsOfEspeciality pueden ir aquí...
    }
}
