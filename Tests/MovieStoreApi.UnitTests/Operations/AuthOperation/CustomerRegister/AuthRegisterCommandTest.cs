using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.Common.Response;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Command;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.UnitTests.TestSetups;

namespace MovieStoreApi.UnitTests.Operations.AuthOperation.CustomerRegister
{
    public class AuthRegisterCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public AuthRegisterCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
            _configuration = testFixture.Configuration;
        }

        [Fact]
        public async Task Handle_CustomerRegisterCommand_WhenModelIsValid_ReturnsSuccessResponse()
        {
            // Arrange
            var registerCommand = new CustomerRegisterCommand(new CustomerRegisterRequest
            {
                Username = "johndoe",
                Name = "John",
                Surname = "Doe",
                Password = "password123"
            });

            var handler = new AuthCommandHandler(_context, _mapper, _configuration);

            // Act
            var result = await handler.Handle(registerCommand, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ApiResponse>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Handle_CustomerRegisterCommand_WhenUsernameAlreadyExists_ThrowsException()
        {
            // Arrange
            Random random = new Random();
            var randomCustomer = await _context.Customers.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            var registerCommand = new CustomerRegisterCommand(new CustomerRegisterRequest
            {
                Username = randomCustomer.Username, // Zaten var olan bir kullanıcı adı
                Name = "Jane",
                Surname = "Doe",
                Password = "newpassword123"
            });

            var handler = new AuthCommandHandler(_context, _mapper, _configuration);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(async () => await handler.Handle(registerCommand, new CancellationToken()));
        }

    }
}