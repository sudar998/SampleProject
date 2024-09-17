using SampleProject.Models;

namespace SampleProject.DAL.Interface
{
    public interface IProductRepository : IRepository<Product>  
    {

        Task<IEnumerable<Product>> GetAll();

        Task<Product> GetById(int Id);

        Task Create(Product user);

        Task<bool> Update(Product user, int id);

        Task<bool> Delete(int id);
    }


}
