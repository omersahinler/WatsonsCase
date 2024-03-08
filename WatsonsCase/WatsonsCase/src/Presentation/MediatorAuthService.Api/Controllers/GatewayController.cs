using MassTransit;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using WatsonsCase.Application.Event;
using WatsonsCase.Application.Models.Request;

namespace WatsonsCase.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly IBusControl _bus;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public GatewayController(IBusControl bus)
        {
            _bus = bus;
        }

        [HttpPost("add-basket")]
        public IActionResult AddBasket([FromBody] BasketItemRequest request)
        {
            if (request == null)
                throw new Exception("Request dolu olmalı!");

            _bus.Publish(new BasketItemAddedQueueModel
            {
                ProductName = request.ProductName,
                Quantity = request.Quantity,
                Price = request.Price
            });
            _logger.Info("BasketItemAddedQueueModel gönderildi");

            return Ok();
        }
        [HttpPost("send-order")]
        public IActionResult SendOrder()
        {
            _bus.Publish(new BasketItemGetQueueModel { });
            _logger.Info("BasketItemGetQueueModel gönderildi");

            return Ok();
        }
    }
}
