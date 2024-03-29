// <auto-generated> - Template:APIController, Version:2021.9.14, Id:70a21a48-7ee1-42f5-b1eb-4891e290a17d
using Marvin.JsonPatch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using ArtistSiteAAD.Api.Infrastructure;
using ArtistSiteAAD.Repository.Infrastructure;
using ArtistSiteAAD.Repository.Mappers;
using ArtistSiteAAD.Repository.Repositories;
using ArtistSiteAAD.Shared.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ArtistSiteAAD.Repository.Infrastructure.Enums;
using dtoAS = ArtistSiteAAD.Shared.DTO;
using entAS = ArtistSiteAAD.Repository.Entities;
using Enums = ArtistSiteAAD.Shared.Constants.Enums;

namespace ArtistSiteAAD.Api.Controllers
{
	public partial class ArtworkMediumXrefController : ASBaseApiController
	{
		private const string GET_LIST_ROUTE_NAME = "ArtworkMediumXrefASList";
		private IASGenericFactory<entAS.ArtworkMediumXref, dtoAS.ArtworkMediumXref> _factory;
		private int MaxPageSize { get; set; } = 100;

		public ArtworkMediumXrefController(ILogger<ArtworkMediumXrefController> logger,
			IServiceProvider serviceProvider,
			IHttpContextAccessor httpContextAccessor,
			LinkGenerator linkGenerator,
			IASRepository repository,
			IASGenericFactory<entAS.ArtworkMediumXref, dtoAS.ArtworkMediumXref> factory)
			: base(logger, serviceProvider, httpContextAccessor, linkGenerator, repository)
		{
				_factory = factory;
				RunCustomLogicAfterCtor();
		}

		[HttpDelete("{artworkId}/{mediumId}")]
		[VersionedActionConstraint(allowedVersion: 1, order: 100)]
		public async Task<IActionResult> Delete(int artworkId, int mediumId)
		{
				try
				{
						if (!base.OnActionExecuting(out int httpStatusCode, out string message))
							return StatusCode(httpStatusCode, message);

						var result = await Repo.Delete_ArtworkMediumXrefAsync(artworkId, mediumId);

						if (result.Status == RepositoryActionStatus.Deleted)
						{
								return NoContent();
						}
						else if (result.Status == RepositoryActionStatus.NotFound)
						{
								return PrepareNotFoundResponse();
						}

						return PrepareExpectationFailedResponse(result.Exception);
				}
				catch (Exception ex)
				{
						return PrepareInternalServerErrorResponse(ex);
				}
		}

		[HttpGet(template: "{relatedEntitiesType:relatedEntitiesType=None}", Name = GET_LIST_ROUTE_NAME)]
		[VersionedActionConstraint(allowedVersion: 1, order: 100)]
		public async Task<IActionResult> Get(Enums.RelatedEntitiesType relatedEntitiesType,
			string sort = null, string fields = null, string filter = null,
			int page = 1, int pageSize = 100)
		{
				try
				{
						if (!base.OnActionExecuting(out int httpStatusCode, out string message))
							return StatusCode(httpStatusCode, message);

						var fieldList = GetListByDelimiter(fields);
						var filterList = GetListByDelimiter(filter);

						if (pageSize > MaxPageSize)
						{
								pageSize = MaxPageSize;
						}

						var request = new RepositoryPageDataRequest(fieldList: fieldList,
							filterList: filterList, page: page, pageSize: pageSize, sort: sort,
							relatedEntitiesType: relatedEntitiesType);

						RepositoryPageDataResponse<IList<entAS.ArtworkMediumXref>> response =
							await Repo.GetPageData_ArtworkMediumXrefAsync(request);

						PageData paginationHeader = BuildPaginationHeader(nameof(Get), page: page,
							totalCount: response.TotalCount, pageSize: response.PageSize, sort: response.Sort);

						HttpContextAccessor.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
						HttpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));

