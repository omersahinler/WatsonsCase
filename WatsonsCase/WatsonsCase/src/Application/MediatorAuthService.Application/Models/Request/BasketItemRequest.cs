using System.ComponentModel.DataAnnotations;

namespace WatsonsCase.Application.Models.Request
{
    public class BasketItemRequest
    {
        [Required(ErrorMessage = "ProductName alanı boş bırakılamaz.")]
        public string ProductName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity 1'den büyük olmalıdır.")]
        public int Quantity { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price 0.01'den büyük olmalıdır.")]
        public decimal Price { get; set; }
    }
}
