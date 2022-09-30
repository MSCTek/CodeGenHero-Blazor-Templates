// <auto-generated> - Template:AdminListPageViewModel, Version:2021.11.12, Id:b76e62ec-fe5b-47c6-bb58-fb58ed7399e5
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ArtistSiteAAD.Client.Components;
using ArtistSiteAAD.Client.Services;
using ArtistSiteAAD.Client.Shared;
using ArtistSiteAAD.Shared.Constants;
using ArtistSiteAAD.Shared.DataService;
using ArtistSiteAAD.Shared.DTO;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enums = ArtistSiteAAD.Shared.Constants.Enums;

namespace ArtistSiteAAD.Client.Pages.Admin
{
	[Authorize]
	public partial class ASArtworkMediumXrefsListViewModel : AdminPageBase
	{
		public ASArtworkMediumXrefsListViewModel()
		{
		}

		public IList<ArtworkMediumXref> ArtworkMediumXrefs { get; set; } = new List<ArtworkMediumXref>();

		[Inject]
		public IWebApiDataServiceAS WebApiDataServiceAS { get; set; }

		protected bool Bordered { get; set; } = false;
		protected bool Dense { get; set; } = false;
		protected bool Hover { get; set; } = true;
		protected bool Striped { get; set; } = true;

		[Inject]
		protected ILocalHttpClientService LocalHttpClientService { get; set; }

		protected string SearchString1 { get; set; } = "";

		protected ArtworkMediumXref SelectedArtworkMediumXref { get; set; }

		[Inject]
		private IDialogService DialogService { get; set; }

		protected async Task ConfirmDeleteAsync(ArtworkMediumXref item)
		{
				var parameters = new DialogParameters();
				parameters.Add("ContentText", $"Are you sure you want to delete this?");
				parameters.Add("ButtonText", "Yes");
				parameters.Add("Color", Color.Success);

				var result = await DialogService.Show<ConfirmationDialog>("Confirm", parameters).Result;
				if (!result.Cancelled)
				{
						await DeleteArtworkMediumXrefAsync(item.ArtworkId, item.MediumId);
				}
		}

		protected async Task DeleteArtworkMediumXrefAsync(int artworkId, int mediumId)
		{
				var result = await WebApiDataServiceAS.DeleteArtworkMediumXrefAsync(artworkId, mediumId);
				if (result.IsSuccessStatusCode)
				{

                    StatusClass = "alert-success";
                    Message = "Deleted successfully";
                    await SetSavedAsync(true);
				}
				else
				{

                    StatusClass = "alert-danger";
                    Message = "Something went wrong deleting the item. Please try again.";
                    await SetSavedAsync(false);
				}
		}

		protected bool FilterArtworkMediumXref1(ArtworkMediumXref item) => FilterFunc(item, SearchString1);

		protected bool FilterFunc(ArtworkMediumXref item, string searchString)
		{
				if (string.IsNullOrWhiteSpace(searchString))
					return true;

				searchString = searchString.Trim();
				// Replace with the property you intend a search to work against
				var ArtworkIdString = item.ArtworkId.ToString();
				if (!string.IsNullOrWhiteSpace(ArtworkIdString) && ArtworkIdString.Contains(searchString, StringComparison.OrdinalIgnoreCase))
					return true;

				// Replace with the property you intend a search to work against
				var MediumIdString = item.MediumId.ToString();
				if (!string.IsNullOrWhiteSpace(MediumIdString) && MediumIdString.Contains(searchString, StringComparison.OrdinalIgnoreCase))
					return true;

				return false;
		}

		protected override async Task OnInitializedAsync()
		{
				await base.OnInitializedAsync();
				IsReady = false;
				await SetSavedAsync(false);

				try
				{
						List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>()
						{
								new BreadcrumbItem("Home", "/"),
								new BreadcrumbItem("List of ArtworkMediumXrefs", "/admin/artworkMediumXrefs", disabled: true)
						};

						NavigationService.SetBreadcrumbs(breadcrumbs);
						if (ArtworkMediumXrefs == null || !ArtworkMediumXrefs.Any())
						{
								if (User != null && User.Identity.IsAuthenticated)
								{
										// Add your filtering logic in here, by adding FilterCriterion to the filterCriteria list
										var filterCriteria = new List<IFilterCriterion>();
										ArtworkMediumXrefs = await WebApiDataServiceAS.GetAllPagesArtworkMediumXrefsAsync();
								}
						}
				}
				finally
				{
						IsReady = true;
				}
		}

		protected void ReturnToList()
		{
				NavigationManager.NavigateTo("/admin/artworkMediumXrefs");
		}
		
        protected async Task SetSavedAsync(bool value)
        {
            Saved = value;
            if (value == true)
            {
                await JsRuntime.InvokeVoidAsync("blazorExtensions.ScrollToTop");
            }
        }

	}
}