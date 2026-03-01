using Ingestion.Application.Dtos;
using Ingestion.Application.Mensageria;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Ingestion.Infrastructure.Mensageria
{
    public class RabbitMqLeituraPublisher : ILeituraPublisher
    {
        private readonly RabbitMqOptions _opt;

        public RabbitMqLeituraPublisher(IOptions<RabbitMqOptions> opt)
            => _opt = opt.Value;

        public async Task PublicarAsync(LeituraSensorRecebidaEvent evento, CancellationToken ct)
        {
            var factory = new ConnectionFactory
            {
                HostName = _opt.HostName,
                Port = _opt.Port,
                UserName = _opt.UserName,
                Password = _opt.Password
            };

            await using var connection = await factory.CreateConnectionAsync(ct);
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: _opt.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: ct);

            var json = JsonSerializer.Serialize(evento);
            var body = Encoding.UTF8.GetBytes(json);

            var props = new BasicProperties
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: _opt.QueueName,
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: ct);
        }
    }
}
