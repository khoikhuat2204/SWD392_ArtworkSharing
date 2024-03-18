﻿using DataAccessLayer.Models;

namespace Services.Interface;

public interface ISubscriptionService
{
    public IQueryable<ActiveSubscription> GetAllActiveSubscriptions();
    
    public void AddSubscription(ActiveSubscription activeSubscription);

    public void UpdateSubscription(ActiveSubscription activeSubscription);
    
    public void RemoveSubscription(ActiveSubscription activeSubscription);

    public IQueryable<ActiveSubscription> GetAllActiveSubscriptionsByUserId(int userId);

    public ActiveSubscription? GetActiveSubscriptionById(int id);

    public string GetStripeSubscriptionIdBySubscriptionId(int subscriptionId);
}