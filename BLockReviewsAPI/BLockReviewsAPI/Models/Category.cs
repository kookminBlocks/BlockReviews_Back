using System;
using System.Collections.Generic;

#nullable disable

namespace BLockReviewsAPI.Models
{
    public partial class Category
    {
        public Category()
        {
            Reviews = new HashSet<Review>();
            Stores = new HashSet<Store>();
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public DateTime? StDate { get; set; }
        public DateTime? FnsDate { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
}
