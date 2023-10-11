using MediatR;
using MovieStoreApi.Common.Response;
using MovieStoreApi.DTOs;

namespace MovieStoreApi.Operation.Cqrs;

public record CustomerLoginCommand(CustomerLoginRequest Model) : IRequest<ApiResponse<LoginResponse>>;
public record CustomerRegisterCommand(CustomerRegisterRequest Model) : IRequest<ApiResponse>;