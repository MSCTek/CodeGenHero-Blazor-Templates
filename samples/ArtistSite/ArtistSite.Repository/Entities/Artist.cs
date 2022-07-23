using System;
using System.Collections.Generic;

namespace ArtistSite.Repository.Entities
{
    public partial class Artist
    {
        public Artist()
        {
            Artworks = new HashSet<Artwork>();
            Categories = new HashSet<Category>();
        }

        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string AboutBlurb { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string AppBarImgSrcSmall { get; set; }
        public string AppBarImgSrcLarge { get; set; }
        public string HeaderCssClass { get; set; }
        public string MainContentCssClass { get; set; }
        public string FooterCssClass { get; set; }

        public virtual ICollection<Artwork> Artworks { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
