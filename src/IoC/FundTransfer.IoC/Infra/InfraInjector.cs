using FundTransfer.Domain.Bus.Publishers;
using FundTransfer.Domain.Repositories;
using FundTransfer.Infra.RabbitMq.Publishers;
using FundTransfer.Infra.Storage.RavenDb.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;

namespace FundTransfer.IoC.Infra
{
    internal static class InfraInjector
    {
        public static Task Inject(IServiceCollection services, IConfiguration configuration)
        {
            InjectStorage(services, configuration);
            InjectBus(services);
            return Task.CompletedTask;
        }

        private static void InjectStorage(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(a => ConfigureDataBase(configuration));
            services.AddScoped<ITransferRepository, TransferRepository>();
        }

        private static DocumentStore ConfigureDataBase(IConfiguration configuration) =>
            new()
            {
                Urls = new string[] { configuration["ConnectionStrings:RavenDb:Url"] },
                Database = configuration["ConnectionStrings:RavenDb:DataBase"]
            };

        private static void InjectBus(IServiceCollection services) =>
            services.AddScoped<IBusPublisher, RabbitMqBusPublisher>();
    }
}