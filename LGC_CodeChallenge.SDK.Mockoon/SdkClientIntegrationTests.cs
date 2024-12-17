using LGC_Code_Challenge.Contracts;
using LGC_CodeChallenge.SDK.Exceptions;
using LGC_CodeChallenge.SDK.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LGC_CodeChallenge.SDK.Mockoon
{
    [TestFixture]
    public class SdkClientIntegrationTests
    {
        private HttpClient _httpClient;
        private ISdkClient _sdkClient;

        [SetUp]
        public void SetUp()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:3000") // Mockoon base URL
            };
            _sdkClient = new SdkClient(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose the HttpClient
            _httpClient?.Dispose();
        }

        [Test]
        public async Task GetAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            Guid productId = Guid.Parse("550e8400-e29b-41d4-a716-446655440004");
            var productResponse = new ProductResponse
            {
                Id = productId,
                Name = "Mock Product",
                Description = "This is a mock product.",
                Price = 99.99m
            };

            var endpoint = $"/api/products/{productId}";


            // Act
            var result = await _sdkClient.GetAsync<ProductResponse>(endpoint);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(productResponse.Id, result.Id);
            Assert.AreEqual(productResponse.Name, result.Name);
            Assert.AreEqual(productResponse.Description, result.Description);
            Assert.AreEqual(productResponse.Price, result.Price);
        }

        [Test]
        public async Task GetAsync_ShouldThrowProblemDetailException_WhenProductNotFound()
        {
            // Arrange
            var invalidProductId = "invalid-id"; // Invalid GUID format
            var endpoint = $"/api/products/{invalidProductId}";

            // Act & Assert
            var ex = Assert.ThrowsAsync<ProblemDetailException>(async () =>
                await _sdkClient.GetAsync<ProductResponse>(endpoint)
            );

            Assert.AreEqual(HttpStatusCode.NotFound, ex.StatusCode);
            Assert.AreEqual("https://example.com/probs/product-not-found", ex.Type);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnListOfProducts_WhenRequestIsSuccessful()
        {
            // Arrange
            var endpoint = "/api/products";

            // Act
            var result = await _sdkClient.GetAllAsync<ProductResponse>(endpoint);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<ProductResponse>>(result);
            Assert.AreEqual(3, result.Count); // Assuming the mock returns 3 products
        }

        
        [Test]
        public async Task GetAllAsync_ShouldThrowProblemDetailException_WhenBadRequest()
        {
            // Arrange
            var endpoint = "/api/products?filter=invalid"; // Assume an invalid response is returned for this endpoint

            // Act & Assert
            var ex = Assert.ThrowsAsync<ProblemDetailException>(async () =>
                await _sdkClient.GetAllAsync<ProductResponse>(endpoint)
            );

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
            Assert.AreEqual("https://example.com/probs/invalid-query", ex.Type);
        }

        [Test]
        public async Task PostAsync_ShouldReturnCreatedProduct_WhenRequestIsValid()
        {
            // Arrange
            var newProduct = new ProductRequest
            {
                Name = "New Product",
                Price = 100.0m,
                Stock = 50,
                Description = "A new product"
            };

            var endpoint = "/api/products";

            // Act
            var result = await _sdkClient.PostAsync<ProductRequest, ProductResponse>(endpoint, newProduct);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(newProduct.Name, result.Name);
            Assert.AreEqual(newProduct.Price, result.Price);
        }

        [Test]
        public async Task PostAsync_ShouldThrowProblemDetailException_WhenRequestIsInvalid()
        {
            // Arrange
            var invalidProduct = new ProductRequest
            {
                Name = "",
                Price = -10,
                Stock = -1,
                Description = "" // Invalid product request
            };

            var endpoint = "/api/products?invalidData=true";

            // Act & Assert
            var ex = Assert.ThrowsAsync<ProblemDetailException>(async () =>
                await _sdkClient.PostAsync<ProductRequest, ProductResponse>(endpoint, invalidProduct)
            );

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
            Assert.AreEqual("https://example.com/probs/invalid-product", ex.Type);
        }

        [Test]
        public async Task PutAsync_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            Guid productId = Guid.Parse("550e8400-e29b-41d4-a716-446655440004");

            var updatedProduct = new ProductRequest
            {
                Name = "Updated Product",
                Price = 150.0m,
                Stock = 30,
                Description = "Updated description"
            };

            var endpoint = $"/api/products/{productId}";

            // Act
            var result = await _sdkClient.PutAsync<ProductRequest, ProductResponse>(endpoint, updatedProduct);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Updated Product", result.Name);
            Assert.AreEqual(150.0m, result.Price);
        }

        [Test]
        public async Task PutAsync_ShouldThrowProblemDetailException_WhenProductNotFound()
        {
            // Arrange
            var invalidProductId = "invalid-id";
            var updatedProduct = new ProductRequest
            {
                Name = "Updated Product",
                Price = 150.0m,
                Stock = 30,
                Description = "Updated description"
            };

            var endpoint = $"/api/products/{invalidProductId}";

            // Act & Assert
            var ex = Assert.ThrowsAsync<ProblemDetailException>(async () =>
                await _sdkClient.PutAsync<ProductRequest, ProductResponse>(endpoint, updatedProduct)
            );

            Assert.AreEqual(HttpStatusCode.NotFound, ex.StatusCode);
            Assert.AreEqual("https://example.com/probs/resource-not-found", ex.Type);
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            Guid productId = Guid.Parse("550e8400-e29b-41d4-a716-446655440004");
            var endpoint = $"/api/products/{productId}";

            // Act
            await _sdkClient.DeleteAsync(endpoint);

            // Assert: Verify that the product is deleted (you can check logs or the Mockoon response)
        }

        [Test]
        public async Task DeleteAsync_ShouldThrowProblemDetailException_WhenProductNotFound()
        {
            // Arrange
            var invalidProductId = "invalid-id";
            var endpoint = $"/api/products/{invalidProductId}";

            // Act & Assert
            var ex = Assert.ThrowsAsync<ProblemDetailException>(async () =>
                await _sdkClient.DeleteAsync(endpoint)
            );

            Assert.AreEqual(HttpStatusCode.NotFound, ex.StatusCode);
            Assert.AreEqual("https://example.com/probs/resource-not-found", ex.Type);
        }

    }
}
