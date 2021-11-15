using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Backend
{
    public static class ServiceExtenions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<ApiUser>(q =>
            {
                q.User.RequireUniqueEmail = true;
                q.SignIn.RequireConfirmedAccount = false;
                q.SignIn.RequireConfirmedEmail = false;
                q.SignIn.RequireConfirmedPhoneNumber = false;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder
                .AddEntityFrameworkStores<SetContext>()
                .AddDefaultTokenProviders();
        }
    }
}