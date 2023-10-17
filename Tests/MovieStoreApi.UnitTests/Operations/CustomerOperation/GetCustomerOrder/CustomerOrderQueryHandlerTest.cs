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
using MovieStoreApi.Operation.Query;

namespace MovieStoreApi.UnitTests.Operations.CustomerOperation.GetCustomerOrder
{
    public class CustomerOrderQueryHandlerTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public CustomerOrderQueryHandlerTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_CustomerOrderQuery_WhenOrdersExist_ReturnsListOfCustomerOrderResponse()
        {
            Random random = new Random();
            var customer = await _context.Customers.Include(x => x.Movies).FirstOrDefaultAsync();
            var orders = await _context.Orders.Where(x => x.CustomerId == customer.Id).ToListAsync();
            var customerOrderQuery = new CustomeOrderQuery(customer.Id);

            var handler = new CustomerQueryHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(customerOrderQuery, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ApiResponse<List<CustomerOrderResponse>>>(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Response);
            Assert.Equal(orders.Count, result.Response.Count);
        }

        [Fact]
        public async Task Handle_CustomerOrderQuery_WhenNoOrdersExist_ReturnsEmptyList()
        {
            // Arrange
            var newCustomer = new CustomerRegisterRequest
            {
                Username = "zostin22425",
                Name = "John",
                Surname = "Doe",
                Password = BCrypt.Net.BCrypt.HashPassword("password123")
            };

            Customer mapped = _mapper.Map<Customer>(newCustomer);
            await _context.Customers.AddAsync(mapped);
            await _context.SaveChangesAsync();

            Random random = new Random();
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Username == newCustomer.Username);

            var customerOrderQuery = new CustomeOrderQuery(customer.Id);
            var handler = new CustomerQueryHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(customerOrderQuery, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ApiResponse<List<CustomerOrderResponse>>>(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Response);
            Assert.Empty(result.Response);
        }

    }
}