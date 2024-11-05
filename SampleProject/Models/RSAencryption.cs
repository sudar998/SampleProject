namespace SampleProject.Models
{
    public class RSAencryption
    {

        public string? Text { get; set; } = string.Empty;

        public string PublicKey { get; set; } = string.Empty;

        public string UserInputKey { get; set; } = string.Empty; 
        public string PrivateKey { get; set; } = string.Empty; 

        public string HexString { get; set; } = string.Empty;   
    }

    public class RSAdecryption
    {
        public string DecryptedText { get; set; } = string.Empty;


        public string publicKey { get; set; } = string.Empty;

        public string privateKey { get; set; } = string.Empty;


        public string EncryptedText { get; set; } = string.Empty;   
    }
}
