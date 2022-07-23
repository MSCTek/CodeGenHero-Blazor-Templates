// <auto-generated> - Template:APIControllerCustom, Version:2022.06.09, Id:44d5b085-471a-4f79-9440-4254c967f282
using ArtistSite.Repository.Infrastructure;
using ArtistSite.Repository.Repositories;
using entAS = ArtistSite.Repository.Entities;
using Enums = ArtistSite.Shared.Constants.Enums;

namespace ArtistSite.Api.Controllers
{
	public partial class ArtworkCategoryXrefController
	{
		protected IASRepository ArtworkCategoryXrefRepository { get; set; }
		protected override void RunCustomLogicAfterCtor()
		{
			ArtworkCategoryXrefRepository = ServiceProvider.GetService<IASRepository>();
		}

		partial void RunCustomLogicAfterInsert(ref entAS.ArtworkCategoryXref newDBItem, ref IRepositoryActionResult<entAS.ArtworkCategoryXref> result)
		{

		}

		partial void RunCustomLogicAfterUpdatePatch(ref entAS.ArtworkCategoryXref updatedDBItem, ref IRepositoryActionResult<entAS.ArtworkCategoryXref> result)
		{

		}

		partial void RunCustomLogicAfterUpdatePut(ref entAS.ArtworkCategoryXref updatedDBItem, ref IRepositoryActionResult<entAS.ArtworkCategoryXref> result)
		{

		}

		partial void RunCustomLogicBeforeUpdatePut(ref entAS.ArtworkCategoryXref updatedDBItem, int artworkId, int categoryId)
		{

		}

		partial void RunCustomLogicOnGetEntityByPK(ref entAS.ArtworkCategoryXref dbItem, int artworkId, int categoryId, Enums.RelatedEntitiesType relatedEntitiesType)
		{

		}

	}
}