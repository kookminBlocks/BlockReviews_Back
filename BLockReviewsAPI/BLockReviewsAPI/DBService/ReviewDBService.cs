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
        public Task<bool> CreateReview(Review review);
        public Review ReviewDetail(string reviewId);
        public List<Review> ReviewList(string st, string fns);
    }
    public class ReviewDBService : IReviewService
    {
        private IEtherConn blockService;
        private BlockReviewContext context;
        public ReviewDBService(BlockReviewContext _context, IEtherConn _blockService)
        {
            context = _context;
            blockService = _blockService;
        }

        public async Task<bool> CreateReview(Review review)
        {
            await blockService.ReviewContract(review, ReviewActions.create);
            context.Reviews.Add(review);
            int i = await context.SaveChangesAsync();


            if (i == 1)
            {
                return true;
            }
            else
                return false;

        }

        public Review ReviewDetail(string reviewId)
        {
            return context.Reviews.FirstOrDefault(e => e.Id == reviewId);
        }

        public List<Review> ReviewList(string st, string fns)
        {
            return context.Reviews.OrderBy(e => e.StDate).ToList();
        }
    }
}
