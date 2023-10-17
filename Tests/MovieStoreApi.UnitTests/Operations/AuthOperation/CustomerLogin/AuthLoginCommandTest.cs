using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.Common.Response;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Command;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.UnitTests.TestSetups;

namespace MovieStoreApi.UnitTests.Operations.AuthOperation.CustomerLogin
{
    public class AuthLoginCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public AuthLoginCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
            _configuration = testFixture.Configuration;
        }

        [Fact]
        public async Task Handle_CustomerLoginCommand_WhenCustomerNotFound_ThrowsException()
        {
            // Arrange
            var nonExistentUsername = "NonExistentUser"; // Varsayılan olarak mevcut olmayan bir kullanıcı adı
            var loginCommand = new CustomerLoginCommand(new CustomerLoginRequest
            {
                Username = nonExistentUsername,
                Password = "password" // Herhangi bir şifre
            });

            var handler = new AuthCommandHandler(_context, _mapper, _configuration);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(async () => await handler.Handle(loginCommand, new CancellationToken()));
        }

        [Fact]
        public async Task Handle_CustomerLoginCommand_WhenPasswordIncorrect_ThrowsException()
        {
            Random random = new Random();
            var randomActor = await _context.Customers.OrderBy(x => random.Next()).FirstOrDefaultAsync();
            // Arrange

            var loginCommand = new CustomerLoginCommand(new CustomerLoginRequest
            {
                Username = randomActor.Username,
                Password = "wrongPassword" // Yanlış bir şifre
            });

            var handler = new AuthCommandHandler(_context, _mapper, _configuration);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(async () => await handler.Handle(loginCommand, new CancellationToken()));
        }

        [Fact]
        public async Task Handle_CustomerLoginCommand_WhenCredentialsCorrect_ReturnsToken()
        {
            // Arrange
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Username == "altan");

            var loginCommand = new CustomerLoginCommand(new CustomerLoginRequest
            {
                Username = existingCustomer.Username,
                Password = "altan" // Doğru şifre
            });

            var handler = new AuthCommandHandler(_context, _mapper, _configuration);

            // Act
            var result = await handler.Handle(loginCommand, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ApiResponse<LoginResponse>>(result);
            Assert.NotNull(result.Response);
            Assert.NotNull(result.Response.Token);
            Assert.True(result.Success);
        }

    }
}