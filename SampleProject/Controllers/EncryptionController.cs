using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using SampleProject.DAL;
using SampleProject.Models;

namespace SampleProject.Controllers
{
    public class EncryptionController : Controller
    {
       
        public IActionResult Index()
        {
            return View( new EncryptionModel());
        }

        public IActionResult Encrypt(EncryptionModel model)
        {
            if (string.IsNullOrWhiteSpace(model.TextToEncrypt))
            {
                TempData["EmptyTextToEncrypt"] = "Empty text for encryption";
                return View("Index" , model); 
            }

            string output = TripleDesService.Encrypt(model.TextToEncrypt , model.Key);

            model.EncryptedText = output;
            model.Key = string.Empty;
            return View("Index" , model);

        }

        public IActionResult Decrypt(EncryptionModel model)
        {         
               if(string.IsNullOrEmpty( model.EncryptedText))
            {
                TempData["EmptyEncryptedText"] = "Empty text to decrypt";
                return View("Index" , model); 
            }
             var output=   TripleDesService.Decrypt(model.EncryptedText , model.Key);

            model.DecryptedText = output;   
           
            return View("Index" , model);
        }

        public IActionResult DesEncryptDecryptIndex()
        {
            return View( "DesEncryptDecryptIndex" , new EncryptionModel()); 
        }


        public IActionResult DesEncryption(EncryptionModel model)
        {
            if (string.IsNullOrWhiteSpace(model.TextToEncrypt))
            {
                TempData["EmptyToEncrypt"] = "Empty text for Encryption";
                return View("DesEncryptDecryptIndex" , model); 
            }

           var output=   DESService.Encrypt(model.TextToEncrypt, model.Key , model.Key2 , model.Key3); 
            model.EncryptedText = output;

            return View("DesEncryptDecryptIndex", model); 

        }

        public IActionResult DesDecryption(EncryptionModel model)
        {
             if(string.IsNullOrEmpty(model.EncryptedText))
            {
                TempData["EmptyEncryptedText"] = "Empty text to decrypt";
                return View("DesEncryptDecryptIndexIndex" , model);
            }

            var output=  DESService.Decrypt(model.EncryptedText, model.Key , model.Key2 , model.Key3); 
            model.DecryptedText = output;   

            return View("DesEncryptDecryptIndex", model);
              
        }
    }



}
