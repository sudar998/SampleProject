using System.Security.AccessControl;

namespace SampleProject.Models
{
    public class UserRegister
    {

        public int SecurityNumber { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string password { get; set; }
    }
}
