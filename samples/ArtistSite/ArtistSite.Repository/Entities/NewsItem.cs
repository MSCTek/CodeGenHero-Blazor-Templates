using System;
using System.Collections.Generic;

namespace ArtistSite.Repository.Entities
{
    public partial class NewsItem
    {
        public int NewsItemId { get; set; }
        public string Headline { get; set; }
        public string NewsCopy { get; set; }
    }
}
