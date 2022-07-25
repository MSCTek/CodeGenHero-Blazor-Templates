// <auto-generated> - Template:RepositoryInterface, Version:2021.9.14, Id:ff160397-8584-4518-8ec7-a9549b37515a
using ArtistSite.Repository.Entities;
using ArtistSite.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using entAS = ArtistSite.Repository.Entities;
using Enums = ArtistSite.Shared.Constants.Enums;

namespace ArtistSite.Repository.Repositories
{
	public partial interface IASRepository : IASRepositoryCrud
{
		entAS.ArtistSiteDbContext ArtistSiteDbContext { get; }

// Artist
		Task<IRepositoryActionResult<entAS.Artist>> Delete_ArtistAsync(int artistId);

		Task<entAS.Artist> Get_ArtistAsync(int artistId, Enums.RelatedEntitiesType relatedEntitiesType = Enums.RelatedEntitiesType.None);

		Task<RepositoryPageDataResponse<IList<entAS.Artist>>> GetPageData_ArtistAsync(RepositoryPageDataRequest request);

// Artwork
		Task<IRepositoryActionResult<entAS.Artwork>> Delete_ArtworkAsync(int artworkId);

		Task<entAS.Artwork> Get_ArtworkAsync(int artworkId, Enums.RelatedEntitiesType relatedEntitiesType = Enums.RelatedEntitiesType.None);

		Task<RepositoryPageDataResponse<IList<entAS.Artwork>>> GetPageData_ArtworkAsync(RepositoryPageDataRequest request);

// ArtworkCategoryXref
		Task<IRepositoryActionResult<entAS.ArtworkCategoryXref>> Delete_ArtworkCategoryXrefAsync(int artworkId, int categoryId);

		Task<entAS.ArtworkCategoryXref> Get_ArtworkCategoryXrefAsync(int artworkId, int categoryId, Enums.RelatedEntitiesType relatedEntitiesType = Enums.RelatedEntitiesType.None);

		Task<RepositoryPageDataResponse<IList<entAS.ArtworkCategoryXref>>> GetPageData_ArtworkCategoryXrefAsync(RepositoryPageDataRequest request);

// ArtworkMediumXref
		Task<IRepositoryActionResult<entAS.ArtworkMediumXref>> Delete_ArtworkMediumXrefAsync(int artworkId, int mediumId);

		Task<entAS.ArtworkMediumXref> Get_ArtworkMediumXrefAsync(int artworkId, int mediumId, Enums.RelatedEntitiesType relatedEntitiesType = Enums.RelatedEntitiesType.None);

		Task<RepositoryPageDataResponse<IList<entAS.ArtworkMediumXref>>> GetPageData_ArtworkMediumXrefAsync(RepositoryPageDataRequest request);

// Category
		Task<IRepositoryActionResult<entAS.Category>> Delete_CategoryAsync(int categoryId);

		Task<entAS.Category> Get_CategoryAsync(int categoryId, Enums.RelatedEntitiesType relatedEntitiesType = Enums.RelatedEntitiesType.None);

		Task<RepositoryPageDataResponse<IList<entAS.Category>>> GetPageData_CategoryAsync(RepositoryPageDataRequest request);

// Medium
		Task<IRepositoryActionResult<entAS.Medium>> Delete_MediumAsync(int mediumId);

		Task<entAS.Medium> Get_MediumAsync(int mediumId, Enums.RelatedEntitiesType relatedEntitiesType = Enums.RelatedEntitiesType.None);

		Task<RepositoryPageDataResponse<IList<entAS.Medium>>> GetPageData_MediumAsync(RepositoryPageDataRequest request);

// NewsItem
		Task<IRepositoryActionResult<entAS.NewsItem>> Delete_NewsItemAsync(int newsItemId);

		Task<entAS.NewsItem> Get_NewsItemAsync(int newsItemId, Enums.RelatedEntitiesType relatedEntitiesType = Enums.RelatedEntitiesType.None);

		Task<RepositoryPageDataResponse<IList<entAS.NewsItem>>> GetPageData_NewsItemAsync(RepositoryPageDataRequest request);

	}
}
