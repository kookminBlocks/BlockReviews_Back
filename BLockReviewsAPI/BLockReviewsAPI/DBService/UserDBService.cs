using BLockReviewsAPI.ApiService;
using BLockReviewsAPI.BlockChainDI;
using BLockReviewsAPI.Data;
using BLockReviewsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLockReviewsAPI.DBService
{
    public interface IUserDBService
    {
        public Task<bool> IdExistCheck(string Id);
        public Task<UserInfo> RegisterUser(UserInfo user);
        public Task<UserInfo> Login(UserInfo user);
    }
    public class UserDBService : IUserDBService
    {
        private IBlockChainCall blockChainService;
        private IEtherConn etherConn;
        private BlockReviewContext context;
        public UserDBService(BlockReviewContext _context, IEtherConn _etherConn, IBlockChainCall _blockChainService)
        {
            context = _context;
            etherConn = _etherConn;
            blockChainService = _blockChainService;
        }

        public async Task<bool> IdExistCheck(string ID)
        {
            var id = context.UserInfos.FirstOrDefault(e => e.Id == ID);

            if (id == null)
                return true;
            else
                return false;
        }

        public async Task<UserInfo> RegisterUser(UserInfo user)
        {                 
            user.Password = GetHash(user.Password);

            var account = await blockChainService.CreateAccount();

            user.AccountPrivateKey = GetHash(account.payload.privatekey);
            user.AccountPublicKey = account.payload.address;
            context.UserInfos.Add(user);

            try
            {
                var i = await context.SaveChangesAsync();

                if (i > 0)
                {
                    user.OriginPrivateKey = account.payload.privatekey;
                    return user;
                }  
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserInfo> Login(UserInfo user)
        {
            var LoginUser = context.UserInfos.FirstOrDefault(e => e.Id == user.Id 
                                                                && e.Password == GetHash(user.Password) 
                                                                && e.AccountPrivateKey == GetHash(user.AccountPrivateKey));

            return LoginUser;
        }

        private string GetHash(string origin)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(origin));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
