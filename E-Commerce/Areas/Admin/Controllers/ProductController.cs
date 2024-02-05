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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> getAllProduct = _productRepository.GetAll(includeProperties: "Category").ToList();
            return View(getAllProduct);
        }
        public IActionResult Upsert(int? id)
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
            if (id == null || id == 0)
            {
                return View(productVm);
            }
            else
            {
                productVm.Product = _productRepository.Get(p => p.Id == id);
                return View(productVm);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(RootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVm.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(RootPath,productVm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath,fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVm.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if (productVm.Product.Id == 0)
                {
                    _productRepository.Add(productVm.Product);
                }
                else
                {
                    _productRepository.Update(productVm.Product);
                }
                _productRepository.Save();
                TempData["success"] = $"Product {productVm.Product.Title} Is {(productVm.Product.Id == 0 ? "Created" : "Updated")} Successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productVm.CategoryList = _categoryRepository.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }
            return View(productVm);
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
