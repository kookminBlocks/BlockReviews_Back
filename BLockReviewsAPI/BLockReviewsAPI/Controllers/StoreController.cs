using BLockReviewsAPI.Models;
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
    public class StoreController : ControllerBase
    {
        public StoreController()
        {

        }

        [HttpPost("Create")]
        public Task<IActionResult> CreateStore([FromBody] Store review)
        {

        }
    }
}
