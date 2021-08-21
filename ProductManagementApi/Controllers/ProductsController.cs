using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductManagementApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductsService _service;
        public ProductsController(IProductsService service)
        {
            _service = service;
        }

        [HttpGet("/api/products")]
        public ActionResult<List<Product>> GetAllProducts()
        {
            return _service.GetAllProducts();
        }

        [HttpGet("/api/products/{id}")]
        public ActionResult<Product> GetProductById(string id)
        {
            return _service.GetProductById(id);
        }

        [HttpPost("/api/products")]
        public ActionResult<Product> AddProduct(Product product)
        {
            _service.AddProduct(product);
            return product;
        }

        [HttpPut("/api/products/{id}")]
        public ActionResult<Product> UpdateProduct(string id, Product product)
        {
            _service.UpdateProduct(id, product);
            return product;
        }

        [HttpDelete("/api/products/{id}")]
        public ActionResult<string> DeleteProduct(string id)
        {
            _service.DeleteProduct(id);
            return id;
        }
    }
}
