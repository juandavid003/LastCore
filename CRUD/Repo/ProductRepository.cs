using System.Collections.Generic;
using System.Linq;
using DB;

public class ProductRepository
{
    private readonly DB_CrudLogInEntities _dbContext;

    public ProductRepository(string connectionString)
    {
        _dbContext = new DB_CrudLogInEntities(connectionString);
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _dbContext.products
            .Select(p => new Product
            {
                ProductId = p.product_id,
                Name = p.name,
                Description = p.description,
                Price = p.price,
                Provider = p.provider,
                State = p.state,
                AvailableQuantity = p.availableQuantity
            })
            .ToList();
    }
}
