using BLockReviewsAPI.ApiService;
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
        private IBlockChainCall block { get; set; }
        private IEtherConn etherService { get; set; }
        public BlockChainController(IEtherConn _etherService, IBlockChainCall _block)
        {
            etherService = _etherService;
            block = _block;
        }

        [HttpPost("GetBlockNumber")]
        public async Task<IActionResult> GetBlockNumber()
        {
            await etherService.GetBlockNumber();
            return Ok();
            //if (result) { return Ok(); }
            //else { return BadRequest(); }
        }              

        [HttpPost("TEST")]
        public async Task<IActionResult> test()
        {
            await block.CreateLiked(4,new Models.UserInfo
            {                
                AccountPrivateKey = "0xc8ea77271577557b0ea20cbf69894e194472e54188c765fe10892c5fd5ade8d0",
                AccountPublicKey= "0xcf2336e23F39638a1a42e7dd4A2Aa8cDBe9bFE42",                
            });
            return Ok();
            //if (result) { return Ok(); }
            //else { return BadRequest(); }
        }
    }
}

