using System;
using System.Collections.Generic;

namespace ArtistSiteAAD.Repository.Entities
{
    public partial class NewsItem
    {
        public int NewsItemId { get; set; }
        public string Headline { get; set; }
        public string NewsCopy { get; set; }
    }
}
