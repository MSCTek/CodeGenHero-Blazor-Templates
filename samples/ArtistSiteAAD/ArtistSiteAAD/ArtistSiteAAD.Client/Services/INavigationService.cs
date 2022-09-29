using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor;

namespace ArtistSiteAAD.Client.Services
{
    public interface INavigationService
    {
        event Action OnBreadcrumbsChanged;

        List<BreadcrumbItem> GetBreadcrumbs();

        void SetBreadcrumbs(IList<BreadcrumbItem> newBreadcrumbs);
        
    }
}
