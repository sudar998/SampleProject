namespace SampleProject.Models
{
    public class EncryptionModel
    {
        public string TextToEncrypt { get; set; } = string.Empty; 
        public string Key { get; set; } 
        public string EncryptedText { get; set; }
        public string DecryptedText { get; set; }
    }
}
