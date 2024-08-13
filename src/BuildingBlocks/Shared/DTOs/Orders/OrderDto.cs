using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.DTOs.Orders
{
    public class OrderDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }

        public decimal TotalPrice { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid CustomerNo { get; set; } = Guid.NewGuid();

        public Guid DocumentNo { get; set; } = Guid.NewGuid();

        public string EmailAddress { get; set; }

        public string ShippingAddress { get; set; }

        public string InvoiceAddress { get; set; }

        public EOrderStatus Status { get; set; }
    }
}
