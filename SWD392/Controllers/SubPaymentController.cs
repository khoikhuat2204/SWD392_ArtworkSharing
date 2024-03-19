using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.DTOs.ResponseDTO;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Stripe;

namespace SWD392.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubPaymentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAntiforgery _antiForgery;
        private readonly IUserService _userService;
        private readonly IPackageService _packageService;
        private readonly ISubscriptionService _subscriptionService;

        public SubPaymentController(IConfiguration configuration, IAntiforgery antiForgery, IUserService userService,
            IPackageService packageService, ISubscriptionService subscriptionService)
        {
            _configuration = configuration;
            _antiForgery = antiForgery;
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe:SecretKey").Value;
            _userService = userService;
            _packageService = packageService;
            _subscriptionService = subscriptionService;
        }


        [HttpGet("get-antiforgery-token")]
        public IActionResult GetAntiForgeryToken()
        {
            var tokens = _antiForgery.GetAndStoreTokens(HttpContext);
            return Ok(tokens.RequestToken);
        }

        [HttpPost("buy-new-subscription")]
        public IActionResult BuySubscription([FromBody] BuySubRequestDTO buySubDto)
        {
            BuySubResponseDTO responseDto = _subscriptionService.BuySubscription(buySubDto);

            /*ViewBag.stripeKey = _configuration.GetSection("Stripe:PublishableKey").Value;
            ViewBag.subscription = subscription.ToJson();

            // STORE THE SUBSCRIPTION DATA IN THE SESSION (OR DATABASE)
            HttpContext.Session.SetString("Subscription", subscription.ToJson());*/

            if (responseDto.Success)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }

        [HttpPost("update-subscription")]
        public IActionResult UpdateSubscription([FromBody] UpdateSubRequestDTO updateSubDto)
        {
            UpdateSubResponseDTO responseDto = _subscriptionService.UpdateSubscription(updateSubDto);
            if (responseDto.Success)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }

        [HttpGet("get-subscription-data")]
        public ActionResult GetSubscriptionData()
        {
            // Retrieve the subscription data from the session (or database)
            var subscriptionData = HttpContext.Session.GetString("Subscription");

            return Json(new { subscription = subscriptionData });
        }

        [HttpGet("subscriptions")]
        public ActionResult GetSubscriptions(string email)
        {
            var userId = _userService.GetIdByEmail(email);
            var subscriptions = _subscriptionService.GetAllActiveSubscriptionsByUserId(userId).ToList();
            return Ok(subscriptions);
        }

        [HttpPost("cancelSubscription")]
        public ActionResult CancelSubscription(int subscriptionId)
        {
            // Get the Stripe subscription ID from your database or another source
            var stripeSubscriptionId = _subscriptionService.GetStripeSubscriptionIdBySubscriptionId(subscriptionId);

            // Cancel the subscription at the end of the current billing period
            var subscriptionService = new SubscriptionService();
            var cancelOptions = new SubscriptionCancelOptions { InvoiceNow = false, Prorate = false };
            var canceledSubscription = subscriptionService.Cancel(stripeSubscriptionId, cancelOptions);

            // Update the subscription status in your database NO DATABASE YET MAN IM SAD RN
/*            _subscriptionService.CancelSubscription(subscriptionId);*/

            return Json(new { status = "success", message = "Subscription cancelled successfully. The subscription will remain active until the end of the current billing period." });
        }


        /*
                [HttpPost("change-package")]
                public ActionResult ChangePackage(string userId, string newPackage, string newPlan)
                {
                    // Get the new plan ID based on the selected package and plan
                    var newPlanId = _configuration.GetSection($"Stripe:Packages:{newPackage}:{newPlan}PlanId").Value;

                    // Retrieve the user's current subscription from Stripe
                    var subscriptionService = new SubscriptionService();
                    var subscription = subscriptionService.Get(userId);

                    // Modify the subscription
                    var options = new SubscriptionUpdateOptions
                    {
                        CancelAtPeriodEnd = false,
                        Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Id = subscription.Items.Data[0].Id,
                        Plan = newPlanId,
                    },
                },
                    };
                    subscriptionService.Update(subscription.Id, options);

                    return Json(new { success = true });
                }*/



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
