using BLockReviewsAPI.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BLockReviewsAPI.ApiService
{
    public interface IBlockChainCall
    {
        public Task<BlockReviewAccount> CreateAccount();
        public Task<bool> CreateReview(Review review);
        public Task<bool> CreateLiked(string reviewId, UserInfo user);

        public Task<bool> OnSale(string reviewId, int price, UserInfo user);
        public Task<bool> OffSale(string reviewId, UserInfo user);

        public Task<List<Review>> GetReviewsByStore(string storeId);
    }

    public class BlockChainAPICalling : IBlockChainCall
    {
        private const string ApiUrl = "";
        private string ApiKey = "";
        private string AdminAccount = "";
        private IConfiguration configuration;
        private IHttpClientFactory httpFactory;
        private HttpClient httpClient;

        public BlockChainAPICalling(IConfiguration _configuration, IHttpClientFactory _httpFactory)
        {
            configuration = _configuration;
            httpFactory = _httpFactory;
            httpClient = httpFactory.CreateClient("BlockReview");
            httpClient.Timeout = TimeSpan.FromMinutes(10);
            ApiKey = configuration["ApiKey"];
            AdminAccount = configuration["Ether:AdminAccount"];
        }

        public async Task<BlockReviewAccount> CreateAccount()
        {
            var account = await httpClient.GetAsync("api/blockreview/user/eoa/create");

            var value = account.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<BlockReviewAccount>(value);

            if (await Approve(result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        private async Task<bool> Approve(BlockReviewAccount account)
        {
            BlockApproveAccount req = new BlockApproveAccount
            {
                privatekey = account.payload.privatekey,
                pubkey = account.payload.address
            };

            var payload = JsonConvert.SerializeObject(req);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/blockreview/user/approve", content);

            var error = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreateReview(Review review)
        {
            ReviewRequest req = new ReviewRequest
            {
                admin = "0xB28333cab47389DE99277F1A79De9a80A8d8678b",
                amount = 1000,
                category = review.StoreId,
                description = review.Content,
                //privatekey = review.User.AccountPrivateKey,
                //pubkey = review.User.AccountPublicKey,
                privatekey = "0xc8ea77271577557b0ea20cbf69894e194472e54188c765fe10892c5fd5ade8d0",
                pubkey = "0xcf2336e23F39638a1a42e7dd4A2Aa8cDBe9bFE42",
                title = review.Title,
                nftUri = "test"
            };

            var payload = JsonConvert.SerializeObject(req);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.Add("Keep-Alive", "10000");

            var response = await httpClient.PostAsync("api/blockreview/review/create", content);

            var error = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

            }
            else
            {

            }

            return true;
        }

        public async Task<bool> CreateLiked(string reviewId, UserInfo user)
        {
            ReviewLikeReq req = new ReviewLikeReq
            {
                admin = AdminAccount,
                privatekey = user.AccountPrivateKey,
                pubkey = user.AccountPublicKey,
                reviewId = reviewId
            };

            var payload = JsonConvert.SerializeObject(req);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/blockreview/review/like", content);

            return false;
        }

        public async Task<bool> OffSale(string reviewId, UserInfo user)
        {
            ReviewSaleReq req = new ReviewSaleReq
            {
                reviewId = reviewId,
                privatekey = user.AccountPrivateKey,
                pubkey = user.AccountPublicKey
            };

            var payload = JsonConvert.SerializeObject(req);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/blockreview/review/sale/offsale", content);

            return false;
        }

        public async Task<bool> OnSale(string revewId, int price, UserInfo user)
        {
            ReviewSaleReq req = new ReviewSaleReq
            {
                price = price,
                privatekey = user.AccountPrivateKey,
                pubkey = user.AccountPublicKey,
                reviewId = revewId,
            };

            var payload = JsonConvert.SerializeObject(req);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/blockreview/review/sale/onsale", content);

            return false;
        }

        public async Task Trade(string reviewId, UserInfo buy_user)
        {
            TradeReq req = new TradeReq
            {
                reviewId = reviewId,
                privateKey_buyer = buy_user.AccountPrivateKey,
                pubKey_buyer = buy_user.AccountPublicKey
            };

            var payload = JsonConvert.SerializeObject(req);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/blockreview/review/sale/trade", content);
        }

        public async Task<List<Review>> GetReviewsByStore(string storeId)
        {
            var req = new
            {
                storeId = storeId,
            };

            var payload = JsonConvert.SerializeObject(req);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/blockreview/review/read/category", content);

            return null;
        }
    }
}
