using FundTransfer.Domain.Bus.Publishers;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;
using System.Text;

namespace FundTransfer.Infra.RabbitMq.Publishers
{
    public class RabbitMqBusPublisher : IBusPublisher
    {
        private readonly IConfiguration _configuration;

        public RabbitMqBusPublisher(IConfiguration configuration) =>
            _configuration = configuration;

        public Task SendAsync(string message)
        {
            var factory = new ConnectionFactory() { HostName = GetHostName() };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: GetTransferQueueName(), durable: true, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(message);
            try
            {
                Log.Debug($"Posting message to queue({GetTransferQueueName}): {message}");
                channel.BasicPublish(exchange: "", routingKey: GetRoutingKeyName(), basicProperties: null, body: body);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                throw;
            }
            return Task.CompletedTask;
        }

        private string GetRoutingKeyName() =>
            _configuration["RabbitMq:RoutingKey"];

        private string GetTransferQueueName() =>
            _configuration["RabbitMq:TransferQueue"];

        private string GetHostName() =>
            _configuration["RabbitMq:Url"];
    }
}