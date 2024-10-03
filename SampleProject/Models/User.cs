namespace SampleProject.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int SecurityNumber { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Salt { get; set; } = string.Empty; 
        public string Password { get; set; } 


    }
}
