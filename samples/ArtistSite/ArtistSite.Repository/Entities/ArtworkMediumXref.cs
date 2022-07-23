using System;
using System.Collections.Generic;

namespace ArtistSite.Repository.Entities
{
    public partial class ArtworkMediumXref
    {
        public int ArtworkId { get; set; }
        public int MediumId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
