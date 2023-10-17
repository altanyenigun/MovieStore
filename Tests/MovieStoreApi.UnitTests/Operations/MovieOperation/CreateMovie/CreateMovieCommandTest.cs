using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.UnitTests.TestSetups;
using MovieStoreApi.Operation.Command;

namespace MovieStoreApi.UnitTests.Operations.MovieOperation.CreateMovie
{
    public class CreateMovieCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public CreateMovieCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_CreateMovieCommand_ValidData_Success()
        {
            // Arrange
            var handler = new MovieCommandHandler(_context, _mapper);

            var createModel = new MovieCreateRequest
            {
                Name = "<New Movie>",
                Year = 2022,
                Price = 15.0M,
                GenreIds = new List<int> { 1, 2 },
                DirectorId = 1,
                ActorIds = new List<int> { 1, 2 }
            };

            var createCommand = new CreateMovieCommand(createModel);

            // Act
            var result = await handler.Handle(createCommand, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Response);
            Assert.Equal("<New Movie>", result.Response.Name);
            Assert.Equal(2022, result.Response.Year);
            Assert.Equal(15.0M, result.Response.Price);

            var createdMovie = await _context.Movies.FindAsync(result.Response.Id);
            Assert.NotNull(createdMovie);
            Assert.Equal("<New Movie>", createdMovie.Name);
            Assert.Equal(2022, createdMovie.Year);
            Assert.Equal(15.0M, createdMovie.Price);
        }

        [Fact]
        public async Task Handle_CreateMovieCommand_InvalidData_ThrowsValidationException()
        {
            // Arrange
           
            var handler = new MovieCommandHandler(_context, _mapper);

            var createModel = new MovieCreateRequest
            {
                // Geçersiz veri, isim boş olamaz
                Name = "",
                Year = 2022,
                Price = 15.0M,
                GenreIds = new List<int> { 1, 2 },
                DirectorId = 1,
                ActorIds = new List<int> { 1, 2 }
            };

            var createCommand = new CreateMovieCommand(createModel);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await handler.Handle(createCommand, new CancellationToken()));
        }

    }
}