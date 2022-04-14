using Microsoft.EntityFrameworkCore;
using ProductsCatalog.Models;

namespace ProductsCatalog.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext  _dbContext;
        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> Create(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var result = await _dbContext.Products.AddAsync(product);

            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var products = await _dbContext.Products.ToListAsync();

            return products;
        }

        public async Task<Product> GetById(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if(product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            return product;
        }

        public async Task<Product> Update(Product product)
        {
            _dbContext.Products.Update(product);

            await _dbContext.SaveChangesAsync();

            return product;
        }
    }
}
