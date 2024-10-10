using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SampleProject.DAL.Interface;
using SampleProject.Models;
using SampleProject.Models.Utils;

namespace SampleProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public IActionResult Migrate()

        {
            _userRepository.Migrate();
            return View();
        }


        public async Task Login(User user)
        {

        }

        public ActionResult Register()
        {
            return View(); 
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(UserRegister register)
        //{

        //    if(register == null)
        //    {
        //        return BadRequest();
        //    }

        //   var user =  ( await _userRepository.GetAll()).
        //        FirstOrDefault(x=>x.SecurityNumber == register.SecurityNumber);

        //    if (user != null)
        //    {
        //        ModelState.AddModelError("", $"User with Security Number {register.SecurityNumber} already exist");
        //        return View(); 
        //    }
        //    string salt;
        //    PasswordUtils.HashPassword(register.password , out salt);

        //    await _userRepository.Create(
        //        new Models.User()
        //        {
        //            Email = register.Email,
        //            Name = register.Name,
        //            Password = register.password,

        //        }
        //    );
        //    return RedirectToAction("Index");
        //}
        public async Task<IActionResult> Create()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {

            if (user == null) return BadRequest();
            if (ModelState.IsValid)
            {
                await _userRepository.Create(user);
                return Redirect("Index");
            }
            return View(user);
        }

        public async Task<IActionResult> Index()
        {
            var response = await _userRepository.GetAll();
            return View(response);
        }


        public async Task<IActionResult> ViewUser(int userId)
        {
            var response = await _userRepository.GetById(userId);
            return View(response);
        }

        public async Task<IActionResult> Delete(int userId)
        {
            var response = await _userRepository.GetById(userId);
            return View(response);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userId)
        {
            await _userRepository.Delete(userId);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> UpdateUser(int? userId)
        {

            var response = await _userRepository.GetById(userId.Value);
            // var response = await _userRepository.Update(new Models.User() { SecurityNumber = securityNumber, Name = name }, id);
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(User user)
        {
            if (ModelState.IsValid)
            {
                var response = await _userRepository.Update(user, user.Id);
                if (response)
                {
                    return RedirectToAction("Index");
                }

            }
            return View(user);
        }
    }
}
