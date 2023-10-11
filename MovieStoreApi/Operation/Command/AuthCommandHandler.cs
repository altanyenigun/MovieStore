using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieStoreApi.Common.Response;
using MovieStoreApi.Data;
using MovieStoreApi.DTOs;
using MovieStoreApi.Models;
using MovieStoreApi.Operation.Cqrs;

namespace Vk.Operation.Command;

public class AuthCommandHandler :
    IRequestHandler<CustomerLoginCommand, ApiResponse<LoginResponse>>,
    IRequestHandler<CustomerRegisterCommand, ApiResponse>

{
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;


    public AuthCommandHandler(DataContext dbContext, IMapper mapper,IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }


    public async Task<ApiResponse<LoginResponse>> Handle(CustomerLoginCommand request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(request.Model.Username.ToLower()));

        //Checking if the user exists and the password is correct
        if (customer is null || !BCrypt.Net.BCrypt.Verify(request.Model.Password, customer.PasswordHash))
            return new ApiResponse<LoginResponse>("Wrong Username or Password!");

        string token = CreateToken(customer);
        var LoginResponse = new LoginResponse{
            Token=token
        };

        return new ApiResponse<LoginResponse>(LoginResponse);
    }

    public async Task<ApiResponse> Handle(CustomerRegisterCommand request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers.FirstOrDefaultAsync(u => u.Username == request.Model.Username);
        if (customer is not null)
            return new ApiResponse("Username already in use!");


        string PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Model.Password);
        request.Model.Password = PasswordHash;
        Customer mapped = _mapper.Map<Customer>(request.Model);

        var entity = await _dbContext.Customers.AddAsync(mapped, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();

    }

    private string CreateToken(Customer customer)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()), // Adding Id information to the token
                new Claim(ClaimTypes.Name,customer.Username), // Adding Username information to the token
                new Claim(ClaimTypes.Role,customer.Role) // Adding Role information to the token
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value
        )); //Receiving the token we created in appsettings.json

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); //some encryption operations

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        ); //Defining the information to be included in the token.

        var jwt = new JwtSecurityTokenHandler().WriteToken(token); //Creating the token after all processing

        return jwt;
    }
}