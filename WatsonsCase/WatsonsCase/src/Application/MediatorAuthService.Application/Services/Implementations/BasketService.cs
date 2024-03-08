using Newtonsoft.Json;
using StackExchange.Redis;
using WatsonsCase.Application.Models.Request;
using WatsonsCase.Application.Models.Response;

namespace WatsonsCase.Application.Services.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly IDatabase _redisDatabase;
        private readonly ConnectionMultiplexer _redisConnection;

        public BasketService(ConnectionMultiplexer redisConnection)
        {
            _redisDatabase = redisConnection.GetDatabase();
            _redisConnection = redisConnection;
        }
        public async Task AddToBasket(BasketItemRequest item)
        {
            if (item == null)
                throw new Exception("Basket Item dolu olmalı!");

            var key = $"Basket:{item.ProductName}";
            var existingBasket = await _redisDatabase.StringGetAsync(key);
            var serializedBasket = JsonConvert.SerializeObject(item);

            if (!existingBasket.IsNull)
                await _redisDatabase.StringSetAsync(key, serializedBasket);
            else
                await _redisDatabase.StringSetAsync(key, serializedBasket);
        }

        public async Task<BasketItemResponse> GetBasket(string productName)
        {
            var key = $"Basket:{productName}";
            var basketJson = await _redisDatabase.StringGetAsync(key);
            return JsonConvert.DeserializeObject<BasketItemResponse>(basketJson);

        }
        public async Task DeleteBasket()
        {
            var endpoints = _redisConnection.GetEndPoints();
            var server = _redisConnection.GetServer(endpoints.First());

            var keys = server.Keys(pattern: "Basket:*");

            foreach (var key in keys)
            {
                await _redisDatabase.KeyDeleteAsync(key);
            }
        }
        public IEnumerable<BasketItemResponse> GetAllBaskets()
        {
            var keys = _redisConnection.GetServer(_redisConnection.GetEndPoints()[0]).Keys(pattern: "Basket:*");

            var baskets = new List<BasketItemResponse>();

            foreach (var key in keys)
            {
                var basketJson = _redisDatabase.StringGet(key);
                var basketItemSendEvent = JsonConvert.DeserializeObject<BasketItemResponse>(basketJson);
                baskets.Add(basketItemSendEvent);
            }

            if (baskets == null || !baskets.Any())
                throw new Exception("Sepettekiler Redisten bulunamdı!");
            return baskets;
        }
    }
}
