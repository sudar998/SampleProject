namespace SampleProject.Models
{
    public class UserProductList
    {

        public ICollection<Product> Products { get; set; } = new List<Product>();   

        public ICollection<User> Users { get; set; } = new List<User>();    
    }
}
