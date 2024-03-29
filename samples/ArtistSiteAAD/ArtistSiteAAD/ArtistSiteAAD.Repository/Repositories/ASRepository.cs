// <auto-generated> - Template:Repository, Version:2021.9.14, Id:f51d4b1e-d1f8-45c4-bc9c-9e50b9090fbe
using ArtistSiteAAD.Repository.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using cghEnums = ArtistSiteAAD.Repository.Infrastructure.Enums;
using ArtistSiteAAD.Repository.Entities;
using Enums = ArtistSiteAAD.Shared.Constants.Enums;
using System.Collections.Generic;

namespace ArtistSiteAAD.Repository.Repositories
{
	public partial class ASRepository : IASRepository
	{
		private ArtistSiteDbContext _ctx;
		public ASRepository(ArtistSiteDbContext ctx)
		{
				_ctx = ctx;
				ctx.ChangeTracker.LazyLoadingEnabled = false;
		}

		public ArtistSiteDbContext ArtistSiteDbContext { get { return _ctx; } }

		#region Generic Operations

                private async Task<IRepositoryActionResult<TEntity>> DeleteAsync<TEntity>(TEntity item) where TEntity : class
		        {
			        IRepositoryActionResult<TEntity> retVal = null;

			        try
			        {
				        if (item == null)
				        {
					        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.NotFound);
				        }
				        else
				        {
					        DbSet<TEntity> itemSet = _ctx.Set<TEntity>();
					        itemSet.Remove(item);
					        await _ctx.SaveChangesAsync();
					        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Deleted);
				        }
			        }
			        catch (Exception ex)
			        {
				        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Error, ex);
			        }

			        return retVal;
		        }
		public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class
		{
				return _ctx.Set<TEntity>();
		}


                public async Task<IRepositoryActionResult<TEntity>> InsertAsync<TEntity>(TEntity item)
			        where TEntity : class
		        {
			        IRepositoryActionResult<TEntity> retVal = null;

			        try
			        {
				        DbSet<TEntity> itemSet = _ctx.Set<TEntity>();
				        itemSet.Add(item);
				        var result = await _ctx.SaveChangesAsync();
				        RunCustomLogicAfterEveryInsert<TEntity>(item, result);

				        if (result > 0)
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.Created);
				        }
				        else
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.NothingModified, null);
				        }
			        }
			        catch (Exception ex)
			        {
				        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Error, ex);
			        }

			        return retVal;
		        }

                private async Task<IRepositoryActionResult<TEntity>> UpdateAsync<TEntity>(TEntity item, TEntity existingItem) where TEntity : class
		        {
			        IRepositoryActionResult<TEntity> retVal = null;

			        try
			        { // only update when a record already exists for this id
				        if (existingItem == null)
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.NotFound);
				        }

				        // change the original entity status to detached; otherwise, we get an error on attach as the entity is already in the dbSet
				        // set original entity state to detached
				        _ctx.Entry(existingItem).State = EntityState.Detached;
				        DbSet<TEntity> itemSet = _ctx.Set<TEntity>();
				        itemSet.Attach(item); // attach & save
				        _ctx.Entry(item).State = EntityState.Modified; // set the updated entity state to modified, so it gets updated.

				        var result = await _ctx.SaveChangesAsync();
				        RunCustomLogicAfterEveryUpdate<TEntity>(newItem: item, oldItem: existingItem, numObjectsWritten: result);

				        if (result > 0)
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.Updated);
				        }
				        else
				        {
					        retVal = new RepositoryActionResult<TEntity>(item, cghEnums.RepositoryActionStatus.NothingModified, null);
				        }
			        }
			        catch (Exception ex)
			        {
				        retVal = new RepositoryActionResult<TEntity>(null, cghEnums.RepositoryActionStatus.Error, ex);
			        }

			        return retVal;
		        }
		partial void RunCustomLogicAfterEveryInsert<T>(T item, int numObjectsWritten) where T : class;

		partial void RunCustomLogicAfterEveryUpdate<T>(T newItem, T oldItem, int numObjectsWritten) where T : class;

		#endregion

		#region Artist

		public async Task<IRepositoryActionResult<Artist>> InsertAsync(Artist item)
		{
				var result = await InsertAsync<Artist>(item);
				RunCustomLogicAfterInsert_Artist(item, result);

				return result;
		}

		public IQueryable<Artist> GetQueryable_Artist(Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var retVal = GetQueryable<Artist>();
			ApplyRelatedEntitiesType(ref retVal, relatedEntitiesType);

			return retVal;
		}

		public async Task<RepositoryPageDataResponse<IList<Artist>>> GetPageData_ArtistAsync(RepositoryPageDataRequest request)
		{
				var qry = GetQueryable_Artist(request.RelatedEntitiesType).AsNoTracking();
				var retVal = new RepositoryPageDataResponse<IList<Artist>>(request);

				IList<string> filterList = new List<string>(request.FilterList);
				RunCustomLogicAfterGetQueryableList_Artist(ref qry, ref filterList);
				qry = qry.ApplyFilter(filterList);
				qry = qry.ApplySort(request.Sort ?? (typeof(Artist).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
					.First().Name);

				retVal.TotalCount = qry.Count();
				retVal.Data = await qry.Skip(request.PageSize * (request.Page - 1))
					.Take(request.PageSize).ToListAsync();

				return retVal;
		}

		public async Task<Artist> Get_ArtistAsync(int artistId, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_Artist(relatedEntitiesType);
				RunCustomLogicOnGetQueryableByPK_Artist(ref qry, artistId, relatedEntitiesType);
				qry = qry.AsNoTracking();

				var dbItem = await qry
					.Where(x => x.ArtistId == artistId)
					.FirstOrDefaultAsync();
				if (!(dbItem is null))
				{
						RunCustomLogicOnGetEntityByPK_Artist(ref dbItem, artistId, relatedEntitiesType);
				}

			return dbItem;
		}

		public async Task<Artist> GetFirstOrDefaultAsync(
			Artist item, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_Artist(relatedEntitiesType)
					.Where(x => x.ArtistId == item.ArtistId);
				var retVal = await qry.FirstOrDefaultAsync();

			return retVal;
		}

		public async Task<IRepositoryActionResult<Artist>> UpdateAsync(Artist item)
		{
			var oldItem = await _ctx.Artists
				.FirstOrDefaultAsync(x => x.ArtistId == item.ArtistId);
			var result = await UpdateAsync<Artist>(item, oldItem);
			RunCustomLogicAfterUpdate_Artist(newItem: item, oldItem: oldItem, result: result);

			return result;
		}

		public async Task<IRepositoryActionResult<Artist>> Delete_ArtistAsync(int artistId)
		{
				return await DeleteAsync<Artist>(_ctx.Artists
					.Where(x => x.ArtistId == artistId)
					.FirstOrDefault());
		}

		public async Task<IRepositoryActionResult<Artist>> DeleteAsync(Artist item)
		{
				return await DeleteAsync<Artist>(_ctx.Artists
					.Where(x => x.ArtistId == item.ArtistId)
					.FirstOrDefault());
		}

		partial void RunCustomLogicAfterGetQueryableList_Artist(
			ref IQueryable<Artist> dbItems,
			ref IList<string> filterList);

		partial void RunCustomLogicAfterInsert_Artist(Artist item, IRepositoryActionResult<Artist> result);

		partial void RunCustomLogicAfterUpdate_Artist(Artist newItem, Artist oldItem, IRepositoryActionResult<Artist> result);

		partial void RunCustomLogicOnGetQueryableByPK_Artist(ref IQueryable<Artist> qryItem, int artistId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void RunCustomLogicOnGetEntityByPK_Artist(ref Artist dbItem, int artistId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void ApplyRelatedEntitiesType(
			ref IQueryable<Artist> qry, Enums.RelatedEntitiesType relatedEntitiesType);

		#endregion

		#region Artwork

		public async Task<IRepositoryActionResult<Artwork>> InsertAsync(Artwork item)
		{
				var result = await InsertAsync<Artwork>(item);
				RunCustomLogicAfterInsert_Artwork(item, result);

				return result;
		}

		public IQueryable<Artwork> GetQueryable_Artwork(Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var retVal = GetQueryable<Artwork>();
			ApplyRelatedEntitiesType(ref retVal, relatedEntitiesType);

			return retVal;
		}

		public async Task<RepositoryPageDataResponse<IList<Artwork>>> GetPageData_ArtworkAsync(RepositoryPageDataRequest request)
		{
				var qry = GetQueryable_Artwork(request.RelatedEntitiesType).AsNoTracking();
				var retVal = new RepositoryPageDataResponse<IList<Artwork>>(request);

				IList<string> filterList = new List<string>(request.FilterList);
				RunCustomLogicAfterGetQueryableList_Artwork(ref qry, ref filterList);
				qry = qry.ApplyFilter(filterList);
				qry = qry.ApplySort(request.Sort ?? (typeof(Artwork).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
					.First().Name);

				retVal.TotalCount = qry.Count();
				retVal.Data = await qry.Skip(request.PageSize * (request.Page - 1))
					.Take(request.PageSize).ToListAsync();

				return retVal;
		}

		public async Task<Artwork> Get_ArtworkAsync(int artworkId, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_Artwork(relatedEntitiesType);
				RunCustomLogicOnGetQueryableByPK_Artwork(ref qry, artworkId, relatedEntitiesType);
				qry = qry.AsNoTracking();

				var dbItem = await qry
					.Where(x => x.ArtworkId == artworkId)
					.FirstOrDefaultAsync();
				if (!(dbItem is null))
				{
						RunCustomLogicOnGetEntityByPK_Artwork(ref dbItem, artworkId, relatedEntitiesType);
				}

			return dbItem;
		}

		public async Task<Artwork> GetFirstOrDefaultAsync(
			Artwork item, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_Artwork(relatedEntitiesType)
					.Where(x => x.ArtworkId == item.ArtworkId);
				var retVal = await qry.FirstOrDefaultAsync();

			return retVal;
		}

		public async Task<IRepositoryActionResult<Artwork>> UpdateAsync(Artwork item)
		{
			var oldItem = await _ctx.Artworks
				.FirstOrDefaultAsync(x => x.ArtworkId == item.ArtworkId);
			var result = await UpdateAsync<Artwork>(item, oldItem);
			RunCustomLogicAfterUpdate_Artwork(newItem: item, oldItem: oldItem, result: result);

			return result;
		}

		public async Task<IRepositoryActionResult<Artwork>> Delete_ArtworkAsync(int artworkId)
		{
				return await DeleteAsync<Artwork>(_ctx.Artworks
					.Where(x => x.ArtworkId == artworkId)
					.FirstOrDefault());
		}

		public async Task<IRepositoryActionResult<Artwork>> DeleteAsync(Artwork item)
		{
				return await DeleteAsync<Artwork>(_ctx.Artworks
					.Where(x => x.ArtworkId == item.ArtworkId)
					.FirstOrDefault());
		}

		partial void RunCustomLogicAfterGetQueryableList_Artwork(
			ref IQueryable<Artwork> dbItems,
			ref IList<string> filterList);

		partial void RunCustomLogicAfterInsert_Artwork(Artwork item, IRepositoryActionResult<Artwork> result);

		partial void RunCustomLogicAfterUpdate_Artwork(Artwork newItem, Artwork oldItem, IRepositoryActionResult<Artwork> result);

		partial void RunCustomLogicOnGetQueryableByPK_Artwork(ref IQueryable<Artwork> qryItem, int artworkId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void RunCustomLogicOnGetEntityByPK_Artwork(ref Artwork dbItem, int artworkId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void ApplyRelatedEntitiesType(
			ref IQueryable<Artwork> qry, Enums.RelatedEntitiesType relatedEntitiesType);

		#endregion

		#region ArtworkCategoryXref

		public async Task<IRepositoryActionResult<ArtworkCategoryXref>> InsertAsync(ArtworkCategoryXref item)
		{
				var result = await InsertAsync<ArtworkCategoryXref>(item);
				RunCustomLogicAfterInsert_ArtworkCategoryXref(item, result);

				return result;
		}

		public IQueryable<ArtworkCategoryXref> GetQueryable_ArtworkCategoryXref(Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var retVal = GetQueryable<ArtworkCategoryXref>();
			ApplyRelatedEntitiesType(ref retVal, relatedEntitiesType);

			return retVal;
		}

		public async Task<RepositoryPageDataResponse<IList<ArtworkCategoryXref>>> GetPageData_ArtworkCategoryXrefAsync(RepositoryPageDataRequest request)
		{
				var qry = GetQueryable_ArtworkCategoryXref(request.RelatedEntitiesType).AsNoTracking();
				var retVal = new RepositoryPageDataResponse<IList<ArtworkCategoryXref>>(request);

				IList<string> filterList = new List<string>(request.FilterList);
				RunCustomLogicAfterGetQueryableList_ArtworkCategoryXref(ref qry, ref filterList);
				qry = qry.ApplyFilter(filterList);
				qry = qry.ApplySort(request.Sort ?? (typeof(ArtworkCategoryXref).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
					.First().Name);

				retVal.TotalCount = qry.Count();
				retVal.Data = await qry.Skip(request.PageSize * (request.Page - 1))
					.Take(request.PageSize).ToListAsync();

				return retVal;
		}

		public async Task<ArtworkCategoryXref> Get_ArtworkCategoryXrefAsync(int artworkId, int categoryId, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_ArtworkCategoryXref(relatedEntitiesType);
				RunCustomLogicOnGetQueryableByPK_ArtworkCategoryXref(ref qry, artworkId, categoryId, relatedEntitiesType);
				qry = qry.AsNoTracking();

				var dbItem = await qry
					.Where(x => x.ArtworkId == artworkId && x.CategoryId == categoryId)
					.FirstOrDefaultAsync();
				if (!(dbItem is null))
				{
						RunCustomLogicOnGetEntityByPK_ArtworkCategoryXref(ref dbItem, artworkId, categoryId, relatedEntitiesType);
				}

			return dbItem;
		}

		public async Task<ArtworkCategoryXref> GetFirstOrDefaultAsync(
			ArtworkCategoryXref item, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_ArtworkCategoryXref(relatedEntitiesType)
					.Where(x => x.ArtworkId == item.ArtworkId && x.CategoryId == item.CategoryId);
				var retVal = await qry.FirstOrDefaultAsync();

			return retVal;
		}

		public async Task<IRepositoryActionResult<ArtworkCategoryXref>> UpdateAsync(ArtworkCategoryXref item)
		{
			var oldItem = await _ctx.ArtworkCategoryXrefs
				.FirstOrDefaultAsync(x => x.ArtworkId == item.ArtworkId && x.CategoryId == item.CategoryId);
			var result = await UpdateAsync<ArtworkCategoryXref>(item, oldItem);
			RunCustomLogicAfterUpdate_ArtworkCategoryXref(newItem: item, oldItem: oldItem, result: result);

			return result;
		}

		public async Task<IRepositoryActionResult<ArtworkCategoryXref>> Delete_ArtworkCategoryXrefAsync(int artworkId, int categoryId)
		{
				return await DeleteAsync<ArtworkCategoryXref>(_ctx.ArtworkCategoryXrefs
					.Where(x => x.ArtworkId == artworkId && x.CategoryId == categoryId)
					.FirstOrDefault());
		}

		public async Task<IRepositoryActionResult<ArtworkCategoryXref>> DeleteAsync(ArtworkCategoryXref item)
		{
				return await DeleteAsync<ArtworkCategoryXref>(_ctx.ArtworkCategoryXrefs
					.Where(x => x.ArtworkId == item.ArtworkId && x.CategoryId == item.CategoryId)
					.FirstOrDefault());
		}

		partial void RunCustomLogicAfterGetQueryableList_ArtworkCategoryXref(
			ref IQueryable<ArtworkCategoryXref> dbItems,
			ref IList<string> filterList);

		partial void RunCustomLogicAfterInsert_ArtworkCategoryXref(ArtworkCategoryXref item, IRepositoryActionResult<ArtworkCategoryXref> result);

		partial void RunCustomLogicAfterUpdate_ArtworkCategoryXref(ArtworkCategoryXref newItem, ArtworkCategoryXref oldItem, IRepositoryActionResult<ArtworkCategoryXref> result);

		partial void RunCustomLogicOnGetQueryableByPK_ArtworkCategoryXref(ref IQueryable<ArtworkCategoryXref> qryItem, int artworkId, int categoryId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void RunCustomLogicOnGetEntityByPK_ArtworkCategoryXref(ref ArtworkCategoryXref dbItem, int artworkId, int categoryId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void ApplyRelatedEntitiesType(
			ref IQueryable<ArtworkCategoryXref> qry, Enums.RelatedEntitiesType relatedEntitiesType);

		#endregion

		#region ArtworkMediumXref

		public async Task<IRepositoryActionResult<ArtworkMediumXref>> InsertAsync(ArtworkMediumXref item)
		{
				var result = await InsertAsync<ArtworkMediumXref>(item);
				RunCustomLogicAfterInsert_ArtworkMediumXref(item, result);

				return result;
		}

		public IQueryable<ArtworkMediumXref> GetQueryable_ArtworkMediumXref(Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var retVal = GetQueryable<ArtworkMediumXref>();
			ApplyRelatedEntitiesType(ref retVal, relatedEntitiesType);

			return retVal;
		}

		public async Task<RepositoryPageDataResponse<IList<ArtworkMediumXref>>> GetPageData_ArtworkMediumXrefAsync(RepositoryPageDataRequest request)
		{
				var qry = GetQueryable_ArtworkMediumXref(request.RelatedEntitiesType).AsNoTracking();
				var retVal = new RepositoryPageDataResponse<IList<ArtworkMediumXref>>(request);

				IList<string> filterList = new List<string>(request.FilterList);
				RunCustomLogicAfterGetQueryableList_ArtworkMediumXref(ref qry, ref filterList);
				qry = qry.ApplyFilter(filterList);
				qry = qry.ApplySort(request.Sort ?? (typeof(ArtworkMediumXref).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
					.First().Name);

				retVal.TotalCount = qry.Count();
				retVal.Data = await qry.Skip(request.PageSize * (request.Page - 1))
					.Take(request.PageSize).ToListAsync();

				return retVal;
		}

		public async Task<ArtworkMediumXref> Get_ArtworkMediumXrefAsync(int artworkId, int mediumId, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_ArtworkMediumXref(relatedEntitiesType);
				RunCustomLogicOnGetQueryableByPK_ArtworkMediumXref(ref qry, artworkId, mediumId, relatedEntitiesType);
				qry = qry.AsNoTracking();

				var dbItem = await qry
					.Where(x => x.ArtworkId == artworkId && x.MediumId == mediumId)
					.FirstOrDefaultAsync();
				if (!(dbItem is null))
				{
						RunCustomLogicOnGetEntityByPK_ArtworkMediumXref(ref dbItem, artworkId, mediumId, relatedEntitiesType);
				}

			return dbItem;
		}

		public async Task<ArtworkMediumXref> GetFirstOrDefaultAsync(
			ArtworkMediumXref item, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_ArtworkMediumXref(relatedEntitiesType)
					.Where(x => x.ArtworkId == item.ArtworkId && x.MediumId == item.MediumId);
				var retVal = await qry.FirstOrDefaultAsync();

			return retVal;
		}

		public async Task<IRepositoryActionResult<ArtworkMediumXref>> UpdateAsync(ArtworkMediumXref item)
		{
			var oldItem = await _ctx.ArtworkMediumXrefs
				.FirstOrDefaultAsync(x => x.ArtworkId == item.ArtworkId && x.MediumId == item.MediumId);
			var result = await UpdateAsync<ArtworkMediumXref>(item, oldItem);
			RunCustomLogicAfterUpdate_ArtworkMediumXref(newItem: item, oldItem: oldItem, result: result);

			return result;
		}

		public async Task<IRepositoryActionResult<ArtworkMediumXref>> Delete_ArtworkMediumXrefAsync(int artworkId, int mediumId)
		{
				return await DeleteAsync<ArtworkMediumXref>(_ctx.ArtworkMediumXrefs
					.Where(x => x.ArtworkId == artworkId && x.MediumId == mediumId)
					.FirstOrDefault());
		}

		public async Task<IRepositoryActionResult<ArtworkMediumXref>> DeleteAsync(ArtworkMediumXref item)
		{
				return await DeleteAsync<ArtworkMediumXref>(_ctx.ArtworkMediumXrefs
					.Where(x => x.ArtworkId == item.ArtworkId && x.MediumId == item.MediumId)
					.FirstOrDefault());
		}

		partial void RunCustomLogicAfterGetQueryableList_ArtworkMediumXref(
			ref IQueryable<ArtworkMediumXref> dbItems,
			ref IList<string> filterList);

		partial void RunCustomLogicAfterInsert_ArtworkMediumXref(ArtworkMediumXref item, IRepositoryActionResult<ArtworkMediumXref> result);

		partial void RunCustomLogicAfterUpdate_ArtworkMediumXref(ArtworkMediumXref newItem, ArtworkMediumXref oldItem, IRepositoryActionResult<ArtworkMediumXref> result);

		partial void RunCustomLogicOnGetQueryableByPK_ArtworkMediumXref(ref IQueryable<ArtworkMediumXref> qryItem, int artworkId, int mediumId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void RunCustomLogicOnGetEntityByPK_ArtworkMediumXref(ref ArtworkMediumXref dbItem, int artworkId, int mediumId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void ApplyRelatedEntitiesType(
			ref IQueryable<ArtworkMediumXref> qry, Enums.RelatedEntitiesType relatedEntitiesType);

		#endregion

		#region Category

		public async Task<IRepositoryActionResult<Category>> InsertAsync(Category item)
		{
				var result = await InsertAsync<Category>(item);
				RunCustomLogicAfterInsert_Category(item, result);

				return result;
		}

		public IQueryable<Category> GetQueryable_Category(Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var retVal = GetQueryable<Category>();
			ApplyRelatedEntitiesType(ref retVal, relatedEntitiesType);

			return retVal;
		}

		public async Task<RepositoryPageDataResponse<IList<Category>>> GetPageData_CategoryAsync(RepositoryPageDataRequest request)
		{
				var qry = GetQueryable_Category(request.RelatedEntitiesType).AsNoTracking();
				var retVal = new RepositoryPageDataResponse<IList<Category>>(request);

				IList<string> filterList = new List<string>(request.FilterList);
				RunCustomLogicAfterGetQueryableList_Category(ref qry, ref filterList);
				qry = qry.ApplyFilter(filterList);
				qry = qry.ApplySort(request.Sort ?? (typeof(Category).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
					.First().Name);

				retVal.TotalCount = qry.Count();
				retVal.Data = await qry.Skip(request.PageSize * (request.Page - 1))
					.Take(request.PageSize).ToListAsync();

				return retVal;
		}

		public async Task<Category> Get_CategoryAsync(int categoryId, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_Category(relatedEntitiesType);
				RunCustomLogicOnGetQueryableByPK_Category(ref qry, categoryId, relatedEntitiesType);
				qry = qry.AsNoTracking();

				var dbItem = await qry
					.Where(x => x.CategoryId == categoryId)
					.FirstOrDefaultAsync();
				if (!(dbItem is null))
				{
						RunCustomLogicOnGetEntityByPK_Category(ref dbItem, categoryId, relatedEntitiesType);
				}

			return dbItem;
		}

		public async Task<Category> GetFirstOrDefaultAsync(
			Category item, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_Category(relatedEntitiesType)
					.Where(x => x.CategoryId == item.CategoryId);
				var retVal = await qry.FirstOrDefaultAsync();

			return retVal;
		}

		public async Task<IRepositoryActionResult<Category>> UpdateAsync(Category item)
		{
			var oldItem = await _ctx.Categories
				.FirstOrDefaultAsync(x => x.CategoryId == item.CategoryId);
			var result = await UpdateAsync<Category>(item, oldItem);
			RunCustomLogicAfterUpdate_Category(newItem: item, oldItem: oldItem, result: result);

			return result;
		}

		public async Task<IRepositoryActionResult<Category>> Delete_CategoryAsync(int categoryId)
		{
				return await DeleteAsync<Category>(_ctx.Categories
					.Where(x => x.CategoryId == categoryId)
					.FirstOrDefault());
		}

		public async Task<IRepositoryActionResult<Category>> DeleteAsync(Category item)
		{
				return await DeleteAsync<Category>(_ctx.Categories
					.Where(x => x.CategoryId == item.CategoryId)
					.FirstOrDefault());
		}

		partial void RunCustomLogicAfterGetQueryableList_Category(
			ref IQueryable<Category> dbItems,
			ref IList<string> filterList);

		partial void RunCustomLogicAfterInsert_Category(Category item, IRepositoryActionResult<Category> result);

		partial void RunCustomLogicAfterUpdate_Category(Category newItem, Category oldItem, IRepositoryActionResult<Category> result);

		partial void RunCustomLogicOnGetQueryableByPK_Category(ref IQueryable<Category> qryItem, int categoryId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void RunCustomLogicOnGetEntityByPK_Category(ref Category dbItem, int categoryId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void ApplyRelatedEntitiesType(
			ref IQueryable<Category> qry, Enums.RelatedEntitiesType relatedEntitiesType);

		#endregion

		#region Medium

		public async Task<IRepositoryActionResult<Medium>> InsertAsync(Medium item)
		{
				var result = await InsertAsync<Medium>(item);
				RunCustomLogicAfterInsert_Medium(item, result);

				return result;
		}

		public IQueryable<Medium> GetQueryable_Medium(Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var retVal = GetQueryable<Medium>();
			ApplyRelatedEntitiesType(ref retVal, relatedEntitiesType);

			return retVal;
		}

		public async Task<RepositoryPageDataResponse<IList<Medium>>> GetPageData_MediumAsync(RepositoryPageDataRequest request)
		{
				var qry = GetQueryable_Medium(request.RelatedEntitiesType).AsNoTracking();
				var retVal = new RepositoryPageDataResponse<IList<Medium>>(request);

				IList<string> filterList = new List<string>(request.FilterList);
				RunCustomLogicAfterGetQueryableList_Medium(ref qry, ref filterList);
				qry = qry.ApplyFilter(filterList);
				qry = qry.ApplySort(request.Sort ?? (typeof(Medium).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
					.First().Name);

				retVal.TotalCount = qry.Count();
				retVal.Data = await qry.Skip(request.PageSize * (request.Page - 1))
					.Take(request.PageSize).ToListAsync();

				return retVal;
		}

		public async Task<Medium> Get_MediumAsync(int mediumId, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_Medium(relatedEntitiesType);
				RunCustomLogicOnGetQueryableByPK_Medium(ref qry, mediumId, relatedEntitiesType);
				qry = qry.AsNoTracking();

				var dbItem = await qry
					.Where(x => x.MediumId == mediumId)
					.FirstOrDefaultAsync();
				if (!(dbItem is null))
				{
						RunCustomLogicOnGetEntityByPK_Medium(ref dbItem, mediumId, relatedEntitiesType);
				}

			return dbItem;
		}

		public async Task<Medium> GetFirstOrDefaultAsync(
			Medium item, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_Medium(relatedEntitiesType)
					.Where(x => x.MediumId == item.MediumId);
				var retVal = await qry.FirstOrDefaultAsync();

			return retVal;
		}

		public async Task<IRepositoryActionResult<Medium>> UpdateAsync(Medium item)
		{
			var oldItem = await _ctx.Media
				.FirstOrDefaultAsync(x => x.MediumId == item.MediumId);
			var result = await UpdateAsync<Medium>(item, oldItem);
			RunCustomLogicAfterUpdate_Medium(newItem: item, oldItem: oldItem, result: result);

			return result;
		}

		public async Task<IRepositoryActionResult<Medium>> Delete_MediumAsync(int mediumId)
		{
				return await DeleteAsync<Medium>(_ctx.Media
					.Where(x => x.MediumId == mediumId)
					.FirstOrDefault());
		}

		public async Task<IRepositoryActionResult<Medium>> DeleteAsync(Medium item)
		{
				return await DeleteAsync<Medium>(_ctx.Media
					.Where(x => x.MediumId == item.MediumId)
					.FirstOrDefault());
		}

		partial void RunCustomLogicAfterGetQueryableList_Medium(
			ref IQueryable<Medium> dbItems,
			ref IList<string> filterList);

		partial void RunCustomLogicAfterInsert_Medium(Medium item, IRepositoryActionResult<Medium> result);

		partial void RunCustomLogicAfterUpdate_Medium(Medium newItem, Medium oldItem, IRepositoryActionResult<Medium> result);

		partial void RunCustomLogicOnGetQueryableByPK_Medium(ref IQueryable<Medium> qryItem, int mediumId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void RunCustomLogicOnGetEntityByPK_Medium(ref Medium dbItem, int mediumId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void ApplyRelatedEntitiesType(
			ref IQueryable<Medium> qry, Enums.RelatedEntitiesType relatedEntitiesType);

		#endregion

		#region NewsItem

		public async Task<IRepositoryActionResult<NewsItem>> InsertAsync(NewsItem item)
		{
				var result = await InsertAsync<NewsItem>(item);
				RunCustomLogicAfterInsert_NewsItem(item, result);

				return result;
		}

		public IQueryable<NewsItem> GetQueryable_NewsItem(Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var retVal = GetQueryable<NewsItem>();
			ApplyRelatedEntitiesType(ref retVal, relatedEntitiesType);

			return retVal;
		}

		public async Task<RepositoryPageDataResponse<IList<NewsItem>>> GetPageData_NewsItemAsync(RepositoryPageDataRequest request)
		{
				var qry = GetQueryable_NewsItem(request.RelatedEntitiesType).AsNoTracking();
				var retVal = new RepositoryPageDataResponse<IList<NewsItem>>(request);

				IList<string> filterList = new List<string>(request.FilterList);
				RunCustomLogicAfterGetQueryableList_NewsItem(ref qry, ref filterList);
				qry = qry.ApplyFilter(filterList);
				qry = qry.ApplySort(request.Sort ?? (typeof(NewsItem).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
					.First().Name);

				retVal.TotalCount = qry.Count();
				retVal.Data = await qry.Skip(request.PageSize * (request.Page - 1))
					.Take(request.PageSize).ToListAsync();

				return retVal;
		}

		public async Task<NewsItem> Get_NewsItemAsync(int newsItemId, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_NewsItem(relatedEntitiesType);
				RunCustomLogicOnGetQueryableByPK_NewsItem(ref qry, newsItemId, relatedEntitiesType);
				qry = qry.AsNoTracking();

				var dbItem = await qry
					.Where(x => x.NewsItemId == newsItemId)
					.FirstOrDefaultAsync();
				if (!(dbItem is null))
				{
						RunCustomLogicOnGetEntityByPK_NewsItem(ref dbItem, newsItemId, relatedEntitiesType);
				}

			return dbItem;
		}

		public async Task<NewsItem> GetFirstOrDefaultAsync(
			NewsItem item, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				var qry = GetQueryable_NewsItem(relatedEntitiesType)
					.Where(x => x.NewsItemId == item.NewsItemId);
				var retVal = await qry.FirstOrDefaultAsync();

			return retVal;
		}

		public async Task<IRepositoryActionResult<NewsItem>> UpdateAsync(NewsItem item)
		{
			var oldItem = await _ctx.NewsItems
				.FirstOrDefaultAsync(x => x.NewsItemId == item.NewsItemId);
			var result = await UpdateAsync<NewsItem>(item, oldItem);
			RunCustomLogicAfterUpdate_NewsItem(newItem: item, oldItem: oldItem, result: result);

			return result;
		}

		public async Task<IRepositoryActionResult<NewsItem>> Delete_NewsItemAsync(int newsItemId)
		{
				return await DeleteAsync<NewsItem>(_ctx.NewsItems
					.Where(x => x.NewsItemId == newsItemId)
					.FirstOrDefault());
		}

		public async Task<IRepositoryActionResult<NewsItem>> DeleteAsync(NewsItem item)
		{
				return await DeleteAsync<NewsItem>(_ctx.NewsItems
					.Where(x => x.NewsItemId == item.NewsItemId)
					.FirstOrDefault());
		}

		partial void RunCustomLogicAfterGetQueryableList_NewsItem(
			ref IQueryable<NewsItem> dbItems,
			ref IList<string> filterList);

		partial void RunCustomLogicAfterInsert_NewsItem(NewsItem item, IRepositoryActionResult<NewsItem> result);

		partial void RunCustomLogicAfterUpdate_NewsItem(NewsItem newItem, NewsItem oldItem, IRepositoryActionResult<NewsItem> result);

		partial void RunCustomLogicOnGetQueryableByPK_NewsItem(ref IQueryable<NewsItem> qryItem, int newsItemId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void RunCustomLogicOnGetEntityByPK_NewsItem(ref NewsItem dbItem, int newsItemId, Enums.RelatedEntitiesType relatedEntitiesType);

		partial void ApplyRelatedEntitiesType(
			ref IQueryable<NewsItem> qry, Enums.RelatedEntitiesType relatedEntitiesType);

		#endregion

	}
}
