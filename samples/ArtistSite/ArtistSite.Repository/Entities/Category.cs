using System;
using System.Collections.Generic;

namespace ArtistSite.Repository.Entities
{
    public partial class Category
    {
        public Category()
        {
            InverseParentCategory = new HashSet<Category>();
        }

        public int CategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string CssClass { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> InverseParentCategory { get; set; }
    }
}
