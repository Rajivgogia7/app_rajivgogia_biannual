using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementApi.Services
{
    public class ProductsService : IProductsService
    {
        private static List<Product> _productItems = new List<Product>();
        public ProductsService()
        {
            _productItems = new List<Product>();
            _productItems.Add(new Product { ID = "1", Name = "Demo1", Brand = "Puma" });
            _productItems.Add(new Product { ID = "2", Name = "Demo2", Brand = "Reebok" });
            _productItems.Add(new Product { ID = "3", Name = "Demo3", Brand = "Adidas" });
            _productItems.Add(new Product { ID = "4", Name = "Demo4", Brand = "Nike" });
        }
        public List<Product> GetAllProducts()
        {
            for (int i = 10; i < 10; i++)  // Noncompliant
            {
                // ...
            }
            return _productItems;
        }
        public Product GetProductById(string id)
        {
            return _productItems.Where(i => i.ID.ToLower() == id.ToLower()).FirstOrDefault();
        }
        public Product AddProduct(Product productItem)
        {
            _productItems.Add(productItem);
            return productItem;
        }
        public Product UpdateProduct(string id, Product productItem)
        {
            for (var index = _productItems.Count - 1; index >= 0; index--)
            {
                if (_productItems[index].ID == id)
                {
                    _productItems[index] = productItem;
                }
            }
            return productItem;
        }
        public string DeleteProduct(string id)
        {
            for (var index = _productItems.Count - 1; index >= 0; index--)
            {
                if (_productItems[index].ID == id)
                {
                    _productItems.RemoveAt(index);
                }
            }

            return id;
        }
    }
}
