﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLockReviewsAPI.Models
{
    public class BlockReviewAccount
    {
        public payloads payload { get; set; }
        public class payloads
        {
            public string address { get; set; }
            public string privatekey { get; set; }
        }
    }

    public class BlockApproveAccount
    {
        public string pubkey { get; set; }
        public string privatekey { get; set; }
    }

    public class ReviewRequest
    {
        public string title { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string pubkey { get; set; }
        public string privatekey { get; set; }
        public string admin { get; set; }
        public string nftUri { get; set; }
        public int amount { get; set; }
    }

    public class ReviewLikeReq
    {
        public int reviewId { get; set; }
        public string pubkey { get; set; }
        public string privatekey { get; set; }
        public string admin { get; set; }
        public int amount { get; set; }
    }

    public class ReviewSaleReq
    {
        public int reviewId { get; set; }
        public string pubkey { get; set; }
        public string privatekey { get; set; }
        public int price { get; set; }
    }

    public class TradeReq
    {
        public int reviewId { get; set; }
        public string pubKey_buyer { get; set; }
        public string privateKey_buyer { get; set; }
    }

    public class ReviewRes
    {
        public payloads payload { get; set; }
        public class payloads
        {
            public string id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string owner { get; set; }
            public string[] liked { get; set; }
            public string nftId { get; set; }
            public string price { get; set; }
            public string category { get; set; }
            public string createAt { get; set; }
        }
    }
}
