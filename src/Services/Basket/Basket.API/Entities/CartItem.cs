namespace Basket.API.Entities;

public class CartItem
{
    public int Quantity { get; set; }
    public decimal ItemPrice { get; set; }
    public string ItemNo { get; set; }
    public string ItemName { get; set; }

    public int Availableuantity { get;  set; }

    public void SetAvailableQuantity(int stock) => Availableuantity = stock;  
}
