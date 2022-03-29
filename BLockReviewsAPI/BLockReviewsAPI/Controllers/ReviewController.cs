using BLockReviewsAPI.DBService;
using BLockReviewsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLockReviewsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private IReviewService reviewDBService;
        public ReviewController(IReviewService _reviewDBService)
        {
            reviewDBService = _reviewDBService;
        }

        /// <summary>
        /// 리뷰 생성
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateReview([FromBody] Review review)
        {
            await reviewDBService.CreateReview(review);

            return Ok();
        }

        /// <summary>
        /// 리뷰 좋아요
        /// </summary>
        [HttpPost("Like")]
        public async Task<IActionResult> AddLike([FromBody] string userId, string reviewId)
        {
            return Ok();
        }
    }
}
