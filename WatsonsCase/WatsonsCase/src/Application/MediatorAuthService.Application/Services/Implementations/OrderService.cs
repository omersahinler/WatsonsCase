using WatsonsCase.Application.Models.Request;
using WatsonsCase.Domain.Entities;
using WatsonsCase.Infrastructure.UnitOfWork;

namespace WatsonsCase.Application.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> ProcessOrder(OrderRequset orderRequset)
        {
            if (orderRequset == null || !orderRequset.Items.Any())
                throw new Exception("Sipariş dolu olmalı!");

            Order order = new Order
            {
                Items = orderRequset.Items.Select(_ => new BasketItem { Price = _.Price, ProductName = _.ProductName, Quantity = _.Quantity }).ToList()
            };
            await _unitOfWork.GetRepository<Order>().AddAsync(order);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

            return true;
        }
    }
}
