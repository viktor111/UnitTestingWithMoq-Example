using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductsCatalog.Controllers;
using ProductsCatalog.Data.Repositories;
using ProductsCatalog.Dtos;
using ProductsCatalog.Models;
using ProductsCatalog.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductsCatalog.Test.Controllers
{
    public class ProductsControllerTests
    {
        private Mock<IProductRepository> _repositoryMock;

        public ProductsControllerTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
        }

        [Fact]
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
            Assert.Equal(1, listCount);
        }

        [Fact]
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
            Assert.Equal(product.Name, updatedProductDto.Name);
            Assert.Equal(product.Price, updatedProductDto.Price);
        }

        [Fact]
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
            Assert.Equal(productToGet, product);
        }

        [Fact]
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
            Assert.Equal(products, products);
        }

        [Fact]
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
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
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
            Assert.IsType<OkObjectResult>(actionResult);
        }
    }
}
