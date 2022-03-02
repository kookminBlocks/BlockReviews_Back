using BLockReviewsAPI.Data;
using BLockReviewsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLockReviewsAPI.DBService
{
    public interface IUserDBService
    {
        public Task<bool> RegisterUser(UserInfo user);
    }
    public class UserDBService : IUserDBService
    {
        private BlockReviewContext context;
        public UserDBService(BlockReviewContext _context)
        {
            context = _context;
        }

        public async Task<bool> RegisterUser(UserInfo user)
        {
            context.UserInfos.Add(user);            
            int i = await context.SaveChangesAsync();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
