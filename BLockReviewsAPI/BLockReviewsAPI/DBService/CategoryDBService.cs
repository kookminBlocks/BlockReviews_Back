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
        public List<Category> SelectCate(int level, string parentId);
    }

    public class CategoryDBService : ICategoryDBService
    {
        private BlockReviewContext _context;
        public CategoryDBService(BlockReviewContext context)
        {
            _context = context;
        }

        public List<Category> SelectCate(int level, string parentId)
        {
            if (level > 1)
            {
                return _context.Categories.Where(e => e.ParentId.Equals(parentId) && e.Level == level).ToList();
            }
            else
            {
                return _context.Categories.Where(e => e.Level == level).ToList();
            }
        }
    }
}
