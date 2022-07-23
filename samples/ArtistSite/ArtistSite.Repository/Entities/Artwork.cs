using System;
using System.Collections.Generic;

namespace ArtistSite.Repository.Entities
{
    public partial class Artwork
    {
        public int ArtworkId { get; set; }
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public string LayoutPageName { get; set; }
        public string CssClass { get; set; }
        public string IconUri { get; set; }
        public string ImageUri { get; set; }
        public bool IsActive { get; set; }

        public virtual Artist Artist { get; set; }
    }
}
