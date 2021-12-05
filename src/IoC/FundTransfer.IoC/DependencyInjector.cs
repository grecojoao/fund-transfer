using FundTransfer.IoC.Domain;
using FundTransfer.IoC.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FundTransfer.IoC
{
    public static class DependencyInjector
    {
        public static Task InjectDependencies(IServiceCollection services, IConfiguration configuration)
        {
            DomainInjector.Inject(services, configuration).Wait();
            InfraInjector.Inject(services, configuration).Wait();
            return Task.CompletedTask;
        }
    }
}