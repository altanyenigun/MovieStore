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
    public class DirectorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DirectorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<List<DirectorResponse>>> GetAll()
        {
            var operation = new GetAllDirectorQuery();
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<DirectorResponse>> Get(int id)
        {
            var operation = new GetDirectorByIdQuery(id);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<DirectorResponse>> Post([FromBody] DirectorCreateRequest request)
        {
            var operation = new CreateDirectorCommand(request);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody] DirectorUpdateRequest request)
        {
            var operation = new UpdateDirectorCommand(request, id);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteDirectorCommand(id);
            var result = await _mediator.Send(operation);
            return result;
        }
    }
}