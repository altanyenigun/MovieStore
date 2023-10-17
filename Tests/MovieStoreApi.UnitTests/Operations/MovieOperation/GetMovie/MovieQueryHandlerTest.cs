using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.Common.Response;
using MovieStoreApi.DTOs;
using MovieStoreApi.Models;
using MovieStoreApi.Operation.Cqrs;
using MovieStoreApi.Operation.Query;
using MovieStoreApi.UnitTests.TestSetups;

namespace MovieStoreApi.UnitTests.Operations.MovieOperation.GetDirector
{
    public class MovieQueryHandlerTest : IClassFixture<CommonTestFixture>
    {
        private readonly FakeDataContext _context;
        private readonly IMapper _mapper;

        public MovieQueryHandlerTest(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public async Task Handle_GetAllMovieQuery_ReturnsList()
        {
            // Arrange
            var handler = new MovieQueryHandler(_context, _mapper);
            var getAllQuery = new GetAllMovieQuery();

            // Act
            var result = await handler.Handle(getAllQuery, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ApiResponse<List<MovieResponse>>>(result);
            Assert.NotNull(result.Response);
            Assert.NotEmpty(result.Response);
        }

        [Fact]
        public async Task Handle_GetMovieByIdQuery_ReturnsSingleMovie()
        {
            // Arrange
            Random random = new Random();
            var randomMovie = await _context.Movies.OrderBy(x => random.Next()).FirstOrDefaultAsync();

            var handler = new MovieQueryHandler(_context, _mapper);
            var getByIdQuery = new GetMovieByIdQuery(randomMovie.Id);

            // Act
            var result = await handler.Handle(getByIdQuery, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ApiResponse<MovieResponse>>(result);
            Assert.NotNull(result.Response);
            Assert.Equal(randomMovie.Id, result.Response.Id);
        }

        // [Fact]
        // public async Task Handle_GetAllMovieQuery_WhenNoMovies_ReturnsEmptyList()
        // {
        //     // Arrange
        //     _context.Movies.RemoveRange(_context.Movies); // Tüm filmleri temizle
        //     await _context.SaveChangesAsync();

        //     var handler = new MovieQueryHandler(_context, _mapper);
        //     var getAllQuery = new GetAllMovieQuery();

        //     // Act
        //     var result = await handler.Handle(getAllQuery, new CancellationToken());

        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.IsType<ApiResponse<List<MovieResponse>>>(result);
        //     Assert.NotNull(result.Response);
        //     Assert.Empty(result.Response);
        // }

        [Fact]
        public async Task Handle_GetMovieByIdQuery_WhenMovieNotFound_ThrowsException()
        {
            // Arrange
            // Arrange
            var nonExistentId = -1; // varsayılan olarak mevcut olmayan bir Id
            var getByIdQuery = new GetMovieByIdQuery(nonExistentId);
            var handler = new MovieQueryHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(getByIdQuery, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ApiResponse<MovieResponse>>(result);
            Assert.Null(result.Response);
        }
    }
}