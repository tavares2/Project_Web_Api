using LGC_Code_Challenge.Contracts;
using LGC_CodeChallenge.Models;

namespace LGC_CodeChallenge.Interfaces
{
    public interface IProductService 
    {
        Task<Product> GetProductAsync(Guid id);
        Task<List<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
    }
}
