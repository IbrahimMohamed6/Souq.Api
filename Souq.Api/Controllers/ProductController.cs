
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Souq.Api.Dtos;
using Souq.Api.Helper.Errors;
using Souq.Api.Helper.Paginitions;
using Souq.Core.Entites.Products;
using Souq.Core.RepositoryContract;
using Souq.Core.Specification;

namespace Souq.Api.Controllers
{

    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _repository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Brand> _brandRepo;
        private readonly IGenericRepository<Category> _ctegoryRepo;
        private readonly IProductRepository _productRepository;

        public ProductController(IGenericRepository<Product> repository
            , IMapper mapper
            , IGenericRepository<Brand> brandRepo
            , IGenericRepository<Category> ctegoryRepo,
            IProductRepository productRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _brandRepo = brandRepo;
            _ctegoryRepo = ctegoryRepo;
            _productRepository = productRepository;
        }
        // Get All Products With Pagination and Filter
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> Producs([FromQuery] ProductSpecParams spec)
        {
            var Spec = new ProductSpecification(spec);
            var product = await _repository.GetAllWithSpecAsync(Spec);

            var MappedProduct = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(product);
            var CountSpec = new ProductWithFiltersForCountSpecification(spec);
            var Count = await _repository.GetCountForPagination(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(spec.PageIndex, spec.PageSize, MappedProduct, Count));


        }
        // Get Product By Id

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProdcts(int id)
        {
            var Spec = new ProductSpecification(id);
            var Product = await _repository.GetByIdWithSpecAsync(Spec);
            if (Product == null)
                return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<Product, ProductToReturnDto>(Product));


        }
        // Get All Brands
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<Brand>>> Brands()
        {
            var Barnds = await _brandRepo.GetAllAsync();
            return Ok(Barnds);

        }
        // Get All Category

        [HttpGet("Category")]
        public async Task<ActionResult<IReadOnlyList<Brand>>> Category()
        {
            var Ctegory = await _ctegoryRepo.GetAllAsync();
            return Ok(Ctegory);

        }
        // Get Product By Category Name

        [HttpGet("Productcategory/{categoryName}")]
        public async Task<ActionResult<IReadOnlyList<Product>>> ProductByCategoryName(string categoryName)
        {
            var Product = await _productRepository.GetAllProductByCategory(categoryName);
            if (Product is null)
                return NotFound(new ApiResponse(404, "Products Not Found"));
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductWithBrandAndCategoryToReturnDto>>(Product));

        }
        // Get Product By Brand Name
        [HttpGet("ProductBrand/{brandName}")]
        public async Task<ActionResult<IReadOnlyList<Product>>> ProductByBrandNameName(string brandName)
        {

            var Product = await _productRepository.GetAllProductByBrand(brandName);
            if (Product is null)
                return NotFound(new ApiResponse(404, "Products Not Found"));
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductWithBrandAndCategoryToReturnDto>>(Product));
        }

        // Add Product
        [Authorize("Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductToReturnDto>> AddProduct(ProductToReturnDto productDto)
        {
            var Product = _mapper.Map<ProductToReturnDto, Product>(productDto);
            var AddedProduct = await _productRepository.AddProduct(Product);
            if (AddedProduct is null)
                return BadRequest(new ApiResponse(400, "Product Not Added"));
            return Ok(_mapper.Map<Product, ProductToReturnDto>(AddedProduct));
        }
        // Update Product
        [Authorize("Admin")]
        [HttpPut]
        public async Task<ActionResult<ProductToReturnDto>> UpdateProduct(ProductToReturnDto productDto)
        {
            var Product = _mapper.Map<ProductToReturnDto, Product>(productDto);
            var UpdatedProduct = await _productRepository.UpdateProduct(Product);
            if (UpdatedProduct is null)
                return BadRequest(new ApiResponse(400, "Product Not Updated"));
            return Ok(_mapper.Map<Product, ProductToReturnDto>(UpdatedProduct));
        }
    }

}
