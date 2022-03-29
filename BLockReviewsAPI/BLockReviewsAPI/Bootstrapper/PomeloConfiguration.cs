using BLockReviewsAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLockReviewsAPI.Bootstrapper
{
    public static class PomeloConfiguration
    {

        public static void AddPoemloConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStr = configuration["ConnectionStrings:BlockReviewConn"];

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 28));

            services.AddDbContext<BlockReviewContext>(options =>
            {
                options.UseLazyLoadingProxies()
                    .UseMySql(connectionStr, serverVersion);
//#if DEBUG
//                options
//                    .UseLoggerFactory(LoggerFactory.Create(
//                        builder => builder.AddConsole()))
//                //     .EnableSensitiveDataLogging()
//                ;
//#endif
            });

        }
    }
}
