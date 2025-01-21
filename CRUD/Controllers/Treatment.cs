using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Configuration;
using System.Data.Entity;

namespace CRUD.Controllers
{
    public class TreatmentController : ApiController
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

        [HttpGet]
        [Route("api/treatments/efficiency")]
        public IHttpActionResult GetSpecialistEfficiency()
        {
            using (DB_CrudLogInEntities db = new DB_CrudLogInEntities(connectionString))
            {
                var results = new List<dynamic>();

                var especialidades = db.especialities.ToList();

                foreach (var especialidad in especialidades)
                {
                    var especialistas = db.specialists
                                                .Where(s => s.especialityId == especialidad.id)
                                                .ToList();

                    foreach (var especialista in especialistas)
                    {
                        var tratamientos = db.treatments
                                                  .Where(t => t.specialistId == especialista.id)
                                                  .ToList();

                        decimal totalConsumption = 0;

                        foreach (var tratamiento in tratamientos)
                        {
                            var consumos = db.consumptions
                                                   .Where(c => c.treatment_id == tratamiento.id)
                                                   .ToList();

                        }
                    }
                }

                var especialistaMasEficiente = results
                    .Where(r => !r.SuperaPresupuesto)
                    .OrderBy(r => r.Consumo / r.Presupuesto)
                    .FirstOrDefault();

                return Ok(new
                {
                    Resultados = results,
                    EspecialistaMasEficiente = especialistaMasEficiente
                });
            }
        }
    }
}
