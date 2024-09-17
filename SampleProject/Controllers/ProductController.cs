using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Win32.SafeHandles;
using SampleProject.DAL;
using SampleProject.DAL.Interface;
using SampleProject.Models;

namespace SampleProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IActionResult> Index()
        {
           var response = await _productRepository.GetAll();    
            return View(response);
        }
        public async Task<IActionResult> Create()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {

            if (product == null) return BadRequest();
            if (ModelState.IsValid)
            {
                await _productRepository.Create(product);
                return Redirect("Index");
            }
            return View(product);
        }
       
        public async Task<IActionResult> ViewProduct(int productId)
        {
            var response = await _productRepository.GetById(productId);
           
            return View(response); 
        }

        public async Task<IActionResult> Delete(int productid)
        {
           var response =  await  _productRepository.GetById(productid);
            return View(response);  
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int productId)
        {
            await _productRepository.Delete(productId);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> UpdateProduct(int productId)
        {

            var response = await _productRepository.GetById(productId);
           
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var response = await _productRepository.Update(product, product.Id);
                if (response)
                {
                    return RedirectToAction("Index");
                }

            }
            return View(product);
        }
    }
}
