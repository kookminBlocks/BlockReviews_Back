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
            await block.CreateReview(new Models.Review
            {
                Id = Guid.NewGuid().ToString(),
                Title = "test",
                Content = "test",
                StoreId = "test"
            });
            return Ok();
            //if (result) { return Ok(); }
            //else { return BadRequest(); }
        }
    }
}

