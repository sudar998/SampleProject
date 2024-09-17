using SampleProject.Models;

namespace SampleProject.DAL.Interface
{
    public interface IUserRepository :  IRepository<User>
    {

        Task<IEnumerable<User>> GetAll();

        Task<User> GetById(int Id);

        Task Create(User user);

        Task<bool> Update(User user , int id);

        Task<bool> Delete(int id); 
    }
}
