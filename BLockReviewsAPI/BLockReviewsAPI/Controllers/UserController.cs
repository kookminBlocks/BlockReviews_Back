using BLockReviewsAPI.DBService;
using BLockReviewsAPI.Models;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
        public async Task<FileStreamResult> RegisterUser([FromBody] UserInfo user)
        {       
            var result = await userDBService.RegisterUser(user);

            var records = new List<BlockApproveAccount>
            {
                new BlockApproveAccount { pubkey = result.AccountPublicKey, privatekey = result.AccountPrivateKey }
            };

            var stream = new MemoryStream();
            using (var writeFile = new StreamWriter(stream, leaveOpen: true))
            {
                var csv = new CsvWriter(writeFile, CultureInfo.InvariantCulture) ;                
                csv.WriteRecords(records);
            }
            stream.Position = 0;
            Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();

            if (result != null)
            {
                return new FileStreamResult(stream, "text/csv") { FileDownloadName = $"{result.Id}_account.csv" };
            }
            else { return null; }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserInfo user)
        {
            var result = await userDBService.Login(user.Id, user.Password);
            if (result != null) { return Ok(result); }
            else { return BadRequest(); }
        }
                
    }
}
