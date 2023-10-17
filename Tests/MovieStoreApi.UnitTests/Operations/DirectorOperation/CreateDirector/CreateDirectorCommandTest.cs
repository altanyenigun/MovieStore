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

namespace MovieStoreApi.UnitTests.Operations.DirectorOperation.CreateDirector
{
    public class CreateDirectorCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public CreateDirectorCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_CreateDirectorCommand_ValidData_Success()
        {
            // Arrange
            var createModel = new DirectorCreateRequest { Name = "John", Surname = "Doe" };
            var createCommand = new CreateDirectorCommand(createModel);

            var handler = new DirectorCommandHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(createCommand, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Response);
            Assert.Equal("John", result.Response.Name);
            Assert.Equal("Doe", result.Response.Surname);

            var createdDirector = await _context.Directors.FindAsync(result.Response.Id);
            Assert.NotNull(createdDirector);
            Assert.Equal("John", createdDirector.Name);
            Assert.Equal("Doe", createdDirector.Surname);
        }

        [Fact]
        public async Task Handle_CreateDirectorCommand_InvalidData_ThrowsValidationException()
        {
            // Arrange
            var createModel = new DirectorCreateRequest { Name = null, Surname = "Doe" }; // Geçersiz veri, isim boş olamaz
            var createCommand = new CreateDirectorCommand(createModel);

            var validator = new CreateDirectorValidator();
            var handler = new DirectorCommandHandler(_context, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await handler.Handle(createCommand, new CancellationToken()));
        }

    }
}