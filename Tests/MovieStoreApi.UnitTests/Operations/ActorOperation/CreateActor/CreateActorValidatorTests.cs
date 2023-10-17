using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Validation;

namespace MovieStoreApi.UnitTests.Operations.ActorOperation.CreateActor
{
    public class CreateActorValidatorTests
    {
        [Fact]
        public void Name_ShouldNotBeEmpty()
        {
            // Arrange
            var validator = new CreateActorValidator();
            var actorDto = new ActorCreateRequest { Name = "", Surname = "Doe" };

            // Act
            var result = validator.Validate(actorDto);

            // Assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Name_ShouldNotExceedMaximumLength()
        {
            // Arrange
            var validator = new CreateActorValidator();
            var actorDto = new ActorCreateRequest { Name = "JohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohnJohn", Surname = "Doe" };

            // Act
            var result = validator.Validate(actorDto);

            // Assert
            result.Errors.Count.Should().BeGreaterThan(0);

        }

        [Fact]
        public void Surname_ShouldNotBeEmpty()
        {
            // Arrange
            var validator = new CreateActorValidator();
            var actorDto = new ActorCreateRequest { Name = "John", Surname = "" };

            // Act
            var result = validator.Validate(actorDto);

            // Assert
            result.Errors.Count.Should().BeGreaterThan(0);

        }

        [Fact]
        public void Surname_ShouldNotExceedMaximumLength()
        {
            // Arrange
            var validator = new CreateActorValidator();
            var actorDto = new ActorCreateRequest { Name = "John", Surname = "DoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoeDoe" };

            // Act
            var result = validator.Validate(actorDto);

            // Assert
            // Assert.False(result.IsValid);
            // Assert.Contains(result.Errors, error => error.PropertyName == nameof(actorDto.Surname));
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void AllPropertiesShouldBeValid()
        {
            // Arrange
            var validator = new CreateActorValidator();
            var actorDto = new ActorCreateRequest { Name = "John", Surname = "Doe" };

            // Act
            var result = validator.Validate(actorDto);

            // Assert
            // Assert.True(result.IsValid);
            // Assert.Empty(result.Errors);
            result.Errors.Count.Should().Be(0);

        }
    }
}