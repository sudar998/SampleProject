
using System.Security.Cryptography;
using System.Text;
namespace SampleProject.DAL

{
    public class TripleDesService
    {

        public TripleDesService()
        {

        }



        private static string key1 = "12345678abcdefgh";
        private static string key2 = "abcdefgh12345678";
        private static string key3 = "87654321abcdefgh";
        private static string iv = "12345678";
        public static string Encrypt(string plainText, string inputKey)
        {

            // encrypt , decrypt , encrypt 
            byte[] encrypted = EncryptTextWithTripleDes(plainText, inputKey);

            Console.WriteLine("Encryption 1 : " + Convert.ToBase64String(encrypted));

            return Convert.ToBase64String(encrypted);

        }


        public static string Decrypt(string encryptedText, string inputKey)
        {
            //decrypt , encrypt , decrypt 

            var decryptedText = DecryptTextWithTripleDes(Convert.FromBase64String(encryptedText), inputKey);



            return decryptedText;

        }

        protected static byte[] EncryptTextWithTripleDes(string data, string key)
        {
           
            using (var tripleDes = TripleDES.Create())
            {
                tripleDes.Key = Encoding.UTF8.GetBytes(key);
                tripleDes.IV = Encoding.UTF8.GetBytes(iv);
                tripleDes.Padding = PaddingMode.PKCS7;

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, tripleDes.CreateEncryptor(tripleDes.Key, tripleDes.IV), CryptoStreamMode.Write))
                    {
                        // cryptoStream.Write(data, 0, data.Length);
                        //cryptoStream.FlushFinalBlock();
                        // return memoryStream.ToArray(); 

                        using (var swWriter = new StreamWriter(cryptoStream))
                        {
                            swWriter.Write(data);
                        }
                        return memoryStream.ToArray();
                    }

                }
            }
        }


        protected static string DecryptTextWithTripleDes(byte[] data, string key)
        {


            using (var tripledes = TripleDES.Create())
            {
                tripledes.Key = Encoding.UTF8.GetBytes(key);
                tripledes.IV = Encoding.UTF8.GetBytes(iv);
                tripledes.Padding = PaddingMode.PKCS7;
                using (var memoryStream = new MemoryStream(data))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, tripledes.CreateDecryptor(tripledes.Key, tripledes.IV), CryptoStreamMode.Read))
                    {
                        using (var newmemoryStream = new  MemoryStream())
                        {
                              cryptoStream.CopyTo(newmemoryStream);
                            return Encoding.UTF8.GetString(newmemoryStream.ToArray());
                        }

                    }

                }
            }
        }

    }
}
