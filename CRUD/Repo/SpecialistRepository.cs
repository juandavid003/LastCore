using DB;
using System.Collections.Generic;
using System.Linq;

namespace CRUD.Repositories
{
    public class SpecialistRepository
    {
        private readonly DB_CrudLogInEntities _dbContext;

        public SpecialistRepository(string connectionString)
        {
            _dbContext = new DB_CrudLogInEntities(connectionString);
        }

        public IEnumerable<SpecialistWithSpeciality> GetAllSpecialists()
        {
            return (from specialist in _dbContext.specialists
                    join especiality in _dbContext.specialities on specialist.specialityId equals especiality.id
                    select new SpecialistWithSpeciality
                    {
                        Id = specialist.id,
                        Code = specialist.code,
                        FirstName = specialist.firstName,
                        LastName = specialist.lastName,
                        Password = specialist.password,
                        BirthDate = specialist.birthDate,
                        Especiality = especiality.name,
                        Efficiency = specialist.efficiency,
                    }).ToList();
        }

        public SpecialistWithSpeciality GetSpecialistById(int id)
        {
            return (from specialist in _dbContext.specialists
                    join especiality in _dbContext.specialities on specialist.specialityId equals especiality.id
                    where specialist.id == id
                    select new SpecialistWithSpeciality
                    {
                        Id = specialist.id,
                        Code = specialist.code,
                        FirstName = specialist.firstName,
                        LastName = specialist.lastName,
                        Password = specialist.password,
                        BirthDate = specialist.birthDate,
                        Especiality = especiality.name,
                        Efficiency = specialist.efficiency,
                    }).FirstOrDefault();
        }

        public void AddSpecialist(specialist newSpecialist)
        {
            _dbContext.specialists.Add(newSpecialist);
            _dbContext.SaveChanges();
        }

        public void UpdateSpecialist(int id, SpecialistWithSpeciality updatedSpecialist)
        {
            var existingSpecialist = _dbContext.specialists.Find(id);
            if (existingSpecialist != null)
            {
                existingSpecialist.code = updatedSpecialist.Code;
                existingSpecialist.firstName = updatedSpecialist.FirstName;
                existingSpecialist.lastName = updatedSpecialist.LastName;
                existingSpecialist.password = updatedSpecialist.Password;
                existingSpecialist.birthDate = updatedSpecialist.BirthDate;
                existingSpecialist.efficiency = updatedSpecialist.Efficiency;

                var especialidad = _dbContext.specialities.FirstOrDefault(e => e.name == updatedSpecialist.Especiality);
                if (especialidad != null)
                {
                    existingSpecialist.specialityId = especialidad.id;
                }

                _dbContext.SaveChanges();
            }
        }

        public void RemoveSpecialist(int id)
        {
            var specialist = _dbContext.specialists.Find(id);
            if (specialist != null)
            {
                _dbContext.specialists.Remove(specialist);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<specialist> GetSpecialistsBySpecialityId(int specialityId)
        {
            return _dbContext.specialists
                .Where(s => s.specialityId == specialityId)
                .ToList();
        }
    }
}
