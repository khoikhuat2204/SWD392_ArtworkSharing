using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Services.Interface;
using Stripe.Checkout;
using Stripe;

namespace SWD392.Controllers
{

    public class Cart
    {
        public Artwork artwork { get; set; }
        public int Quantity { get; set; }
    }


    [ApiController]
    [Route("/")]
    public class CartController : Controller
    {

        private readonly ILogger<CartController> _logger;
        private readonly IArtworkService _artworkService;
        public const string CARTKEY = "cart";
        private readonly IConfiguration _configuration;
        private readonly string domain = "http://localhost:5272";
        const string endpointSecret = "whsec_1a8a4fb38cccd3a877097b54cbc7b1aab916b660a604728687452e9fb56b0f0d";

        public CartController(ILogger<CartController> logger, IArtworkService artworkService, IConfiguration configuration)
        {
            _logger = logger;
            _artworkService = artworkService;
            _configuration = configuration;
        }

        [HttpPost("add-artwork-to-cart/{id:int}")]
        public async Task<IActionResult> AddArtworkToCart(int id)
        {
            var cart = HttpContext.Session.GetString("cart"); //get key cart
            if (cart == null)
            {
                var artwork = _artworkService.GetAll().FirstOrDefault(a => a.Id == id);
                if (artwork == null)
                {
                    return NotFound();
                }
                List<Cart> listCart = new List<Cart>()
                        {
                            new Cart
                            {
                                artwork = artwork,
                                Quantity = 1
                            }
                        };
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(listCart));
            }
            else
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                bool check = true;
                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].artwork.Id == id)
                    {
                        dataCart[i].Quantity++;
                        check = false;
                    }
                }
                if (check)
                {
                    dataCart.Add(new Cart
                    {
                        artwork = _artworkService.GetAll().FirstOrDefault(a => a.Id == id),
                        Quantity = 1
                    });
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
            }

            // Test session
            string sessioncart = HttpContext.Session.GetString("cart");

            //print to console
            Console.WriteLine("Session cart: " + sessioncart);

            // I want to keep stay in Index after adding artwork to cart
            return RedirectToAction(nameof(Cart));
        }
        /*
                [HttpPost("add-artwork-to-cart/{id:int}")]
                public async Task<IActionResult> AddArtworkToCart(int id)
                {
                    var cart = TempData["cart"] as string; //get key cart
                    if (cart == null)
                    {
                        var artwork = _artworkService.GetAll().FirstOrDefault(a => a.Id == id);
                        if (artwork == null)
                        {
                            return NotFound();
                        }
                        List<Cart> listCart = new List<Cart>()
                {
                    new Cart
                    {
                        artwork = artwork,
                        Quantity = 1
                    }
                };
                        TempData["cart"] = JsonConvert.SerializeObject(listCart);
                    }
                    else
                    {
                        List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                        bool check = true;
                        for (int i = 0; i < dataCart.Count; i++)
                        {
                            if (dataCart[i].artwork.Id == id)
                            {
                                dataCart[i].Quantity++;
                                check = false;
                            }
                        }
                        if (check)
                        {
                            dataCart.Add(new Cart
                            {
                                artwork = _artworkService.GetAll().FirstOrDefault(a => a.Id == id),
                                Quantity = 1
                            });
                        }
                        TempData["cart"] = JsonConvert.SerializeObject(dataCart);
                    }

                    // Test TempData
                    string tempDataCart = TempData["cart"] as string;

                    //print to console
                    Console.WriteLine("TempData cart: " + tempDataCart);

                    // I want to keep stay in Index after adding artwork to cart
                    return RedirectToAction(nameof(Cart));
                }
        */


        [HttpGet("cart", Name = "cart")]
        public async Task<IActionResult> Cart()
        {
            // Get the cart items from the session
            string cartItems = HttpContext.Session.GetString("cart");

            Console.WriteLine("Cart items: " + cartItems);

            // Check if the cart is null
            if (string.IsNullOrEmpty(cartItems))
            {
                // Return an empty list
                return Ok(new List<object>());
            }

            // Deserialize the cart items into a list of Cart objects
            List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(cartItems);


            // Create a new list to hold the cart item details
            var cartDetails = new List<object>();

            // Iterate over each item in the cart
            foreach (var item in cartList)
            {
                // Add the artwork details and quantity to the cartDetails list
                cartDetails.Add(new
                {
                    ArtworkId = item.artwork.Id,
                    ArtworkName = item.artwork.Name,
                    ArtworkPrice = 300.00m,
                    Quantity = item.Quantity
                });
            }

            // Return the cartDetails list
            return Ok(cartDetails);
        }


        /*        [HttpGet("cart", Name = "cart")]
                public async Task<IActionResult> Cart()
                {
                    // Get the cart items from the TempData
                    string cartItems = TempData["cart"] as string;

                    // Check if the cart is null
                    if (string.IsNullOrEmpty(cartItems))
                    {
                        // Return an empty list
                        return Ok(new List<object>());
                    }

                    // Deserialize the cart items into a list of Cart objects
                    List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(cartItems);

                    // Create a new list to hold the cart item details
                    var cartDetails = new List<object>();

                    // Iterate over each item in the cart
                    foreach (var item in cartList)
                    {
                        // Add the artwork details and quantity to the cartDetails list
                        cartDetails.Add(new
                        {
                            ArtworkId = item.artwork.Id,
                            ArtworkName = item.artwork.Name,
                            ArtworkPrice = 300.00m,
                            Quantity = item.Quantity
                        });
                    }

                    // Return the cartDetails list
                    return Ok(cartDetails);
                }*/



        // Update cart
        [HttpPut("updatecart/{id:int}/{quantity:int}")]
        public async Task<IActionResult> UpdateCart(int id, int quantity)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                if (quantity > 0)
                {
                    for (int i = 0; i < dataCart.Count; i++)
                    {
                        if (dataCart[i].artwork.Id == id)
                        {
                            dataCart[i].Quantity = quantity;
                        }
                    }

                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                }
                return Ok(quantity);
            }
            return BadRequest();
        }



        // Delete cart item
        [HttpDelete("deletecart/{id:int}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);

                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].artwork.Id == id)
                    {
                        dataCart.RemoveAt(i);
                        break; // Break the loop after removing the item
                    }
                }

                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                return Ok(id); // Return the id of the deleted item
            }
            return BadRequest();
        }


        /*        // Update cart
                [HttpPut("updatecart/{id:int}/{quantity:int}")]
                public async Task<IActionResult> UpdateCart(int id, int quantity)
                {
                    var cart = TempData["cart"] as string;
                    if (cart != null)
                    {
                        List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                        if (quantity > 0)
                        {
                            for (int i = 0; i < dataCart.Count; i++)
                            {
                                if (dataCart[i].artwork.Id == id)
                                {
                                    dataCart[i].Quantity = quantity;
                                }
                            }

                            TempData["cart"] = JsonConvert.SerializeObject(dataCart);
                        }
                        return Ok(quantity);
                    }
                    return BadRequest();
                }

                // Remove artwork from cart
                [HttpDelete("deletecart/{id:int}")]
                public async Task<IActionResult> DeleteCart(int id)
                {
                    var cart = TempData["cart"] as string;
                    if (cart != null)
                    {
                        List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);

                        for (int i = 0; i < dataCart.Count; i++)
                        {
                            if (dataCart[i].artwork.Id == id)
                            {
                                dataCart.RemoveAt(i);
                            }
                        }
                        TempData["cart"] = JsonConvert.SerializeObject(dataCart);
                        return RedirectToAction(nameof(Cart));
                    }
                    return RedirectToAction(nameof(Index));
                }*/



        [HttpPost("create-checkout-session")]
        public async Task<ActionResult> Create()
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe:SecretKey").Value;

            // Get the cart from the session
            var cartString = HttpContext.Session.GetString("cart");

            // Check if the cart is null and return so json can read
            if (cartString == null)
            {
                return BadRequest("Session data is missing");
            }

            // TEST STUFF REMBER TO COMMENT AFTER THAT
            /*            string cartString = "[{\"artwork\": {\"Id\": 1,\"Name\": \"Artwork 1\"},\"Quantity\": 2},{\"artwork\": {\"Id\": 2,\"Name\": \"Artwork 2\"},\"Quantity\": 1}]";*/

            // Deserialize the cart string into a list of Cart objects
            List<Cart> cart = JsonConvert.DeserializeObject<List<Cart>>(cartString);

            var lineItems = new List<SessionLineItemOptions>();

            // Add each item in the cart to the lineItems list
            foreach (var item in cart)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(300 * 100), // Convert dollars to cents and decimal to long
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Artwork: {item.artwork.Name}",
                        },
                    },
                    Quantity = item.Quantity,
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



        /*        [HttpPost("create-checkout-session")]
                public async Task<ActionResult> Create()
                {
                    StripeConfiguration.ApiKey = _configuration.GetSection("Stripe:SecretKey").Value;

                    // Get the cart from TempData
                    var cartString = TempData["cart"] as string;

                    // Check if the cart is null and return so json can read
                    if (cartString == null)
                    {
                        return BadRequest("TempData is missing");
                    }

                    // Deserialize the cart string into a list of Cart objects
                    List<Cart> cart = JsonConvert.DeserializeObject<List<Cart>>(cartString);

                    var lineItems = new List<SessionLineItemOptions>();

                    // Add each item in the cart to the lineItems list
                    foreach (var item in cart)
                    {
                        lineItems.Add(new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)(300 * 100), // Convert dollars to cents and decimal to long
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = $"Artwork: {item.artwork.Name}",
                                },
                            },
                            Quantity = item.Quantity,
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
        */

        [HttpPost("webhook")]
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                // Handle the event
                if (stripeEvent.Type == Events.InvoicePaymentSucceeded)
                {
                    var session = stripeEvent.Data.Object as Session;
                    Console.WriteLine("Payment succeeded for session: {0}", session.Id);
                    // TODO: Send an invoice to the customer
                    // using the information in the invoice
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }



    }
}
