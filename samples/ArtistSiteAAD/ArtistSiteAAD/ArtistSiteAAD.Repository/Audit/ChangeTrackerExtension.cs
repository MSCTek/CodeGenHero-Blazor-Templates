using ArtistSiteAAD.Shared.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ArtistSiteAAD.Repository.Audit
{
    public static class ChangeTrackerExtensions
    {
        public static void SetAuditProperties(this ChangeTracker changeTracker, ICurrentUserService currentUserService)
        {
            changeTracker.DetectChanges();
            IEnumerable<EntityEntry> entities =
                changeTracker
                    .Entries()
                    .Where(t => t.Entity is IAuditEntity &&
                    (
                        t.State == EntityState.Deleted
                        || t.State == EntityState.Added
                        || t.State == EntityState.Modified
                    ));

            if (entities.Any())
            {
                DateTimeOffset timestamp = DateTimeOffset.UtcNow;

                string user = currentUserService.GetCurrentUser().LoginName;

                foreach (EntityEntry entry in entities)
                {
                    IAuditEntity entity = (IAuditEntity)entry.Entity;

                    /// Setup - Set these to match what your entity's audit properties are

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entity.CreatedDate = timestamp;
                            entity.CreatedBy = user;
                            entity.ModifiedDate = timestamp;
                            entity.ModifiedBy = user;
                            break;
                        case EntityState.Modified:
                            entity.ModifiedDate = timestamp;
                            entity.ModifiedBy = user;
                            break;
                            //case EntityState.Deleted:
                            //    entity.IsDeleted = true;
                            //    entry.State = EntityState.Modified;
                            //    break;
                    }
                }
            }
        }
    }
}