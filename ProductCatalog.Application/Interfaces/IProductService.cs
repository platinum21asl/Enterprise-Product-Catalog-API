using ProductCatalog.Application.Models;
using ProductCatalog.Application.Models.DTOs.Req;
using ProductCatalog.Application.Models.DTOs.Res;

namespace ProductCatalog.Application.Interfaces
{
    public interface IProductService
    {
        Task<BaseResponse<IEnumerable<ProductResponseDto>>> GetAllProductsAsync();
        Task<BaseResponse<ProductResponseDto>> GetProductByIdAsync(Guid id);
        Task<BaseResponse<ProductResponseDto>> CreateProductAsync(CreateProductRequestDto request);
        Task<BaseResponse<bool>> UpdateProductAsync(UpdateProductRequestDto request);
        Task<BaseResponse<bool>> DeleteProductAsync(Guid id);

        Task<BaseResponse<PagedResult<ProductResponseDto>>> GetPagedProductsAsync(ProductFilterDto filter);
    }
}