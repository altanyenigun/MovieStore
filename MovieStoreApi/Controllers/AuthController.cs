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
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/login")]
        public async Task<ApiResponse<LoginResponse>> Post([FromBody] CustomerLoginRequest request)
        {
            var operation = new CustomerLoginCommand(request);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPost("/register")]
        public async Task<ApiResponse> Put([FromBody] CustomerRegisterRequest request)
        {
            var operation = new CustomerRegisterCommand(request);
            var result = await _mediator.Send(operation);
            return result;
        }
    }
}