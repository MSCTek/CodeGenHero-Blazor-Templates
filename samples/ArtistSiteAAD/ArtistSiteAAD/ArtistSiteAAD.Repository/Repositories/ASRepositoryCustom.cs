using ArtistSiteAAD.Repository.Entities;
using ArtistSiteAAD.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistSiteAAD.Repository.Repositories
{
    public partial class ASRepository
    {
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
