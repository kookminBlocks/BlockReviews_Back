using BLockReviewsAPI.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BLockReviewsAPI.ApiService
{
    public interface IBlockChainCall
    {
        public Task CreateAccount();
        public Task<bool> CreateReview();
        public Task<bool> CreateLiked(Review review);
        public Task<Review> GetReviewById(string Id);
        public Task<Review> GetReviewByCategory();
        public Task RegisterForSale();
    }

    public class BlockChainAPICalling : IBlockChainCall
    {
        private const string ApiUrl = "";
        private string ApiKey = "";
        private IConfiguration configuration;
        private IHttpClientFactory httpFactory;
        private HttpClient httpClient;
        public BlockChainAPICalling(IConfiguration _configuration, IHttpClientFactory _httpFactory)
        {
            configuration = _configuration;
            httpFactory = _httpFactory;
            httpClient = httpFactory.CreateClient();

            ApiKey = configuration["ApiKey"];
        }

        public async Task CreateAccount()
        {
            BlockReviewAccount account = new BlockReviewAccount();                        
        }

        public async Task<bool> CreateReview()
        {
            BlockReviewAccount account = new BlockReviewAccount();
            
            return true;
        }

        public async Task<bool> CreateLiked(Review review)
        {
            return false;
        }

        public async Task<Review> GetReviewById(string Id)
        {

            return null;
        }

        public async Task<Review> GetReviewByCategory()
        {

            return null;
        }

        public async Task RegisterForSale()
        {
            
        }


    }
}
