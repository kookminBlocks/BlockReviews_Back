using BLockReviewsAPI.DBService;
using BLockReviewsAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
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
            try
            {
                await reviewDBService.CreateReview(review);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 리뷰 좋아요
        /// </summary>        
        [HttpPost("Like/{reviewId}")]
        public async Task<IActionResult> AddLike([FromBody] UserInfo userId, [FromRoute] int reviewId)
        {
            var like = await reviewDBService.AddLike(reviewId, userId);

            return Ok();
        }


        /// <summary>
        /// 리뷰 조회
        /// </summary>

        [HttpGet("GetReviewByStore/{storeId}")]

        public async Task<IActionResult> GetReviewByStore([FromRoute] string storeId)
        {
            var result = await reviewDBService.GetReviewByStore(storeId);
            return Ok(result);
        }

        /// <summary>
        /// 리뷰 조회
        /// </summary>

        [HttpGet("GetReviewByUser/{UserId}")]

        public async Task<IActionResult> GetReviewByUser([FromRoute] string userId)
        {
            var result = await reviewDBService.GetReviewByUser(userId);
            return Ok(result);
        }


        /// <summary>
        /// IPFS 생성
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("ipfs")]
        public async Task<IActionResult> CreateIpfs([FromBody] CreateIpfs ipfs)
        {
            var result = await reviewDBService.IpfsCreate(ipfs);
            return Ok(result);
        }
    }

    public class CreateIpfs
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile file { get; set; }
    }
}