						var retVal = response.Data.Select(x => _factory.CreateDataShapedObject(x, fieldList));
						return Ok(retVal);
				}
				catch (Exception ex)
				{
						return PrepareInternalServerErrorResponse(ex);
				}
		}

		[HttpGet(template: "ById/{artworkId}/{mediumId}/{relatedEntitiesType:relatedEntitiesType=None}")]
		[VersionedActionConstraint(allowedVersion: 1, order: 100)]
		public async Task<IActionResult> Get(int artworkId, int mediumId, Enums.RelatedEntitiesType relatedEntitiesType)
		{
				try
				{
						if (!base.OnActionExecuting(out int httpStatusCode, out string message))
							return StatusCode(httpStatusCode, message);

						var dbItem = await Repo.Get_ArtworkMediumXrefAsync(artworkId, mediumId, relatedEntitiesType);
						if (dbItem == null)
							return PrepareNotFoundResponse();

						RunCustomLogicOnGetEntityByPK(ref dbItem, artworkId, mediumId, relatedEntitiesType);
						return Ok(_factory.Create(dbItem));
				}
				catch (Exception ex)
				{
						return PrepareInternalServerErrorResponse(ex);
				}
		}

		[HttpPatch("{artworkId}/{mediumId}")]
		[VersionedActionConstraint(allowedVersion: 1, order: 100)]
		public async Task<IActionResult> Patch(int artworkId, int mediumId, [FromBody] JsonPatchDocument<dtoAS.ArtworkMediumXref> patchDocument)
		{
				try
				{
						if (!base.OnActionExecuting(out int httpStatusCode, out string message))
						return StatusCode(httpStatusCode, message);

						if (patchDocument == null)
							return BadRequest();

						var dbItem = await Repo.Get_ArtworkMediumXrefAsync(artworkId, mediumId);
						if (dbItem == null)
							return NotFound();

						var dtoItem = _factory.Create(dbItem); // Map the DB Entity to a DTO.

						// Apply changes to the DTO
						patchDocument.ApplyTo(dtoItem);
						dtoItem.ArtworkId = artworkId;
						dtoItem.MediumId = mediumId;

						// Map the DTO with applied changes back to the DB Entity and perform the update.
						var updatedDBItem = _factory.Create(dtoItem); // Map the DTO to a DB Entity.
						var result = await Repo.UpdateAsync(updatedDBItem);
						RunCustomLogicAfterUpdatePatch(ref updatedDBItem, ref result);

						if (result.Status == RepositoryActionStatus.Updated)
						{
								var patchedDTOItem = _factory.Create(result.Entity); // Map the updated DB Entity to a DTO.
								return Ok(patchedDTOItem);
						}

						return PrepareExpectationFailedResponse(result.Exception);
				}
				catch (Exception ex)
				{
						return PrepareInternalServerErrorResponse(ex);
				}
		}

		[HttpPost]
		[VersionedActionConstraint(allowedVersion: 1, order: 100)]
		public async Task<IActionResult> Post([FromBody] dtoAS.ArtworkMediumXref dtoItem)
		{
				try
				{
						if (!base.OnActionExecuting(out int httpStatusCode, out string message))
							return StatusCode(httpStatusCode, message);

						if (dtoItem == null)
							return BadRequest();

						var newDBItem = _factory.Create(dtoItem); // Map incoming DTO to DB Entity

						var result = await Repo.InsertAsync(newDBItem);
						RunCustomLogicAfterInsert(ref newDBItem, ref result);
						if (result.Status == RepositoryActionStatus.Created)
						{
								var newDTOItem = _factory.Create(result.Entity); // Map created DB Entity to a DTO

								var uriFormatted = LinkGenerator.GetUriByAction(
									httpContext: HttpContextAccessor.HttpContext,
									action: nameof(Get),
									controller: null, // Stay in this controller
									values: new { newDTOItem.ArtworkId, newDTOItem.MediumId }
									);

								return Created(uriFormatted, newDTOItem);
						}

						return PrepareExpectationFailedResponse(result.Exception);
				}
				catch (Exception ex)
				{
						return PrepareInternalServerErrorResponse(ex);
				}
		}

		[HttpPut("{artworkId}/{mediumId}")]
		[VersionedActionConstraint(allowedVersion: 1, order: 100)]
		public async Task<IActionResult> Put(int artworkId, int mediumId, [FromBody] dtoAS.ArtworkMediumXref dtoItem)
		{
				try
				{
						if (!base.OnActionExecuting(out int httpStatusCode, out string message))
							return StatusCode(httpStatusCode, message);

						if (dtoItem == null)
							return BadRequest();

						// ensure we update the record matching the parameter
						dtoItem.ArtworkId = artworkId;
						dtoItem.MediumId = mediumId;

						var updatedDBItem = _factory.Create(dtoItem); // Map the incoming DTO to a DB entity.
						RunCustomLogicBeforeUpdatePut(ref updatedDBItem, artworkId, mediumId);
						var result = await Repo.UpdateAsync(updatedDBItem);
						RunCustomLogicAfterUpdatePut(ref updatedDBItem, ref result);

						if (result.Status == RepositoryActionStatus.Updated)
						{
								var updatedDTOItem = _factory.Create(result.Entity); // Map the updated DB Entity to a DTO
								return Ok(updatedDTOItem);
						}
						else if (result.Status == RepositoryActionStatus.NotFound)
						{
								return NotFound();
						}

						return PrepareExpectationFailedResponse(result.Exception);
				}
				catch (Exception ex)
				{
						return PrepareInternalServerErrorResponse(ex);
				}
		}

		partial void RunCustomLogicAfterInsert(ref entAS.ArtworkMediumXref newDBItem, ref IRepositoryActionResult<entAS.ArtworkMediumXref> result);

		partial void RunCustomLogicAfterUpdatePatch(ref entAS.ArtworkMediumXref updatedDBItem, ref IRepositoryActionResult<entAS.ArtworkMediumXref> result);

		partial void RunCustomLogicAfterUpdatePut(ref entAS.ArtworkMediumXref updatedDBItem, ref IRepositoryActionResult<entAS.ArtworkMediumXref> result);

		partial void RunCustomLogicBeforeUpdatePut(ref entAS.ArtworkMediumXref updatedDBItem, int artworkId, int mediumId);

		partial void RunCustomLogicOnGetEntityByPK(ref entAS.ArtworkMediumXref dbItem, int artworkId, int mediumId, Enums.RelatedEntitiesType relatedEntitiesType);

	}
}
