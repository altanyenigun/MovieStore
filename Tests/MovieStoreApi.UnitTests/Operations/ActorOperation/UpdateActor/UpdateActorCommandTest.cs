using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.Operation.Validation;
using MovieStoreApi.UnitTests.TestSetups;
using Vk.Operation.Command;

namespace MovieStoreApi.UnitTests.Operations.ActorOperation.UpdateActor
{
    public class UpdateActorCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public UpdateActorCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_UpdateActorCommand_ValidData_Success()
        {
            // Arrange
            Random random = new Random();
            var randomActor = await _context.Actors.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            var updateModel = new ActorUpdateRequest { Name = "Jane", Surname = "Doe" };
            var updateCommand = new UpdateActorCommand(updateModel, randomActor.Id);

            var handler = new ActorCommandHandler(_context, _mapper);

            // Act
            var result = await Record.ExceptionAsync(() => handler.Handle(updateCommand, new CancellationToken()));

            // Assert
            var updatedActor = await _context.Actors.FindAsync(randomActor.Id);
            Assert.Equal("Jane", updatedActor!.Name);
            Assert.Equal("Doe", updatedActor.Surname);
        }

        [Fact]
        public async Task Handle_UpdateActorCommand_ActorNotFound()
        {
            // Arrange
            var updateCommand = new UpdateActorCommand(null, -1);

            var handler = new ActorCommandHandler(_context, _mapper);

            // Act & Assert
            // var exception = await Assert.ThrowsAsync<CustomException>(async () => await handler.Handle(updateCommand, new CancellationToken()));
            // Assert.Equal(700, exception.ErrorCode);
            // Assert.Equal("ACTOR_NOT_FOUND", exception.Error);
            // Assert.Equal("There is no actor with this id.", exception.Message);

            await FluentActions
                .Invoking(() => handler.Handle(updateCommand, new CancellationToken()))
                .Should().ThrowAsync<CustomException>()
                .Where(ex => ex.ErrorCode == 700 &&
                             ex.Error == "ACTOR_NOT_FOUND" &&
                             ex.Message == "There is no actor with this id.");
        }

    }
}