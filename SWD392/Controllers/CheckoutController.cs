/*using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SWD392.Controllers
{
*//*    public class Cart
    {
        public List<CartItem> Items { get; set; }
    }

    public class CartItem
    {
        public string Type { get; set; }
        public ItemDetails Item { get; set; }
        public int Quantity { get; set; }
    }

    public class ItemDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }*//*

    [Route("/")]
    [ApiController]
    public class CheckoutController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string domain = "http://localhost:5272";

        public CheckoutController(IConfiguration configuration)
        {
            _configuration = configuration;
        }




        [HttpPost("create-checkout-session")]
        public async Task<ActionResult> Create()
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe:SecretKey").Value;

            // Get the artwork details from the session
            var artworkId = HttpContext.Session.GetInt32("ArtworkId");
            var name = HttpContext.Session.GetString("Name");
            var priceString = HttpContext.Session.GetString("Price");
            var quantity = HttpContext.Session.GetInt32("Quantity");

            // Check if any session variable is null and return so json can read
            if (artworkId == null || name == null || priceString == null || quantity == null)
            {
                return BadRequest("Session data is missing");
            }

            var price = decimal.Parse(priceString);

            var cart = new Cart
            {
                Items = new List<CartItem>
        {
            new CartItem
            {
                Type = "Artwork",
                Item = new ItemDetails { Id = artworkId.Value, Name = name, Price = price },
                Quantity = quantity.Value
            }
        }
            };

            var lineItems = new List<SessionLineItemOptions>();

            // Add each item in the cart to the lineItems list
            foreach (var item in cart.Items)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Item.Price * 100), // Convert dollars to cents and decimal to long
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"{item.Type}: {item.Item.Name}",
                        },
                    },
                    Quantity = item.Quantity, // Use the Quantity property here
                });
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
        {
            "card",
        },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = domain + "/success",
                CancelUrl = domain + "/cancel",
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return Json(new { id = session.Id });
        }




    }
}
*/