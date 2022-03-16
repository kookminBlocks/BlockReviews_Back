using BLockReviewsAPI.BlockChainDI;
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
    public class BlockChainController : ControllerBase
    {
        private IEtherConn etherService { get; set; }
        public BlockChainController(IEtherConn _etherService)
        {
            etherService = _etherService;
        }

        [HttpPost("GetBlockNumber")]
        public async Task<IActionResult> GetBlockNumber()
        {
            await etherService.GetBlockNumber();
            return Ok();
            //if (result) { return Ok(); }
            //else { return BadRequest(); }
        }

        public async Task<IActionResult> CreateAccount()
        {
            
        }

    }
}
