using DataAccessLayer.Models;
using Repository.BaseRepository;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos
{
    public class ActiveSubscriptionRepository : BaseRepository<ActiveSubscription>, IActiveSubscriptionRepository
    {
        // GetSubscriptionById
        public ActiveSubscription? GetSubscriptionById(int id)
        {
            return GetAll().ToList().Find(subscription => subscription.Id == id);
        }

        // Get stripe id based on subscription id
        public string GetStripeSubscriptionIDById(int subscriptionId)
        {
            var activeSubscription = GetAll().FirstOrDefault(s => s.Id == subscriptionId);

            // Return the Stripe subscription ID
            return activeSubscription?.StripeSubscriptionId;
        }


    }
}
