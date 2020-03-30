using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerseClient.Models;
using EcommerseClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerseClient.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            Cart cart = CartService.GetCart(HttpContext.Request.Cookies["UserId"]);
            return View("TheCart", cart);
        }

        [HttpPost]
        [Route("api/cart")]
        public IActionResult AddProductToCart(itemToCart info)
        {
            CartService.Cart(new AddProductToCart()
            {
                idClient = HttpContext.Request.Cookies["UserId"],
                idProduct = info.idProduct,
                quantity = info.quantity
            });
            CurrencyService.cur = info.currency;
            return Ok();
        }

        [HttpDelete]
        [Route("api/cart/delete")]
        public IActionResult DeteleCart()
        {
            CartService.DeleteCart(HttpContext.Request.Cookies["UserId"]);
            return Ok();
        }
    }
}