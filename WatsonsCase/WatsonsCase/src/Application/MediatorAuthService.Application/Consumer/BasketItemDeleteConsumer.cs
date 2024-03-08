using MassTransit;
using NLog;
using WatsonsCase.Application.Event;
using WatsonsCase.Application.Services;

namespace WatsonsCase.Application.Consumer
{
    public class BasketItemDeleteConsumer : IConsumer<BasketItemDeleteQueueModel>
    {
        private readonly IBasketService _basketService;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public BasketItemDeleteConsumer(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public Task Consume(ConsumeContext<BasketItemDeleteQueueModel> context)
        {
            _logger.Info("BasketItemDeleteConsumer başladı.");
            try
            {
                _basketService.DeleteBasket();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }

            return Task.CompletedTask;
        }
    }
}
