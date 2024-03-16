using DataAccessLayer.Models;

namespace Services.Interface;

public interface ISubscriptionService
{
    public IQueryable<ActiveSubscription> GetAllActiveSubscriptions();
    
    public void AddSubscription(ActiveSubscription activeSubscription);

    public void UpdateSubscription(ActiveSubscription activeSubscription);
    
    public void RemoveSubscription(ActiveSubscription activeSubscription);
}