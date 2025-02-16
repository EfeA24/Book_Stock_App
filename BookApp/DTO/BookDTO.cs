using System.ComponentModel.DataAnnotations;

namespace BookApp.DTO
{
    public class BookDTO
    {
        public string? Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal? Price { get; set; }
    }
}
