using DataAccessLayer.Enum;
using Microsoft.Extensions.DependencyInjection;

namespace Services.Extensions;

public static class AuthorizationService
{
    public static void AddAuthorizationService(this IServiceCollection services)
    {
        services.AddAuthorization(o =>
        {
            o.AddPolicy(Role.Customer.ToString(), policy => policy.RequireClaim(Role.Customer.ToString()));
            o.AddPolicy(Role.Admin.ToString(), policy => policy.RequireClaim(Role.Admin.ToString()));
            o.AddPolicy(Role.Creator.ToString(), policy => policy.RequireClaim(Role.Creator.ToString())); });
    }
}