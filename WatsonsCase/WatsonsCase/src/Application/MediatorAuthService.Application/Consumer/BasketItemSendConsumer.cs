using MassTransit;
using WatsonsCase.Application.Models.Request;
using WatsonsCase.Application.Event;
using WatsonsCase.Application.Services;
using NLog;

namespace WatsonsCase.Application.Consumer
{
    public class BasketItemSendConsumer : IConsumer<BasketItemSendQueueModel>
    {
        private readonly IOrderService _orderService;
        private readonly IBusControl _bus;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public BasketItemSendConsumer(IOrderService orderService, IBusControl bus)
        {
            _orderService = orderService;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<BasketItemSendQueueModel> context)
        {
            _logger.Info("BasketItemSendConsumer başladı.");

            var basketItem = context.Message;
            try
            {
                var order = new OrderRequset
                {
                    Items = basketItem.BasketItemEvents.Select(_ => new BasketItemRequest { Price = _.Price, ProductName = _.ProductName, Quantity = _.Quantity }).ToList()
                };

                bool orderProcessed = await _orderService.ProcessOrder(order);

                if (orderProcessed)
                {
                    _bus.Publish(new BasketItemDeleteQueueModel());
                    _logger.Info("BasketItemDeleteQueueModel gönderildi");

                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }

        }
    }
}
