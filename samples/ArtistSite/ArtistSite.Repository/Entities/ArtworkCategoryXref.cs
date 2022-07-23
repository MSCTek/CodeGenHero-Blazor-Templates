using System;
using System.Collections.Generic;

namespace ArtistSite.Repository.Entities
{
    public partial class ArtworkCategoryXref
    {
        public int ArtworkId { get; set; }
        public int CategoryId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
