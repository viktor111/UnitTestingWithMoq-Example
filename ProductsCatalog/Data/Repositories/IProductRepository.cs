using ProductsCatalog.Models;

namespace ProductsCatalog.Data.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetById(int id);

        Task<IEnumerable<Product>> GetAll();

        Task<Product> Create(Product product);

        Task<Product> Update(Product product);
    }
}
