using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Primitives;
using SampleProject.DAL;
using SampleProject.Models;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SampleProject.Controllers
{
    public class EncryptionController : Controller
    {

        public IActionResult Index()
        {
            return View(new EncryptionModel());
        }

        public IActionResult Encrypt(EncryptionModel model)
        {
            if (string.IsNullOrWhiteSpace(model.TextToEncrypt))
            {
                TempData["EmptyTextToEncrypt"] = "Empty text for encryption";
                return View("Index", model);
            }

            string output = TripleDesService.Encrypt(model.TextToEncrypt, model.Key);

            model.EncryptedText = output;
            model.Key = string.Empty;
            return View("Index", model);

        }

        public IActionResult Decrypt(EncryptionModel model)
        {
            if (string.IsNullOrEmpty(model.EncryptedText))
            {
                TempData["EmptyEncryptedText"] = "Empty text to decrypt";
                return View("Index", model);
            }
            var output = TripleDesService.Decrypt(model.EncryptedText, model.Key);

            model.DecryptedText = output;

            return View("Index", model);
        }

        public IActionResult DesEncryptDecryptIndex()
        {
            return View("DesEncryptDecryptIndex", new EncryptionModel());
        }


        public IActionResult DesEncryption(EncryptionModel model)
        {
            if (string.IsNullOrWhiteSpace(model.TextToEncrypt))
            {
                TempData["EmptyToEncrypt"] = "Empty text for Encryption";
                return View("DesEncryptDecryptIndex", model);
            }

            var output = DESService.Encrypt(model.TextToEncrypt, model.Key, model.Key2, model.Key3);
            model.EncryptedText = output;

            return View("DesEncryptDecryptIndex", model);

        }


        public IActionResult DesDecryption(EncryptionModel model)
        {
            if (string.IsNullOrEmpty(model.EncryptedText))
            {
                TempData["EmptyEncryptedText"] = "Empty text to decrypt";
                return View("DesEncryptDecryptIndexIndex", model);
            }

            var output = DESService.Decrypt(model.EncryptedText, model.Key, model.Key2, model.Key3);
            model.DecryptedText = output;

            return View("DesEncryptDecryptIndex", model);

        }


        string encryptedText = "";
        public IActionResult RSAEncryptDecryptIndex()
        {
            return View("RSAEncryptDecryptIndex", new RSAModel());


        }


        [HttpPost]
        public IActionResult RsaEncrypt(RSAModel model, string client)
        {
            if (!string.IsNullOrWhiteSpace(model.RSAencryption.Text) && !string.IsNullOrWhiteSpace(model.RSAencryption.PublicKey))
            {

                var output = Convert.ToBase64String(RSAService.EncryptMessage(model.RSAencryption.Text, model.RSAencryption.PublicKey));


                Console.WriteLine(output);

                Console.WriteLine(" ");
                if (client == "client1")
                {
                    System.IO.File.WriteAllText(@"C:\Users\adark\source\repos\SampleProject\client2\EncryptedText.txt", output);
                }
                else if(client =="client2")
                {
                    System.IO.File.WriteAllText(@"C:\Users\adark\source\repos\SampleProject\client1\EncryptedText.txt", output);
                }
                TempData["success"] = "Successfully encrypted";
                return View("RSAStatus", true);
             

            }
            return View("RSAStatus", false);
        }

        [HttpPost]
        public IActionResult RsaDecrypt(RSAModel model, string client)
        {
            string output = string.Empty;

            string ReadTextFromFile = string.Empty;
            string ReadkeyFromFile = string.Empty;
            if (string.IsNullOrEmpty(model.RSAdecryption.EncryptedText))

            {
                if (client == "client2")
                {
                    ReadTextFromFile = System.IO.File.ReadAllText(@"C:\Users\adark\source\repos\SampleProject\client2\EncryptedText.txt");
                    ReadkeyFromFile = System.IO.File.ReadAllText(@"C:\Users\adark\source\repos\SampleProject\client2\MyPrivateKey.xml");
                }
                else if (client == "client1")
                {

                    ReadTextFromFile = System.IO.File.ReadAllText(@"C:\Users\adark\source\repos\SampleProject\client1\EncryptedText.txt");
                    ReadkeyFromFile = System.IO.File.ReadAllText(@"C:\Users\adark\source\repos\SampleProject\client1\MyPrivateKey.xml");
                }

                output = RSAService.DecryptMessage(Convert.FromBase64String(ReadTextFromFile), ReadkeyFromFile);
            }
            model.RSAdecryption.EncryptedText = ReadTextFromFile;
            model.RSAdecryption.DecryptedText = output;
            model.client = client; 
            return View("RSAencryptdecryptIndex", model);
        }
        public IActionResult RSAdecryption(string cipherText)
        {

            //var output = RSAService.DecryptMessage(cipherText);
            //Console.WriteLine(output.plaintext);
            //Console.WriteLine("");
            //Console.WriteLine("PrivateKey :" + output.privateKey);

            //return Ok(output.plaintext + "\n\n\n" + "Private Key : " + output.privateKey);

            return Ok();

        }

        public IActionResult SubmitPublicKey()
        {
            return View("SubmitKeys", new SubmitKeyModel());
        }
        public IActionResult SubmitPublicKeyFromClient1()
        {
            return View("SubmitKeysToClient2", new SubmitKeyModel());
        }

        [HttpPost]
        public IActionResult SubmitPublicKeyForClient1(SubmitKeyModel model)
        {

            var output = RSAService.GenerateKeysforClient2();

            try
            {
                //EmailService.SendPublicKeyViaEmail(model.OriginEmail, model.EmailPassword, model.SourceEmail, output.publicKey);

                System.IO.File.WriteAllText(@"C:\Users\adark\source\repos\SampleProject\client1\Client2_PublicKey.xml", output.publicKey);
                System.IO.File.WriteAllText(@"C:\Users\adark\source\repos\SampleProject\client2\MyPrivateKey.xml", output.privateKey);
                Console.WriteLine("Done submit public keys");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception :" + ex.Message);
                return BadRequest("Exception :" + ex.Message);
            }


            TempData["SubmitPublicKeyFromClientsuccess"] = "Succesfully submitted";
            return View("RSAencryptdecryptIndex", new RSAModel());
        }

        [HttpPost]
        public IActionResult SubmitPublicKeyForClient2(SubmitKeyModel model)
        {

            var output = RSAService.GenerateKeysforClient1();

            try
            {
                //EmailService.SendPublicKeyViaEmail(model.OriginEmail, model.EmailPassword, model.SourceEmail, output.publicKey);

                System.IO.File.WriteAllText(@"C:\Users\adark\source\repos\SampleProject\client2\Client1_PublicKey.xml", output.publicKey);
                System.IO.File.WriteAllText(@"C:\Users\adark\source\repos\SampleProject\client1\MyPrivateKey.xml", output.privateKey);
                Console.WriteLine("Done submit public keys");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception :" + ex.Message);
                return BadRequest("Exception :" + ex.Message);
            }


            TempData["SubmitPublicKeyFromClientsuccess"] = "Succesfully submitted";
            return View("RSAencryptdecryptIndex", new RSAModel());
        }


    }



}
