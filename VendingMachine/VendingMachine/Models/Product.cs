using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VendingMachine.Models
{
    public class Product
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Price")]
        [Column(TypeName = "decimal(18,3)")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Available Quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative integer")]
        public int QuantityAvailable { get; set; }

        [Display(Name = "Purchase Quantity")]
        [NotMapped] // This property will not be mapped to a database column
        [Range(0, int.MaxValue, ErrorMessage = "Purchase Quantity must be a non-negative integer")]
        public int PurchaseQuantity { get; set; }
    }
}
