using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.Operation.Validation;
using MovieStoreApi.UnitTests.TestSetups;
using MovieStoreApi.Operation.Command;
using MovieStoreApi.Common.Response;
using Microsoft.Extensions.Configuration;
using MovieStoreApi.Models;

namespace MovieStoreApi.UnitTests.Operations.CustomerOperation.AddFavoriteGenre
{
    public class AddFavoriteGenreCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public AddFavoriteGenreCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_CustomerAddFavoriteGenreCommand_WhenGenreExistsAndCustomerDoesNotHaveIt_ReturnsSuccessResponse()
        {
            // Arrange
            var newCustomer = new CustomerRegisterRequest
            {
                Username = "zest11",
                Name = "John",
                Surname = "Doe",
                Password = BCrypt.Net.BCrypt.HashPassword("password123")
            };

            Customer mapped = _mapper.Map<Customer>(newCustomer);
            await _context.Customers.AddAsync(mapped);
            await _context.SaveChangesAsync();

            Random random = new Random();
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Username == newCustomer.Username);
            var randomGenre = await _context.Genres.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            var addFavoriteGenreCommand = new CustomerAddFavoriteGenreCommand(customer.Id, randomGenre.Id);
            var handler = new CustomerCommandHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(addFavoriteGenreCommand, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ApiResponse>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Handle_CustomerAddFavoriteGenreCommand_WhenGenreNotFound_ThrowsException()
        {
            // Arrange
            Random random = new Random();
            var randomCustomer = await _context.Customers.OrderBy(x => random.Next()).FirstOrDefaultAsync();
            var addFavoriteGenreCommand = new CustomerAddFavoriteGenreCommand(randomCustomer.Id, -1);

            var handler = new CustomerCommandHandler(_context, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(async () => await handler.Handle(addFavoriteGenreCommand, new CancellationToken()));
        }

        [Fact]
        public async Task Handle_CustomerAddFavoriteGenreCommand_WhenCustomerAlreadyHasTheGenre_ThrowsException()
        {
            Random random = new Random();
            var randomCustomer = await _context.Customers.Include(x => x.Genres).OrderBy(x => random.Next()).FirstOrDefaultAsync();
            var randomGenre = await _context.Genres.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            randomCustomer.Genres.Add(randomGenre);
            await _context.SaveChangesAsync();

            var addFavoriteGenreCommand = new CustomerAddFavoriteGenreCommand(randomCustomer.Id, randomGenre.Id);


            var handler = new CustomerCommandHandler(_context, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(async () => await handler.Handle(addFavoriteGenreCommand, new CancellationToken()));
        }


    }
}