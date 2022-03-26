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
        public Task<BlockApproveAccount> RegisterUser(UserInfo user);
        public Task<UserInfo> Login(string id, string pwd);
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

        public async Task<BlockApproveAccount> RegisterUser(UserInfo user)
        {
            //await registerContract.RegisterAccount(user);            
            user.Password = GetHash(user.Password);

            var account = await blockChainService.CreateAccount();

            context.UserInfos.Add(user);

            try
            {
                var i = await context.SaveChangesAsync();

                if (i > 0)
                {
                    return account;
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

        public async Task<UserInfo> Login(string id, string pwd)
        {
            var LoginUser = context.UserInfos.FirstOrDefault(e => e.Id == id && e.Password == GetHash(pwd));

            if (LoginUser == null)
                return LoginUser;
            else
                return null;
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
