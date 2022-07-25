// <auto-generated> - Template:AdminEditViewModel, Version:2021.11.12, Id:17ae856a-a589-40c0-a5be-1579b0714385
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ArtistSite.App.Services;
using ArtistSite.App.Shared;
using ArtistSite.Shared.Constants;
using ArtistSite.Shared.DTO;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ArtistSite.App.Pages
{
	[Authorize(Roles = Consts.ROLE_ADMIN_OR_USER)]
	public partial class ASCategoryEditViewModel : AdminPageBase
	{
		public Category Category { get; set; } = new Category();

		[Inject]
		public IWebApiDataServiceAS WebApiDataServiceAS { get; set; }

		[Parameter]
		public int CategoryId { get; set; }

		protected override async Task OnInitializedAsync()
		{
				await base.OnInitializedAsync();

				List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>()
				{
						new BreadcrumbItem("Home", "/"),
						new BreadcrumbItem("List of Categories", "/admin/categories"),
						new BreadcrumbItem("Edit Category", $"/admin/categoryedit/{CategoryId}", disabled: true)
				};

				NavigationService.SetBreadcrumbs(breadcrumbs);
		}

		protected async override Task OnParametersSetAsync()
		{
				IsReady = false;
				await SetSavedAsync(false);

				try
				{
						if (CategoryId == 0) // A new item is being created - opportunity to populate initial/default state
						{
								// Define entity defaults
								Category = new Category { };
						}
						else
						{
								if (Category == null || Category.CategoryId != CategoryId)
								{
										var result = await WebApiDataServiceAS.GetCategoryAsync(CategoryId);
										if (result.IsSuccessStatusCode)
										{
												var category = result.Data;
												// Admins and other approved user claims (Add below) only
												if (!User.IsInRole(Consts.ROLE_ADMIN))
												{
														NavigationManager.NavigateTo($"/Authorization/AccessDenied");
												}
												else
												{
														Category = category;
												}
										}
								}
						}
				}
				finally
				{
						IsReady = true;
				}
		}

		protected async Task OnValidSubmit()
		{
				await SetSavedAsync(false);

				ClearNoneValues();

					if (CategoryId == 0) // A new item is being created - opportunity to populate initial/default state
				{
						var result = await WebApiDataServiceAS.CreateCategoryAsync(Category);
						if (result.IsSuccessStatusCode)
						{
								Category = result.Data;
								StatusClass = "alert-success";
								Message = "New item added successfully.";
								await SetSavedAsync(true);
						}
						else
						{
								StatusClass = "alert-danger";
								Message = "Something went wrong adding the new item. Please try again.";
								await SetSavedAsync(false);
						}
				}
				else
				{
						var result = await WebApiDataServiceAS.UpdateCategoryAsync(Category);
						if (result.IsSuccessStatusCode)
						{
								StatusClass = "alert-success";
								Message = "Category updated successfully.";
								await SetSavedAsync(true);
						}
						else
						{
								StatusClass = "alert-danger";
								Message = "Something went wrong updating the new item. Please try again.";
								await SetSavedAsync(false);
						}
				}
		}

		protected void ReturnToList()
		{
				NavigationManager.NavigateTo("/admin/categories");
		}

        protected async Task SetSavedAsync(bool value)
        {
            Saved = value;
            if (value == true)
            {
                await JsRuntime.InvokeVoidAsync("blazorExtensions.ScrollToTop");
            }
        }

        private void ClearNoneValues()
        {
            // Add handling for null values here
        }
	}
}

