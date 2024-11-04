using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiStore.Data.Entities;

[Table("tblCategories")]
public class CategoryEntity
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(255)]
    public string Name { get; set; } = string.Empty;
    [StringLength(4000)]
    public string? Description { get; set; }
    [StringLength(200)]
    public string? Image { get; set; }
    public virtual ICollection<ProductEntity>? Products { get; set; }
}
