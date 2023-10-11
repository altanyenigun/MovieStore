using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Common.Response;
using MovieStoreApi.Data;
using MovieStoreApi.DTOs;
using MovieStoreApi.Operation.Cqrs;


namespace MovieStoreApi.Operation.Query;

public class CustomerQueryHandler :
    IRequestHandler<CustomeOrderQuery, ApiResponse<List<CustomerOrderResponse>>>
{
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;

    public CustomerQueryHandler(DataContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<CustomerOrderResponse>>> Handle(CustomeOrderQuery request, CancellationToken cancellationToken)
    {
        var data = await _dbContext.Orders
            .Include(x => x.Movie)
            .Where(x => x.CustomerId == request.customerId)
            .ToListAsync();

        var response = _mapper.Map<List<CustomerOrderResponse>>(data);
        return new ApiResponse<List<CustomerOrderResponse>>(response);
    }

}