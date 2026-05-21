using System;

namespace ProductCatalog.Application.Models.DTOs.Req
{
    public class UpdateProductRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
