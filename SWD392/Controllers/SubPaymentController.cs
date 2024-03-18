using DataAccessLayer.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
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


        /*        [HttpPost("subscribe")]
                *//*        [ValidateAntiForgeryToken]*//*
                public ActionResult Subscribe(string email, string plan, string package, string stripeToken)
                {
                    var id = _userService.GetIdByEmail(email);
                    var packageId = _packageService.GetPackageIdByName(package);

                    var customerOptions = new CustomerCreateOptions
                    {
                        Email = email,
                        Source = stripeToken,
                    };

                    var customerService = new CustomerService();
                    var customer = customerService.Create(customerOptions);
                    var planId = _configuration.GetSection($"Stripe:Packages:{package}:{plan}PlanId").Value;
                    // Previous code in action
                    *//*            var planId = _configuration.GetSection("Stripe:MonthlyPlanId").Value;*/
        /*            if (plan == "Yearly")
                    {
                        planId = _configuration.GetSection("Stripe:YearlyPlanId").Value;
                    }*//*

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

        // Add the subscription to your database
        var createdSubscription = new ActiveSubscription()
        {
            UserId = id,
            PackageId = packageId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(plan == "Yearly" ? 12 : 1) // Set the end date based on the plan
        };
        _subscriptionService.AddSubscription(createdSubscription);

        *//*            ViewBag.stripeKey = _configuration["Stripe:PublishableKey"];*//*
        ViewBag.stripeKey = _configuration.GetSection("Stripe:PublishableKey").Value;
        ViewBag.subscription = subscription.ToJson();

        // Store the subscription data in the session (or database)
        HttpContext.Session.SetString("Subscription", subscription.ToJson());
        return Json(new { redirectUrl = Url.Content("~/SubscribeResult.html"), response = subscription.ToJson() });
    }*/

        [HttpPost("subscribe")]
        public ActionResult Subscribe(string email, string plan, string package, string stripeToken, int? subscriptionId = null)
        {
            // Stripe planid 
            // user id --> name and email 

            // DATA NEED TO GET FROM FE: USERID --> EMAIL AND NAME FOR CUSTOMER
            //                           STRIPETOKEN NEEDS TO GENERATE FROM FE TOO 
            //                           PACKAGE ID OR PACKAGE NAME --> PACKAGE ID
            //                           STRIPE PLAN ID OR PLAN NAME --> PLAN ID 
            // THIS ACTION HANDLES BOTH UPDATE (UPGRADE) AND ADD BY CHECK IF ACTIV_SUBSCRIPTION_ID IS NULL OR NOT

            // HERE IS WHERE TO GET AND GENERATE SOME NE
            var id = _userService.GetIdByEmail(email);

            var name = _userService.GetNameByEmail(email);
            var packageId = _packageService.GetPackageIdByName(package);
            var packageData = _packageService.GetPackageByName(package);
            var planId = plan == "Yearly" ? packageData.YearlyStripePlanId : packageData.MonthlyStripePlanId;

            var customerOptions = new CustomerCreateOptions
            {
                Email = email,
                Name = name,
                Source = stripeToken,
            };

            var customerService = new CustomerService();
            var customer = customerService.Create(customerOptions);
            /*            var planId = _configuration.GetSection($"Stripe:Packages:{package}:{plan}PlanId").Value;*/
            // planid = _PackageService.GetPlanIdByName(plan);

            // CHECK IF THE SUBSCRIPTION IS ALREADY EXISTED OR NOT
            // IF YES CHECK IF THE NEW PLAN AND PACKAGE MATCH THE CURRENT PLAN AND PACKAGE 
            // IF YES RETURN ERROR MESSAGE
            // IF NO UPDATE THE SUBSCRIPTION

            // HERE IS WHERE THE SUBSCRIPTION IS ALREADY EXISTED AND THE NEW PLAN AND PACKAGE IS DIFFERENT FROM THE CURRENT PLAN AND PACKAGE
            if (subscriptionId.HasValue)
            {
                // Get the current plan and package of the subscription from your database or another source
                var activeSubs = _subscriptionService.GetActiveSubscriptionById(subscriptionId.Value);
                var currentPackageId = activeSubs.PackageId;
                var currentPlan = (activeSubs.EndDate - activeSubs.StartDate).TotalDays > 365 ? "Yearly" : "Monthly";

                // Check if the new planId and packageId match the current plan and package
                if (plan == currentPlan && packageId == currentPackageId)
                {
                    // The new plan and package match the current plan and package
                    return Json(new { status = "error", message = "The new plan and package match the current plan and package. Please select a different plan or package.", response = subscriptionId });
                }
            }

            // HERE IS WHERE IF THE SUBSCRIPTION IS NOT EXISTED OR THE NEW PLAN AND PACKAGE IS DIFFERENT FROM THE CURRENT PLAN AND PACKAGE
            // IF THE SUBSCRIPTION IS NOT EXISTED, CREATE A NEW SUBSCRIPTION
            var subscriptionService = new SubscriptionService();
            Subscription subscription;

            if (subscriptionId.HasValue)
            {
                // Get the Stripe subscription ID from your database or another source
                var stripeSubscriptionId = _subscriptionService.GetStripeSubscriptionIdBySubscriptionId(subscriptionId.Value);

                // Retrieve the existing subscription
                var existingSub = subscriptionService.Get(stripeSubscriptionId);

                // Get the ID of the existing subscription item
                var itemId = existingSub.Items.Data[0].Id;

                // Update the existing subscription item with the new plan
                var itemService = new SubscriptionItemService();
                var itemOptions = new SubscriptionItemUpdateOptions
                {
                    Plan = planId,
                };
                itemService.Update(itemId, itemOptions);

                // Update the subscription
                var subscriptionOptions = new SubscriptionUpdateOptions
                {
                    Items = new List<SubscriptionItemOptions>
                    {
                        new SubscriptionItemOptions
                        {
                            Id = itemId,
                            Plan = planId,
                        },
                    },
                };
                subscriptionOptions.AddExpand("latest_invoice.payment_intent");
                subscription = subscriptionService.Update(stripeSubscriptionId, subscriptionOptions);
            }
            else
            {
                // Create a new subscription
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

                subscription = subscriptionService.Create(subscriptionOptions);
            }

            // HERE IS WHERE TO UPDATE THE SUBSCRIPTION IN THE DATABASE
            if (subscriptionId.HasValue)
            {
                var activeSubscription = _subscriptionService.GetActiveSubscriptionById(subscriptionId.Value);
                activeSubscription.PackageId = packageId;
                activeSubscription.EndDate = DateTime.Now.AddMonths(plan == "Yearly" ? 12 : 1); // Set the end date based on the plan
                activeSubscription.StripeSubscriptionId = subscription.Id; // Store the Stripe subscription ID
                _subscriptionService.UpdateSubscription(activeSubscription);
                return Json(new { redirectUrl = Url.Content("~/SubscribeResult.html"), status = "updatesuccess", message = "Subscription updated successfully.", response = subscription.ToJson() });
            }
            else
            {
                var activeSubscription = new ActiveSubscription()
                {
                    UserId = id,
                    StripeSubscriptionId = subscription.Id, // Store the Stripe subscription ID
                    PackageId = packageId,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(plan == "Yearly" ? 12 : 1) // Set the end date based on the plan
                };
                _subscriptionService.AddSubscription(activeSubscription);
            }

            ViewBag.stripeKey = _configuration.GetSection("Stripe:PublishableKey").Value;
            ViewBag.subscription = subscription.ToJson();

            // STORE THE SUBSCRIPTION DATA IN THE SESSION (OR DATABASE)
            HttpContext.Session.SetString("Subscription", subscription.ToJson());
            return Json(new { redirectUrl = Url.Content("~/SubscribeResult.html"), response = subscription.ToJson(), status = "addsuccess", message = "Subscription created successfully." });
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
