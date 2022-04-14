using Moq;
using ProductsCatalog.Data.Repositories;
using ProductsCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductsCatalog.Test.Repositories
{
    public class ProductsRepositoryTests
    {
        private Mock<IProductRepository> _repositoryMock;

        public ProductsRepositoryTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task Should_Create_Product()
        {
            // Arrange
            var productToAdd = new Product() { Name = "Test product", Price = 2.34m };

            var productsList = new List<Product>();

            _repositoryMock
                .Setup(r => r.Create(productToAdd).Result)
                .Callback<Product>(p => productsList.Add(p))
                .Returns<Product>(p => p);

            // Act
            await _repositoryMock.Object.Create(productToAdd);

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

            var updatedProduct = new Product() { Id = 1, Name = "TEST", Price = 3.43m };

            // Act
            await _repositoryMock.Object.Update(updatedProduct);

            // Assert
            Assert.Equal(product.Name, updatedProduct.Name);
            Assert.Equal(product.Price, updatedProduct.Price);
        }

        [Fact]
        public async Task Should_Get_Correct_Product_By_Id()
        {
            // Arrange
            var productToGet = new Product() { Id = 1, Name = "Test product", Price = 2.34m };

            _repositoryMock
                .Setup(r => r.GetById(1).Result)
                .Returns(productToGet);

            // Act
            var product = await _repositoryMock.Object.GetById(1);

            // Assert
            _repositoryMock.Verify(r => r.GetById(1));
            Assert.Equal(product, productToGet);
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

            // Act
            var products = await _repositoryMock.Object.GetAll();

            //Assert
            _repositoryMock.Verify(r => r.GetAll());
            Assert.Equal(productsToGet, products);
        }
    }
}