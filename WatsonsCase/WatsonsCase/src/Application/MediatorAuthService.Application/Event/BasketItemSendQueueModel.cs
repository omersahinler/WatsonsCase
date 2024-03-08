namespace WatsonsCase.Application.Event
{
    public class BasketItemSendQueueModel
    {
        public List<BasketItemEvent> BasketItemEvents { get; set; }
    }
    public class BasketItemEvent
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
