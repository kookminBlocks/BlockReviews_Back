using BLockReviewsAPI.DBService;
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
    public class UserController : ControllerBase
    {
        private IUserDBService userDBService { get; set; }
        public UserController(IUserDBService _userDBService)
        {
            userDBService = _userDBService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserInfo user)
        {
            var result = await userDBService.RegisterUser(user);
            if (result) { return Ok(); }
            else { return BadRequest(); }
        }
    }
}
