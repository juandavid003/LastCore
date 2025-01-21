using DB;
using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace CRUD.Controllers
{
    public class LogInController : ApiController
    {
        // GET: api/LogIn
        public IEnumerable<usuario> Get()
        {
            using (DB_CrudLogInEntities db = new DB_CrudLogInEntities())
            { 
             return db.usuarios.ToList();
            }
        }

        public IHttpActionResult Get(int id)
        {
            using (DB_CrudLogInEntities db = new DB_CrudLogInEntities())
            {
                var usuario = db.usuarios.FirstOrDefault(u => u.Id == id);
                if (usuario == null)
                {
                    return NotFound();
                }
                return Ok(usuario);
            }
        }



        public IHttpActionResult Post([FromBody] usuario newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (DB_CrudLogInEntities db = new DB_CrudLogInEntities())
            {
                db.usuarios.Add(newUser);
                db.SaveChanges();
            }

            return CreatedAtRoute("DefaultApi", new { id = newUser.Id }, newUser);
        }




        public IHttpActionResult Put(int id, [FromBody] usuario updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (DB_CrudLogInEntities db = new DB_CrudLogInEntities())
            {
                var existingUser = db.usuarios.FirstOrDefault(u => u.Id == id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.UserName = updatedUser.UserName;
                existingUser.Mail = updatedUser.Mail;
                existingUser.Password = updatedUser.Password;
                existingUser.CompleteName = updatedUser.CompleteName;
                existingUser.StartDate = updatedUser.StartDate;
                existingUser.State = updatedUser.State;
                existingUser.Rol = updatedUser.Rol;

                db.SaveChanges();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }




        public IHttpActionResult Delete(int id)
        {
            using (DB_CrudLogInEntities db = new DB_CrudLogInEntities())
            {
                var usuario = db.usuarios.FirstOrDefault(u => u.Id == id);
                if (usuario == null)
                {
                    return NotFound();
                }

                db.usuarios.Remove(usuario);
                db.SaveChanges();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
