using DataAccessLayer.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;

namespace SWD392.Controllers
{
    [ApiController]
    [Route("/")]
    public class SubPaymentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAntiforgery _antiForgery;

        public SubPaymentController(IConfiguration configuration, IAntiforgery antiForgery)
        {
            _configuration = configuration;
            _antiForgery = antiForgery;
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe:SecretKey").Value;
        }

        [HttpGet("get-antiforgery-token")]
        public IActionResult GetAntiForgeryToken()
        {
            var tokens = _antiForgery.GetAndStoreTokens(HttpContext);
            return Ok(tokens.RequestToken);
        }


        [HttpPost("subscribe")]
        /*        [ValidateAntiForgeryToken]*/
        public ActionResult Subscribe(string email, string plan, string package, string stripeToken)
        {
            var customerOptions = new CustomerCreateOptions
            {
                Email = email,
                Source = stripeToken,
            };

            var customerService = new CustomerService();
            var customer = customerService.Create(customerOptions);
            var planId = _configuration.GetSection($"Stripe:Packages:{package}:{plan}PlanId").Value;
            // Previous code in action
            /*            var planId = _configuration.GetSection("Stripe:MonthlyPlanId").Value;*/
            /*            if (plan == "Yearly")
                        {
                            planId = _configuration.GetSection("Stripe:YearlyPlanId").Value;
                        }*/

            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = customer.Id,
                Items = new List<SubscriptionItemOptions>
        {
            new SubscriptionItemOptions
            {
                Plan = planId
            },
        },
            };
            subscriptionOptions.AddExpand("latest_invoice.payment_intent");

            var subscriptionService = new SubscriptionService();
            var subscription = subscriptionService.Create(subscriptionOptions);

            /*            ViewBag.stripeKey = _configuration["Stripe:PublishableKey"];*/
            ViewBag.stripeKey = _configuration.GetSection("Stripe:PublishableKey").Value;
            ViewBag.subscription = subscription.ToJson();

            // Store the subscription data in the session (or database)
            HttpContext.Session.SetString("Subscription", subscription.ToJson());
            return Json(new { redirectUrl = Url.Content("~/SubscribeResult.html"), response = subscription.ToJson() });
        }

        [HttpGet("get-subscription-data")]
        public ActionResult GetSubscriptionData()
        {
            // Retrieve the subscription data from the session (or database)
            var subscriptionData = HttpContext.Session.GetString("Subscription");

            return Json(new { subscription = subscriptionData });
        }


        [HttpPost("subscription-webhook")]
        public IActionResult SubscriptionWebhook()
        {
            string signingSecret = _configuration["Stripe:signing_secret"];

            var json = new StreamReader(HttpContext.Request.Body).ReadToEnd();

            try
            {

                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"],
                    signingSecret,
                    throwOnApiVersionMismatch: true);

                if (stripeEvent == null)
                {
                    return BadRequest("Event was null");
                }

                switch (stripeEvent.Type)
                {
                    case "invoice.payment_succeeded":
                        // Do something with the event for when the payment goes through
                        Invoice successInvoice = (Invoice)stripeEvent.Data.Object;
                        return Ok();
                    case "invoice.payment_failed":
                        // Do something with the event for when the payment fails
                        Invoice failInvoice = (Invoice)stripeEvent.Data.Object;
                        return Ok();
                    default:
                        return BadRequest("Event was not valid type");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

    }
}
