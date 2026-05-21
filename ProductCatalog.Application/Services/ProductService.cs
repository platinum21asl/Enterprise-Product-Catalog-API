using FluentValidation;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Application.Models;
using ProductCatalog.Application.Models.DTOs.Req;
using ProductCatalog.Application.Models.DTOs.Res;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<CreateProductRequestDto> _createValidator;
        private readonly IValidator<UpdateProductRequestDto> _updateValidator;

        public ProductService(
             IProductRepository productRepository,
             IValidator<CreateProductRequestDto> createValidator,
             IValidator<UpdateProductRequestDto> updateValidator)
        {
            _productRepository = productRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<BaseResponse<IEnumerable<ProductResponseDto>>> GetAllProductsAsync()
        {
            // --- SIMULATION ERROR  ---
            //throw new Exception("Test Error Database Down! Server on fire!");
            var products = await _productRepository.GetAllAsync();

            var productDtos = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            });

            return BaseResponse<IEnumerable<ProductResponseDto>>.SuccessResponse(productDtos);
        }

        public async Task<BaseResponse<ProductResponseDto>> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return BaseResponse<ProductResponseDto>.ErrorResponse("Product not found.");
            }

            var productDto = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };

            return BaseResponse<ProductResponseDto>.SuccessResponse(productDto);
        }

        public async Task<BaseResponse<ProductResponseDto>> CreateProductAsync(CreateProductRequestDto request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BaseResponse<ProductResponseDto>.ErrorResponse("Validation failed.", errors);
            }

            var product = new Product
            {
                Name = request.Name,
                SKU = request.SKU,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity
            };

            var createdProduct = await _productRepository.AddAsync(product);

            var responseDto = new ProductResponseDto
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                SKU = createdProduct.SKU,
                Description = createdProduct.Description,
                Price = createdProduct.Price,
                StockQuantity = createdProduct.StockQuantity
            };

            return BaseResponse<ProductResponseDto>.SuccessResponse(responseDto, "Product created successfully.");
        }

        public async Task<BaseResponse<bool>> UpdateProductAsync(UpdateProductRequestDto request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BaseResponse<bool>.ErrorResponse("Validation failed.", errors);
            }

            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                return BaseResponse<bool>.ErrorResponse("Product not found.");
            }

            product.Name = request.Name;
            product.SKU = request.SKU;
            product.Description = request.Description;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;

            await _productRepository.UpdateAsync(product);

            return BaseResponse<bool>.SuccessResponse(true, "Product updated successfully.");
        }

        public async Task<BaseResponse<bool>> DeleteProductAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return BaseResponse<bool>.ErrorResponse("Product not found.");
            }

            await _productRepository.DeleteAsync(id);

            return BaseResponse<bool>.SuccessResponse(true, "Product deleted successfully.");
        }

        public async Task<BaseResponse<PagedResult<ProductResponseDto>>> GetPagedProductsAsync(ProductFilterDto filter)
        {
            var (items, totalCount) = await _productRepository.GetPagedProductsAsync(filter.PageNumber, filter.PageSize, filter.Keyword);

            var productDtos = items.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            });

            var pagedResult = new PagedResult<ProductResponseDto>
            {
                Items = productDtos,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };

            return BaseResponse<PagedResult<ProductResponseDto>>.SuccessResponse(pagedResult);
        }
    }
}