using DB;
using System;
using System.Collections.Generic;
using System.Linq;

public class TreatmentRepository
{
    private readonly DB_CrudLogInEntities _dbContext;

    public TreatmentRepository(string connectionString)
    {
        _dbContext = new DB_CrudLogInEntities(connectionString);
    }

    public IEnumerable<object> GetAllTreatments()
    {
        return _dbContext.treatments
            .Select(t => new
            {
                t.id,
                t.description,
                t.startDate,
                t.endDate,
                t.standardCost,
                t.standardConsumption,
                patientName = t.patient.firstName + " " + t.patient.lastName,
                specialistName = t.specialist.firstName + " " + t.specialist.lastName,
                t.adminId
            }).ToList();
    }

    public object GetTreatmentById(int id)
    {
        return _dbContext.treatments
            .Where(t => t.id == id)
            .Select(t => new
            {
                t.id,
                t.description,
                t.startDate,
                t.endDate,
                t.standardCost,
                t.standardConsumption,
                patientName = t.patient.firstName + " " + t.patient.lastName,
                specialistName = t.specialist.firstName + " " + t.specialist.lastName,
                specialty = _dbContext.specialists
                    .Where(s => s.id == t.specialist.specialityId)
                    .Select(s => s.firstName)
                    .FirstOrDefault(),
                t.adminId
            })
            .FirstOrDefault();
    }

    public void AddTreatment(treatment treatment)
    {
        _dbContext.treatments.Add(treatment);
        _dbContext.SaveChanges();
    }

    public bool UpdateTreatment(int id, treatment updatedTreatment)
    {
        var existingTreatment = _dbContext.treatments.FirstOrDefault(t => t.id == id);
        if (existingTreatment == null)
            return false;

        // Actualizar propiedades
        existingTreatment.description = updatedTreatment.description;
        existingTreatment.startDate = updatedTreatment.startDate;
        existingTreatment.endDate = updatedTreatment.endDate;
        existingTreatment.standardCost = updatedTreatment.standardCost;
        existingTreatment.standardConsumption = updatedTreatment.standardConsumption;
        existingTreatment.patientId = updatedTreatment.patientId;
        existingTreatment.specialistId = updatedTreatment.specialistId;
        existingTreatment.adminId = updatedTreatment.adminId;

        _dbContext.SaveChanges();
        return true;
    }

    public bool DeleteTreatment(int id)
    {
        var treatment = _dbContext.treatments.FirstOrDefault(t => t.id == id);
        if (treatment == null)
            return false;

        // Eliminar consumos relacionados
        var consumptions = _dbContext.consumptions.Where(c => c.treatment_id == id).ToList();
        foreach (var consumption in consumptions)
        {
            var product = _dbContext.products.FirstOrDefault(p => p.product_id == consumption.product_id);
            if (product != null)
            {
                product.availableQuantity += consumption.usedQuantity;
            }
            _dbContext.consumptions.Remove(consumption);
        }

        _dbContext.treatments.Remove(treatment);
        _dbContext.SaveChanges();
        return true;
    }
}
