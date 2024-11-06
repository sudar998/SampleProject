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
            model.EncryptedText = output.finalText;
            model.encryptionResult.encrypted1 = output.Text1;
            model.encryptionResult.decrypted1 = output.Text2;
            model.encryptionResult.finaltext = output.finalText;

            return View("DesEncryptDecryptIndex", model);

        }


        public IActionResult DesDecryption(EncryptionModel model)
        {

            Console.WriteLine(model.TextToEncrypt);


            model.encryptionResult.finaltext = model.EncryptedText;
            if (string.IsNullOrEmpty(model.encryptionResult.finaltext))
            {
                TempData["EmptyEncryptedText"] = "Empty text to decrypt";
                return View("DesEncryptDecryptIndex", model);
            }

            var output = DESService.Decrypt(model.encryptionResult.finaltext, model.Key, model.Key2, model.Key3);
            //model.DecryptedText = output;
            model.decryptionResult.decrypted1 = output.text1;
            model.decryptionResult.encrypted1 = output.text2;
            model.decryptionResult.finaltext = output.finaltext;



            return View("DesEncryptDecryptIndex", model);

        }


        string encryptedText = "";
        public IActionResult RSAEncryptDecryptIndex()
        {
            return View("RSAEncryptDecryptIndex", new RSAModel());


        }


        [HttpPost]
        public IActionResult RsaEncrypt(RSAModel model, string client, string privateKey, string publicKey)
        {
            if (!string.IsNullOrWhiteSpace(model.RSAencryption.Text) && !string.IsNullOrWhiteSpace(model.RSAencryption.UserInputKey))
            {

                var output = Convert.ToHexString(RSAService.EncryptMessage(model.RSAencryption.Text, model.RSAencryption.UserInputKey));


                Console.WriteLine(output);

                Console.WriteLine(" ");

                model.RSAencryption.HexString = output;
                model.RSAencryption.PublicKey = publicKey;
                model.RSAencryption.PrivateKey = privateKey;
                model.publicKeygeneration = string.Empty;
                model.privateKeygeneration = string.Empty;



                return View("RSAencryptdecryptIndex", model);
                //if (client == "client1")
                //{
                //    System.IO.File.WriteAllText(@"C:\Users\adark\source\repos\SampleProject\client2\EncryptedText.txt", output);
                //}
                //else if(client =="client2")
                //{
                //    System.IO.File.WriteAllText(@"C:\Users\adark\source\repos\SampleProject\client1\EncryptedText.txt", output);
                //}
                //TempData["success"] = "Successfully encrypted";
                //return View("RSAStatus", true);


            }
            return View("RSAStatus", false);
        }

        [HttpPost]
        public IActionResult RsaDecrypt(RSAModel model, string client, string originalText, string privateKey, string publicKey)
        {
            string output = string.Empty;

            string ReadTextFromFile = string.Empty;
            string ReadkeyFromFile = string.Empty;
            if (!string.IsNullOrEmpty(model.RSAencryption?.HexString))

            {
                //if (client == "client2")
                //{
                //    ReadTextFromFile = System.IO.File.ReadAllText(@"C:\Users\adark\source\repos\SampleProject\client2\EncryptedText.txt");
                //    ReadkeyFromFile = System.IO.File.ReadAllText(@"C:\Users\adark\source\repos\SampleProject\client2\MyPrivateKey.xml");
                //}
                //else if (client == "client1")
                //{

                //    ReadTextFromFile = System.IO.File.ReadAllText(@"C:\Users\adark\source\repos\SampleProject\client1\EncryptedText.txt");
                //    ReadkeyFromFile = System.IO.File.ReadAllText(@"C:\Users\adark\source\repos\SampleProject\client1\MyPrivateKey.xml");
                //}
                model.RSAencryption.HexString = model.RSAencryption.HexString.Trim();
                if (model.RSAencryption?.HexString.Length % 2 != 0)
                {
                    model.RSAencryption.HexString = "0" + model.RSAencryption.HexString;  // Make it even-length
                }
                output = RSAService.DecryptMessage(Convert.FromHexString(model.RSAencryption?.HexString), privateKey);
            }
            model.RSAdecryption.EncryptedText = model.RSAencryption.HexString;
            model.RSAdecryption.DecryptedText = output;

            //encryption model
            // model.RSAencryption.HexString = string.Empty;
            model.RSAencryption.Text = originalText;
            if (string.IsNullOrEmpty(model.RSAencryption.PublicKey))
            {
                model.RSAencryption.PublicKey = publicKey;
            }
            model.RSAencryption.PrivateKey = privateKey;
            model.client = client;
            //model.Signature = message;
            model.DisplaySignature = false;
            model.DisplayDecryptedTextInTextBox = true;
            return View("RSAencryptdecryptIndex", model);
        }


        [HttpPost]
        public IActionResult HashText(RSAModel model, string publicKey, string privateKey)
        {
            if (!string.IsNullOrEmpty(model.RSAdecryption.DecryptedText))
            {
                var hashOutput = RSAService.HashMessage(model.RSAdecryption.DecryptedText);
                model.HashOutput = Convert.ToHexString(hashOutput);

                if (string.IsNullOrEmpty(model.RSAencryption.PrivateKey))
                {
                    model.RSAencryption.PrivateKey = privateKey;
                }
                if (string.IsNullOrEmpty(model.RSAencryption.PublicKey))
                {
                    model.RSAencryption.PublicKey = publicKey;
                }
                model.DisplaySignature = false;
               // model.Signature = message;

                model.RSAdecryption.EncryptedText = model.RSAencryption.HexString;
                model.RSAencryption.HexString = string.Empty;




                return View("RSAencryptdecryptIndex", model);
            }
            return View("RSAStatus", false);
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


        [HttpPost]
        public IActionResult GenerateKeyforClient1()
        {
            var output = RSAService.GenerateKeysforClient1();

            return View("RSAencryptdecryptIndex", new RSAModel()
            {
                publicKeygeneration = output.publicKey,

                privateKeygeneration = output.privateKey
            });
        }

        [HttpPost]

        public IActionResult SignMessage(string message, string privateKey, string publickey, string hexString)
        {

            if (string.IsNullOrWhiteSpace(message))
            {

                throw new Exception("Empty message");
            }

            var output = Convert.ToHexString(RSAService.SignMessage(message, privateKey));


            return View("RSAencryptdecryptIndex", new RSAModel() { RSAencryption = new RSAencryption() { PrivateKey = privateKey, PublicKey = publickey, HexString = hexString }, Signature = output });
        }

        [HttpPost]

        public IActionResult VerifySignature(RSAModel model, string publicKey)
        {
            var response = RSAService.VerifySignature(model.HashOutput, model.Signature, publicKey);
            if (response)
            {
                model.VerifiedSignature = RSAModel.VerifiedSign.YES;
            }else
            {
                   model.VerifiedSignature = RSAModel.VerifiedSign.NO;
            }
            model.DisplaySignature = false;
            return View("RSAencryptdecryptIndex", model);
        }


    }



}
