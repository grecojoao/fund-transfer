using FundTransfer.Domain.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FundTransfer.IoC.Domain
{
    internal static class DomainInjector
    {
        public static Task Inject(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<FundTransferHandler>();
            return Task.CompletedTask;
        }
    }
}