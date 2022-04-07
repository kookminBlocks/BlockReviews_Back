using BLockReviewsAPI.Data;
using BLockReviewsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLockReviewsAPI.DBService
{
    public interface ICategoryDBService
    {
        public List<Category> SelectCate(int level);
        public List<Category> SelectCateByParentId(string parentId);
    }

    public class CategoryDBService : ICategoryDBService
    {
        private BlockReviewContext _context;
        public CategoryDBService(BlockReviewContext context)
        {
            _context = context;
        }

        public List<Category> SelectCate(int level)
        {            
            return _context.Categories.Where(e => e.Level == level).ToList();            
        }

        public List<Category> SelectCateByParentId(string parentId)
        {
            return _context.Categories.Where(e => e.ParentId == parentId).ToList();
        }
    }
}
