using WatsonsCase.Domain.Core.Base.Abstract;
using WatsonsCase.Domain.Core.Base.Concrete;

namespace WatsonsCase.Domain.Entities;

public class BasketItem : BaseEntity, IEntity
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public long OrderId { get; set; }
}

