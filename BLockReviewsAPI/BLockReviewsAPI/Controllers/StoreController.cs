﻿using BLockReviewsAPI.DBService;
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
        private IStoreDBService _storeDBService;
        public StoreController(IStoreDBService storeDBService)
        {
            _storeDBService = storeDBService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateStore([FromBody] Store store)
        {
            var result = await _storeDBService.CreateStore(store);

            if (result)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost("Get")]
        public async Task<IActionResult> GetStores([FromBody] Store store)
        {
            var result = await _storeDBService.GetStores();
            
            return Ok(result);            
        }
    }
}
