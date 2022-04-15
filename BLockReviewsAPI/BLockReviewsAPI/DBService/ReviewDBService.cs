using BLockReviewsAPI.ApiService;
using BLockReviewsAPI.BlockChainDI;
using BLockReviewsAPI.Data;
using BLockReviewsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLockReviewsAPI.DBService
{
    public interface IReviewService
    {
        public Task CreateReview(Review review);
        public Task<bool> AddLike(int reviewId, UserInfo user);
        public Task<ReviewRes> ReviewDetail(int reviewId);        
        public Task<ReviewRes> GetReviewByStore(string storeId);
        public Task<ReviewResByUser> GetReviewByUser(string pubkey);
    }
    public class ReviewDBService : IReviewService
    {
        private IBlockChainCall blockChainCall;
        private BlockReviewContext context;
        public ReviewDBService(BlockReviewContext _context, IBlockChainCall _blockChainCall)
        {
            context = _context;
            blockChainCall = _blockChainCall;
        }

        public async Task<bool> AddLike(int reviewId, UserInfo user)
        {
            var result = await blockChainCall.CreateLiked(reviewId, user);

            if (result)
                return true;
            else
                return false;
        }

        public async Task CreateReview(Review review)
        {
            await blockChainCall.CreateReview(review);
            review.User = null;
            context.Reviews.Add(review);
            int i = await context.SaveChangesAsync();
        }

        public async Task<ReviewRes> GetReviewByStore(string storeId)
        {
            var result = blockChainCall.GetReviewsByStore(storeId).Result;
            return result;
        }

        public async Task<ReviewResByUser> GetReviewByUser(string pubkey)
        {
            var result = blockChainCall.GetReviewsByUser(pubkey).Result;
            return result;
        }

        public async Task<ReviewRes> ReviewDetail(int reviewId)
        {
            var result = blockChainCall.GetReviewDetail(reviewId).Result;
            return result;
        }

        public List<Review> ReviewList(string st, string fns)
        {
            return context.Reviews.OrderBy(e => e.StDate).ToList();
        }
    }
}
