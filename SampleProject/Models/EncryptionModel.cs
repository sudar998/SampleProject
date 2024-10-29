namespace SampleProject.Models
{
    public class EncryptionModel
    {
        public string TextToEncrypt { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;

        public string? Key2 { get; set; } = string.Empty;

        public string? Key3 { get; set; } = string.Empty;
        public string EncryptedText { get; set; }


        public (string? encrypted1, string? decrypted1, string? finaltext) encryptionResult;

        public (string decrypted1, string encrypted1, string finaltext) decryptionResult; 
        public string DecryptedText { get; set; }
    } 
}
  