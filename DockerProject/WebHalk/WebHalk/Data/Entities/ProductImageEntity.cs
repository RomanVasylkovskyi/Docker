using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebHalk.Data.Entities
{
    public class ProductImageEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Image { get; set; } = string.Empty;
        public int Priotity { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual ProductEntity? Product { get; set; }
    }
}