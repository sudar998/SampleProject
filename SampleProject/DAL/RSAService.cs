using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;
using System.Security.Cryptography;
using System.Text;

namespace SampleProject.DAL
{
    public class RSAService
    {

        private static RSACryptoServiceProvider rsaClient2;
        private static RSACryptoServiceProvider rsaClient1;



        public static RSACryptoServiceProvider RSAClient2
        {
            get
            {
                return rsaClient2;
            }
        }

        public static RSACryptoServiceProvider RSAClient1
        {
            get
            {
                return rsaClient1;
            }
        }



        public RSAService()
        {

        }





        public static string ReadClient2Text()
        {
            var output = System.IO.File.ReadAllText(@"C:\Users\adark\source\repos\SampleProject\client2\EncryptedText.txt");
            return output;

        }
        public static string GetPem(RSA rsa)
        {
            var publicKeyInfo = rsa.ExportSubjectPublicKeyInfo();
            var sb = new StringBuilder();
            sb.AppendLine("-----BEGIN PUBLIC KEY-----");
            sb.AppendLine(Convert.ToBase64String(publicKeyInfo, Base64FormattingOptions.InsertLineBreaks));
            sb.AppendLine("-----END PUBLIC KEY-----");
            return sb.ToString();
        }
        public static (string publicKey, string privateKey) GenerateKeysforClient1()
        {
            using (rsaClient1 = new RSACryptoServiceProvider(2048))
            {
                // var privatekey=  rsa.ToXmlString(false);


                rsaClient1.PersistKeyInCsp = false;
                string publicKey = rsaClient1.ToXmlString(false);  // Public key
                string privateKey = rsaClient1.ToXmlString(true);  // Private key
                return (publicKey, privateKey);
            }
        }


        public static (string publicKey, string privateKey) GenerateKeysforClient2()
        {

            using (rsaClient2 = new RSACryptoServiceProvider(2048))
            {

                rsaClient2.PersistKeyInCsp = false; // Don't store the keys in a container
                string publicKey = rsaClient2.ToXmlString(false);  // Public key
                string privateKey = rsaClient2.ToXmlString(true);  // Private key
                return (publicKey, privateKey);
            }
        }

        public static byte[] EncryptMessage(string message, string key)
        {
            using (RSACng rsa = new RSACng(2048))
            {




                rsa.FromXmlString(key);
                //  File.WriteAllText(@"C:\Users\adark\source\repos\encryptedText\publickey.xml" , rsa.ToXmlString())
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                byte[] encryptedBytes = rsa.Encrypt(messageBytes, padding: RSAEncryptionPadding.OaepSHA256);

                // rsa.Encrypt(encryptedBytes, RSAEncryptionPadding.o);
                return encryptedBytes;
            }
        }

        public static string DecryptMessage(byte[] encryptedMessage, string key)
        {
            using (RSACng rsa = new RSACng(2048))
            {
                try
                {
                    rsa.FromXmlString(key);

                    // byte[] decryptedBytes = rsa.Decrypt(encryptedMessage, false);
                    byte[] decryptedBytes = rsa.Decrypt(encryptedMessage, padding: RSAEncryptionPadding.OaepSHA256);
                    return Encoding.UTF8.GetString(decryptedBytes);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        public static byte[] SignMessage(string message, string privateKey)
        {
            byte[] signature = null;
            using (RSACng rsa = new RSACng(2048))
            {
                rsa.FromXmlString(privateKey);

                var hashOutput = HashMessage(message);
                signature = rsa.SignData(hashOutput, hashAlgorithm: HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                return signature;


            }
        }

        public static byte[] HashMessage(string message)
        {
            using (var sha256 = SHA256.Create())
            {
                var output = sha256.ComputeHash(Encoding.UTF8.GetBytes(message));
                return output;
            }
        }


        public static bool VerifySignature(string hashOutput, string signature, string publicKey)
        {
            using (var rsa = new RSACng(2048))
            {
                rsa.FromXmlString(publicKey);
                return rsa.VerifyData(Convert.FromHexString(hashOutput), Convert.FromHexString(signature), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            }
        }
    }
}
