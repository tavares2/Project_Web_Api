using AutoMapper;
using LGC_Code_Challenge.Contracts;
using LGC_CodeChallenge.Mappings;
using LGC_CodeChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGC_CodeChallenge.UnitTests.Mappings
{
    [TestFixture]
    public class ProductProfileTests
    {
        [Test]
        public void ProductProfile_ConfigurationIsValid()
        {
            //Arrange
            var configuration = new MapperConfiguration(config => config.AddProfile<ProductProfile>());

            //Act & Assert
            configuration.AssertConfigurationIsValid();
        }

        [Test]
        public void Product_To_ProductResponse_MappingIsValid()
        {
            // Arrange
            var configuration = new MapperConfiguration(config => config.AddProfile<ProductProfile>());
            var mapper = configuration.CreateMapper();

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Price = 99.99m,
                Stock = 10,
                Description = "A test product"
            };

            // Act
            var productResponse = mapper.Map<ProductResponse>(product);

            // Assert
            Assert.NotNull(productResponse);
            Assert.AreEqual(product.Id, productResponse.Id);
            Assert.AreEqual(product.Name, productResponse.Name);
            Assert.AreEqual(product.Price, productResponse.Price);
            Assert.AreEqual(product.Description, productResponse.Description);
        }

        [Test]
        public void ProductRequest_To_Product_MappingIsValid()
        {
            // Arrange
            var configuration = new MapperConfiguration(config => config.AddProfile<ProductProfile>());
            var mapper = configuration.CreateMapper();

            var productRequest = new ProductRequest
            {
                Name = "New Product",
                Price = 49.99m,
                Stock = 20,
                Description = "A new product"
            };

            // Act
            var product = mapper.Map<Product>(productRequest);

            // Assert
            Assert.NotNull(product);
            Assert.AreEqual(productRequest.Name, product.Name);
            Assert.AreEqual(productRequest.Price, product.Price);
            Assert.AreEqual(productRequest.Stock, product.Stock);
            Assert.AreEqual(productRequest.Description, product.Description);
        }
    }
}
