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

        [HttpPost("Create")]
        public async Task<IActionResult> CreateReview([FromBody] Review review)
        {
            reviewDBService.CreateReview(review);

            return Ok();
        }
    }
}
