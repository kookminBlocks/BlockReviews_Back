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
        public Task<bool> CreateStore(Store review);
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
    }    
}
