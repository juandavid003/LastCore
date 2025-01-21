using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

namespace CRUD.Controllers
{
    public class PatientController : ApiController
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

        // GET: api/consumption
        public IEnumerable<Patient> Get()
        {
            using (DB_CrudLogInEntities db = new DB_CrudLogInEntities(connectionString))
            {
                var patients = from patient in db.patients
                               select new Patient
                               {
                                   id = patient.id,
                                   code = patient.code,
                                   firstName = patient.firstName,
                                   lastName = patient.lastName,
                                   birthDate = patient.birthDate,
                                   CI = patient.CI,
                                   medicalHistory = patient.medicalHistory
                               };

                return patients.ToList();
            }
        }
    }
}


