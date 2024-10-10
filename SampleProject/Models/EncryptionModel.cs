namespace SampleProject.Models
{
    public class EncryptionModel
    {
        public string TextToEncrypt { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;

        public string? Key2 { get; set; } = string.Empty;

        public string? Key3 { get; set; } = string.Empty;
        public string EncryptedText { get; set; }
        public string DecryptedText { get; set; }
    }
}
