using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementApi.Services
{
    public interface IProductsService
    {
        public List<Product> GetAllProducts();
        public Product GetProductById(string id);
        public Product AddProduct(Product productItem);
        public Product UpdateProduct(string id, Product productItem);
        public string DeleteProduct(string id);
    }
}
