using Microsoft.AspNetCore.Mvc;
using ProductsCatalog.Data.Repositories;
using ProductsCatalog.Dtos;
using ProductsCatalog.Models;

namespace ProductsCatalog.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var products = await _productRepository.GetById(id);

            if (products == null) return BadRequest();

            return Ok(products);
        }

        [HttpGet]
        public async Task<ActionResult> All()
        {
            var allProducts = await _productRepository.GetAll();

            if(allProducts == null) return BadRequest();

            return Ok(allProducts);
        }

        [HttpPost]
        public async  Task<ActionResult> Create(CreateProductDto createProductDto)
        {
            var createdProduct = await _productRepository.Create(new Product() { Name = createProductDto.Name, Price = createProductDto.Price });

            return Ok(createdProduct);
        }

        [HttpPost]
        public async Task<ActionResult> Update(UpdateProductDto updateProductDto)
        {
            var productToUpdate = await _productRepository.GetById(updateProductDto.Id);

            if (productToUpdate == null) return BadRequest();

            productToUpdate.Name = updateProductDto.Name;
            productToUpdate.Price = updateProductDto.Price;

            await _productRepository.Update(productToUpdate);

            return Ok(productToUpdate);
        }
    }
}
