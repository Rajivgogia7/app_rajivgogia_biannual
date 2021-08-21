using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using ProductManagementApi;
using ProductManagementApi.Controllers;
using ProductManagementApi.Services;
using System.Collections.Generic;

namespace ProductManagementApi_tests
{
    public class Tests
    {
        ProductsController _controller;
        IProductsService _service;

        [SetUp]
        public void Setup()
        {
        }

        public Tests()
        {
            _service = new ProductsService();
            _controller = new ProductsController(_service);
        }

        [Test]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            var testProducts = GetTestProducts();
            var result = _controller.GetAllProducts();
            Assert.AreEqual(testProducts.Count, result.Value.Count);
        }

        [Test]
        public void GetProduct_ShouldReturnCorrectProduct()
        {
            var testProducts = GetTestProducts();
            var result = _controller.GetProductById("4");
            Assert.AreEqual(testProducts[3].Name, result.Value.Name);
        }
        [Test]
        public void GetProduct_ShouldNotFindProduct()
        {
            var result = _controller.GetProductById("999");
            Assert.IsNull(result.Value);
        }

        private List<Product> GetTestProducts()
        {
            var testProducts = new List<Product>();
            testProducts.Add(new Product { ID = "1", Name = "Demo1", Brand = "Puma" });
            testProducts.Add(new Product { ID = "2", Name = "Demo2", Brand = "Reebok" });
            testProducts.Add(new Product { ID = "3", Name = "Demo3", Brand = "Adidas" });
            testProducts.Add(new Product { ID = "4", Name = "Demo4", Brand = "Nike" });

            return testProducts;
        }
    }
}