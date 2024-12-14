using LGC_CodeChallenge.Interfaces;
using LGC_CodeChallenge.Models;
using LGC_CodeChallenge.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGC_CodeChallenge.UnitTests.Services
{
    [TestFixture]
    public class ProductServiceTests
    {
        [Test]
        public async Task GetProductAsync_ShouldReturnProduct_WhenProductExists()
        {
            //Arrange
            var mockRepository = new Mock<IProductRepository>();
            var productId = Guid.NewGuid();
            var expectedProduct = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "A sample product",
                Price = 99.99m,
                Stock = 10
            };
            mockRepository.Setup(repo => repo.GetAsync(productId)).ReturnsAsync(expectedProduct);

            var service = new ProductService(mockRepository.Object);

            //Act
            var product = await service.GetProductAsync(productId);

            //Assert
            Assert.NotNull(product);
            Assert.AreEqual(productId, product.Id);
            Assert.AreEqual("Test Product", product.Name);
            Assert.AreEqual("A sample product", product.Description);
            Assert.AreEqual(99.99m, product.Price);
            Assert.AreEqual(10, product.Stock);
        }

        [Test]
        public async Task GetProductAsync_ShouldThrowNotFoundException_WhenProductDoesNotExists()
        {
            //Arrange
            var mockRepository = new Mock<IProductRepository>();
            var productId = Guid.NewGuid();

            mockRepository.Setup(repo => repo.GetAsync(productId)).ReturnsAsync((Product)null);

            var service = new ProductService(mockRepository.Object);

            //Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetProductAsync(productId));
            Assert.AreEqual("Product not found", ex.Message);

        }

        [Test]
        public async Task GetAllProductAsync_ShouldReturnListOfProducts_WhenProductsExist()
        {
            //Arrange
            var mockRepository = new Mock<IProductRepository>();
            var expectedProducts = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1",Description = "Description 1", Price = 10.0m, Stock = 5 },
                new Product { Id = Guid.NewGuid(), Name = "Product 2", Description = "Description 2", Price = 20.0m, Stock = 10 }
            };

            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedProducts);

            var service = new ProductService(mockRepository.Object);

            //Act
            var result = await service.GetAllProductsAsync();

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedProducts.Count, result.Count);
            Assert.AreEqual(expectedProducts[0].Id, result[0].Id);
            Assert.AreEqual(expectedProducts[1].Name, result[1].Name);
            Assert.AreEqual(expectedProducts[0].Description, result[0].Description);
            Assert.AreEqual(expectedProducts[1].Price, result[1].Price);
            Assert.AreEqual(expectedProducts[0].Stock, result[0].Stock);

        }

        [Test]
        public async Task GetAllProductAsync_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            //Arrange
            var mockRepository = new Mock<IProductRepository>();

            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Product>());

            var service = new ProductService(mockRepository.Object);

            //Act
            var result = await service.GetAllProductsAsync();

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public async Task AddProductAsync_ShouldAddProduct_WhenProductIsValid()
        {
            //Arrange
            var mockRepository = new Mock<IProductRepository>();
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "A sample product",
                Price = 99.99m,
                Stock = 10
            };

            mockRepository.Setup(repo => repo.AddAsync(newProduct)).Returns(Task.CompletedTask);

            var service = new ProductService(mockRepository.Object);

            //Act
            var result = await service.AddProductAsync(newProduct);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(newProduct.Id, result.Id);
            Assert.AreEqual(newProduct.Name, result.Name);

            mockRepository.Verify(repo => repo.AddAsync(newProduct), Times.Once);
        }

        [Test]
        public void AddProductAsync_ShouldThrowArgumentNullException_WhenProductIsNull()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var service = new ProductService(mockRepository.Object);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.AddProductAsync(null));
        }

        [Test]
        public async Task UpdateProductAsync_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var productToUpdate = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "A sample product",
                Price = 99.99m,
                Stock = 10
            };

            var updatedProduct = new Product
            {
                Id = productToUpdate.Id,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 120.00m,
                Stock = 15
            };

            mockRepository.Setup(repo => repo.GetAsync(productToUpdate.Id))
                          .ReturnsAsync(productToUpdate); // Simulate product exists
            mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
                          .Returns(Task.CompletedTask); // Simulate successful update

            var service = new ProductService(mockRepository.Object);

            // Act
            var result = await service.UpdateProductAsync(updatedProduct);

            // Assert
            Assert.NotNull(result); // Verify the result is not null
            Assert.AreEqual(updatedProduct.Id, result.Id); // Verify the ID matches
            Assert.AreEqual(updatedProduct.Name, result.Name); // Verify the Name matches
            Assert.AreEqual(updatedProduct.Description, result.Description); // Verify Description
            Assert.AreEqual(updatedProduct.Price, result.Price); // Verify Price
            Assert.AreEqual(updatedProduct.Stock, result.Stock); // Verify Stock

            mockRepository.Verify(repo => repo.GetAsync(productToUpdate.Id), Times.Once);
            mockRepository.Verify(repo => repo.UpdateAsync(It.Is<Product>(p =>
                p.Id == updatedProduct.Id &&
                p.Name == updatedProduct.Name &&
                p.Description == updatedProduct.Description &&
                p.Price == updatedProduct.Price &&
                p.Stock == updatedProduct.Stock
            )), Times.Once); // Ensure UpdateAsync was called with the correct product
        }
        [Test]
        public void UpdateProductAsync_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var productToUpdate = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Nonexistent Product",
                Description = "A sample product",
                Price = 20.00m,
                Stock = 5
            };

            mockRepository.Setup(repo => repo.GetAsync(productToUpdate.Id)).ReturnsAsync((Product)null);  // Simulate product not found


            var service = new ProductService(mockRepository.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.UpdateProductAsync(productToUpdate));
            Assert.AreEqual($"Product with ID {productToUpdate.Id} not found.", ex.Message);

            // Ensure no update attempt is made
            mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Test]
        public void UpdateProductAsync_ShouldThrowArgumentNullException_WhenProductIsNull()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var service = new ProductService(mockRepository.Object);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateProductAsync(null));

            // Ensure no repository calls are made
            mockRepository.Verify(repo => repo.GetAsync(It.IsAny<Guid>()), Times.Never);
            mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Test]
        public async Task DeleteProductAsync_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var productId = Guid.NewGuid();

            mockRepository.Setup(repo => repo.GetAsync(productId)).ReturnsAsync(new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "A sample product",
                Price = 99.99m,
                Stock = 10
            });

            mockRepository.Setup(repo => repo.DeleteAsync(productId)).Returns(Task.CompletedTask); // Simulate successful delete


            var service = new ProductService(mockRepository.Object);

            // Act
            await service.DeleteProductAsync(productId);

            // Assert
            mockRepository.Verify(repo => repo.GetAsync(productId), Times.Once);
            mockRepository.Verify(repo => repo.DeleteAsync(productId), Times.Once);
        }

        [Test]
        public void DeleteProductAsync_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var productId = Guid.NewGuid();

            mockRepository.Setup(repo => repo.GetAsync(productId))
                          .ReturnsAsync((Product)null); // Simulate product not found

            var service = new ProductService(mockRepository.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.DeleteProductAsync(productId));
            Assert.AreEqual($"Product with ID {productId} not found.", ex.Message);
        }
    }
}
