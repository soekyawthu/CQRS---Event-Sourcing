using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SocialMedia.Core.Consumers;

namespace Post.Query.Infrastructure.Consumers;

public class ConsumerBackgroundService : BackgroundService
{
    private readonly ILogger<ConsumerBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ConsumerBackgroundService(ILogger<ConsumerBackgroundService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Event consumer background service running");

        using var serviceScope = _serviceProvider.CreateScope();
        var consumer = serviceScope.ServiceProvider.GetRequiredService<IEventConsumer>();
        const string topic = "SocialMediaPostEvent";
        await Task.Run(() => consumer.Consume(topic), stoppingToken);
        //consumer.Consume(topic);
    }
}