using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace SampleProject.DAL
{
    public class DESService
    {

        protected static string IV = "12345678";
        private static RSACryptoServiceProvider _rSACryptoServiceProvider;
        private static RSAParameters privateKey;
        private static RSAParameters publicKey;
        public DESService()
        {

        }
        public static string GetPublicKey()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, publicKey);
            return sw.ToString();
        }


        public static (string plaintext, string privateKey) DecryptWithRSA(string cipherText)
        {
            var data = Convert.FromBase64String(cipherText);
            using (_rSACryptoServiceProvider = new RSACryptoServiceProvider())
            {
                //_rSACryptoServiceProvider.ImportParameters(privateKey);
                var privateKey = _rSACryptoServiceProvider.ExportRSAPrivateKey();

                var plainText = _rSACryptoServiceProvider.Decrypt(data, false);
                return (Encoding.UTF8.GetString(plainText), Convert.ToBase64String(privateKey));
            }
        }
        public static string Encrypt(string plainText, string inputKey1, string inputKey2, string inputKey3)
        {

            // encrypt , decrypt , encrypt 
            //encrypt with key 1
            byte[] encrypted = EncryptTextWithDes(Encoding.UTF8.GetBytes(plainText), inputKey1);


            Console.WriteLine("Encryption 1 : " + Convert.ToBase64String(encrypted));

            //decrypted with key 2 

            encrypted = DecryptTextWithDes(encrypted, inputKey2);

            Console.WriteLine("Decryption 1 : " + Convert.ToBase64String(encrypted));

            //encrypted with key 3 
            encrypted = EncryptTextWithDes(encrypted, inputKey3);
            Console.WriteLine("Encryption 2 : " + Convert.ToBase64String(encrypted));

            return Convert.ToBase64String(encrypted);

        }


        public static string Decrypt(string encryptedText, string inputKey1, string inputKey2, string inputKey3)
        {
            //decrypt , encrypt , decrypt 
            Console.WriteLine("DECRYPTION");

            //decrypt with key 3 

            string decrypt1 = Convert.ToBase64String(DecryptTextWithDes(Convert.FromBase64String(encryptedText), inputKey3));


            Console.WriteLine("Decryption 1 : " + decrypt1);

            //encrypt with key 2 

            byte[] secondEncrypt = EncryptTextWithDes(Convert.FromBase64String(decrypt1), inputKey2);

            Console.WriteLine("Encryption 1: " + Convert.ToBase64String(secondEncrypt));


            ////decrypt back with key 1 

            string finalDecrypt = Encoding.UTF8.GetString(DecryptTextWithDes(secondEncrypt, inputKey1));

            Console.WriteLine("Decryption 2 : " + finalDecrypt);


            return finalDecrypt;

        }

        public static byte[] EncryptTextWithDes(byte[] data, string inputKey)
        {

            using (var des = DES.Create())
            {
                des.Key = Encoding.UTF8.GetBytes(inputKey);
                des.IV = Encoding.UTF8.GetBytes(IV);
                des.Padding = PaddingMode.None;
                des.Mode = CipherMode.CBC;

                using (var memorystream = new MemoryStream())
                {

                    using (var cryptostream = new CryptoStream(memorystream, des.CreateEncryptor(des.Key, des.IV), CryptoStreamMode.Write))
                    {


                        cryptostream.Write(data, 0, data.Length);

                        return memorystream.ToArray();
                        //using (var swWriter = new StreamWriter(cryptostream))
                        //{
                        //    swWriter.Write(plainText);
                        //}
                        //return memorystream.ToArray();
                    }
                }
            }
        }


        protected static byte[] DecryptTextWithDes(byte[] data, string key)
        {

                using (var des = DES.Create())
                {
                    des.Key = Encoding.UTF8.GetBytes(key);
                    des.IV = Encoding.UTF8.GetBytes(IV);
                    des.Padding = PaddingMode.None;
                    des.Mode = CipherMode.CBC;

                    using (var memoryStream = new MemoryStream(data))
                    {
                        using (var crytoStream = new CryptoStream(memoryStream, des.CreateDecryptor(des.Key, des.IV), CryptoStreamMode.Read))
                        {

                            using (var newMemoryStream = new MemoryStream())
                            {
                                crytoStream.CopyTo(newMemoryStream);
                                return newMemoryStream.ToArray();
                            }
                        }
                    }
                }


            }
           
        }

    }
