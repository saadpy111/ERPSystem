using Events.WebsiteEvents;
using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Consumers
{
    public class OrderCreatedAnalyticsConsumer : INotificationHandler<OrderCreatedEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerAnalyticsRepository _analyticsRepository;

        public OrderCreatedAnalyticsConsumer(
            IOrderRepository orderRepository,
            ICustomerAnalyticsRepository analyticsRepository)
        {
            _orderRepository = orderRepository;
            _analyticsRepository = analyticsRepository;
        }

        public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            // Use repository to fetch order with items
            var order = await _orderRepository.GetOrderForAnalyticsAsync(notification.OrderId, cancellationToken);

            if (order == null) return;

            // Use repository to fetch analytics record
            var analytics = await _analyticsRepository.GetByUserIdAsync(order.UserId, cancellationToken);

            bool isNew = false;
            if (analytics == null)
            {
                analytics = new CustomerAnalytics
                {
                    UserId = order.UserId,
                    TenantId = order.TenantId,
                    FirstOrderDate = order.OrderDate
                };
                isNew = true;
            }
            else
            {
                // Ensure we are working with a fresh tracked entity for updates
                // (Though GetByUserIdAsync used AsNoTracking, we'll update via repository)
            }

            // Update basic stats
            analytics.OrdersCount += 1;
            analytics.TotalSpent += order.TotalAmount;
            analytics.AverageOrderValue = analytics.TotalSpent / analytics.OrdersCount;
            
            // Average Days Between Orders
            if (analytics.LastOrderDate.HasValue)
            {
                var daysSinceLast = (order.OrderDate - analytics.LastOrderDate.Value).TotalDays;
                if (analytics.OrdersCount > 1)
                {
                    int intervals = analytics.OrdersCount - 1;
                    analytics.AverageDaysBetweenOrders = ((analytics.AverageDaysBetweenOrders * (intervals - 1)) + daysSinceLast) / intervals;
                }
                else
                {
                    analytics.AverageDaysBetweenOrders = daysSinceLast;
                }
            }
            
            analytics.LastOrderDate = order.OrderDate;

            // Total items purchased
            var orderItemsCount = order.Items.Sum(i => i.Quantity);
            analytics.TotalItemsPurchased += orderItemsCount;

            // Return stats
            analytics.ReturnRate = analytics.TotalItemsPurchased > 0 
                ? (double)analytics.ReturnedItems / analytics.TotalItemsPurchased * 100 
                : 0;

            analytics.UpdatedAt = DateTime.UtcNow;

            // Use repository for complex behavior stats
            analytics.FavoritePurchaseDay = await _orderRepository.GetFavoritePurchaseDayAsync(order.UserId, cancellationToken);
            analytics.MostPurchasedCategory = await _orderRepository.GetMostPurchasedCategoryAsync(order.UserId, cancellationToken);

            if (isNew)
            {
                await _analyticsRepository.CreateAsync(analytics, cancellationToken);
            }
            else
            {
                await _analyticsRepository.UpdateAsync(analytics, cancellationToken);
            }
        }
    }
}
