using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_Commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _productRepository.GetAll(includeProperties: "Category");
            return View(productList);
        }
        public IActionResult Details(int ProductId)
        {
            Product product = _productRepository.Get(p => p.Id == ProductId, includeProperties: "Category");
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
