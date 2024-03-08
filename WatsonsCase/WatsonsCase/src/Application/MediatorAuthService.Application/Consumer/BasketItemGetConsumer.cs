using MassTransit;
using NLog;
using WatsonsCase.Application.Event;
using WatsonsCase.Application.Services;

namespace WatsonsCase.Application.Consumer
{
    public class BasketItemGetConsumer : IConsumer<BasketItemGetQueueModel>
    {
        private readonly IBasketService _basketService;
        private readonly IBusControl _bus;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public BasketItemGetConsumer(IBasketService basketService, IBusControl bus)
        {
            _basketService = basketService;
            _bus = bus;
        }

        public Task Consume(ConsumeContext<BasketItemGetQueueModel> context)
        {
            _logger.Info("BasketItemGetConsumer başladı.");

            try
            {
                var basketItem = context.Message;
                var getAll = _basketService.GetAllBaskets();
                if (getAll == null || !getAll.Any())
                    throw new Exception("Baskette kayıt bulunamadı");

                var basketItemSendEvent = new BasketItemSendQueueModel();

                basketItemSendEvent.BasketItemEvents = getAll.Select(_ => new BasketItemEvent
                {
                    Price = _.Price,
                    ProductName = _.ProductName,
                    Quantity = _.Quantity
                }).ToList();

                _logger.Info("BasketItemSendQueueModel gönderildi.");

                _bus.Publish(basketItemSendEvent);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
            return Task.CompletedTask;
        }
    }
}
