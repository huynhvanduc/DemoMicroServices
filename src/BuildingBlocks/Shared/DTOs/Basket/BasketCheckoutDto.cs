using System.ComponentModel.DataAnnotations;

namespace Basket.API.Entities;

public class BasketCheckoutDto
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public decimal TotalPrice { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }    
    public string InvoiceAddress { get; set; }
    public string ShippingAddress { get; set; }
}
