using DataAccessLayer.Models;
using Repository.Interface;
using Services.Interface;

namespace Services.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IActiveSubscriptionRepository _activeSubscriptionRepository;
    public SubscriptionService(IActiveSubscriptionRepository activeSubscriptionRepository)
    {
        _activeSubscriptionRepository = activeSubscriptionRepository;
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
}