using WatsonsCase.Application.Models.Request;

namespace WatsonsCase.Application.Services
{
    public interface IOrderService
    {
        Task<bool> ProcessOrder(OrderRequset orderRequset);
    }
}
