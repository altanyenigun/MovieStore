using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Validation;
using MovieStoreApi.UnitTests.TestSetups;

namespace MovieStoreApi.UnitTests.Operations.MovieOperation.UpdateMovie
{
    public class UpdateMovieValidatorTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public UpdateMovieValidatorTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void Validate_NameNotEmpty()
        {
            // Arrange
            var validator = new CreateMovieValidator(_context);
            var movieDto = new MovieCreateRequest { Name = "", Year = 2000, Price = 10.0M, GenreIds = new List<int> { 1 }, DirectorId = 1, ActorIds = new List<int> { 1, 2 } };

            // Act
            var result = validator.Validate(movieDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("'Name' must not be empty.", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public void Validate_NameMaxLength()
        {
            // Arrange
            var validator = new CreateMovieValidator(_context);
            var movieDto = new MovieCreateRequest { Name = new string('A', 51), Year = 2000, Price = 10.0M, GenreIds = new List<int> { 1 }, DirectorId = 1, ActorIds = new List<int> { 1, 2 } };

            // Act
            var result = validator.Validate(movieDto);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_YearGreaterThan1900()
        {
            // Arrange
            var validator = new CreateMovieValidator(_context);
            var movieDto = new MovieCreateRequest { Name = "Movie Name", Year = 1800, Price = 10.0M, GenreIds = new List<int> { 1 }, DirectorId = 1, ActorIds = new List<int> { 1, 2 } };

            // Act
            var result = validator.Validate(movieDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Year must be greater than 1900.", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public void Validate_PriceGreaterThanZero()
        {
            // Arrange
            var validator = new CreateMovieValidator(_context);
            var movieDto = new MovieCreateRequest { Name = "Movie Name", Year = 2000, Price = 0.0M, GenreIds = new List<int> { 1 }, DirectorId = 1, ActorIds = new List<int> { 1, 2 } };

            // Act
            var result = validator.Validate(movieDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Price must be greater than 0.", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public void Validate_GenreIdsNotEmpty()
        {
            // Arrange
            var validator = new CreateMovieValidator(_context);
            var movieDto = new MovieCreateRequest { Name = "Movie Name", Year = 2000, Price = 10.0M, GenreIds = new List<int>(), DirectorId = 1, ActorIds = new List<int> { 1, 2 } };

            // Act
            var result = validator.Validate(movieDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("You must specify at least one genre.", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public void Validate_GenreIdsValid()
        {
            // Arrange
            var validator = new CreateMovieValidator(_context);
            var movieDto = new MovieCreateRequest { Name = "Movie Name", Year = 2000, Price = 10.0M, GenreIds = new List<int> { -1 }, DirectorId = 1, ActorIds = new List<int> { 1, 2 } };

            // Act
            var result = validator.Validate(movieDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Invalid Genre ID sent.", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public void Validate_DirectorIdNotNull()
        {
            // Arrange
            var validator = new CreateMovieValidator(_context);
            var movieDto = new MovieCreateRequest { Name = "Movie Name", Year = 2000, Price = 10.0M, DirectorId = null, GenreIds = new List<int> { 1 }, ActorIds = new List<int> { 1, 2 } };

            // Act
            var result = validator.Validate(movieDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("'Director Id' must not be empty.", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public void Validate_DirectorIdValid()
        {
            // Arrange
            var validator = new CreateMovieValidator(_context);
            var movieDto = new MovieCreateRequest { Name = "Movie Name", Year = 2000, Price = 10.0M, GenreIds = new List<int> { 1 }, DirectorId = -1, ActorIds = new List<int> { 1, 2 } };

            // Act
            var result = validator.Validate(movieDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Invalid DirectorId, please enter a valid DirectorId!", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public void Validate_ActorIdsNotEmpty()
        {
            // Arrange
            var validator = new CreateMovieValidator(_context);
            var movieDto = new MovieCreateRequest { Name = "Movie Name", Year = 2000, Price = 10.0M, GenreIds = new List<int> { 1 }, DirectorId = 1, ActorIds = new List<int>() };

            // Act
            var result = validator.Validate(movieDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("You must specify at least one actor.", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public void Validate_ActorIdsValid()
        {
            // Arrange
            var validator = new CreateMovieValidator(_context);
            var movieDto = new MovieCreateRequest { Name = "Movie Name", Year = 2000, Price = 10.0M, GenreIds = new List<int> { 1 }, DirectorId = 1, ActorIds = new List<int> { -1 } };

            // Act
            var result = validator.Validate(movieDto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Invalid Actor ID sent.", result.Errors.First().ErrorMessage);
        }
    }
}