using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Infrastructure.Data;

namespace ProductCatalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.IsDeleted = true;
                product.DeletedAt = DateTime.UtcNow;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedProductsAsync(int pageNumber, int pageSize, string keyword)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p => p.Name.Contains(keyword) || p.SKU.Contains(keyword));
            }

            int totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}