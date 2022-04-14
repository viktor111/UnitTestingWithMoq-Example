using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ProductsCatalog.Controllers;
using ProductsCatalog.Data.Repositories;
using ProductsCatalog.Dtos;
using ProductsCatalog.Models;
using ProductsCatalogNunit.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsCatalogNunit.Test.Controllers
{
    public class ProductsControllerTests
    {
        private Mock<IProductRepository> _repositoryMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IProductRepository>();
        }

        [Test]
        public async Task Should_Create_Product()
        {
            // Arrange
            var productToAddDto = new CreateProductDto() { Name = "Test product", Price = 2.34m };

            var productsList = new List<Product>();

            _repositoryMock
                .Setup(r => r.Create(It.IsAny<Product>()).Result)
                .Callback<Product>(p => productsList.Add(p));

            var controller = new ProductsController(_repositoryMock.Object);

            // Act
            await controller.Create(productToAddDto);

            var listCount = productsList.Count();

            // Assert
            Assert.AreEqual(1, listCount);
        }

        [Test]
        public async Task Should_Update_Product()
        {
            // Arrange
            var product = new Product() { Id = 1, Name = "Test product", Price = 2.34m };

            _repositoryMock
                .Setup(r => r.Update(It.IsAny<Product>()).Result)
                .Callback<Product>(p =>
                {
                    product.Name = p.Name;
                    product.Price = p.Price;
                });

            _repositoryMock
                .Setup(r => r.GetById(It.IsAny<int>()).Result)
                .Returns(product);

            var updatedProductDto = new UpdateProductDto() { Id = 1, Name = "TEST", Price = 3.43m };

            var controller = new ProductsController(_repositoryMock.Object);

            // Act
            await controller.Update(updatedProductDto);

            // Assert
            Assert.AreEqual(product.Name, updatedProductDto.Name);
            Assert.AreEqual(product.Price, updatedProductDto.Price);
        }

        [Test]
        public async Task Should_Get_Details_For_Product()
        {
            // Arrange
            var productToGet = new Product() { Id = 1, Name = "Test product", Price = 2.34m };

            _repositoryMock
                .Setup(r => r.GetById(1).Result)
                .Returns(productToGet);

            var controller = new ProductsController(_repositoryMock.Object);

            // Act
            var action = await controller.Details(1);

            var product = ActionResultHelpers.GetObjectValue<Product>(action);

            // Assert
            Assert.AreEqual(productToGet, product);
        }

        [Test]
        public async Task Should_Get_All_Products()
        {
            // Arrange
            var productsToGet = new List<Product>()
            {
                new Product() { Id = 1, Name = "Test product", Price = 2.34m },
                new Product() { Id = 2, Name = "Test product", Price = 2.34m }
            };

            _repositoryMock
                .Setup(r => r.GetAll().Result)
                .Returns(productsToGet.AsEnumerable());

            var controller = new ProductsController(_repositoryMock.Object);

            // Act
            var action = await controller.All();

            var products = ActionResultHelpers.GetObjectValue<IEnumerable<Product>>(action);

            // Assert
            Assert.AreEqual(products, products);
        }

        [Test]
        public async Task Details_Returns_Ok()
        {
            // Arrange
            var controller = new ProductsController(_repositoryMock.Object);

            var productToGet = new Product() { Id = 1, Name = "Test product", Price = 2.34m };

            _repositoryMock
                .Setup(r => r.GetById(1).Result)
                .Returns(productToGet);

            // Act
            var actionResult = await controller.Details(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }

        [Test]
        public async Task All_Returns_Ok()
        {
            // Arrange
            var controller = new ProductsController(_repositoryMock.Object);
            var productsToGet = new List<Product>()
            {
                new Product() { Id = 1, Name = "Test product", Price = 2.34m },
                new Product() { Id = 2, Name = "Test product", Price = 2.34m }
            };

            _repositoryMock
                .Setup(r => r.GetAll().Result)
                .Returns(productsToGet.AsEnumerable());

            // Act
            var actionResult = await controller.All();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
        }
    }
}
