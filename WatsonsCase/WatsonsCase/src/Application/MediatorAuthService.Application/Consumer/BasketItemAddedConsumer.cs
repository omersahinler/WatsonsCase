using MassTransit;
using NLog;
using WatsonsCase.Application.Event;
using WatsonsCase.Application.Models.Request;
using WatsonsCase.Application.Services;

namespace WatsonsCase.Application.Consumer
{
    public class BasketItemAddedConsumer : IConsumer<BasketItemAddedQueueModel>
    {
        private readonly IBasketService _basketService;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public BasketItemAddedConsumer(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public Task Consume(ConsumeContext<BasketItemAddedQueueModel> context)
        {
            _logger.Info("BasketItemDeleteConsumer başladı.");
            try
            {
                var basketItem = context.Message;
                _basketService.AddToBasket(new BasketItemRequest
                {
                    Price = basketItem.Price,
                    ProductName = basketItem.ProductName,
                    Quantity = basketItem.Quantity
                });
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
