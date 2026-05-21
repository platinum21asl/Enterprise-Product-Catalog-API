using ProductCatalog.Domain.Entities;


namespace ProductCatalog.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
        // Mengembalikan Tuple berisi daftar produk dan total keseluruhan data
        Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedProductsAsync(int pageNumber, int pageSize, string keyword);
    }
}
