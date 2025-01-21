using DB;
using System;
using System.Collections.Generic;
using System.Linq;

public class CoreObjectiveRepository
{
    private readonly DB_CrudLogInEntities _dbContext;

    public CoreObjectiveRepository(string connectionString)
    {
        _dbContext = new DB_CrudLogInEntities(connectionString);
    }

    public IEnumerable<object> GetEfficiencyBySpecialty()
    {
        var results = new List<object>();

        foreach (var specialty in _dbContext.specialities)
        {
            decimal? budget = specialty.budget;
            decimal? totalConsumption = 0;

            foreach (var treatment in _dbContext.treatments)
            {
                foreach (var consumption in _dbContext.consumptions)
                {
                    if (consumption.treatment_id == treatment.id && _dbContext.specialists.Any(s => s.id == treatment.specialistId && s.specialityId == specialty.id))
                    {
                        var product = _dbContext.products.FirstOrDefault(p => p.product_id == consumption.product_id);
                        if (product != null)
                        {
                            decimal? consumptionValue = (consumption.usedQuantity * product.price) / budget;
                            totalConsumption += consumptionValue;
                        }
                    }
                }
            }

            bool exceededBudget = totalConsumption > budget;

            var specialists = _dbContext.specialists.Where(s => s.specialityId == specialty.id).ToList();
            string mostEfficientSpecialistName = "N/A";
            decimal? highestEfficiency = 0;

            foreach (var specialist in specialists)
            {
                decimal? totalEfficiency = 0;
                int treatmentCount = 0;

                foreach (var treatment in _dbContext.treatments.Where(t => t.specialistId == specialist.id))
                {
                    decimal? treatmentEfficiency = 0;
                    foreach (var consumption in _dbContext.consumptions.Where(c => c.treatment_id == treatment.id))
                    {
                        var product = _dbContext.products.FirstOrDefault(p => p.product_id == consumption.product_id);
                        if (product != null)
                        {
                            if (treatment.startDate.HasValue && treatment.endDate.HasValue && treatment.startDate.Value < treatment.endDate.Value)
                            {
                                var treatmentDuration = (decimal)(treatment.endDate.Value - treatment.startDate.Value).TotalHours;
                                treatmentEfficiency += (product.price * consumption.usedQuantity) / treatmentDuration;
                            }
                        }
                    }

                    if (treatmentEfficiency > 0)
                    {
                        totalEfficiency += treatmentEfficiency;
                        treatmentCount++;
                    }
                }

                decimal? averageEfficiency = treatmentCount > 0 ? totalEfficiency / treatmentCount : 0;

                if (averageEfficiency > highestEfficiency)
                {
                    highestEfficiency = averageEfficiency;
                    mostEfficientSpecialistName = $"{specialist.firstName} {specialist.lastName}";
                }
            }

            results.Add(new
            {
                SpecialtyId = specialty.id,
                SpecialtyName = specialty.name,
                TotalConsumption = totalConsumption,
                Budget = budget,
                ExceededBudget = exceededBudget,
                MostEfficientSpecialist = mostEfficientSpecialistName,
                EfficiencyScore = highestEfficiency
            });
        }

        return results;
    }

    public IEnumerable<object> GetSpecialistsConsumptionsBySpecialty(DateTime startDate, DateTime endDate)
    {
        var results = new List<object>();

        foreach (var specialty in _dbContext.specialities)
        {
            decimal? monthlyBudget = specialty.budget;

            var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            var validStartDate = startDate < new DateTime(startDate.Year, startDate.Month, 1)
                                    ? new DateTime(startDate.Year, startDate.Month, 1)
                                    : startDate;
            var validEndDate = endDate > new DateTime(startDate.Year, startDate.Month, daysInMonth)
                                ? new DateTime(startDate.Year, startDate.Month, daysInMonth)
                                : endDate;

            var daysInRange = (validEndDate - validStartDate).TotalDays + 1;
            decimal? adjustedBudget = (monthlyBudget * (decimal)daysInRange) / daysInMonth;

            var specialistsData = new List<object>();

            var specialists = _dbContext.specialists.Where(s => s.specialityId == specialty.id).ToList();
            foreach (var specialist in specialists)
            {
                decimal? totalConsumption = 0;

                foreach (var treatment in _dbContext.treatments.Where(t => t.specialistId == specialist.id))
                {
                    foreach (var consumption in _dbContext.consumptions
                        .Where(c => c.treatment_id == treatment.id && c.usedDate >= startDate && c.usedDate <= endDate))
                    {
                        var product = _dbContext.products.FirstOrDefault(p => p.product_id == consumption.product_id);
                        if (product != null && product.availableQuantity > 0)
                        {
                            decimal? consumptionValue = (consumption.usedQuantity * product.price) / adjustedBudget;

                            totalConsumption += consumptionValue;
                        }
                    }
                }

                specialistsData.Add(new
                {
                    SpecialistId = specialist.id,
                    SpecialistName = $"{specialist.firstName} {specialist.lastName}",
                    TotalConsumption = totalConsumption
                });
            }

            results.Add(new
            {
                SpecialtyId = specialty.id,
                SpecialtyName = specialty.name,
                AdjustedBudget = adjustedBudget,
                Specialists = specialistsData
            });
        }

        return results;
    }
}
