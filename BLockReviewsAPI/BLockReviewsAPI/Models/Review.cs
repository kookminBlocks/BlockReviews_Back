using System;
using System.Collections.Generic;

#nullable disable

namespace BLockReviewsAPI.Models
{
    public partial class Review
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CategoryId { get; set; }
        public string StoreId { get; set; }
        public string Title { get; set; }
        public string ThumbNail { get; set; }
        public string Content { get; set; }
        public string NftUrl { get; set; }
        public DateTime? StDate { get; set; }
        public DateTime? FnsDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual Store Store { get; set; }
        public virtual UserInfo User { get; set; }
    }
}
