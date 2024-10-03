using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SampleProject.DAL.Interface;
using SampleProject.Models;

namespace SampleProject.Controllers
{
    public class UserProductController : Controller
    {
        

        private readonly IUserProductRepository userProductRepository;
        public UserProductController(IUserProductRepository userProductRepository)
        {

            this.userProductRepository = userProductRepository;
        }



        public async Task<IActionResult> Index()
        {
            var userproductResponse = await userProductRepository.GetAll();
            return View(userproductResponse);

        }

        public async Task<IActionResult> Create()
        {
            var response = await userProductRepository.GetAllUserProductList();
            return View(response); 
        }



     
        //public async Task<IActionResult> AddOrder(int userId, int productId)
        //{
        //    //get user by id
        //    var user = await _userRepository.GetById(userId);

        //    if (user == null) { return NotFound(); }

        //     var product = await  _productRepository.GetById(productId);

        //      if(product == null) return NotFound();    


        //     var response=   await userProductRepository.PurchaseProduct(userId, productId);

        //    return View(response);

        //    //check if its valid 


        //    //add user to the product in userproduct table 


        //    //if (user == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //   return View(user);
        //}
    }
}
