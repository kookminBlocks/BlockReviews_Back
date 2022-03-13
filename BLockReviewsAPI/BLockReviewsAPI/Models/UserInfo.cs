using System;
using System.Collections.Generic;

#nullable disable

namespace BLockReviewsAPI.Models
{
    public partial class UserInfo
    {
        public UserInfo()
        {
            Reviews = new HashSet<Review>();
            Stores = new HashSet<Store>();
        }

        public string Id { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? UserType { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public DateTime? StDate { get; set; }
        public DateTime? FnsDate { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
}
