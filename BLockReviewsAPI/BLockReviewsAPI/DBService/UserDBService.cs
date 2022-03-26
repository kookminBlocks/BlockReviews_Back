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
        public Task<bool> RegisterUser(UserInfo user);
        public Task<bool> Login(string id, string pwd);
    }
    public class UserDBService : IUserDBService
    {
        private IEtherConn etherConn;
        private IRegisterContract registerContract;
        private BlockReviewContext context;
        public UserDBService(BlockReviewContext _context, IEtherConn _etherConn, IRegisterContract _registerContract)
        {
            context = _context;
            etherConn = _etherConn;
            registerContract = _registerContract;
        }

        public async Task<bool> IdExistCheck(string ID)
        {
            var id = context.UserInfos.FirstOrDefault(e => e.Id == ID);

            if (id == null)
                return true;
            else
                return false;
        }

        public async Task<bool> RegisterUser(UserInfo user)
        {
            await registerContract.RegisterAccount(user);            

            context.UserInfos.Add(user);

            try
            {
                var i = await context.SaveChangesAsync();

                if (i > 0)
                {
                    return true;
                }  
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Login(string id, string pwd)
        {
            var LoginUser = context.UserInfos.FirstOrDefault(e => e.Id == id && e.Password == GetHash(pwd));

            if (LoginUser == null)
                return false;
            else
                return true;
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
