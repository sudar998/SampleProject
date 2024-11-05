﻿namespace SampleProject.Models
{
    public class RSAModel
    {

        public RSAencryption? RSAencryption { get; set; }    = new RSAencryption();

        public RSAdecryption? RSAdecryption { get; set; }= new RSAdecryption();

        public bool DisplayDecryptedTextInTextBox { get; set; } = false; 
        public string publicKeygeneration {  get; set; }

        public string Signature { get; set; } = string.Empty;

        public bool VerifiedSignature {  get; set; } = false;   

        public bool DisplaySignature { get; set; } = true;

        public string HashOutput { get; set; } = string.Empty; 
        public string privateKeygeneration { get; set; }    
        public string client { get; set; } = string.Empty;
    }
}
