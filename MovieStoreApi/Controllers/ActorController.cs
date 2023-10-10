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
    public class ActorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<List<ActorResponse>>> GetAll()
        {
            var operation = new GetAllActorQuery();
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<ActorResponse>> Get(int id)
        {
            var operation = new GetActorByIdQuery(id);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<ActorResponse>> Post([FromBody] ActorCreateRequest request)
        {
            var operation = new CreateActorCommand(request);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, [FromBody] ActorUpdateRequest request)
        {
            var operation = new UpdateActorCommand(request, id);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            var operation = new DeleteActorCommand(id);
            var result = await _mediator.Send(operation);
            return result;
        }
    }
}