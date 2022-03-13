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

        [HttpGet("IdCheck/{Id}")]
        public async Task<IActionResult> IdCheck(string Id)
        {
            return Ok(await userDBService.IdExistCheck(Id));
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserInfo user)
        {            
            var result = await userDBService.RegisterUser(user);
            if (result) { return Ok(); }
            else { return BadRequest(); }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] string id, string pwd)
        {
            var result = await userDBService.Login(id,pwd);
            if (result) { return Ok(); }
            else { return BadRequest(); }
        }
                
    }
}
