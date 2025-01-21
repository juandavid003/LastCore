using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;
using DB;
using System.Linq;

namespace CRUD.Controllers
{
    public class TaskController : ApiController
    {
        private readonly TaskRepository _repository;

        public TaskController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            _repository = new TaskRepository(connectionString);
        }

        // GET: api/Task
        public IHttpActionResult Get()
        {
            try
            {
                var tasks = _repository.GetAllTasks();
                return Ok(tasks); 
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // GET: api/Task/5
        public IHttpActionResult Get(int id)
        {
            try
            {
                var task = _repository.GetTaskById(id);
                if (task == null)
                    return NotFound();

                return Ok(task);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/Task
        public IHttpActionResult Post([FromBody] tarea newTask)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _repository.AddTask(newTask);
                return CreatedAtRoute("DefaultApi", new { id = newTask.task_id }, newTask);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/Task/5
        public IHttpActionResult Put(int id, [FromBody] tarea updatedTask)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool updated = _repository.UpdateTask(id, updatedTask);
                if (!updated)
                    return NotFound();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/Task/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                bool deleted = _repository.DeleteTask(id);
                if (!deleted)
                    return NotFound();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/task/searchByDate
        [HttpGet]
        [Route("api/task/searchByDate")]
        public IHttpActionResult GetTasksByDate([FromUri] DateTime startDate, [FromUri] DateTime endDate)
        {
            try
            {
                var tasks = _repository.GetTasksByDate(startDate, endDate);
                if (!tasks.Any())
                    return NotFound();

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
