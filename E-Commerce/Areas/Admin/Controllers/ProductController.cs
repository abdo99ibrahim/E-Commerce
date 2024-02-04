using ECommerce.DataAccess.Repository;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            List<Product> getAllProduct = _productRepository.GetAll().ToList();
            return View(getAllProduct);
        }
        public IActionResult Create()
        {
            ProductVM productVm = new()
            {
                CategoryList = _categoryRepository.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                Product = new Product()
            };
            return View(productVm);
        }

        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Add(productVM.Product);
                _productRepository.Save();
                TempData["success"] = $"Product {productVM.Product.Title} Is Created Successfully";
                return RedirectToAction("Index", "Product");
            }
            else {
                productVM.CategoryList = _categoryRepository.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }
            return View(productVM);
        }
        public IActionResult Edit(int? id)
        {
            Product? product = _productRepository.Get(c => c.Id == id);
            if (id == null || id == 0 || product == null)
            {
                return NotFound();
            }
            return View(product);
            
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Update(product);
                _productRepository.Save();
                TempData["success"] = $"Category {product.Title} Is Created Successfully";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }
            public IActionResult Delete(int? id)
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }

                Product? productFromDb = _productRepository.Get(c => c.Id == id);
                if (productFromDb == null)
                {
                    return NotFound();
                }
                return View(productFromDb);
            }
            [HttpPost, ActionName("Delete")]
            public IActionResult DeletePost(int? id)
            {
                Product? obj = _productRepository.Get(c => c.Id == id);    
                if (obj == null)
                {
                    return NotFound();
                }
                _productRepository.Remove(obj);
                _productRepository.Save();
                TempData["success"] = $"Product {obj.Title} Is Deleted Successfully";
                return RedirectToAction("Index", "Product");
            }
        }
}
