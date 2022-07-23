using System;
using System.Collections.Generic;

namespace ArtistSite.Repository.Entities
{
    public partial class Medium
    {
        public int MediumId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }
}
