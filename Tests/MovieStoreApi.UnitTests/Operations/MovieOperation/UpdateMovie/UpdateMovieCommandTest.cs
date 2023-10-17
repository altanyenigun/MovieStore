using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.UnitTests.TestSetups;
using MovieStoreApi.Operation.Command;

namespace MovieStoreApi.UnitTests.Operations.MovieOperation.UpdateMovie
{
    public class UpdateMovieCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public UpdateMovieCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_UpdateMovieCommand_ValidData_Success()
        {
            // Arrange
            Random random = new Random();
            var randomMovie = await _context.Movies.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            await _context.SaveChangesAsync();

            var updateModel = new MovieUpdateRequest
            {
                Name = "Updated Movie",
                Year = 2022,
                Price = 15.0M,
                GenreIds = new List<int> { 1 },
                DirectorId = 1,
                ActorIds = new List<int> { 1 }
            };

            var updateCommand = new UpdateMovieCommand(updateModel, randomMovie.Id);
            var handler = new MovieCommandHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(updateCommand, new CancellationToken());

            // Assert
            Assert.NotNull(result);

            var updatedMovie = await _context.Movies.FindAsync(randomMovie.Id);
            Assert.NotNull(updatedMovie);
            Assert.Equal("Updated Movie", updatedMovie.Name);
            Assert.Equal(2022, updatedMovie.Year);
            Assert.Equal(15.0M, updatedMovie.Price);
        }

        [Fact]
        public async Task Handle_UpdateMovieCommand_EntityNotFound_ThrowsException()
        {
            // Arrange
            var nonExistentId = -1; // varsayılan olarak mevcut olmayan bir Id
            var updateModel = new MovieUpdateRequest
            {
                Name = "Updated Movie",
                Year = 2022,
                Price = 15.0M,
                GenreIds = new List<int> { 1 },
                DirectorId = 1,
                ActorIds = new List<int> { 1 }
            };

            var updateCommand = new UpdateMovieCommand(updateModel, nonExistentId);
            var handler = new MovieCommandHandler(_context, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(async () => await handler.Handle(updateCommand, new CancellationToken()));
        }

        [Fact]
        public async Task Handle_UpdateMovieCommand_DuplicateMovieName_ThrowsException()
        {
            // Arrange
            Random random = new Random();
            var randomMovie = await _context.Movies.OrderBy(x => random.Next()).FirstOrDefaultAsync();
            var randomMovie2 = await _context.Movies.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            var updateModel = new MovieUpdateRequest
            {
                Name = randomMovie.Name, // Bu isimde başka bir film zaten mevcut
                Year = 2022,
                Price = 15.0M,
                GenreIds = new List<int> { 1 },
                DirectorId = 1,
                ActorIds = new List<int> { 1 }
            };

            var updateCommand = new UpdateMovieCommand(updateModel,randomMovie2.Id);
            var handler = new MovieCommandHandler(_context, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(async () => await handler.Handle(updateCommand, new CancellationToken()));
        }
    }

}