using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Data;
using MovieStoreApi.Services.Logger;

namespace MovieStoreApi.Common.DIContainer
{
    public static class DIContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddMediatR(typeof(Program).Assembly);
            services.AddSingleton<ILoggerService, ConsoleLogger>();
        }
    }
}