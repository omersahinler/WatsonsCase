using WatsonsCase.Application.Models.Request;
using WatsonsCase.Application.Models.Response;

namespace WatsonsCase.Application.Services
{
    public interface IBasketService
    {
        Task AddToBasket(BasketItemRequest item);
        Task DeleteBasket();
        Task<BasketItemResponse> GetBasket(string productName);
        IEnumerable<BasketItemResponse> GetAllBaskets();
    }
}
