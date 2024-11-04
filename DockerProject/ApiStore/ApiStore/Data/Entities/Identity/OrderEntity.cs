using ApiStore.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiStore.Data.Entities
{
    public class OrderEntity
    {
        [Key]
        public int OrderId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserEntity? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public virtual ICollection<OrderItemEntity>? OrderItems { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class OrderItemEntity
    {
        [Key]
        public int OrderItemId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual OrderEntity Order { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
