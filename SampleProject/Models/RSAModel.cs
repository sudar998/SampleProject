namespace SampleProject.Models
{
    public class RSAModel
    {

        public RSAencryption? RSAencryption { get; set; }    = new RSAencryption();

        public RSAdecryption? RSAdecryption { get; set; }= new RSAdecryption();

        public string client { get; set; } = string.Empty;
    }
}
