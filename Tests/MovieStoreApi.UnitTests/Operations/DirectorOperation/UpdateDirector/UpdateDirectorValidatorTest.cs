using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Validation;

namespace MovieStoreApi.UnitTests.Operations.DirectorOperation.UpdateDirector
{
    public class UpdateDirectorValidatorTest
    {
        [Fact]
        public void Name_ShouldNotBeEmpty()
        {
            // Arrange
            var validator = new UpdateDirectorValidator();
            var directorDto = new DirectorUpdateRequest { Name = "", Surname = "Doe" };

            // Act
            var result = validator.Validate(directorDto);

            // Assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            // Arrange
            var validator = new UpdateDirectorValidator();
            var directorDto = new DirectorUpdateRequest { Name = "JohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohn", Surname = "Doe" };

            // Act
            var result = validator.Validate(directorDto);

            // Assert
            result.Errors.Count.Should().BeGreaterThan(0);

        }

        [Fact]
        public void Surname_ShouldNotBeEmpty()
        {
            // Arrange
            var validator = new UpdateDirectorValidator();
            var directorDto = new DirectorUpdateRequest { Name = "John", Surname = "" };

            // Act
            var result = validator.Validate(directorDto);

            // Assert
            result.Errors.Count.Should().BeGreaterThan(0);

        }

        [Fact]
        public void Surname_ShouldNotExceedMaximumLength()
        {
            // Arrange
            var validator = new UpdateDirectorValidator();
            var directorDto = new DirectorUpdateRequest { Name = "John", Surname = "DoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoe" };

            // Act
            var result = validator.Validate(directorDto);

            // Assert
            // Assert.False(result.IsValid);
            // Assert.Contains(result.Errors, error => error.PropertyName == nameof(directorDto.Surname));
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void AllPropertiesShouldBeValid()
        {
            // Arrange
            var validator = new UpdateDirectorValidator();
            var directorDto = new DirectorUpdateRequest { Name = "John", Surname = "Doe" };

            // Act
            var result = validator.Validate(directorDto);

            // Assert
            // Assert.True(result.IsValid);
            // Assert.Empty(result.Errors);
            result.Errors.Count.Should().Be(0);

        }
    }
}