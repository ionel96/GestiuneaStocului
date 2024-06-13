using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestiuneaStocului.Models;

public class Products {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductId { get; set; }
    public required string Name { get; set; }
    public required int Stock { get; set; }
    public required decimal Price { get; set; }

    public virtual ICollection<ProductReports> ProductReports { get; set; } = new List<ProductReports>();
}