using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.DTOs;
using MovieStoreApi.Models;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.UnitTests.TestSetups;
using Vk.Operation.Command;

namespace MovieStoreApi.UnitTests.Operations.MovieOperation.DeleteMovie
{
    public class DeleteMovieCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public DeleteMovieCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_DeleteMovieCommand_ValidData_Success()
        {
            // Arrange
            Random random = new Random();
            var randomMovie = await _context.Movies.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            var deleteCommand = new DeleteMovieCommand(randomMovie.Id);
            var handler = new MovieCommandHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(deleteCommand, new CancellationToken());

            // Assert
            Assert.NotNull(result);

            var deletedMovie = await _context.Movies.FindAsync(randomMovie.Id);
            Assert.NotNull(deletedMovie);
            Assert.False(deletedMovie.isActive);
        }

        [Fact]
        public async Task Handle_DeleteMovieCommand_EntityNotFound_ThrowsException()
        {
            // Arrange
            var nonExistentId = -1; // varsayılan olarak mevcut olmayan bir Id
            var deleteCommand = new DeleteMovieCommand(nonExistentId);
            var handler = new MovieCommandHandler(_context, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(async () => await handler.Handle(deleteCommand, new CancellationToken()));
        }

    }
}