using Microsoft.AspNetCore.Mvc;
using SampleProject.Models.Utils;
using SampleProject.Models;
using SampleProject.DAL.Interface;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SampleProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {

            return View();
        }

        public IActionResult RegistrationSuccess()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserRegister register)
        {

            if (register == null)
            {
                return BadRequest();
            }

            var user = (await _userRepository.GetAll()).
                 FirstOrDefault(x => x.SecurityNumber == register.SecurityNumber);

            if (user != null)
            {
                ModelState.AddModelError("", $"User with securityNumber {register.SecurityNumber} alread exist");
                TempData["UserExist"] = $"*User with Security Number {register.SecurityNumber} already exist "; 
                return View("Register");
            }
            string salt;
            var hashPassword = PasswordUtils.HashPassword(register.password, out salt);

            await _userRepository.Create(
                new Models.User()
                {
                    Email = register.Email,
                    Name = register.Name,
                    SecurityNumber = register.SecurityNumber,
                    Password = hashPassword,
                    Salt = salt

                }
            );
            return RedirectToAction("RegistrationSuccess");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if (userLogin == null) return BadRequest();
            if (userLogin.SecurityNumber == 0 ||
                string.IsNullOrWhiteSpace(userLogin.Password))
            {
                ModelState.AddModelError("", "Credentials are empty. Please Enter again");
                return View("Login");
            }
            var userResponse = await _userRepository.GetBySecurityNumber(userLogin.SecurityNumber);

            if (userResponse == null)
            {
                TempData["UserNotExist"] = "*User not Exist";
                return View("Login");
            }
            var response=  PasswordUtils.VerifyPassword(userResponse.Salt, userResponse.Password, userLogin.Password);

            if(response)
            {
                TempData["SuccessMessage"] = "Successfully Login"; 
                return RedirectToAction("Index" , "Home"); 
            }
            TempData["ErrorMessage"] = "Failed to login";
            return View("Login");


        }
    }
}
