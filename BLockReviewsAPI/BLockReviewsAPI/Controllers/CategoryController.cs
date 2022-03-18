using BLockReviewsAPI.DBService;
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
    public class CategoryController : ControllerBase
    {
        private ICategoryDBService _cateDBService;
        public CategoryController(ICategoryDBService categoryDBService)
        {
            _cateDBService = categoryDBService;
        }

        /// <summary>
        /// 카테고리 목록 조회
        /// </summary>
        /// <param name="level"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet("Get/{level}/{parentId}")]
        public async Task<IActionResult> SelectCateList(int level, string parentId)
        {
            return Ok(_cateDBService.SelectCate(level, parentId));
        }
    }
}
