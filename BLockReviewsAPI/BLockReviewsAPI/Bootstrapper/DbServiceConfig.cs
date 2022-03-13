using BLockReviewsAPI.DBService;
using Microsoft.Extensions.DependencyInjection;

namespace BLockReviewsAPI.Bootstrapper
{
    public static class DbServiceConfig
    {
        public static void AddDBservices(this IServiceCollection services)
        {
            services.AddTransient<IUserDBService, UserDBService>();
            services.AddTransient<IReviewService, ReviewDBService>();
            services.AddTransient<IStoreDBService, StoreDBService>();
        }

    }
}
