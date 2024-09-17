using SampleProject.Models;

namespace SampleProject.DAL.Interface
{
    public interface IUserProductRepository : IRepository<UserProductResponse>
    {

        Task<bool> PurchaseProduct(int userId, int productId);
        Task<IEnumerable<UserProductResponse>> GetAll();

        Task<UserProductList> GetAllUserProductList();
    }
}
