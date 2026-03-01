using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Telemetry.Application.Dtos;
using Telemetry.Application.Services.Interfaces;

namespace Telemetry.Infrastructure.Mensageria
{
    public class RabbitMqLeiturasConsumerHostedService : BackgroundService
    {
        private readonly RabbitMqOptions _opt;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RabbitMqLeiturasConsumerHostedService> _logger;

        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMqLeiturasConsumerHostedService(
            IOptions<RabbitMqOptions> opt,
            IServiceScopeFactory scopeFactory,
            ILogger<RabbitMqLeiturasConsumerHostedService> logger)
        {
            _opt = opt.Value;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _opt.HostName,
                Port = _opt.Port,
                UserName = _opt.UserName,
                Password = _opt.Password
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync();

            // Garante que a fila existe (igual no publisher)
            await _channel.QueueDeclareAsync(
                queue: _opt.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken);

            // Evita “estouro” processando muitas mensagens de uma vez
            await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 10, global: false, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var evt = JsonSerializer.Deserialize<LeituraSensorRecebidaEvent>(
                        json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (evt is null)
                        throw new InvalidOperationException("Mensagem inválida (JSON não desserializou).");

                    // Cria um escopo de DI por mensagem
                    using var scope = _scopeFactory.CreateScope();
                    var processador = scope.ServiceProvider.GetRequiredService<IProcessadorLeituraService>();

                    // Reaproveita o mesmo DTO que seu processador já espera (ou adapte)
                    await processador.ProcessarLeituraAsync(
                        new RegistrarLeituraRequest(
                            evt.TalhaoId,
                            evt.DataHoraLeituraUtc,
                            evt.UmidadeSolo,
                            evt.Temperatura,
                            evt.Precipitacao),
                        stoppingToken);

                    // ACK: removemos da fila
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro consumindo mensagem do RabbitMQ. DeliveryTag={DeliveryTag}", ea.DeliveryTag);

                    // NACK: você escolhe requeue ou não.
                    await _channel!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: _opt.QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            _logger.LogInformation("Consumer RabbitMQ ativo. Fila: {Queue}", _opt.QueueName);

            // Mantém o serviço vivo até cancelar
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_channel is not null) await _channel.CloseAsync(cancellationToken);
                if (_connection is not null) await _connection.CloseAsync(cancellationToken);
            }
            catch { /* ignore */ }

            await base.StopAsync(cancellationToken);
        }
    }
}