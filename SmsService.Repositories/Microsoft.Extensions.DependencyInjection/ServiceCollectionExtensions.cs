using Microsoft.EntityFrameworkCore;
using SmsService.Repositories;
using Tgs.Sms.Repositories.Entities;
using Tgs.SmsService;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSmsServiceRepositories(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> dbContextConfigure)
        => services
            .AddDbContext<TgsContext>(dbContextConfigure)
            .AddTransient<ITgsRepository, TgsRepository>();
}
