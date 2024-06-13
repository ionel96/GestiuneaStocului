using System.ComponentModel.DataAnnotations;

namespace GestiuneaStocului.Models;

public class ProductReports {

    [Key]
    public int ReportId {  get; set; }
    public int ProductId {  get; set; }
    public int QuantityIn { get; set; }
    public int QuantityOut { get; set; }
    public int Stock { get; set; }

    public virtual Products? Product { get; set; }
}