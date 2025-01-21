using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace CRUD.Controllers
{
    public class ProductController : ApiController
    {
        private readonly ProductRepository _repository;

        public ProductController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            _repository = new ProductRepository(connectionString);
        }

        // GET: api/product
        [HttpGet]
        [Route("api/product")]
        public IHttpActionResult Get()
        {
            try
            {
                var products = _repository.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
