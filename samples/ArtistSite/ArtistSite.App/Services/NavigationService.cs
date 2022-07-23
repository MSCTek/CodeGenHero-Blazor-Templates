using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtistSite.App.Services
{
    public class NavigationService : INavigationService
    {
        public event Action OnBreadcrumbsChanged;

        private List<BreadcrumbItem> breadcrumbs { get; set; } = new List<BreadcrumbItem>() { new BreadcrumbItem("Home", "/", disabled: true) };

        private List<BreadcrumbItem> Breadcrumbs
        {
            get => breadcrumbs;
            set
            {
                if (breadcrumbs != value)
                {
                    breadcrumbs = value;
                    BreadcrumbsChanged();
                }
            }
        }

        public List<BreadcrumbItem> GetBreadcrumbs()
        {
            return Breadcrumbs;
        }

        public void SetBreadcrumbs(IList<BreadcrumbItem> newBreadcrumbs)
        {
            Breadcrumbs.Clear();
            Breadcrumbs = newBreadcrumbs.ToList();
        }

        private void BreadcrumbsChanged()
        {
            OnBreadcrumbsChanged?.Invoke();
        }
    }
}