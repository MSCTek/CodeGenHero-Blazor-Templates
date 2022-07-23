using ArtistSite.Repository.Entities;
using ArtistSite.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistSite.Repository.Repositories
{
    public partial class ASRepository
    {
        partial void ApplyRelatedEntitiesType(
            ref IQueryable<Artist> qry, Enums.RelatedEntitiesType relatedEntitiesType)
        {
            switch (relatedEntitiesType)
            {
                case Enums.RelatedEntitiesType.ImmediateChildren:
                    qry = qry.Include(x => x.Artworks).AsQueryable();
                    break;

                default:

                    break;
            }
        }

        // Example P7E2 - Setting up RelatedEntitiesType handling logic for Artwork, to retrieve Artist alongside the requested Artwork.
        partial void ApplyRelatedEntitiesType(
            ref IQueryable<Artwork> qry, Enums.RelatedEntitiesType relatedEntitiesType)
        {
            switch (relatedEntitiesType)
            {
                case Enums.RelatedEntitiesType.ImmediateChildren:
                    qry = qry.Include(x => x.Artist).AsQueryable();
                    break;

                default:

                    break;
            }
        }
    }
}
