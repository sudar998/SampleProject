namespace SampleProject.DAL.Interface
{
    public interface IRepository<T> where T : class
    {
        Task Migrate(); 

    }
   
}
