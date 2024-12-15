using AutoMapper;
using FluentValidation;
using LGC_Code_Challenge.Contracts;
using LGC_CodeChallenge.Controllers;
using LGC_CodeChallenge.Interfaces;
using LGC_CodeChallenge.Models;
using LGC_CodeChallenge.Validators;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGC_CodeChallenge.UnitTests.Controllers
{
    [TestFixture]
    public class ProductControllerTests
    {
        [Test]
        public async Task GetProduct_ShouldReturnOk_WhenProductExists()
        {
            //Arrange
            var mockService = new Mock<IProductService>();
            var mockMapper = new Mock<IMapper>();
            var productId = Guid.NewGuid();

            var product = new Product
            {
                Id = productId,
                Name = "Test",
                Description = "Test Description",
                Price = 20.00m,
                Stock = 10
            };

            var productResponse = new ProductResponse
            {
                Id = productId,
                Name = "Test",
                Description = "Test Description",
                Price = 20.00m,

            };

            mockService.Setup(service => service.GetProductAsync(productId)).ReturnsAsync(product);
            mockMapper.Setup(mapper => mapper.Map<ProductResponse>(product)).Returns(productResponse);

            var controller = new ProductController(mockService.Object, mockMapper.Object);

            //Act
            var result = await controller.GetProduct(productId);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(productResponse, okResult.Value);
        }

        [Test]
        public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            //Arrange
            var mockService = new Mock<IProductService>();
            var mockMapper = new Mock<IMapper>();
            var productId = Guid.NewGuid();

            mockService.Setup(service => service.GetProductAsync(productId)).ReturnsAsync((Product)null);

            var controller = new ProductController(mockService.Object,mockMapper.Object);

            //Act
            var result = await controller.GetProduct(productId);

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual($"Product with ID {productId} not found.", notFoundResult.Value);
        }

        [Test]
        public async Task GetProductsList_ShouldReturnOk_WhenProductsExist()
        {
             // Arrange
             var products = new List<Product>
             {
                new Product { Id = Guid.NewGuid(), Description = "Description 1", Name = "Product 1", Price = 50.0m, Stock = 10 },
                new Product { Id = Guid.NewGuid(), Description = "Description 2", Name = "Product 2", Price = 100.0m, Stock = 5 },
                new Product { Id = Guid.NewGuid(), Description = "Description 3", Name = "Product 3", Price = 200.0m, Stock = 20 },
                new Product { Id = Guid.NewGuid(), Description = "Description 4", Name = "Product 4", Price = 25.0m, Stock = 15 }
             };

            var expectedResponses = products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price
                
            }).ToList();

            var mockService = new Mock<IProductService>();
            var mockMapper = new Mock<IMapper>();

            // Mock the service to return a list of products
            mockService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(products);

            // Mock the mapper to map products to responses
            mockMapper.Setup(mapper => mapper.Map<List<ProductResponse>>(It.IsAny<List<Product>>()))
                      .Returns((List<Product> sourceProducts) =>
                          sourceProducts.Select(p => new ProductResponse
                          {
                              Id = p.Id,
                              Name = p.Name,
                              Description = p.Description,
                              Price = p.Price

                          }).ToList());

            var controller = new ProductController(mockService.Object, mockMapper.Object);

            // Act
            var result = await controller.GetProductsList();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.NotNull(okResult.Value);
            //Assert.AreEqual(200, okResult.StatusCode);

            var actualResponses = okResult.Value as List<ProductResponse>;
            Assert.NotNull(actualResponses);
            Assert.AreEqual(expectedResponses.Count, actualResponses.Count);

            for (int i = 0; i < expectedResponses.Count; i++)
            {
                Assert.AreEqual(expectedResponses[i].Id, actualResponses[i].Id);
                Assert.AreEqual(expectedResponses[i].Name, actualResponses[i].Name);
                Assert.AreEqual(expectedResponses[i].Description, actualResponses[i].Description);
                Assert.AreEqual(expectedResponses[i].Price, actualResponses[i].Price);
                
            }
        }

        [Test]
        public async Task GetProductsList_ShouldReturnOk_WhenNoProductsExist()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var mockMapper = new Mock<IMapper>();

            mockService.Setup(service => service.GetAllProductsAsync()).ReturnsAsync(new List<Product>());
            mockMapper.Setup(mapper => mapper.Map<List<ProductResponse>>(It.IsAny<List<Product>>())).Returns(new List<ProductResponse>());

            var controller = new ProductController(mockService.Object, mockMapper.Object);

            // Act
            var result = await controller.GetProductsList();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsEmpty((List<ProductResponse>)okResult.Value);
        }

        [Test]
        public async Task CreateProduct_ShouldReturnCreated_WhenProductIsValid()
        {
            //Arrange
            var productRequest = new ProductRequest
            {
                Name = "Test",
                Description = "Test Description",
                Price = 20.00m,
                Stock = 10

            };

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Description = "Test Description",
                Price = 20.00m,
                Stock = 10
            };

            var productResponse = new ProductResponse
            {
                Id = product.Id,
                Name = "Test",
                Description = "Test Description",
                Price = 20.00m,
            };

            var mockService = new Mock<IProductService>();
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(mapper => mapper.Map<Product>(productRequest)).Returns(product);
            mockService.Setup(service => service.AddProductAsync(product)).ReturnsAsync(product);
            mockMapper.Setup(mapper => mapper.Map<ProductResponse>(product)).Returns(productResponse);

            var controller = new ProductController(mockService.Object, mockMapper.Object);

            //Act
            var result = await controller.CreateProduct(productRequest);

            //Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(productResponse, createdResult.Value);
        }

        [Test]
        public async Task CreateProduct_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new ProductController(mockService.Object, mockMapper.Object);

            // Create an invalid ProductRequest
            var invalidRequest = new ProductRequest
            {
                Name = "", // Invalid
                Price = -5.0m, // Invalid
                Stock = -1, // Invalid
                Description = "" // Invalid
            };

            // Simulate FluentValidation (manually validate the request)
            var validator = new ProductRequestValidator();
            var validationResult = validator.Validate(invalidRequest);
            foreach (var error in validationResult.Errors)
            {
                controller.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }


            // Act
            var result = await controller.CreateProduct(invalidRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);

            // Check returned validation errors
            var modelState = badRequestResult.Value as SerializableError;
            Assert.NotNull(modelState);
            Assert.IsTrue(modelState.ContainsKey("Name"));
            Assert.AreEqual("Name is required.", ((string[])modelState["Name"])[0]);
            Assert.IsTrue(modelState.ContainsKey("Price"));
            Assert.AreEqual("Price must be greater than 0.", ((string[])modelState["Price"])[0]);
            Assert.IsTrue(modelState.ContainsKey("Stock"));
            Assert.AreEqual("Stock cannot be negative.", ((string[])modelState["Stock"])[0]);
            Assert.IsTrue(modelState.ContainsKey("Description"));
            Assert.AreEqual("Description is required.", ((string[])modelState["Description"])[0]);
        }

        [Test]
        public async Task UpdateProduct_ShouldReturnOk_WhenProductIsUpdatedSuccessfully()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productRequest = new ProductRequest
            {
                Name = "Updated Product",
                Price = 100.0m,
                Stock = 5,
                Description = "Updated Description"
            };

            var product = new Product
            {
                Id = productId,
                Name = "Old Product",
                Price = 50.0m,
                Stock = 10,
                Description = "Old Description"
            };

            var productResponse = new ProductResponse
            {
                Id = productId,
                Name = "Updated Product",
                Price = 100.0m,
                Description = "Updated Description"
            };

            var mockService = new Mock<IProductService>();
            var mockMapper = new Mock<IMapper>();

            mockService.Setup(service => service.GetProductAsync(productId)).ReturnsAsync(product);
            mockMapper.Setup(mapper => mapper.Map<Product>(productRequest)).Returns(product);
            mockService.Setup(service => service.UpdateProductAsync(product)).ReturnsAsync(product);
            mockMapper.Setup(mapper => mapper.Map<ProductResponse>(product)).Returns(productResponse);

            var controller = new ProductController(mockService.Object, mockMapper.Object);

            // Act
            var result = await controller.UpdateProduct(productId, productRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(productResponse, okResult.Value);

            mockService.Verify(service => service.UpdateProductAsync(product), Times.Once);
        }

        [Test]
        public async Task UpdateProduct_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new ProductController(mockService.Object, mockMapper.Object);

            var productId = Guid.NewGuid();

            var invalidRequest = new ProductRequest
            {
                Name = "", // Empty Name
                Price = -5.0m, // Invalid Price
                Stock = -1, // Invalid Stock
                Description = "" // Empty Description
            };

            // Simulate FluentValidation (manually validate the request)
            var validator = new ProductRequestValidator();
            var validationResult = validator.Validate(invalidRequest);
            foreach (var error in validationResult.Errors)
            {
                controller.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            // Act
            var result = await controller.UpdateProduct(productId, invalidRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);

            var modelState = badRequestResult.Value as SerializableError;
            Assert.NotNull(modelState);
            Assert.IsTrue(modelState.ContainsKey("Name"));
            Assert.IsTrue(modelState.ContainsKey("Description"));
            Assert.IsTrue(modelState.ContainsKey("Price"));
            Assert.IsTrue(modelState.ContainsKey("Stock"));
        }

        [Test]
        public async Task UpdateProduct_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new ProductController(mockService.Object, mockMapper.Object);

            var productId = Guid.NewGuid();

            // Act
            var result = await controller.UpdateProduct(productId, null); // Null payload

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.AreEqual("Invalid product request.", badRequestResult.Value);
        }

        [Test]
        public async Task UpdateProduct_ShouldReturnBadRequest_WhenValidationExceptionOccurs()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productRequest = new ProductRequest
            {
                Name = "Invalid Product",
                Price = 0, // Invalid Price
                Stock = -1, // Invalid Stock
                Description = "Invalid Description"
            };

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Old Product",
                Price = 50.0m,
                Stock = 10,
                Description = "Old Description"
            };

            var mockService = new Mock<IProductService>();
            var mockMapper = new Mock<IMapper>();

            mockService.Setup(service => service.GetProductAsync(productId)).ReturnsAsync(existingProduct);
            mockService.Setup(service => service.UpdateProductAsync(existingProduct))
                       .Throws(new ValidationException("Validation error occurred."));

            mockMapper.Setup(mapper => mapper.Map<Product>(productRequest)).Returns(existingProduct);

            var controller = new ProductController(mockService.Object, mockMapper.Object);

            // Act
            var result = await controller.UpdateProduct(productId, productRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);

            var validationErrors = badRequestResult.Value as string;
            Assert.AreEqual("Validation error occurred.", validationErrors);
        }

        [Test]
        public async Task DeleteProduct_ShouldReturnNoContent_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var mockService = new Mock<IProductService>();

            mockService.Setup(service => service.GetProductAsync(productId)).ReturnsAsync(new Product { Id = productId });
            mockService.Setup(service => service.DeleteProductAsync(productId)).Returns(Task.CompletedTask);

            var controller = new ProductController(mockService.Object, null);

            // Act
            var result = await controller.DeleteProduct(productId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            mockService.Verify(service => service.DeleteProductAsync(productId), Times.Once);
        }

        [Test]
        public async Task DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var mockService = new Mock<IProductService>();

            mockService.Setup(service => service.GetProductAsync(productId)).ReturnsAsync((Product)null);

            var controller = new ProductController(mockService.Object, null);

            // Act
            var result = await controller.DeleteProduct(productId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual($"Product with ID {productId} not found.", notFoundResult.Value);
        }

    }
}
