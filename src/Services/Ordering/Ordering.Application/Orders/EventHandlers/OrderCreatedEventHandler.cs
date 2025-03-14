namespace Ordering.Application.Orders.EventHandlers;

public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        /// this will be triggered after event published
        /// we will modify here for to publish integration events
        logger.LogInformation("Domain Event handled: {DomainEvent}",notification.GetType().Name);
        return Task.CompletedTask;
    }
}
