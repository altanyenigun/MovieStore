using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.Operation.Validation;
using MovieStoreApi.UnitTests.TestSetups;
using Vk.Operation.Command;

namespace MovieStoreApi.UnitTests.Operations.ActorOperation.CreateActor
{
    public class CreateActorCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public CreateActorCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_CreateActorCommand_ValidData_Success()
        {
            // Arrange
            var createModel = new ActorCreateRequest { Name = "John", Surname = "Doe" };
            var createCommand = new CreateActorCommand(createModel);

            var validator = new CreateActorValidator();
            var handler = new ActorCommandHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(createCommand, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Response);
            Assert.Equal("John", result.Response.Name);
            Assert.Equal("Doe", result.Response.Surname);

            var createdActor = await _context.Actors.FindAsync(result.Response.Id);
            Assert.NotNull(createdActor);
            Assert.Equal("John", createdActor.Name);
            Assert.Equal("Doe", createdActor.Surname);
        }

        [Fact]
        public async Task Handle_CreateActorCommand_InvalidData_ThrowsValidationException()
        {
            // Arrange
            var createModel = new ActorCreateRequest { Name = null, Surname = "Doe" }; // Geçersiz veri, isim boş olamaz
            var createCommand = new CreateActorCommand(createModel);

            var validator = new CreateActorValidator();
            var handler = new ActorCommandHandler(_context, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await handler.Handle(createCommand, new CancellationToken()));
        }

    }
}