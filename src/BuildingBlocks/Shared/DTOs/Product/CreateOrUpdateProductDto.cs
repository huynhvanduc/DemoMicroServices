using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Product;

public abstract class CreateOrUpdateProductDto
{
    [Required]
    [MaxLength(250, ErrorMessage = "Maximum length for the product name is 250 characters.")]
    public string Name { get; set; }
    [MaxLength(250, ErrorMessage = "Maximum length for the product Sumary is 250 characters.")]
    public string Sumary { get; set; }

    public string Description { get; set; }
    public decimal Price { get; set; }
}
