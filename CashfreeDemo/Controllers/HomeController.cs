using CashfreeDemo.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using cashfree_pg.Client;

using cashfree_pg.Model;


namespace CashfreeDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;   
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateOrder(string customerId , string customerEmail, string customerPhone , string customerName,string orderId, double orderAmount)
        {
            Cashfree.XClientId = "TEST1006617327000b49a3168b4ab3c737166001";
            Cashfree.XClientSecret = "TESTe0ca7b55b5fdf02eff27951c4ef9d4ccb82e7d09";
            Cashfree.XEnvironment = Cashfree.SANDBOX;
            var cashfree = new Cashfree();
            var xApiVersion = "2022-09-01";
            var customerDetails = new CustomerDetails(customerId, customerEmail, customerPhone, customerName);
            var orderMetaUrl = $"https://localhost:7114/Home/OrderResult";
            var orderMeta = new CreateOrderRequestOrderMeta(orderMetaUrl);
            var createOrdersRequest = new CreateOrderRequest(orderId, orderAmount, "INR", customerDetails,null, orderMeta);
            // Create Order
            var result = cashfree.PGCreateOrder(xApiVersion, createOrdersRequest);
            //Console.WriteLine(result);
            //Console.WriteLine(result.StatusCode);
            //Console.WriteLine((result.Content as OrderEntity));
            return Json(result.Data);
        }
        [HttpPost]
        public IActionResult OrderResult(string orderId)
        {           
            Cashfree.XClientId = "TEST1006617327000b49a3168b4ab3c737166001";
            Cashfree.XClientSecret = "TESTe0ca7b55b5fdf02eff27951c4ef9d4ccb82e7d09";
            Cashfree.XEnvironment = Cashfree.SANDBOX;
            var cashfree = new Cashfree();
            var xApiVersion = "2022-09-01";
            var result = cashfree.PGFetchOrder(xApiVersion, orderId);
            Console.WriteLine(result);
            Console.WriteLine(result.StatusCode);
            return Json(result.Data);
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