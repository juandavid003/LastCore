using DB;
using System;
using System.Configuration;
using System.Net;
using System.Web.Http;

namespace CRUD.Controllers
{
    public class LogInController : ApiController
    {
        private readonly UserRepository _repository;
        private readonly IUserValidator _userValidator;

        public LogInController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            _repository = new UserRepository(connectionString);
            _userValidator = new UserValidator(); // Inyección directa. Puede ser por DI también.
        }

        [HttpGet]
        [Route("api/login")]
        public IHttpActionResult GetUsers()
        {
            try
            {
                var usersWithRoles = _repository.GetAllUsers();
                return Ok(usersWithRoles);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/login/{id}")]
        public IHttpActionResult GetUser(int id)
        {
            try
            {
                var user = _repository.GetUserById(id);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult CreateUser([FromBody] user newUser)
        {
            if (!ModelState.IsValid || newUser == null)
                return BadRequest("Datos de usuario inválidos.");

            if (!_userValidator.IsAdult(newUser.birthDate))
                return BadRequest("El usuario debe ser mayor de 18 años.");

            if (!_userValidator.IsValidPassword(newUser.password))
                return BadRequest("La contraseña debe tener al menos 8 caracteres, una letra mayúscula y un número.");

            if (!_userValidator.IsValidStatus(newUser.status))
                return BadRequest("El estado del usuario debe ser 'Active' o 'Inactive'.");

            if (!_userValidator.IsValidName(newUser.firstName) || !_userValidator.IsValidName(newUser.lastName))
                return BadRequest("El nombre y apellido deben tener al menos 2 caracteres.");

            try
            {
                _repository.AddUser(newUser);
                return CreatedAtRoute("DefaultApi", new { id = newUser.id }, newUser);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/login/{id}")]
        public IHttpActionResult UpdateUser(int id, [FromBody] user updatedUser)
        {
            if (!ModelState.IsValid || updatedUser == null)
                return BadRequest("Datos de usuario inválidos.");

            if (!_userValidator.IsAdult(updatedUser.birthDate))
                return BadRequest("El usuario debe ser mayor de 18 años.");

            if (!_userValidator.IsValidPassword(updatedUser.password))
                return BadRequest("La contraseña debe tener al menos 8 caracteres, una letra mayúscula y un número.");

            if (!_userValidator.IsValidStatus(updatedUser.status))
                return BadRequest("El estado del usuario debe ser 'Active' o 'Inactive'.");

            if (!_userValidator.IsValidName(updatedUser.firstName) || !_userValidator.IsValidName(updatedUser.lastName))
                return BadRequest("El nombre y apellido deben tener al menos 2 caracteres.");

            try
            {
                bool updated = _repository.UpdateUser(id, updatedUser);
                if (!updated)
                    return NotFound();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("api/login/{id}")]
        public IHttpActionResult DeleteUser(int id)
        {
            try
            {
                bool deleted = _repository.DeleteUser(id);
                if (!deleted)
                    return NotFound();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
