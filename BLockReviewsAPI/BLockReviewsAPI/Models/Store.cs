using System;
using System.Collections.Generic;

#nullable disable

namespace BLockReviewsAPI.Models
{
    public partial class Store
    {
        public Store()
        {
            Reviews = new HashSet<Review>();
        }

        public string Id { get; set; }
        public string UserId { get; set; }
        public string CategoryId { get; set; }
        public string ThumbNail { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string BuisnessNumber { get; set; }
        public DateTime? StDate { get; set; }
        public DateTime? FnsDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual UserInfo User { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
