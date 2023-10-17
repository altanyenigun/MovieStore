using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.DTOs;
using MovieStoreApi.Models;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.UnitTests.TestSetups;
using Vk.Operation.Command;

namespace MovieStoreApi.UnitTests.Operations.DirectorOperation.DeleteDirector
{
    public class DeleteDirectorCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public DeleteDirectorCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_DeleteDirectorCommand_ValidData_Success()
        {
            // Arrange
            var director = new Director { Id = 9999999, Name = "John", Surname = "Doe" };
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();

            var deleteCommand = new DeleteDirectorCommand(director.Id);
            var handler = new DirectorCommandHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(deleteCommand, new CancellationToken());

            // Assert
            Assert.NotNull(result);

            // Director başarıyla silinmiş olmalı
            var deletedDirector = await _context.Directors.FindAsync(director.Id);
            Assert.Null(deletedDirector);
        }

        [Fact]
        public async Task Handle_DeleteDirectorCommand_DirectorNotFound()
        {
            // Arrange
            var deleteCommand = new DeleteDirectorCommand(-1);
            var handler = new DirectorCommandHandler(_context, _mapper);

            // Act
            var exception = await Record.ExceptionAsync(() => handler.Handle(deleteCommand, new CancellationToken()));

            // Assert
            // Assert.NotNull(exception);
            // Assert.IsType<CustomException>(exception);
            // Assert.Equal(700, ((CustomException)exception).ErrorCode); // Assuming error code is 700 for Director_NOT_FOUND

            await FluentActions
                .Invoking(() => handler.Handle(deleteCommand, new CancellationToken()))
                .Should().ThrowAsync<CustomException>()
                .Where(ex => ex.ErrorCode == 800 &&
                             ex.Error == "DIRECTOR_NOT_FOUND" &&
                             ex.Message == "There is no director with this id.");
        }

        [Fact]
        public async Task Handle_DeleteDirectorCommand_CantDeleteDirector()
        {
            Random random = new Random();
            var randomDirector = await _context.Directors.OrderBy(x => random.Next()).FirstOrDefaultAsync();
            var randomMovie = await _context.Movies.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            // Arrange
            Director Director = _context.Directors.SingleOrDefault(x => x.Id == randomDirector.Id);
            randomMovie.Director = Director;
            await _context.SaveChangesAsync();

            var deleteCommand = new DeleteDirectorCommand(randomDirector.Id);
            var handler = new DirectorCommandHandler(_context, _mapper);

            // Act
            var exception = await Record.ExceptionAsync(() => handler.Handle(deleteCommand, new CancellationToken()));

            // Assert
            // Assert.NotNull(exception);
            // Assert.IsType<CustomException>(exception);
            // Assert.Equal(701, ((CustomException)exception).ErrorCode); // Assuming error code is 701 for CANT_DELETE_Director

            await FluentActions
                .Invoking(() => handler.Handle(deleteCommand, new CancellationToken()))
                .Should().ThrowAsync<CustomException>()
                .Where(ex => ex.ErrorCode == 801 &&
                             ex.Error == "CANT_DELETE_DIRECTOR" &&
                             ex.Message == "You cant delete this Director!, first you should delete him/her movies!");
        }

    }
}