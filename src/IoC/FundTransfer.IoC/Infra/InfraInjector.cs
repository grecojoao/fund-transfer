using FundTransfer.Domain.Repositories;
using FundTransfer.Infra.Storage.Context;
using FundTransfer.Infra.Storage.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FundTransfer.IoC.Infra
{
    internal static class InfraInjector
    {
        public static Task Inject(IServiceCollection services, IConfiguration configuration)
        {
            InjectStorage(services, configuration).Wait();
            return Task.CompletedTask;
        }

        private static Task InjectStorage(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContext, DataContext>(options =>
            {
                options.UseInMemoryDatabase("FundTransferDataBase");
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            services.AddScoped<ITransferRepository, TransferRepository>();
            return Task.CompletedTask;
        }
    }
}