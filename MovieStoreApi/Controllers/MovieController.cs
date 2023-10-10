using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStoreApi.Common.Response;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Cqrs;

namespace MovieStoreApi.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public class MovieController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovieController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<List<MovieResponse>>> GetAll()
        {
            var operation = new GetAllMovieQuery();
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<MovieResponse>> Get(int id)
        {
            var operation = new GetMovieByIdQuery(id);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<MovieResponse>> Post([FromBody] MovieCreateRequest request)
        {
            var operation = new CreateMovieCommand(request);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody] MovieUpdateRequest request)
        {
            var operation = new UpdateMovieCommand(request, id);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteMovieCommand(id);
            var result = await _mediator.Send(operation);
            return result;
        }
    }
}