using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace CRUD.Controllers
{
    public class PatientController : ApiController
    {
        private readonly PatientRepository _repository;

        public PatientController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            _repository = new PatientRepository(connectionString);
        }

        // GET: api/patient
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                var patients = _repository.GetAllPatients();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/patient/{id}
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var patient = _repository.GetPatientById(id);
                if (patient == null)
                    return NotFound();

                return Ok(patient);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/patient
        [HttpPost]
        public IHttpActionResult Post([FromBody] Patient newPatient)
        {
            try
            {
                if (newPatient == null)
                    return BadRequest("Invalid patient data.");

                // Validar cédula ecuatoriana
                if (!IsValidEcuadorianCI(newPatient.CI.ToString()))
                    return BadRequest("Invalid Ecuadorian ID (CI).");

                _repository.AddPatient(newPatient);
                return Created($"api/patient/{newPatient.id}", newPatient);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/patient/{id}
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody] Patient updatedPatient)
        {
            try
            {
                if (updatedPatient == null)
                    return BadRequest("Invalid patient data.");

                if (!IsValidEcuadorianCI(updatedPatient.CI.ToString()))
                    return BadRequest("Invalid Ecuadorian ID (CI).");

                bool updated = _repository.UpdatePatient(id, updatedPatient);
                if (!updated)
                    return NotFound();

                return Ok(updatedPatient);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/patient/{id}
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                bool deleted = _repository.DeletePatient(id);
                if (!deleted)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Método para validar cédula ecuatoriana
        private bool IsValidEcuadorianCI(string ci)
        {
            if (string.IsNullOrEmpty(ci) || ci.Length != 10)
                return false;

            // Verificar que sean solo números
            if (!ci.All(char.IsDigit))
                return false;

            int provinceCode = int.Parse(ci.Substring(0, 2));
            int thirdDigit = int.Parse(ci.Substring(2, 1));

            // Validar provincia y tercer dígito
            if (provinceCode < 1 || provinceCode > 24 && provinceCode != 30)
                return false;
            if (thirdDigit >= 6)
                return false;

            // Calcular el dígito verificador
            int[] coeficients = { 2, 1, 2, 1, 2, 1, 2, 1, 2 }; // Coeficientes para el cálculo
            int total = 0;

            for (int i = 0; i < 9; i++)
            {
                int digit = int.Parse(ci[i].ToString()) * coeficients[i];
                total += digit >= 10 ? digit - 9 : digit;
            }

            int verifier = 10 - (total % 10);
            if (verifier == 10)
                verifier = 0;

            return verifier == int.Parse(ci[9].ToString());
        }
    }
}
