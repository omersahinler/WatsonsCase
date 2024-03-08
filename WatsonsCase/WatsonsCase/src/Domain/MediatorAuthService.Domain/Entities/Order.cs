using WatsonsCase.Domain.Core.Base.Abstract;
using WatsonsCase.Domain.Core.Base.Concrete;

namespace WatsonsCase.Domain.Entities
{
    public class Order:BaseEntity, IEntity
    {
        public List<BasketItem> Items { get; set; }
    }
}
