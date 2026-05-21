using ProductCatalog.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Domain.Entities
{
    [Table("MST_PRODUCT")]
    public class Product : BaseEntity
    {
        [Column("Name")]
        public string Name { get; set; } = string.Empty;
        [Column("SKU")]
        public string SKU { get; set; } = string.Empty;
        [Column("Description")]
        public string? Description { get; set; }
        [Column("Price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column("StockQuantity")]
        public int StockQuantity { get; set; }
    }
}
