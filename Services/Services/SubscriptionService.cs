using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.DTOs.ResponseDTO;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using Repository.Interface;
using Services.Interface;
using Stripe;

namespace Services.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IConfiguration _configuration;
    private readonly IActiveSubscriptionRepository _activeSubscriptionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPackageRepository _packageRepository;
    private readonly Stripe.CustomerService _stripeCustomerService;
    private readonly Stripe.SubscriptionService _stripeSubscriptionService;
    private readonly Stripe.SubscriptionItemService _stripeSubscriptionItemService;

    public SubscriptionService(
        IConfiguration configuration,
        IActiveSubscriptionRepository activeSubscriptionRepository,
        IUserRepository userRepository,
        IPackageRepository packageRepository,
        Stripe.CustomerService stripeCustomerService,
        Stripe.SubscriptionService stripeSubscriptionService,
        Stripe.SubscriptionItemService stripeSubscriptionItemService
        )
    {
        _configuration = configuration;
        StripeConfiguration.ApiKey = _configuration.GetSection("Stripe:SecretKey").Value;
        _activeSubscriptionRepository = activeSubscriptionRepository;
        _userRepository = userRepository;
        _packageRepository = packageRepository;
        _stripeCustomerService = stripeCustomerService;
        _stripeSubscriptionService = stripeSubscriptionService;
        _stripeSubscriptionItemService = stripeSubscriptionItemService;
    }

    public IQueryable<ActiveSubscription> GetAllActiveSubscriptions()
    {
        return _activeSubscriptionRepository.GetAll();
    }

    public void AddSubscription(ActiveSubscription activeSubscription)
    {
        _activeSubscriptionRepository.Add(activeSubscription);
    }

    public void UpdateSubscription(ActiveSubscription activeSubscription)
    {
        _activeSubscriptionRepository.Update(activeSubscription);
    }

    public void RemoveSubscription(ActiveSubscription activeSubscription)
    {
        _activeSubscriptionRepository.Delete(activeSubscription);
    }

    // Get all active subscriptions by subscription id
    public IQueryable<ActiveSubscription> GetAllActiveSubscriptionsByUserId(int userId)
    {
        return _activeSubscriptionRepository.GetAll().Where(a => a.UserId == userId);
    }

    public ActiveSubscription? GetActiveSubscriptionById(int id)
    {
        return _activeSubscriptionRepository.GetSubscriptionById(id);
    }

    public string GetStripeSubscriptionIdBySubscriptionId(int subscriptionId)
    {
        return _activeSubscriptionRepository.GetStripeSubscriptionIDById(subscriptionId);
    }

    public BuySubResponseDTO BuySubscription(BuySubRequestDTO buySubDto)
    {
        // DATA NEED TO GET FROM FE: USERID --> EMAIL AND NAME FOR CUSTOMER
        //                           STRIPETOKEN NEEDS TO GENERATE FROM FE TOO 
        //                           PACKAGE ID --> STRIPE PLAN ID
        User? user = _userRepository.GetById(buySubDto.UserId);
        Package? package = _packageRepository.GetById(buySubDto.PackageId);
        if (user == null || package == null)
        {
            return new BuySubResponseDTO()
            {
                Message = "User or package not found!",
                Success = false
            };
        }

        CustomerCreateOptions customerOptions = new()
        {
            Email = user.Email,
            Name = user.FullName,
            Source = buySubDto.StripeToken
        };
        Customer customer = _stripeCustomerService.Create(customerOptions);

        // CREATE A NEW SUBSCRIPTION
        string stripePlanId = buySubDto.IsYearly ? package.YearlyStripePlanId! : package.MonthlyStripePlanId!;
        SubscriptionCreateOptions subscriptionOptions = new()
        {
            Customer = customer.Id,
            Items = new List<SubscriptionItemOptions> { new SubscriptionItemOptions { Plan = stripePlanId } }
        };
        subscriptionOptions.AddExpand("latest_invoice.payment_intent");
        Subscription stripeSubscription = _stripeSubscriptionService.Create(subscriptionOptions);

        // CREATE THE SUBSCRIPTION IN THE DATABASE
        ActiveSubscription activeSubscription = new()
        {
            UserId = buySubDto.UserId,
            PackageId = buySubDto.PackageId,
            StripeSubscriptionId = stripeSubscription.Id, // Store the Stripe subscription ID
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(buySubDto.IsYearly ? 12 : 1) // Set the end date based on the plan
        };
        _activeSubscriptionRepository.Add(activeSubscription);

        return new BuySubResponseDTO()
        {
            Message = "Subscription created successfully.",
            Success = true
        };
    }

    public UpdateSubResponseDTO UpdateSubscription(UpdateSubRequestDTO updateSubDto)
    {
        // DATA NEED TO GET FROM FE: USERID --> EMAIL AND NAME FOR CUSTOMER
        //                           STRIPETOKEN NEEDS TO GENERATE FROM FE TOO 
        //                           PACKAGE ID --> STRIPE PLAN ID
        User? user = _userRepository.GetById(updateSubDto.UserId);
        Package? package = _packageRepository.GetById(updateSubDto.PackageId);
        if (user == null || package == null)
        {
            return new UpdateSubResponseDTO()
            {
                Message = "User or package not found!",
                Success = false
            };
        }

        // Get the current plan and package of the subscription database
        ActiveSubscription? activeSub = _activeSubscriptionRepository.GetSubscriptionById(updateSubDto.SubId);
        if (activeSub == null || activeSub.EndDate < DateTime.Now)
        {
            return new UpdateSubResponseDTO()
            {
                Message = "No active subscriptions found!",
                Success = false
            };
        }

        // Check if the new packageId match the current package
        if (updateSubDto.PackageId == activeSub.PackageId)
        {
            // The new package match the current package
            return new UpdateSubResponseDTO()
            {
                Message = "The new plan and package match the current plan and package. Please select a different plan or package.",
                Success = false
            };
        }

        // Get the Stripe subscription ID from database
        string stripeSubscriptionId = activeSub.StripeSubscriptionId!;

        // Retrieve the existing subscription
        Subscription existingSub = _stripeSubscriptionService.Get(stripeSubscriptionId);

        // Get the ID of the existing subscription item
        string itemId = existingSub.Items.Data[0].Id;

        // Update the existing subscription item with the new plan
        string stripePlanId = updateSubDto.IsYearly ? package.YearlyStripePlanId! : package.MonthlyStripePlanId!;
        SubscriptionItemUpdateOptions itemOptions = new()
        {
            Plan = stripePlanId,
        };
        _stripeSubscriptionItemService.Update(itemId, itemOptions);

        // Update the subscription
        var subscriptionOptions = new SubscriptionUpdateOptions
        {
            Items = new List<SubscriptionItemOptions> { new SubscriptionItemOptions { Id = itemId, Plan = stripePlanId } }
        };
        subscriptionOptions.AddExpand("latest_invoice.payment_intent");
        Subscription stripeSubscription = _stripeSubscriptionService.Update(stripeSubscriptionId, subscriptionOptions);

        // CREATE THE SUBSCRIPTION IN THE DATABASE
        ActiveSubscription activeSubscription = new()
        {
            UserId = updateSubDto.UserId,
            PackageId = updateSubDto.PackageId,
            StripeSubscriptionId = stripeSubscription.Id, // Store the Stripe subscription ID
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(updateSubDto.IsYearly ? 12 : 1) // Set the end date based on the plan
        };
        _activeSubscriptionRepository.Add(activeSubscription);

        return new UpdateSubResponseDTO()
        {
            Message = "Subscription updated successfully.",
            Success = true
        };
    }
}