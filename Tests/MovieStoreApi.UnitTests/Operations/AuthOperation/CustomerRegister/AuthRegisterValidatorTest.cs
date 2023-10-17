using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.Common.Response;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Command;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.Operation.Validation;
using MovieStoreApi.UnitTests.TestSetups;

namespace MovieStoreApi.UnitTests.Operations.AuthOperation.CustomerRegister
{
    public class AuthRegisterValidatorTest : IClassFixture<CommonTestFixture>
    {

        [Fact]
        public void Validate_WhenUsernameEmpty_ReturnsValidationError()
        {
            // Arrange
            var validator = new CustomerRegisterValidator();
            var model = new CustomerRegisterRequest { Username = "", Name = "John", Surname = "Doe", Password = "password123" };

            // Act
            var result = validator.Validate(model);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == "Username");
        }

        [Fact]
        public void Validate_WhenNameEmpty_ReturnsValidationError()
        {
            // Arrange
            var validator = new CustomerRegisterValidator();
            var model = new CustomerRegisterRequest { Username = "johndoe", Name = "", Surname = "Doe", Password = "password123" };

            // Act
            var result = validator.Validate(model);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == "Name");
        }

        [Fact]
        public void Validate_WhenSurnameEmpty_ReturnsValidationError()
        {
            // Arrange
            var validator = new CustomerRegisterValidator();
            var model = new CustomerRegisterRequest { Username = "johndoe", Name = "John", Surname = "", Password = "password123" };

            // Act
            var result = validator.Validate(model);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == "Surname");
        }

        [Fact]
        public void Validate_WhenPasswordEmpty_ReturnsValidationError()
        {
            // Arrange
            var validator = new CustomerRegisterValidator();
            var model = new CustomerRegisterRequest { Username = "johndoe", Name = "John", Surname = "Doe", Password = "" };

            // Act
            var result = validator.Validate(model);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == "Password");
        }

        [Fact]
        public void Validate_WhenAllFieldsValid_ReturnsValidResult()
        {
            // Arrange
            var validator = new CustomerRegisterValidator();
            var model = new CustomerRegisterRequest { Username = "johndoe", Name = "John", Surname = "Doe", Password = "password123" };

            // Act
            var result = validator.Validate(model);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Validate_WhenUsernameExceedsMaximumLength_ReturnsValidationError()
        {
            // Arrange
            var validator = new CustomerRegisterValidator();
            var model = new CustomerRegisterRequest
            {
                Username = new string('x', 51), // 50 karakteri aşan bir kullanıcı adı
                Name = "John",
                Surname = "Doe",
                Password = "password123"
            };

            // Act
            var result = validator.Validate(model);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_WhenNameExceedsMaximumLength_ReturnsValidationError()
        {
            // Arrange
            var validator = new CustomerRegisterValidator();
            var model = new CustomerRegisterRequest
            {
                Username = "johndoe",
                Name = new string('x', 51), // 50 karakteri aşan bir isim
                Surname = "Doe",
                Password = "password123"
            };

            // Act
            var result = validator.Validate(model);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_WhenSurnameExceedsMaximumLength_ReturnsValidationError()
        {
            // Arrange
            var validator = new CustomerRegisterValidator();
            var model = new CustomerRegisterRequest
            {
                Username = "johndoe",
                Name = "John",
                Surname = new string('x', 51), // 50 karakteri aşan bir soyadı
                Password = "password123"
            };

            // Act
            var result = validator.Validate(model);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_WhenPasswordExceedsMaximumLength_ReturnsValidationError()
        {
            // Arrange
            var validator = new CustomerRegisterValidator();
            var model = new CustomerRegisterRequest
            {
                Username = "johndoe",
                Name = "John",
                Surname = "Doe",
                Password = new string('x', 51) // 50 karakteri aşan bir şifre
            };

            // Act
            var result = validator.Validate(model);

            // Assert
            Assert.False(result.IsValid);
        }

    }
}