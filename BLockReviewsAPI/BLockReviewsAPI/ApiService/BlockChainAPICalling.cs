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
        public Task<bool> CreateLiked(int reviewId, UserInfo user);

        public Task<bool> OnSale(int reviewId, int price, UserInfo user);
        public Task<bool> OffSale(int reviewId, UserInfo user);

        public Task<ReviewRes> GetReviewsByStore(string storeId);
        public Task<ReviewResByUser> GetReviewsByUser(string pubkey);

        public Task<ReviewRes> GetReviewDetail(int reviewId);

    }

    public class BlockChainAPICalling : IBlockChainCall
    {        
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
            AdminAccount = configuration["Ether:AdminAccount"];
        }

        public async Task<BlockReviewAccount> CreateAccount()
        {
            var account = await httpClient.GetAsync("api/blockreview/user/eoa/create");

            var value = account.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<BlockReviewAccount>(value);

            if (await Approve(result))
            {
                var req = new
                {
                    sendTo = result.payload.address,
                };
                var payload = JsonConvert.SerializeObject(req);

                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                var faucet = await httpClient.PostAsync("api/blockreview/user/faucet", content);

                
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
                title = review.Title,
                category = review.StoreId,
                description = review.Content,
                privatekey = review.User?.AccountPrivateKey,
                pubkey = review.User?.AccountPublicKey,
                nftUri = "0xtestIPFSHASH",
                admin = AdminAccount,
                amount = 1000,
            };

            var payload = JsonConvert.SerializeObject(req);

            var content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");                      

            var response = await httpClient.PostAsync("api/blockreview/review/create", content);

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

        public async Task<bool> CreateLiked(int reviewId, UserInfo user)
        {
            ReviewLikeReq req = new ReviewLikeReq
            {
                reviewId = reviewId,
                admin = AdminAccount,
                amount = 100,
                pubkey = user.AccountPublicKey,
                privatekey = user.AccountPrivateKey,
            };

            var payload = JsonConvert.SerializeObject(req);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/blockreview/review/like", content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public async Task<bool> OffSale(int reviewId, UserInfo user)
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

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> OnSale(int revewId, int price, UserInfo user)
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

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Trade(int reviewId, UserInfo buy_user)
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

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ReviewRes> GetReviewsByStore(string storeId)
        {
            var req = new
            {
                category = storeId,
            };

            var payload = JsonConvert.SerializeObject(req);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/blockreview/review/read/category", content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resValue = response.Content.ReadAsStringAsync().Result;
                var reviews = JsonConvert.DeserializeObject<ReviewRes>(resValue);

                //var result = new List<Review>();

                //reviews.ForEach(e => {
                //    result.Add(new Review
                //    {
                //        Id = e.payload.id,
                //        Title = e.payload.title,
                //        Content = e.payload.description,
                //        UserId = e.payload.owner,
                //        StDate = e.payload.owner,
                //        UserId = e.payload.owner,
                //        UserId = e.payload.owner,
                //    });
                //});

                return reviews;
            }
            else
            {
                return null;
            }                        
        }

        public async Task<ReviewRes> GetReviewDetail(int reviewId)
        {
            var req = new
            {
                reviewId = reviewId,
            };

            var payload = JsonConvert.SerializeObject(req);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/blockreview/review/read/id", content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resValue = response.Content.ReadAsStringAsync().Result;
                var review = JsonConvert.DeserializeObject<ReviewRes>(resValue);                

                return review;
            }
            else
            {
                return null;
            }
        }

        public async Task<ReviewResByUser> GetReviewsByUser(string PublicKey)
        {
            var req = new
            {
                pubkey = PublicKey,
            };

            var payload = JsonConvert.SerializeObject(req);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/blockreview/review/read/owner", content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resValue = response.Content.ReadAsStringAsync().Result;
                var reviews = JsonConvert.DeserializeObject<ReviewResByUser>(resValue);

                return reviews;
            }
            else
            {
                return null;
            }
        }
    }
}
