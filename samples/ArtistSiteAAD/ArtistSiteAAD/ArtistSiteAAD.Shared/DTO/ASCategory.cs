// <auto-generated> - Template:DTO, Version:2021.11.12, Id:c97fab8d-db03-4f94-9c85-14d1f9b41aa7
using System;
using System.Collections.Generic;

namespace ArtistSiteAAD.Shared.DTO
{
	public partial class Category
{
		public Category()
		{
				InitializePartial();
		}

		// Primary Key
		public int CategoryId { get; set; }

		public int ArtistId { get; set; }

		public string CssClass { get; set; }

		public string Description { get; set; }

		public int DisplayOrder { get; set; }

		public string Name { get; set; }

		public int? ParentCategoryId { get; set; }

		public virtual Artist Artist { get; set; } // One to One mapping

		public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>(); // Many to many mapping

		public virtual Category ParentCategory { get; set; } // One to One mapping

		partial void InitializePartial();
	}
}
