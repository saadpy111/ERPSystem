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
        private readonly IWebsiteAnalyticsRepository _websiteAnalyticsRepository;

        public OrderCreatedAnalyticsConsumer(
            IOrderRepository orderRepository,
            ICustomerAnalyticsRepository analyticsRepository,
            IWebsiteAnalyticsRepository websiteAnalyticsRepository)
        {
            _orderRepository = orderRepository;
            _analyticsRepository = analyticsRepository;
            _websiteAnalyticsRepository = websiteAnalyticsRepository;
        }

        public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            //  Load order with items
            var order = await _orderRepository.GetOrderForAnalyticsAsync(
                notification.OrderId,
                cancellationToken);

            if (order == null)
                return;

            //  Load customer analytics
            var analytics = await _analyticsRepository.GetByUserIdAsync(
                order.UserId,
                cancellationToken);

            var isNew = analytics == null;

            if (analytics == null)
            {
                analytics = new CustomerAnalytics
                {
                    UserId = order.UserId,
                    TenantId = order.TenantId,
                    FirstOrderDate = order.OrderDate,
                    OrdersCount = 0,
                    TotalSpent = 0,
                    TotalItemsPurchased = 0,
                    ReturnedItems = 0
                };
            }

            //  Update purchase metrics
            analytics.OrdersCount += 1;
            analytics.TotalSpent += order.TotalAmount;

            analytics.AverageOrderValue =
                analytics.OrdersCount > 0
                    ? analytics.TotalSpent / analytics.OrdersCount
                    : 0;

            //  Average days between orders
            if (analytics.LastOrderDate.HasValue)
            {
                var daysSinceLast =
                    (order.OrderDate - analytics.LastOrderDate.Value).TotalDays;

                if (analytics.OrdersCount > 1)
                {
                    var intervals = analytics.OrdersCount - 1;

                    analytics.AverageDaysBetweenOrders =
                        ((analytics.AverageDaysBetweenOrders * (intervals - 1))
                        + daysSinceLast) / intervals;
                }
                else
                {
                    analytics.AverageDaysBetweenOrders = daysSinceLast;
                }
            }

            analytics.LastOrderDate = order.OrderDate;

            //  Total items purchased
            var orderItemsCount = order.Items.Sum(i => i.Quantity);
            analytics.TotalItemsPurchased += orderItemsCount;

            // Return rate
            analytics.ReturnRate =
                analytics.TotalItemsPurchased > 0
                    ? (double)analytics.ReturnedItems / analytics.TotalItemsPurchased * 100
                    : 0;

            analytics.UpdatedAt = DateTime.UtcNow;

            //  Behavior metrics
            analytics.FavoritePurchaseDay =
                await _orderRepository.GetFavoritePurchaseDayAsync(
                    order.UserId,
                    cancellationToken);

            analytics.MostPurchasedCategory =
                await _orderRepository.GetMostPurchasedCategoryAsync(
                    order.UserId,
                    cancellationToken);

            // Persist analytics
            if (isNew)
            {
                await _analyticsRepository.CreateAsync(analytics, cancellationToken);
            }
            else
            {
                await _analyticsRepository.UpdateAsync(analytics, cancellationToken);
            }

            // Update website funnel analytics
            await _websiteAnalyticsRepository.IncrementOrdersAsync(cancellationToken);

            await _websiteAnalyticsRepository.AddRevenueAsync(
                order.TotalAmount,
                cancellationToken);
        }
    }
}