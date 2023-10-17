using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Models;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.Operation.Query;
using MovieStoreApi.UnitTests.TestSetups;

namespace MovieStoreApi.UnitTests.Operations.ActorOperation.GetActor
{
    public class ActorQueryHandlerTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public ActorQueryHandlerTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_GetActorByIdQuery_ValidId_Success()
        {
            // Arrange
            Random random = new Random();
            var randomActor = await _context.Actors.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            var query = new GetActorByIdQuery(randomActor.Id);
            var handler = new ActorQueryHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Response);
            Assert.Equal(randomActor.Id, result.Response.Id);
            Assert.Equal(randomActor.Name, result.Response.Name);
            Assert.Equal(randomActor.Surname, result.Response.Surname);
        }

        [Fact]
        public async Task Handle_GetActorByIdQuery_InvalidId_ReturnsNull()
        {
            // Arrange
            var query = new GetActorByIdQuery(-1); // Bu id ile kayıtlı bir aktör olmadığını varsayalım
            var handler = new ActorQueryHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Response); // Beklenen sonuç, Data alanının null olmasıdır
        }
    }
}