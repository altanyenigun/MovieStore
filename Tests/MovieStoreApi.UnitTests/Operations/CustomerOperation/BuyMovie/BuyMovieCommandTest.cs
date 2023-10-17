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

namespace MovieStoreApi.UnitTests.Operations.CustomerOperation.BuyMovie
{
    public class BuyMovieCommandTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public BuyMovieCommandTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_CustomerBuyMovieCommand_WhenMovieExistsAndCustomerDoesNotOwnIt_ReturnsSuccessResponse()
        {
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
            var randomMovie = await _context.Movies.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            var buyMovieCommand = new CustomerBuyMovieCommand(customer.Id,randomMovie.Id);
            var handler = new CustomerCommandHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(buyMovieCommand, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ApiResponse>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Handle_CustomerBuyMovieCommand_WhenMovieNotFound_ThrowsException()
        {
            Random random = new Random();
            var randomCustomer = await _context.Customers.OrderBy(x => random.Next()).FirstOrDefaultAsync();
            var buyMovieCommand = new CustomerBuyMovieCommand(randomCustomer.Id, -1);

            var handler = new CustomerCommandHandler(_context, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(async () => await handler.Handle(buyMovieCommand, new CancellationToken()));
        }

        [Fact]
        public async Task Handle_CustomerBuyMovieCommand_WhenCustomerAlreadyOwnsTheMovie_ThrowsException()
        {

            Random random = new Random();
            var randomCustomer = await _context.Customers.Include(x => x.Movies).OrderBy(x => random.Next()).FirstOrDefaultAsync();
            var randomMovie = await _context.Movies.Include(x => x.Actors).Include(x => x.Director).Include(x => x.Genres).Include(x => x.Customers).OrderBy(x => random.Next()).FirstOrDefaultAsync();

            randomCustomer.Movies.Add(randomMovie);
            await _context.SaveChangesAsync();

            var buyMovieCommand = new CustomerBuyMovieCommand(randomCustomer.Id, randomMovie.Id);

            var handler = new CustomerCommandHandler(_context, _mapper);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(async () => await handler.Handle(buyMovieCommand, new CancellationToken()));
        }

    }
}