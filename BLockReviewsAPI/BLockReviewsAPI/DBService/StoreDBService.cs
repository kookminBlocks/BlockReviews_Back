using BLockReviewsAPI.Data;
using BLockReviewsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLockReviewsAPI.DBService
{
    public interface IStoreDBService
    {
        public Task<bool> CreateStore(Store store);
        public Task<List<Store>> GetStores();
        public Task<List<Store>> GetUserStore(string userId);
    }

    public class StoreDBService : IStoreDBService
    {
        private BlockReviewContext context;
        public StoreDBService(BlockReviewContext _context)
        {
            context = _context;
        }
        public async Task<bool> CreateStore(Store store)
        {
            context.Stores.Add(store);
            int i = await context.SaveChangesAsync();

            if (i == 1)
                return true;
            else
                return false;
        }
        
        public async Task<List<Store>> GetStores()
        {
            return context.Stores.Take(10).ToList();
        }

        public async Task<List<Store>> GetUserStore(string userId)
        {
            return context.Stores.Where(e => e.UserId == userId).ToList();
        }
    }    
}
