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

namespace MovieStoreApi.UnitTests.Operations.DirectorOperation.UpdateDirector
{
    public class UpdateDirectorCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public UpdateDirectorCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_UpdateDirectorCommand_ValidData_Success()
        {
            // Arrange
            Random random = new Random();
            var randomDirector = await _context.Directors.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            var updateModel = new DirectorUpdateRequest { Name = "Jane", Surname = "Doe" };
            var updateCommand = new UpdateDirectorCommand(updateModel, randomDirector.Id);

            var handler = new DirectorCommandHandler(_context, _mapper);

            // Act
            var result = await Record.ExceptionAsync(() => handler.Handle(updateCommand, new CancellationToken()));

            // Assert
            var updatedDirector = await _context.Directors.FindAsync(randomDirector.Id);
            Assert.Equal("Jane", updatedDirector!.Name);
            Assert.Equal("Doe", updatedDirector.Surname);
        }

        [Fact]
        public async Task Handle_UpdateDirectorCommand_DirectorNotFound()
        {
            // Arrange
            var updateCommand = new UpdateDirectorCommand(null, -1);

            var handler = new DirectorCommandHandler(_context, _mapper);

            // Act & Assert
            await FluentActions
                .Invoking(() => handler.Handle(updateCommand, new CancellationToken()))
                .Should().ThrowAsync<CustomException>()
                .Where(ex => ex.ErrorCode == 800 &&
                             ex.Error == "DIRECTOR_NOT_FOUND" &&
                             ex.Message == "There is no director with this id.");
        }

    }
}