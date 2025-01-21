using System.Collections.Generic;
using System.Linq;
using DB;

public class EspecialityRepository
{
    private readonly DB_CrudLogInEntities _dbContext;

    public EspecialityRepository(string connectionString)
    {
        _dbContext = new DB_CrudLogInEntities(connectionString);
    }

    public IEnumerable<Especiality> GetAllEspecialities()
    {
        return _dbContext.specialities
            .Select(e => new Especiality
            {
                Id = e.id,
                Name = e.name,
                Procedures = e.procedures,
                Budget = e.budget
            })
            .ToList();
    }
}
