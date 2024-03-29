﻿using System.Collections.Generic;
using LambdaForums.Models.Post;

namespace LambdaForums.Models.Home
{
    public class HomeIndexModel
    {
        public string SearchQuery { get; set; }

        public IEnumerable<PostListingModel> LatestPosts { get; set; }

    }
}
