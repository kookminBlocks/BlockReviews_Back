using BLockReviewsAPI.BlockChainDI;
using BLockReviewsAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLockReviewsAPI.Bootstrapper
{
    public static class EtherConfiguration
    {
        public static void AddEtherBlock(this IServiceCollection services)
        {
            services.AddSingleton<IEtherConn, EtherConn>();

        }
    }
}
