using LGC_Code_Challenge.Contracts;
using LGC_CodeChallenge.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGC_CodeChallenge.UnitTests.Validators
{
    [TestFixture]
    public class ProductRequestValidatorTests
    {
        private ProductRequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new ProductRequestValidator();
        }

        [Test]
        public void Validate_ShouldPass_WhenProductRequestIsValid()
        {
            // Arrange
            var validRequest = new ProductRequest
            {
                Name = "Valid Product",
                Price = 100.0m,
                Stock = 10,
                Description = "A valid product description"
            };

            // Act
            var result = _validator.Validate(validRequest);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsEmpty(result.Errors); // No validation errors
        }

        [Test]
        public void Validate_ShouldFail_WhenNameIsEmpty()
        {
            // Arrange
            var invalidRequest = new ProductRequest
            {
                Name = "",
                Price = 100.0m,
                Stock = 10,
                Description = "A valid product description"
            };

            // Act
            var result = _validator.Validate(invalidRequest);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Name is required.", result.Errors.FirstOrDefault(e => e.PropertyName == "Name")?.ErrorMessage);
        }

        [Test]
        public void Validate_ShouldFail_WhenPriceIsZero()
        {
            // Arrange
            var invalidRequest = new ProductRequest
            {
                Name = "Valid Product",
                Price = 0.0m, // Invalid
                Stock = 10,
                Description = "A valid product description"
            };

            // Act
            var result = _validator.Validate(invalidRequest);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Price must be greater than 0.", result.Errors.FirstOrDefault(e => e.PropertyName == "Price")?.ErrorMessage);
        }

        [Test]
        public void Validate_ShouldFail_WhenPriceIsNegative()
        {
            // Arrange
            var invalidRequest = new ProductRequest
            {
                Name = "Valid Product",
                Price = -50.0m, // Invalid
                Stock = 10,
                Description = "A valid product description"
            };

            // Act
            var result = _validator.Validate(invalidRequest);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Price must be greater than 0.", result.Errors.FirstOrDefault(e => e.PropertyName == "Price")?.ErrorMessage);
        }

        [Test]
        public void Validate_ShouldFail_WhenStockIsNegative()
        {
            // Arrange
            var invalidRequest = new ProductRequest
            {
                Name = "Valid Product",
                Price = 100.0m,
                Stock = -5, // Invalid
                Description = "A valid product description"
            };

            // Act
            var result = _validator.Validate(invalidRequest);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Stock cannot be negative.", result.Errors.FirstOrDefault(e => e.PropertyName == "Stock")?.ErrorMessage);
        }

        [Test]
        public void Validate_ShouldFail_WhenDescriptionIsEmpty()
        {
            // Arrange
            var invalidRequest = new ProductRequest
            {
                Name = "Valid Product",
                Price = 100.0m,
                Stock = 10,
                Description = "" // Invalid
            };

            // Act
            var result = _validator.Validate(invalidRequest);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Description is required.", result.Errors.FirstOrDefault(e => e.PropertyName == "Description")?.ErrorMessage);
        }
    }

}
