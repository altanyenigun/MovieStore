using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStoreApi.Common.Response;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Cqrs;

namespace MovieStoreApi.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("/buyMovie")]
        [Authorize(Roles = "Customer")]
        public async Task<ApiResponse> BuyMovie(int movieId)
        {
            var customerId = (User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var operation = new CustomerBuyMovieCommand(int.Parse(customerId), movieId);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPost("/addFavoriteGenre")]
        [Authorize(Roles = "Customer")]
        public async Task<ApiResponse> AddFavoriteGenre(int genreId)
        {
            var customerId = (User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var operation = new CustomerAddFavoriteGenreCommand(int.Parse(customerId), genreId);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpGet("/myMovies")]
        [Authorize(Roles = "Customer")]
        public async Task<ApiResponse<List<CustomerOrderResponse>>> CustomerOrders()
        {
            var customerId = (User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var operation = new CustomeOrderQuery(int.Parse(customerId));
            var result = await _mediator.Send(operation);
            return result;
        }
    }
}