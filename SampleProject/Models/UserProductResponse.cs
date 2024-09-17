namespace SampleProject.Models
{
    public class UserProductResponse
    {

        public int Id { get; set; }

        public User User { get; set; } = new User();    

        public Product Product { get; set; } = new Product();   
    }
}
