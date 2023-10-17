using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.Models;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.UnitTests.TestSetups;
using MovieStoreApi.Operation.Command;

namespace MovieStoreApi.UnitTests.Operations.ActorOperation.DeleteActor
{
    public class DeleteActorCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public DeleteActorCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_DeleteActorCommand_ValidData_Success()
        {
            // Arrange
            Random random = new Random();
            var randomActor = await _context.Actors.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            var deleteCommand = new DeleteActorCommand(randomActor.Id);
            var handler = new ActorCommandHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(deleteCommand, new CancellationToken());

            // Assert
            Assert.NotNull(result);

            // Actor başarıyla silinmiş olmalı
            var deletedActor = await _context.Actors.FindAsync(randomActor.Id);
            Assert.Null(deletedActor);
        }

        [Fact]
        public async Task Handle_DeleteActorCommand_ActorNotFound()
        {
            // Arrange
            var deleteCommand = new DeleteActorCommand(-1);
            var handler = new ActorCommandHandler(_context, _mapper);

            // Act
            var exception = await Record.ExceptionAsync(() => handler.Handle(deleteCommand, new CancellationToken()));

            // Assert
            // Assert.NotNull(exception);
            // Assert.IsType<CustomException>(exception);
            // Assert.Equal(700, ((CustomException)exception).ErrorCode); // Assuming error code is 700 for ACTOR_NOT_FOUND

            await FluentActions
                .Invoking(() => handler.Handle(deleteCommand, new CancellationToken()))
                .Should().ThrowAsync<CustomException>()
                .Where(ex => ex.ErrorCode == 700 &&
                             ex.Error == "ACTOR_NOT_FOUND" &&
                             ex.Message == "There is no actor with this id.");
        }

        [Fact]
        public async Task Handle_DeleteActorCommand_CantDeleteActor()
        {
            Random random = new Random();
            var randomActor = await _context.Actors.OrderBy(x => random.Next()).FirstOrDefaultAsync();
            var randomMovie = await _context.Movies.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            // Arrange
            List<Actor> actors = _context.Actors.Where(a=>a.Id == randomActor.Id).ToList();
            randomMovie.Actors = actors;
            await _context.SaveChangesAsync();

            var deleteCommand = new DeleteActorCommand(randomActor.Id);
            var handler = new ActorCommandHandler(_context, _mapper);

            // Act
            var exception = await Record.ExceptionAsync(() => handler.Handle(deleteCommand, new CancellationToken()));

            // Assert
            // Assert.NotNull(exception);
            // Assert.IsType<CustomException>(exception);
            // Assert.Equal(701, ((CustomException)exception).ErrorCode); // Assuming error code is 701 for CANT_DELETE_ACTOR

            await FluentActions
                .Invoking(() => handler.Handle(deleteCommand, new CancellationToken()))
                .Should().ThrowAsync<CustomException>()
                .Where(ex => ex.ErrorCode == 701 &&
                             ex.Error == "CANT_DELETE_ACTOR" &&
                             ex.Message == "You Cant delete this actor!, You should delete the movies it is linked to!.");
        }

    }
}