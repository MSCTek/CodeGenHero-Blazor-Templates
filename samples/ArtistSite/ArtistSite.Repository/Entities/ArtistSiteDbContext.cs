using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ArtistSite.Repository.Entities
{
    public partial class ArtistSiteDbContext : DbContext
    {
        public ArtistSiteDbContext()
        {
        }

        public ArtistSiteDbContext(DbContextOptions<ArtistSiteDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Artwork> Artworks { get; set; }
        public virtual DbSet<ArtworkCategoryXref> ArtworkCategoryXrefs { get; set; }
        public virtual DbSet<ArtworkMediumXref> ArtworkMediumXrefs { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Medium> Media { get; set; }
        public virtual DbSet<NewsItem> NewsItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=ArtistSite;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "dbo");

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("Artist");

                entity.HasIndex(e => e.Code)
                    .HasName("UC_Artist")
                    .IsUnique();

                entity.Property(e => e.AboutBlurb).HasMaxLength(4000);

                entity.Property(e => e.AppBarImgSrcLarge).HasMaxLength(500);

                entity.Property(e => e.AppBarImgSrcSmall).HasMaxLength(500);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FooterCssClass).HasMaxLength(50);

                entity.Property(e => e.HeaderCssClass).HasMaxLength(50);

                entity.Property(e => e.MainContentCssClass).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Artwork>(entity =>
            {
                entity.ToTable("Artwork");

                entity.Property(e => e.CssClass).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.IconUri)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ImageUri)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LayoutPageName).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Artworks)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Artwork_Artist");
            });

            modelBuilder.Entity<ArtworkCategoryXref>(entity =>
            {
                entity.HasKey(e => new { e.ArtworkId, e.CategoryId })
                    .HasName("PK_ArtworkCategory");

                entity.ToTable("ArtworkCategory_Xref");

                entity.Property(e => e.DisplayOrder).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<ArtworkMediumXref>(entity =>
            {
                entity.HasKey(e => new { e.ArtworkId, e.MediumId })
                    .HasName("PK_ArtworkMedium");

                entity.ToTable("ArtworkMedium_Xref");

                entity.Property(e => e.DisplayOrder).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CssClass).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Category_Artist");

                entity.HasOne(d => d.ParentCategory)
                    .WithMany(p => p.InverseParentCategory)
                    .HasForeignKey(d => d.ParentCategoryId)
                    .HasConstraintName("FK_Category_Category");
            });

            modelBuilder.Entity<Medium>(entity =>
            {
                entity.ToTable("Medium");

                entity.Property(e => e.MediumId).HasColumnName("MediumID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.DisplayOrder).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<NewsItem>(entity =>
            {
                entity.ToTable("NewsItem");

                entity.Property(e => e.Headline).HasMaxLength(2000);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
