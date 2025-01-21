using DB;
using System.Collections.Generic;

public interface ITreatmentRepository
{
    IEnumerable<object> GetAllTreatments();
    object GetTreatmentById(int id);
    void AddTreatment(treatment treatment);
    bool UpdateTreatment(int id, treatment updatedTreatment);
    bool DeleteTreatment(int id);
}
