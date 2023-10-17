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

namespace MovieStoreApi.UnitTests.Operations.DirectorOperation.GetDirector
{
    public class DirectorQueryHandlerTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public DirectorQueryHandlerTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_GetDirectorByIdQuery_ValidId_Success()
        {
            // Arrange
            Random random = new Random();
            var randomDirector = await _context.Directors.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            var query = new GetDirectorByIdQuery(randomDirector.Id);
            var handler = new DirectorQueryHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Response);
            Assert.Equal(randomDirector.Id, result.Response.Id);
            Assert.Equal(randomDirector.Name, result.Response.Name);
            Assert.Equal(randomDirector.Surname, result.Response.Surname);
        }

        [Fact]
        public async Task Handle_GetDirectorByIdQuery_InvalidId_ReturnsNull()
        {
            // Arrange
            var query = new GetDirectorByIdQuery(-1); // Bu id ile kayıtlı bir aktör olmadığını varsayalım
            var handler = new DirectorQueryHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Response); // Beklenen sonuç, Data alanının null olmasıdır
        }
    }
}