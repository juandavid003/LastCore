using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CRUD.Repositories
{
    public class ConsumptionRepository
    {
        private readonly DB_CrudLogInEntities _db;

        public ConsumptionRepository(string connectionString)
        {
            _db = new DB_CrudLogInEntities(connectionString);
        }

        public IEnumerable<ConsumptionWithProduct> GetAllConsumptions()
        {
            return (from consumption in _db.consumptions
                    join product in _db.products on consumption.product_id equals product.product_id
                    join treatment in _db.treatments on consumption.treatment_id equals treatment.id
                    select new ConsumptionWithProduct
                    {
                        Id = consumption.id,
                        UsedDate = consumption.usedDate,
                        ProductName = product.name,
                        TreatmentId = treatment.id,
                        UsedQuantity = consumption.usedQuantity
                    }).ToList();
        }

        public ConsumptionWithProduct GetConsumptionById(int id)
        {
            return (from c in _db.consumptions
                    join p in _db.products on c.product_id equals p.product_id
                    join t in _db.treatments on c.treatment_id equals t.id
                    where c.id == id
                    select new ConsumptionWithProduct
                    {
                        Id = c.id,
                        UsedDate = c.usedDate,
                        ProductName = p.name,
                        TreatmentId = t.id,
                        UsedQuantity = c.usedQuantity
                    }).FirstOrDefault();
        }

        public bool AddOrUpdateConsumption(ConsumptionWithProduct consumptionDTO)
        {
            var product = _db.products.FirstOrDefault(p => p.product_id == consumptionDTO.ProductId);
            if (product == null || product.availableQuantity < consumptionDTO.UsedQuantity)
                return false;

            var existingConsumption = _db.consumptions
                .FirstOrDefault(c => c.treatment_id == consumptionDTO.TreatmentId && c.product_id == consumptionDTO.ProductId);

            if (existingConsumption != null)
            {
                existingConsumption.usedQuantity += consumptionDTO.UsedQuantity;
                existingConsumption.usedDate = DateTime.Now;
            }
            else
            {
                _db.consumptions.Add(new consumption
                {
                    usedDate = DateTime.Now,
                    product_id = consumptionDTO.ProductId,
                    treatment_id = consumptionDTO.TreatmentId,
                    usedQuantity = consumptionDTO.UsedQuantity
                });
            }

            product.availableQuantity -= consumptionDTO.UsedQuantity;
            _db.SaveChanges();
            return true;
        }

        public bool UpdateConsumption(int id, ConsumptionWithProduct consumptionDTO)
        {
            var existingConsumption = _db.consumptions.FirstOrDefault(c => c.id == id);
            if (existingConsumption == null)
                return false;

            var product = _db.products.FirstOrDefault(p => p.name == consumptionDTO.ProductName);
            if (product == null || product.availableQuantity + existingConsumption.usedQuantity < consumptionDTO.UsedQuantity)
                return false;

            int quantityDifference = existingConsumption.usedQuantity - consumptionDTO.UsedQuantity;

            existingConsumption.usedDate = DateTime.Now;
            existingConsumption.product_id = consumptionDTO.ProductId;
            existingConsumption.treatment_id = consumptionDTO.TreatmentId;
            existingConsumption.usedQuantity = consumptionDTO.UsedQuantity;

            product.availableQuantity += quantityDifference;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteConsumption(int id)
        {
            var consumption = _db.consumptions.FirstOrDefault(c => c.id == id);
            if (consumption == null)
                return false;

            var product = _db.products.FirstOrDefault(p => p.product_id == consumption.product_id);
            if (product != null)
            {
                product.availableQuantity += consumption.usedQuantity;
            }

            _db.consumptions.Remove(consumption);
            _db.SaveChanges();
            return true;
        }
    }
}