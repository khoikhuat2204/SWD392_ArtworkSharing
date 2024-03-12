using Microsoft.AspNetCore.Mvc;
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
    public class Cart
    {
        public List<CartItem> Items { get; set; }
    }

    public class CartItem
    {
        public string Type { get; set; }
        public ItemDetails Item { get; set; }
        public int Quantity { get; set; } // Add this line
    }

    public class ItemDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
    }



    [Route("create-checkout-session")]
    [ApiController]
    public class CheckoutController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly string domain = "http://localhost:5272";

        public CheckoutController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> Create()
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe:SecretKey").Value;

            // Create a new Cart object with predefined data
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem
                    {
                        Type = "Artwork",
                        Item = new ItemDetails { Id = 1, Name = "Artwork 1", Type = "Painting", Price = 50.00m },
                        Quantity = 1 // Add this line
                    },
                    new CartItem
                    {
                        Type = "Artwork",
                        Item = new ItemDetails { Id = 2, Name = "Artwork 2", Type = "Sculpture", Price = 120.00m },
                        Quantity = 2 // Add this line
                    },
                    new CartItem
                    {
                        Type = "Artwork",
                        Item = new ItemDetails { Id = 3, Name = "Artwork 3", Type = "Photograph", Price = 30.00m },
                        Quantity = 3 // Add this line
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
                            Name = $"{item.Item.Type}: {item.Item.Name}",
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
                CancelUrl = domain + "/index.html",
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return Json(new { id = session.Id });
        }
    }
}